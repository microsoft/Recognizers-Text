using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestMergedExtractor
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        public void BasicTestNone(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
        }

        public void BasicTestWithOptions(string text, int count, DateTimeOptions options = DateTimeOptions.None)
        {
            IExtractor extractorWithOptions = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options);
            var results = extractorWithOptions.Extract(text);
            Assert.AreEqual(count, results.Count);
        }

        [TestMethod]
        public void TestMergedExtract()
        {
            BasicTest("this is 2 days", 8, 6);

            BasicTest("this is before 4pm", 8, 10);
            BasicTest("this is before 4pm tomorrow", 8, 19);
            BasicTest("this is before tomorrow 4pm ", 8, 19);

            BasicTest("this is after 4pm", 8, 9);
            BasicTest("this is after 4pm tomorrow", 8, 18);
            BasicTest("this is after tomorrow 4pm ", 8, 18);

            BasicTest("I'll be back in 5 minutes", 13, 12);
            BasicTest("past week", 0, 9);
            BasicTest("past Monday", 0, 11);
            BasicTest("schedule a meeting in 10 hours", 19, 11);
        }

        [TestMethod]
        public void TestMergedSkipFromTo()
        {
            BasicTestWithOptions("Change my meeting from 9am to 11am", 2, DateTimeOptions.SplitFromTo);
            BasicTestWithOptions("Change my meeting from Nov.19th to Nov.23th", 2, DateTimeOptions.SplitFromTo);
            BasicTestWithOptions("Schedule a meeting from 9am to 11am", 1);
            BasicTestWithOptions("Schedule a meeting from 9am to 11am tomorrow", 1);
        }

        [TestMethod]
        public void TestAfterBefore()
        {
            BasicTest("after 7/2 ", 0, 9);
            BasicTest("since 7/2 ", 0, 9);
            BasicTest("before 7/2 ", 0, 10);
        }

        [TestMethod]
        public void TestDateWithTime()
        {
            BasicTest("06/06 12:15", 0, 11);
            BasicTest("06/06/12 15:15", 0, 14);
            BasicTest("06/06, 2015", 0, 11);
        }

        [TestMethod]
        public void TestNegativeExtract()
        {
            //Unit tests for text should not extract datetime
            BasicTestNone("which email have gotten a reply");
            BasicTestNone("He is often alone");
            BasicTestNone("often a bird");
            BasicTestNone("michigan hours");
        }
    }
}