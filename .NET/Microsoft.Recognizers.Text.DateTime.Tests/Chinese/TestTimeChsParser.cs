using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestTimeChsParser
    {
        readonly DateObject refTime;
        private readonly TimeExtractorChs extractor = new TimeExtractorChs();
        private readonly TimeParserChs parser = new TimeParserChs(new ChineseDateTimeParserConfiguration());

        public TestTimeChsParser()
        {
            refTime = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Chs", parser, text, pr);
        }


        [TestMethod]
        public void TestTimeParser()
        {
            BasicTest("下午5:00", "T17:00");
            BasicTest("晚上9:30", "T21:30");
            BasicTest("晚上19:30", "T19:30");
            BasicTest("下午十一点半", "T23:30");
            BasicTest("大约十点以后", "T10");
            BasicTest("大约早上十点左右", "T10");
            BasicTest("大约晚上十点以后", "T22");
            BasicTest("早上11点", "T11");
            BasicTest("凌晨2点半", "T02:30");
            BasicTest("零点", "T00");
            BasicTest("零点整", "T00");
        }
    }
}