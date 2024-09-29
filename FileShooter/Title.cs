using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter {

    public class NamedRegex : Regex {

        public string Name { get; set; }

        public NamedRegex(string name, string pattern) : base(pattern) {
            Name = name;
        }
    }

    public class Title : IComparable<Title> {

        private static Properties.Settings _settings = Properties.Settings.Default;

        public string Name { get; set; } = "";
        public SortedSet<string>? Keywords { get; set; }

        public bool IsBlank { get { return String.IsNullOrEmpty(Name); } }

        public bool IsNameOnly { get { return Keywords == null || Keywords.Count == 0; } }

        public Title(string name = "", SortedSet<string>? keywords = null) {
            Name = name.Trim();
            Keywords = keywords ?? new SortedSet<string>();
        }

        public override string ToString() {
            if (IsBlank) { return ""; }
            return IsNameOnly
                ? Name
                : $"{Name}\n{Keywords?.Select(kw => $"  {kw}").JoinToString("\n")}";
        }

        public int CompareTo(Title? other) {
            if (other == null) { return -1; }
            return Name.CompareTo(other.Name);
        }

        public Title Add(string keyword) {
            if (!String.IsNullOrEmpty(keyword)) {
                Keywords?.Add(keyword.Trim());
            }
            return this;
        }

        public Title Merge(Title? other) {
            if (other != null && other.Keywords != null) {
                this.Keywords.AddRange(other.Keywords);
            }
            return this;
        }

        public string ToPattern() {
            return IsNameOnly
                ? Regex.Escape(Name)
                : $"{Regex.Escape(Name)}|{Keywords?.Select(kw => Regex.Escape(kw)).JoinToString("|")}";
        }

        public NamedRegex ToRegex() {
            var patternBoundary = _settings.BoundaryChars.To<BoundaryChars>().ToPattern();
            return IsNameOnly
                ? new NamedRegex(Name, $"(?<Name>{Name})")
                : new NamedRegex(Name, $"(^|{patternBoundary})(?<Name>{ToPattern()}){patternBoundary}");
        }
    }

    public class TitlePatterns : IStringify<TitlePatterns> {
        public string? Simple { get; set; }
        public IEnumerable<string>? Multi { get; set; }

        public bool IsBlank {
            get { return (Simple == null || String.IsNullOrEmpty(Simple)) && (Multi == null || Multi.Count() == 0); }
        }

        public override string? ToString() {
            var items = new List<string>();
            items.Add(Simple ?? "");
            if (Multi != null && Multi.Count() > 0) {
                items.AddRange(Multi);
            }
            return items.Count == 0
                ? ""
                : items.JoinToString("\n");
        }

        public TitlePatterns FromString(string source) {
            if (String.IsNullOrEmpty(source)) { return this; }
            var items = source.Split("\n").SafeToList();
            Simple = items.FirstOrDefault();
            Multi = items.Skip(1).Where(item => !String.IsNullOrEmpty(item));
            return this;
        }
    }

    public class TitleRegexs {
        public Regex? Simple { get; set; }
        public IEnumerable<NamedRegex>? Multi { get; set; }
    }

    public class TitleGroup : SortedSet<Title> {

        private static Properties.Settings _settings = Properties.Settings.Default;

        public bool IsBlank { get { return this.Count == 0; } }

        public IEnumerable<Title> SimpleTitles { get { return this.Where(t => t.IsNameOnly); } }

        public IEnumerable<Title> MultiTitles { get { return this.Where(t => !t.IsNameOnly); } }

        public TitleGroup() { }

        public override string ToString() {
            return this.JoinToString("\n") ?? "";
        }

        public new TitleGroup Add(Title title) {
            if (title != null && !title.IsBlank) {
                Title? existing;
                if (this.TryGetValue(title, out existing)) {
                    existing.Merge(title);
                } else {
                    base.Add(title);
                }
            }
            return this;
        }

        public string? ToSimplePattern() {
            return IsBlank
                ? null
                : SimpleTitles.Select(t => t.ToPattern()).JoinToString("|");
        }

        public Regex? ToSimpleRegex() {
            if (IsBlank) { return null; }
            var patternBoundary = _settings.BoundaryChars.To<BoundaryChars>().ToPattern();
            var pattern = ToSimplePattern();
            return pattern == null
                ? null
                : new Regex($"(^|{patternBoundary})(?<Name>{pattern}){patternBoundary}");
        }

        public IEnumerable<string>? ToMultiPatterns() {
            return IsBlank
                ? null
                : MultiTitles.Select(t => t.ToPattern());
        }

        public IEnumerable<NamedRegex>? ToMultiRegexs() {
            if (IsBlank) { return null; }
            var patterns = ToMultiPatterns();
            return MultiTitles.Count() == 0
                ? null
                : MultiTitles.Select(t => (NamedRegex)t.ToRegex());
        }

        public TitlePatterns ToPatterns() {
            return new TitlePatterns() {
                Simple = ToSimplePattern(),
                Multi = ToMultiPatterns(),
            };
        }

        public TitleRegexs ToRegexs() {
            return new TitleRegexs() {
                Simple = ToSimpleRegex(),
                Multi = ToMultiRegexs(),
            };
        }
    }

    public class Titles : List<TitleGroup>, IStringify<Titles> {

        public override string ToString() {
            return this.JoinToString("\n---\n") ?? "";
        }

        public Titles FromString(string source = "") {
            if (String.IsNullOrEmpty(source)) { return this; }
            var lines = source.Normalize(NormalizationForm.FormC)
                .Split("\n")
                .Select(line => line.TrimEnd());
            var group = new TitleGroup();
            var title = new Title();
            foreach (var line in lines) {
                if (line == "---") {
                    group.Add(title);
                    title = new Title();
                    this.Add(group);
                    group = new TitleGroup();
                } else if (!line.StartsWith("  ")) {
                    group.Add(title);
                    title = new Title(line);
                } else {
                    title.Add(line);
                }
            }
            group.Add(title);
            this.Add(group);
            return this;
        }

        public new Titles Add(TitleGroup group) {
            if (group != null && !group.IsBlank) {
                base.Add(group);
            }
            return this;
        }

        public IEnumerable<TitlePatterns> ToPatterns() {
            return this.Select(g => g.ToPatterns());
        }

        public IEnumerable<TitleRegexs> ToRegexs() {
            return this.Select(g => g.ToRegexs());
        }
    }
}
