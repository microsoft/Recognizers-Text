using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDatePeriodChsParser
    /// </summary>
    [TestClass]
    public class TestDatePeriodChsParser
    {
        readonly DateObject refTime;
        private readonly DatePeriodExtractorChs extractor = new DatePeriodExtractorChs();
        private readonly DatePeriodParserChs parser = new DatePeriodParserChs(new ChineseDateTimeParserConfiguration());

        public TestDatePeriodChsParser()
        {
            refTime = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Chs", parser, text, pr);
        }

        [TestMethod]
        public void TestDatePeriodChs_Parser()
        {
            BasicTest("时间从一月十日到十二日", "(XXXX-01-10,XXXX-01-12,P2D)");
            BasicTest("时间从2016年一月十日到十二日", "(2016-01-10,2016-01-12,P2D)");
            BasicTest("时间从一月19日到20日", "(XXXX-01-19,XXXX-01-20,P1D)");
            BasicTest("从一月十日到20日", "(XXXX-01-10,XXXX-01-20,P10D)");
            BasicTest("明年四月", "2018-04");
            BasicTest("我们去年5月见过", "2016-05");
            BasicTest("下周末", "2017-W14-WE");
            BasicTest("会议在下周", "2017-W14");
            BasicTest("下个月完工", "2017-04");
            BasicTest("下周如何", "2017-W14");
            BasicTest("明年", "2018");
            BasicTest("奥运会在2008年", "2008");
            BasicTest("十月的第一周是国庆节", "XXXX-10-W01");
            BasicTest("三月二十八日到四月15日", "(XXXX-03-28,XXXX-04-15,P18D)");
            BasicTest("前1周", "(2017-03-15,2017-03-22,P1W)");
            BasicTest("上2个月", "(2017-01-22,2017-03-22,P2M)");
            BasicTest("后1年", "(2017-03-23,2018-03-23,P1Y)");
            BasicTest("今年夏天", "2017-SU");
            BasicTest("今年第一季度", "(2017-01-01,2017-04-01,P3M)");
        }
    }
}