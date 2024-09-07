using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter {

    public class BoundaryChars : SortedSet<Char>, IStringify<BoundaryChars> {

        public override string ToString() {
            return this.JoinToString("");
        }

        public BoundaryChars FromString(string source) {
            if (String.IsNullOrEmpty(source)) { return this; }
            this.AddRange(source.Normalize(NormalizationForm.FormC).ToCharArray());
            return this;
        }

        public string ToPattern() {
            return $"[\\s\\b\\-{Regex.Escape(this.ToString())}]";
        }
    }
}
