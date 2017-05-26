using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using Microsoft.Recognizers.Text.DateTime.Chinese.Parsers;
using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsParser
    /// </summary>
    [TestClass]
    public class TestHolidayChsParser
    {
        readonly DateObject refTime;
        private readonly BaseHolidayExtractor extractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());
        private readonly HolidayParserChs parser = new HolidayParserChs(new ChineseDateTimeParserConfiguration());

        public TestHolidayChsParser()
        {
            refTime = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(timex, ((DTParseResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestHolidayChs_Parser()
        {
            BasicTest("元旦", "XXXX-01-01");
            BasicTest("元旦节", "XXXX-01-01");
            BasicTest("教师节", "XXXX-09-10");
            BasicTest("青年节", "XXXX-05-04");
            BasicTest("儿童节", "XXXX-06-01");
            BasicTest("妇女节", "XXXX-03-08");
            BasicTest("植树节", "XXXX-03-12");
            BasicTest("情人节", "XXXX-02-14");
            BasicTest("圣诞节", "XXXX-12-25");
            BasicTest("新年", "XXXX-01-01");
            BasicTest("愚人节", "XXXX-04-01");
            BasicTest("五一", "XXXX-05-01");
            BasicTest("劳动节", "XXXX-05-01");
            BasicTest("万圣节", "XXXX-10-31");
            BasicTest("中秋节", "XXXX-08-15");
            BasicTest("中秋", "XXXX-08-15");
            BasicTest("春节", "XXXX-01-01");
            BasicTest("除夕", "XXXX-12-31");
            BasicTest("元宵节", "XXXX-01-15");
            BasicTest("清明节", "XXXX-04-04");
            BasicTest("清明", "XXXX-04-04");
            BasicTest("端午节", "XXXX-05-05");
            BasicTest("端午", "XXXX-05-05");
            BasicTest("国庆节", "XXXX-10-01");
            BasicTest("建军节", "XXXX-08-01");
            BasicTest("女生节", "XXXX-03-07");
            BasicTest("光棍节", "XXXX-11-11");
            BasicTest("双十一", "XXXX-11-11");
            BasicTest("重阳节", "XXXX-09-09");
            BasicTest("父亲节", "XXXX-06-WXX-6-3");
            BasicTest("母亲节", "XXXX-05-WXX-7-2");
            BasicTest("感恩节", "XXXX-11-WXX-4-4");
        }
    }
}