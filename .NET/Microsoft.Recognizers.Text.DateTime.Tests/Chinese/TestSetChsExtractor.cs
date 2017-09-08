using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsExtractor
    /// </summary>
    [TestClass]
    public class TestSetChsExtractor
    {
        private readonly SetExtractorChs extractor = new SetExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, results[0].Type);
            TestWriter.Write("Chs", extractor, text, results[0]);
        }

        [TestMethod]
        public void TestSetChs_Extract()
        {
            BasicTest("事件 每天都发生", 3, 2);
            BasicTest("事件每日都发生", 2, 2);
            BasicTest("事件每周都发生", 2, 2);
            BasicTest("事件每个星期都发生", 2, 4);
            BasicTest("事件每个月都发生", 2, 3);
            BasicTest("事件每年都发生", 2, 2);

            BasicTest("事件每周一都发生", 2, 3);
            BasicTest("事件每周一下午八点都发生", 2, 7);
        }
    }
}