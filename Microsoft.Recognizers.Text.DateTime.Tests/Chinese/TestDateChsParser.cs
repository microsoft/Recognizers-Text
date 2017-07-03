using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsParser
    /// </summary>
    [TestClass]
    public class TestDateChsParser
    {
        readonly DateObject refTime;
        private readonly DateExtractorChs extractor = new DateExtractorChs();
        private readonly DateParser parser = new DateParser(new ChineseDateTimeParserConfiguration());

        public TestDateChsParser()
        {
            refTime = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestDateChs_Parser()
        {
            BasicTest("2010-01-29", "2010-01-29");
            BasicTest("2010.01.29", "2010-01-29");
            BasicTest("2010/01/29", "2010-01-29");
            BasicTest("2010 01 29", "2010-01-29");
            BasicTest("1987年1月11日", "1987-01-11");
            BasicTest("农历2015年十月初一", "2015-10-01");
            BasicTest("2015年农历正月初一", "2015-01-01");
            BasicTest("三月初一", "XXXX-03-01");
            BasicTest("正月三十", "XXXX-01-30");
            BasicTest("大年初一", "XXXX-01-01");
            BasicTest("大年三十", "XXXX-01-30");
            BasicTest("1月19日", "XXXX-01-19");
            BasicTest("1月19号", "XXXX-01-19");
            BasicTest("10月12号，星期一", "XXXX-10-12");
            BasicTest("2009年10月12号，星期一", "2009-10-12");
            BasicTest("明天", "2017-03-23");
            BasicTest("最近", "2017-03-22");
            BasicTest("星期一", "XXXX-WXX-1");
            BasicTest("上周一", "2017-03-13");
            BasicTest("12号", "XXXX-XX-12");
            BasicTest("这个星期一", "2017-03-20");
            BasicTest("两千零四年八月十五", "2004-08-15");
            BasicTest("两千年八月十五", "2000-08-15");
            BasicTest("二零零四年八月十五", "2004-08-15");
            BasicTest("去年本月十日", "2016-03-10");
            BasicTest("本月十日", "XXXX-03-10");
        }
    }
}