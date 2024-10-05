using System.Text;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using TakeAshUtility;
using WpfUtility;
using System.ComponentModel;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Net.Mime.MediaTypeNames;

namespace FileShooter {

    public enum AppStatus {
        Ready,
        Running,
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private readonly Properties.Settings _settings = Properties.Settings.Default;
        private WindowPlacement? _placement;
        private IntPtr _hWnd;
        private Logic _logic;
        private const int _phaseMax = (int)Phase.Complete;
        private Phase _phase = Phase.Complete;
        private int _progressMax = 0;
        private int _progress = 0;
        private AppStatus _status = AppStatus.Ready;

        public Phase Phase {
            get { return _phase; }
            set {
                var val = (int)(_phase = value);
                this.Dispatcher.Invoke(() => {
                    progressBar_Phase.Value = val;
                    statusText_Phase.Text = $"{Phase} ({val}/{_phaseMax})";
                    TaskbarProgress.SetValue(_hWnd, val, _phaseMax);
                    textOut.Text += $"\n# {_phase.ToDescription()}\n";
                    textOut.CaretIndex = textOut.Text.Length;
                    textOut.ScrollToEnd();
                });
            }
        }

        public int ProgressMax {
            get { return _progressMax; }
            set {
                _progressMax = value;
                this.Dispatcher.Invoke(() => {
                    progressBar_Sub.Maximum = _progressMax;
                });
            }
        }

        public int Progress {
            get { return _progress; }
            set {
                _progress = value;
                this.Dispatcher.Invoke(() => {
                    progressBar_Sub.Value = _progress;
                    statusText_Sub.Text = _progressMax == 0 ?
                        "" :
                        (100.0 * _progress / _progressMax).ToString("0.0") + "%";
                });
            }
        }

        public AppStatus Status {
            get { return _status; }
            set {
                _status = value;
                this.Dispatcher.Invoke(() => {
                    switch (_status) {
                        case AppStatus.Ready:
                            buttonRun.IsEnabled = true;
                            buttonStop.IsEnabled = false;
                            break;
                        case AppStatus.Running:
                            buttonRun.IsEnabled = false;
                            buttonStop.IsEnabled = true;
                            break;
                    }
                });
            }
        }

        public MainWindow() {
            InitializeComponent();
            progressBar_Phase.Maximum = _phaseMax;
            Status = AppStatus.Ready;
            _logic = new Logic(this);
        }

        public void ClearText() {
            this.Dispatcher.Invoke(() => { textOut.Text = textErr.Text = ""; });
        }

        public void AddTextOut(string text) {
            this.Dispatcher.Invoke(() => {
                textOut.Text += $"{text}\n";
                textOut.CaretIndex = textOut.Text.Length;
                textOut.ScrollToEnd();
            });
        }

        public void AddTextErr(string text) {
            this.Dispatcher.Invoke(() => {
                textErr.Text += $"{text}\n";
                textErr.CaretIndex = textErr.Text.Length;
                textErr.ScrollToEnd();
            });
        }

        protected override void OnSourceInitialized(EventArgs e) {
            base.OnSourceInitialized(e);
            _placement = new WindowPlacement(this) {
                Placement = _settings.WindowPlacement,
            };
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            if (!e.Cancel) {
                _settings.WindowPlacement = _placement?.Placement;
                _settings.Save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var source = (HwndSource)HwndSource.FromVisual(this);
            _hWnd = source.Handle;
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e) {
            textTargetFolder.Text = _settings.TargetFolder.ToTargetFolder();
            textBoundaryChars.Text = _settings.BoundaryChars.To<BoundaryChars>().ToString();
            textTitles.Text = _settings.Titles.To<Titles>().ToString();
            textLabels.Text = _settings.Labels.To<Labels>().ToString();
            textPreprocess.Text = _settings.Preprocess.To<Preprocess>().ToString();
        }

        private void TabItem_LostFocus(object sender, RoutedEventArgs e) {
            _settings.TargetFolder = textTargetFolder.Text.ToTargetFolder();
            _settings.BoundaryChars = textBoundaryChars.Text.To<BoundaryChars>().ToString();
            _settings.Titles = textTitles.Text.To<Titles>().ToString();
            _settings.Labels = textLabels.Text.To<Labels>().ToString();
            _settings.Preprocess = textPreprocess.Text.To<Preprocess>().ToString();
            _settings.Save();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void buttonRun_Click(object sender, RoutedEventArgs e) {
            Status = AppStatus.Running;
            _logic.Run();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e) {
            _logic.Stop();
        }

        private void buttonErase_Click(object sender, RoutedEventArgs e) {
            ClearText();
        }

        private void buttonSelectTargetFolder_Click(object sender, RoutedEventArgs e) {
            using (var dlg = new CommonOpenFileDialog() {
                IsFolderPicker = true,
                RestoreDirectory = true,
            }) {
                if (dlg.ShowDialog() != CommonFileDialogResult.Ok) { return; }
                textTargetFolder.Text = dlg.FileName;
            }
        }
    }
}
