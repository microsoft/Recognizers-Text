using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDurtationChsExtractor
    {
        private readonly DurationExtractorChs extractor = new DurationExtractorChs();

        public void BasicTest(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(0, results[0].Start);
            Assert.AreEqual(text.Length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, results[0].Type);
        }

        [TestMethod]
        public void TestTimeChs()
        {
            BasicTest("两年");
            BasicTest("6 天");
            BasicTest("7 周");
            BasicTest("5 小时");
            BasicTest("三年半");
        }
    }
}