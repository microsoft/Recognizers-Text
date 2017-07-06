using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestDurationExtractor
    {
        private readonly BaseDurationExtractor extractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, results[0].Type);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        [TestMethod]
        public void TestDurationExtract()
        {
            BasicTest("I'll leave for 3h", 15, 2);
            BasicTest("I'll leave for 3day", 15, 4);
            BasicTest("I'll leave for 3.5years", 15, 8);

            BasicTest("I'll leave for 3 h", 15, 3);
            BasicTest("I'll leave for 3 hours", 15, 7);
            BasicTest("I'll leave for 3 hrs", 15, 5);
            BasicTest("I'll leave for 3 hr", 15, 4);
            BasicTest("I'll leave for 3 day", 15, 5);
            BasicTest("I'll leave for 3 months", 15, 8);
            BasicTest("I'll leave for 3 minutes", 15, 9);
            BasicTest("I'll leave for 3 min", 15, 5);
            BasicTest("I'll leave for 3.5 second ", 15, 10);
            BasicTest("I'll leave for 123.45 sec", 15, 10);
            BasicTest("I'll leave for two weeks", 15, 9);
            BasicTest("I'll leave for twenty min", 15, 10);
            BasicTest("I'll leave for twenty and four hours", 15, 21);

            BasicTest("I'll leave for all day", 15, 7);
            BasicTest("I'll leave for all week", 15, 8);
            BasicTest("I'll leave for all month", 15, 9);
            BasicTest("I'll leave for all year", 15, 8);

            BasicTest("I'll leave for an hour", 15, 7);
            BasicTest("I'll leave for a year", 15, 6);

            BasicTest("half year", 0, 9);
            BasicTest("half an year", 0, 12);
        }
    }
}