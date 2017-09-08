using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsParser
    /// </summary>
    [TestClass]
    public class TestSetChsParser
    {
        private readonly SetExtractorChs extractor = new SetExtractorChs();
        private readonly SetParserChs parser = new SetParserChs(new ChineseDateTimeParserConfiguration());

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Chs", parser, text, pr);
        }

        [TestMethod]
        public void TestSetChs_Parser()
        {
            BasicTest("事件 每天都发生", "P1D");
            BasicTest("事件每日都发生", "P1D");
            BasicTest("事件每周都发生", "P1W");
            BasicTest("事件每个星期都发生", "P1W");
            BasicTest("事件每个月都发生", "P1M");
            BasicTest("事件每年都发生", "P1Y");

            BasicTest("事件每周一都发生", "XXXX-WXX-1");
            BasicTest("事件每周一下午八点都发生", "XXXX-WXX-1T20");
        }
    }
}