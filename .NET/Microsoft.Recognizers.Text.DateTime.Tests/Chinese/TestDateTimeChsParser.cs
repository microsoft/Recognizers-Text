using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDateTimeChsParser
    {
        readonly DateObject refTime;
        private readonly DateTimeExtractorChs extractor = new DateTimeExtractorChs();
        private readonly DateTimeParserChs parser = new DateTimeParserChs(new ChineseDateTimeParserConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.Chinese, typeof(DateTimeParserChs));
        }

        public TestDateTimeChsParser()
        {
            refTime = new DateObject(2016, 11, 7, 14, 7, 0);
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write(TestCulture.Chinese, parser, refTime, text, pr);
        }

        public void BasicTest(string text, DateObject futreTime, DateObject pastTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(futreTime, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(pastTime, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write(TestCulture.Chinese, parser, refTime, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write(TestCulture.Chinese, parser, refTime, text, pr);
        }

        [TestMethod]
        public void TestDateTimeParseChs()
        {
            // test result value
            BasicTest("2010-01-29早上七点", new DateObject(2010, 1, 29, 7, 0, 0), new DateObject(2010, 1, 29, 7, 0, 0));
            BasicTest("2010.01.29晚上六点", new DateObject(2010, 1, 29, 18, 0, 0), new DateObject(2010, 1, 29, 18, 0, 0));
            BasicTest("2010/01/29中午十二点", new DateObject(2010, 1, 29, 12, 0, 0), new DateObject(2010, 1, 29, 12, 0, 0));
            BasicTest("2010 01 29五点", new DateObject(2010, 1, 29, 5, 0, 0), new DateObject(2010, 1, 29, 5, 0, 0));
            BasicTest("1987年1月11日八点", new DateObject(1987, 1, 11, 8, 0, 0), new DateObject(1987, 1, 11, 8, 0, 0));
            BasicTest("农历2015年十月初一早上九点二十", new DateObject(2015, 10, 1, 9, 20, 0), new DateObject(2015, 10, 1, 9, 20, 0));
            BasicTest("1月19号下午5:00", new DateObject(2017, 1, 19, 17, 0, 0), new DateObject(2016, 1, 19, 17, 0, 0));
            BasicTest("明天下午5:00", new DateObject(2016, 11, 8, 17, 0, 0), new DateObject(2016, 11, 8, 17, 0, 0));

            BasicTest("今晚6点", new DateObject(2016, 11, 7, 18, 0, 0), new DateObject(2016, 11, 7, 18, 0, 0));
            BasicTest("今晨5点", new DateObject(2016, 11, 7, 5, 0, 0), new DateObject(2016, 11, 7, 5, 0, 0));
            BasicTest("今早8点十五分", new DateObject(2016, 11, 7, 8, 15, 0), new DateObject(2016, 11, 7, 8, 15, 0));

            // test timex
            BasicTest("2010-01-29早上七点", "2010-01-29T07");
            BasicTest("2010.01.29晚上六点", "2010-01-29T18");
            BasicTest("2010/01/29中午十二点", "2010-01-29T12");
            BasicTest("2010 01 29五点", "2010-01-29T05");
            BasicTest("1987年1月11日八点", "1987-01-11T08");
            BasicTest("农历2015年十月初一早上九点二十", "2015-10-01T09:20");
            BasicTest("1月19号下午5:00", "XXXX-01-19T17:00");
            BasicTest("明天下午5:00", "2016-11-08T17:00");

            BasicTest("今晚6点", "2016-11-07T18");
            BasicTest("今晨5点", "2016-11-07T05");
            BasicTest("今早8点十五分", "2016-11-07T08:15");
        }
    }
}