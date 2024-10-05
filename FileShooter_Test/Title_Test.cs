using NUnit.Framework.Legacy;
using System.Text.Json;
using FileShooter;
using TakeAshUtility;

namespace FileShooter_Test {

    [TestFixture]
    public class Title_Test {

        [SetUp]
        public void Setup() {
        }

        private static void AreEqualByJson(object expected, object actual) {
            var expectedJson = JsonSerializer.Serialize(expected);
            var actualJson = JsonSerializer.Serialize(actual);
            ClassicAssert.AreEqual(expectedJson, actualJson);
        }

        [TestCase("", "")]
        [TestCase("aaa", "aaa")]
        [TestCase("aaa\nbbb\nccc\nddd", "aaa\nbbb\nccc\nddd")]
        [TestCase("\naaa\nbbb\n\nccc\n", "\naaa\nbbb\nccc")]
        public void TitlePatterns_FromString(string input, string expected) {
            ClassicAssert.AreEqual(expected, input.To<TitlePatterns>().ToString());
        }

        [TestCase("", "")]
        [TestCase("aaa", "aaa")]
        [TestCase("aaa\nbbb\nccc\nddd", "aaa\nbbb\nccc\nddd")]
        [TestCase("bbb\naaa\nddd\nccc", "aaa\nbbb\nccc\nddd")]
        [TestCase("aaa\n  AAA\nbbb\nccc\n  CCC\n  DDD\nddd", "aaa\n  AAA\nbbb\nccc\n  CCC\n  DDD\nddd")]
        [TestCase("bbb\naaa\n  AAA\nddd\nccc\n  CCC\n  DDD", "aaa\n  AAA\nbbb\nccc\n  CCC\n  DDD\nddd")]
        [TestCase(
            "aaa\nbbb\nccc\n---\nddd\neee\nfff\n---\nggg\nhhh\niii",
            "aaa\nbbb\nccc\n---\nddd\neee\nfff\n---\nggg\nhhh\niii"
        )]
        [TestCase(
            "aaa\n  AAA\n  BBB\nbbb\nccc\n---\nddd\neee\nfff\n  CCC\n  DDD\n---\nggg\nhhh\n  EEE\n  FFF\niii",
            "aaa\n  AAA\n  BBB\nbbb\nccc\n---\nddd\neee\nfff\n  CCC\n  DDD\n---\nggg\nhhh\n  EEE\n  FFF\niii"
        )]
        [TestCase(
            "aaa\nbbb\n  444\n  222\nccc\nbbb\n  333\n  111\nddd",
            "aaa\nbbb\n  111\n  222\n  333\n  444\nccc\nddd"
        )]
        [TestCase("aaa/Y", "aaa/Y")]
        [TestCase("aaa/Y\nbbb/Y\nccc/Y\nddd/Y", "aaa/Y\nbbb/Y\nccc/Y\nddd/Y")]
        [TestCase("bbb/Y\naaa/Y\nddd/Y\nccc/Y", "aaa/Y\nbbb/Y\nccc/Y\nddd/Y")]
        [TestCase("aaa/Y\n  AAA\nbbb/Y\nccc/Y\n  CCC\n  DDD\nddd/Y", "aaa/Y\n  AAA\nbbb/Y\nccc/Y\n  CCC\n  DDD\nddd/Y")]
        [TestCase("bbb/Y\naaa/Y\n  AAA\nddd/Y\nccc/Y\n  CCC\n  DDD", "aaa/Y\n  AAA\nbbb/Y\nccc/Y\n  CCC\n  DDD\nddd/Y")]
        [TestCase(
            "aaa/Y\nbbb/Y\nccc/Y\n---\nddd/Y\neee/Y\nfff/Y\n---\nggg/Y\nhhh/Y\niii/Y",
            "aaa/Y\nbbb/Y\nccc/Y\n---\nddd/Y\neee/Y\nfff/Y\n---\nggg/Y\nhhh/Y\niii/Y"
        )]
        [TestCase(
            "aaa/Y\n  AAA\n  BBB\nbbb/Y\nccc/Y\n---\nddd/Y\neee/Y\nfff/Y\n  CCC\n  DDD\n---\nggg/Y\nhhh/Y\n  EEE\n  FFF\niii/Y",
            "aaa/Y\n  AAA\n  BBB\nbbb/Y\nccc/Y\n---\nddd/Y\neee/Y\nfff/Y\n  CCC\n  DDD\n---\nggg/Y\nhhh/Y\n  EEE\n  FFF\niii/Y"
        )]
        [TestCase(
            "aaa/Y\nbbb/Y\n  444\n  222\nccc/Y\nbbb/Y\n  333\n  111\nddd/Y",
            "aaa/Y\nbbb/Y\n  111\n  222\n  333\n  444\nccc/Y\nddd/Y"
        )]
        public void Titles_FromString(string input, string expected) {
            ClassicAssert.AreEqual(expected, input.To<Titles>().ToString());
        }

        [TestCase("", "")]
        [TestCase("aaa", "aaa")]
        [TestCase("aaa\nbbb\nccc\nddd", "aaa|bbb|ccc|ddd")]
        [TestCase("bbb\naaa\nddd\nccc", "aaa|bbb|ccc|ddd")]
        [TestCase("aaa\n  AAA\nbbb\nccc\n  CCC\n  DDD\nddd", "bbb|ddd\naaa|AAA\nccc|CCC|DDD")]
        [TestCase("bbb\naaa\n  AAA\nddd\nccc\n  CCC\n  DDD", "bbb|ddd\naaa|AAA\nccc|CCC|DDD")]
        [TestCase(
            "aaa\nbbb\nccc\n---\nddd\neee\nfff\n---\nggg\nhhh\niii",
            "aaa|bbb|ccc\n\nddd|eee|fff\n\nggg|hhh|iii"
        )]
        [TestCase(
            "aaa\n  AAA\n  BBB\nbbb\nccc\n---\nddd\neee\nfff\n  CCC\n  DDD\n---\nggg\nhhh\n  EEE\n  FFF\niii",
            "bbb|ccc\naaa|AAA|BBB\n\nddd|eee\nfff|CCC|DDD\n\nggg|iii\nhhh|EEE|FFF"
        )]
        [TestCase("×(かける)クラシック", "×\\(かける\\)クラシック")]
        public void Titles_ToPatterns(string input, string expected) {
            AreEqualByJson(
                expected.Split("\n\n").Select(p => p.To<TitlePatterns>()).Where(p => !p.IsBlank),
                input.To<Titles>().ToPatterns()
            );
        }
    }
}
