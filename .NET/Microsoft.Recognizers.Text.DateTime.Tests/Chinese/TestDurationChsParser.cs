using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDurationChsParser
    {
        readonly DateObject referenceDate;
        private readonly DurationExtractorChs extractor = new DurationExtractorChs();
        private readonly DurationParserChs parser = new DurationParserChs(new ChineseDateTimeParserConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.Chinese, typeof(DurationParserChs));
        }

        public TestDurationChsParser()
        {
            referenceDate = new DateObject(2017, 3, 22);
        }

        public void BasicTest(string text, string timex)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDate);
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, pr.Type);
            Assert.AreEqual(timex, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write(TestCulture.Chinese, parser, referenceDate, text, pr);
        }


        [TestMethod]
        public void TestTimeParserChs()
        {
            BasicTest("两年", "P2Y");
            BasicTest("5分钟", "PT5M");
            BasicTest("3天", "P3D");
            BasicTest("15周", "P15W");
            BasicTest("三年半", "P3.5Y");
        }
    }
}