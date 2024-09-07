using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter {

    public class Preprocess : SortedDictionary<string, string>, IStringify<Preprocess> {

        private static Regex _regStartOrEndWithWhiteSpace = new Regex(@"^\s|\s$");
        private static Regex _regFrom = new Regex(@"^-\s(?<q>[""']?)(?<From>.+)\k<q>$");
        private static Regex _regTo = new Regex(@"^(?<q>[""']?)(?<To>.+)\k<q>$");

        public bool IsEmpty { get { return this.Count == 0; } }

        public override string ToString() {
            return this.Keys
                .Select(key => ToPair(key))
                .JoinToString("\n");
        }

        public Preprocess FromString(string source) {
            if (String.IsNullOrEmpty(source)) { return this; }
            var lines = source.Normalize(NormalizationForm.FormC).SplitTrim(new[] { "\n" });
            for (var position = 0; position < lines.Count(); position += 2) {
                var pair = lines.Skip(position).Take(2);
                var mFrom = _regFrom.Match(pair.First());
                var mTo = _regTo.Match(pair.Last());
                if (mFrom == null || !mFrom.Success || mTo == null || !mTo.Success) { continue; }
                this[mFrom.Groups["From"].Value] = mTo.Groups["To"].Value;
            }
            return this;
        }

        public string ToPattern() {
            return $"(?<From>{this.Keys.Select(from => Regex.Escape(from)).JoinToString("|")})";
        }

        public Regex ToRegex() {
            return new Regex(this.ToPattern());
        }

        public string Evaluator(Match match) {
            return this[match.Groups["From"].Value];
        }

        private string ToPair(string key) {
            var key2 = IsStartOrEndWithWhiteSpace(key) ? $"\"{key}\"" : key;
            var val = this[key];
            if (IsStartOrEndWithWhiteSpace(val)) {
                val = $"\"{val}\"";
            }
            return $"- {key2}\n  {val}";
        }

        private bool IsStartOrEndWithWhiteSpace(string text) {
            if (String.IsNullOrEmpty(text)) { return false; }
            return _regStartOrEndWithWhiteSpace.IsMatch(text);
        }
    }
}
