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

        [TestMethod]
        public void TestMergedExtract()
        {
            BasicTest("this is 2 days in p ", 8, 6);

            BasicTest("this is before 4pm", 8, 10);
            BasicTest("this is before 4pm tomorrow", 8, 19);
            BasicTest("this is before tomorrow 4pm ", 8, 19);

            BasicTest("this is after 4pm", 8, 9);
            BasicTest("this is after 4pm tomorrow", 8, 18);
            BasicTest("this is after tomorrow 4pm ", 8, 18);
        }
    }
}