using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestHolidayExtractor
    {
        private readonly BaseHolidayExtractor extractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.English, typeof(BaseHolidayExtractor));
        }

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
            TestWriter.Write(TestCulture.English, extractor, text, results);
        }

        [TestMethod]
        public void TestHolidayExtract()
        {
            BasicTest("I'll go back on christmas", 16, 9);
            BasicTest("I'll go back on christmas day", 16, 13);

            BasicTest("I'll go back on Yuandan", 16, 7);
            BasicTest("I'll go back on thanks giving day", 16, 17);
            BasicTest("I'll go back on father's day", 16, 12);

            BasicTest("I'll go back on Yuandan of this year", 16, 20);
            BasicTest("I'll go back on Yuandan of 2016", 16, 15);
            BasicTest("I'll go back on Yuandan 2016", 16, 12);

            BasicTest("I'll go back on clean monday", 16, 12);
        }
    }
}