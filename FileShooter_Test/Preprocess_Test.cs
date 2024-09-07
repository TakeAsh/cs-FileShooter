using FileShooter;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeAshUtility;

namespace FileShooter_Test {

    [TestFixture]
    public class Preprocess_Test {

        [SetUp]
        public void Setup() {
        }

        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase(
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC",
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC"
        )]
        [TestCase(
            "- bbb\n  BBB\n- aaa\n  AAA\n- ddd\n  DDD\n- ccc\n  CCC",
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC\n- ddd\n  DDD"
        )]
        [TestCase(
            "- aaa\n  AAA\n- bbb\n  \" BBB\"\n- ccc\n  \'CCC \'\n- ddd\n  \" DDD \"",
            "- aaa\n  AAA\n- bbb\n  \" BBB\"\n- ccc\n  \"CCC \"\n- ddd\n  \" DDD \""
        )]
        [TestCase(
            "- aaa\n  AAA\n- \" bbb\"\n  BBB\n- \'ccc \'\n  CCC\n- \" ddd \"\n  DDD",
            "- \" bbb\"\n  BBB\n- \" ddd \"\n  DDD\n- aaa\n  AAA\n- \"ccc \"\n  CCC"
        )]
        public void Preprocess_FromString(string? input, string? expected) {
            ClassicAssert.AreEqual(expected, input.To<Preprocess>().ToString());
        }

        [TestCase(
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC",
            "(?<From>aaa|bbb|ccc)"
        )]
        [TestCase(
            "- aaa?*-\n  AAA\n- [bbb]\n  BBB\n- {ccc}\n  CCC",
            "(?<From>\\[bbb]|\\{ccc}|aaa\\?\\*-)"
        )]
        public void Preprocess_ToPattern(string input, string expected) {
            ClassicAssert.AreEqual(expected, input.To<Preprocess>().ToPattern());
        }

        [TestCase(
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC",
            "wwwaaaxxxbbbyyyccczzz",
            "wwwAAAxxxBBByyyCCCzzz"
        )]
        [TestCase(
            "- aaa\n  AAA\n- bbb\n  BBB\n- ccc\n  CCC",
            "wwwcccxxxbbbyyyaaazzz",
            "wwwCCCxxxBBByyyAAAzzz"
        )]
        public void Preprocess_Replace(string source, string input, string expected) {
            var preprocess = source.To<Preprocess>();
            ClassicAssert.AreEqual(expected, preprocess.ToRegex().Replace(input, preprocess.Evaluator));
        }
    }
}
