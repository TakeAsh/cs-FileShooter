using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TakeAshUtility;

namespace FileShooter {

    public enum ReportType {
        PhaseValue,
        ProgressMax,
        ProgressValue,
    }

    public class Logic {

        private static Properties.Settings _settings = Properties.Settings.Default;
        private static readonly Regex _regInvalidChars = new Regex(@"([/\\:;\?\*\|<>\u0022'])");
        private static readonly Regex _regMultiSpace = new Regex(@"\s{2,}");
        private static readonly Regex _regNoSeries = new Regex(@"\s-\s-\s");
        private MainWindow _window;
        private Regex? _regLabels;
        private Preprocess? _preprocess;
        private Regex? _regPreprocess;
        private Titles? _titles;
        private BackgroundWorker _worker;
        private DoWorkEventArgs _args = new(null);

        public Logic(MainWindow windows) {
            _window = windows;
            _worker = new BackgroundWorker() {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            _worker.DoWork += (s, args) => {
                var worker = s as BackgroundWorker;
                if (worker == null) { return; }
                _args = args;
                var files = GetFiles();
                if (worker.CancellationPending) { _args.Cancel = true; return; }
                files = NormalizeFiles(files);
                if (worker.CancellationPending) { _args.Cancel = true; return; }
                MoveFiles(files);
                if (worker.CancellationPending) { _args.Cancel = true; return; }
            };
            _worker.ProgressChanged += (s, args) => {
                if (args.UserState == null) { return; }
                switch ((ReportType)args.UserState) {
                    case ReportType.PhaseValue:
                        _window.Phase = (Phase)args.ProgressPercentage;
                        break;
                    case ReportType.ProgressMax:
                        _window.ProgressMax = args.ProgressPercentage;
                        break;
                    case ReportType.ProgressValue:
                        _window.Progress = args.ProgressPercentage;
                        break;
                }
            };
            _worker.RunWorkerCompleted += (s, args) => {
                _window.Status = AppStatus.Ready;
                if (args.Cancelled) {
                    _window.AddTextOut("\n# Canceled");
                    return;
                }
                _window.Phase = Phase.Complete;
            };
        }

        public void Run() {
            _regLabels = _settings.Labels.To<Labels>().ToRegex();
            _preprocess = _settings.Preprocess.To<Preprocess>();
            _regPreprocess = _preprocess.ToRegex();
            _titles = _settings.Titles.To<Titles>();
            _worker.RunWorkerAsync();
        }

        public void Stop() {
            if (_worker == null || !_worker.IsBusy) { return; }
            _worker.CancelAsync();
        }

        private IEnumerable<FileInfo>? GetFiles() {
            _worker.ReportProgress((int)Phase.GetFiles, ReportType.PhaseValue);
            _window.ProgressMax = 0;
            if (String.IsNullOrEmpty(_settings.TargetFolder)) { return null; }
            var files = Directory.GetFiles(_settings.TargetFolder, "*.*", SearchOption.TopDirectoryOnly)
                .Select(fname => new FileInfo(fname));
            return files;
        }

        private IEnumerable<FileInfo>? NormalizeFiles(IEnumerable<FileInfo>? files) {
            if (files == null || files.Count() == 0) { return null; }
            _worker.ReportProgress((int)Phase.NormalizeFiles, ReportType.PhaseValue);
            _worker.ReportProgress(files.Count(), ReportType.ProgressMax);
            var index = 0;
            var files2 = new List<FileInfo>();
            foreach (var file in files) {
                if (_worker.CancellationPending) { _args.Cancel = true; return null; }
                _worker.ReportProgress(++index, ReportType.ProgressValue);
                var from = file.Name;
                var normalized = NormalizeFile(file.Name);
                try {
                    if (from != normalized) {
                        file.MoveTo(Path.Join(file.DirectoryName, normalized));
                        _window.AddTextOut($"From:\t{from}\nTo:\t{normalized}");
                    }
                    files2.Add(file);
                } catch (Exception ex) {
                    _window.AddTextErr($"Failed to normalize: {file.Name}\n{ex.Message}");
                }
            }
            return files2.Count == 0
                ? null
                : files2;
        }

        private string NormalizeFile(string fname) {
            if (String.IsNullOrEmpty(fname)) { return fname; }
            var fname2 = fname.Normalize(NormalizationForm.FormC)
                .Latin1_ZenToHan()
                .Replace(_regInvalidChars, (m) => m.Value.Latin1_HanToZen())
                .Replace(_regLabels, (m) => "")
                .Replace(_regMultiSpace, (m) => " ")
                .Replace(_regNoSeries, (m) => " - ");
            if (_preprocess != null && !_preprocess.IsEmpty && _regPreprocess != null) {
                fname2 = _regPreprocess.Replace(fname2, _preprocess.Evaluator);
            }
            return fname2;
        }

        private void MoveFiles(IEnumerable<FileInfo>? files) {
            if (files == null || files.Count() == 0 || _titles == null || _titles.Count == 0) { return; }
            _worker.ReportProgress((int)Phase.MoveFiles, ReportType.PhaseValue);
            _worker.ReportProgress(files.Count(), ReportType.ProgressMax);
            var index = 0;
            foreach (var file in files) {
                if (_worker.CancellationPending) { _args.Cancel = true; return; }
                _worker.ReportProgress(++index, ReportType.ProgressValue);
                new Action(() => {
                    foreach (var group in _titles) {
                        var reg = group.ToRegexs();
                        if (reg.Simple != null) {
                            var m = reg.Simple.Match(file.Name);
                            if (m.Success) {
                                var dir = m.Groups["Name"].Value;
                                var dirFull = Path.Join(_settings.TargetFolder, dir);
                                if (!Directory.Exists(dirFull)) {
                                    Directory.CreateDirectory(dirFull);
                                }
                                try {
                                    file.MoveTo(Path.Join(dirFull, file.Name));
                                    _window.AddTextOut($"{dir}: {file.Name}");
                                } catch (Exception ex) {
                                    _window.AddTextErr($"Failed to move: {file.Name}\n{ex.Message}");
                                }
                                return;
                            }
                        }
                        if (reg.Multi != null) {
                            foreach (var namedReg in reg.Multi) {
                                var m = namedReg.Match(file.Name);
                                if (m.Success) {
                                    var dir = namedReg.Name;
                                    var dirFull = Path.Join(_settings.TargetFolder, dir);
                                    if (!Directory.Exists(dirFull)) {
                                        Directory.CreateDirectory(dirFull);
                                    }
                                    try {
                                        file.MoveTo(Path.Join(dirFull, file.Name));
                                        _window.AddTextOut($"{dir}: {file.Name}");
                                    } catch (Exception ex) {
                                        _window.AddTextErr($"Failed to move: {file.Name}\n{ex.Message}");
                                    }
                                    return;
                                }
                            }
                        }
                    }
                })();
            }
        }
    }
}
