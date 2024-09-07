using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter {

    public class Labels : SortedSet<string>, IStringify<Labels> {

        public override string ToString() {
            return this.JoinToString("\n");
        }

        public Labels FromString(string source) {
            if (String.IsNullOrEmpty(source)) { return this; }
            this.AddRange(source.Normalize(NormalizationForm.FormC).SplitTrim(new[] { "\n" }));
            return this;
        }

        public string ToPattern() {
            return $"({this.JoinToString("|")})";
        }

        public Regex ToRegex() {
            return new Regex(ToPattern());
        }
    }
}
