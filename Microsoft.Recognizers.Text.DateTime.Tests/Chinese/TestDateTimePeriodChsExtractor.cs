using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDateTimePeriodChsExtractor
    {
        private readonly DateTimePeriodExtractorChs extractor = new DateTimePeriodExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestDateTimePeriodExtactChs()
        {
            BasicTest("明天2点到4点", 0, 7);
            BasicTest("从昨天下午两点到四点", 0, 10);
            BasicTest("从昨天下午两点到明天四点", 0, 12);
            BasicTest("从昨天5:00-6:00", 0, 12);
            BasicTest("1月15号4点和2月3号9点之间", 0, 16);
            BasicTest("2点-明天4点", 0, 7);

            BasicTest("昨晚", 0, 2);
            BasicTest("昨天晚上", 0, 4);
            BasicTest("明天上午", 0, 4);

            BasicTest("上个小时", 0, 4);
            BasicTest("之后5分钟", 0, 5);
            BasicTest("之前3小时", 0, 5);
        }
    }
}