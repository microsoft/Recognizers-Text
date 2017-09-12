using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestTimePeriodChsParser
    {
        readonly DateObject refTime;
        private readonly TimePeriodExtractorChs extractor = new TimePeriodExtractorChs();
        private readonly TimePeriodParserChs parser = new TimePeriodParserChs(new ChineseDateTimeParserConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Chs", typeof(TimePeriodParserChs));
        }

        public TestTimePeriodChsParser()
        {
            refTime = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Chs", parser, text, pr);
        }


        [TestMethod]
        public void TestTimeParser()
        {
            BasicTest("从五点半到六点", "(T05:30,T06,PT0H30M)");
            BasicTest("从下午五点一刻到六点", "(T17:15,T18,PT0H45M)");
            BasicTest("17:55:23-18:33:02", "(T17:55:23,T18:33:02,PT0H37M39S)");
            BasicTest("从17点55分23秒至18点33分02秒", "(T17:55:23,T18:33:02,PT0H37M39S)");
            BasicTest("早上五到六点", "(T05,T06,PT1H)");
            BasicTest("下午五点到晚上七点半", "(T17,T19:30,PT2H30M)");
            BasicTest("下午5:00到凌晨3:00", "(T17:00,T03:00,PT10H)");
            BasicTest("下午5:00到6:00", "(T17:00,T18:00,PT1H)");
            BasicTest("5:00到6:00", "(T05:00,T06:00,PT1H)");
        }
    }
}