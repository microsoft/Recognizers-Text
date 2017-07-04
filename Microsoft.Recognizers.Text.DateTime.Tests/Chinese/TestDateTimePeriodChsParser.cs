using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDateTimePeriodChsParser
    {
        readonly DateTimePeriodExtractorChs extractor;
        readonly DateTimePeriodParserChs parser;

        readonly DateObject referenceTime;

        public TestDateTimePeriodChsParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 16, 12, 0);
            extractor = new DateTimePeriodExtractorChs();
            parser = new DateTimePeriodParserChs(new ChineseDateTimeParserConfiguration());
        }

        public void BasicTestFuture(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestDateTimePeriodParseChs()
        {
            BasicTestFuture("从昨天下午两点到四点", new DateObject(2016, 11, 6, 14, 0, 0), new DateObject(2016, 11, 6, 16, 0, 0));
            BasicTestFuture("从昨天下午两点到明天四点", new DateObject(2016, 11, 6, 14, 0, 0), new DateObject(2016, 11, 8, 4, 0, 0));
            BasicTestFuture("从昨天5:00-6:00", new DateObject(2016, 11, 6, 5, 0, 0), new DateObject(2016, 11, 6, 6, 0, 0));
            BasicTestFuture("1月15号4点和2月3号9点之间", new DateObject(2017, 1, 15, 4, 0, 0),
                new DateObject(2017, 2, 3, 9, 0, 0));
            BasicTestFuture("2点-明天4点", new DateObject(2016, 11, 8, 2, 0, 0), new DateObject(2016, 11, 8, 4, 0, 0));

            BasicTestFuture("昨晚", new DateObject(2016, 11, 6, 16, 0, 0), new DateObject(2016, 11, 6, 20, 0, 0));
            BasicTestFuture("昨天晚上", new DateObject(2016, 11, 6, 16, 0, 0), new DateObject(2016, 11, 6, 20, 0, 0));
            BasicTestFuture("明天上午", new DateObject(2016, 11, 8, 8, 0, 0), new DateObject(2016, 11, 8, 12, 0, 0));

            BasicTestFuture("上个小时", new DateObject(2016, 11, 7, 15, 12, 0), new DateObject(2016, 11, 7, 16, 12, 0));
            BasicTestFuture("之后5分钟", new DateObject(2016, 11, 7, 16, 12, 0), new DateObject(2016, 11, 7, 16, 17, 0));
            BasicTestFuture("之前3小时", new DateObject(2016, 11, 7, 13, 12, 0), new DateObject(2016, 11, 7, 16, 12, 0));
        }
    }
}