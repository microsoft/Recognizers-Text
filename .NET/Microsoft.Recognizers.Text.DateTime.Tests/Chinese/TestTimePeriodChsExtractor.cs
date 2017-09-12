using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestTimePeriodChsExtractor
    {
        private readonly TimePeriodExtractorChs extractor = new TimePeriodExtractorChs();

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Chs", typeof(TimePeriodExtractorChs));
        }

        public void BasicTest(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(results[0].Text, text);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, results[0].Type);
            TestWriter.Write("Chs", extractor, text, results);
        }

        [TestMethod]
        public void TestTimeChs()
        {
            BasicTest("从晚上9:30到凌晨3:00");
            BasicTest("下午5点到6点");
            BasicTest("下午五点到六点");
            BasicTest("下午五点半到六点半");
            BasicTest("清晨四点到六点之间");
            BasicTest("4:00到6:00");
            BasicTest("午夜12点到凌晨2点");
            BasicTest("下午四点到晚上八点");
            BasicTest("凌晨四到六点");
            BasicTest("下午2点~4点");
            BasicTest("上午8点45分至上午9点30分");
            BasicTest("深夜20点～凌晨4点");
        }
    }
}