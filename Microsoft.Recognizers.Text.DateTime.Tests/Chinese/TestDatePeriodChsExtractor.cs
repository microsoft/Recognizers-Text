using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDatePeriodChsExtractor
    /// </summary>
    [TestClass]
    public class TestDatePeriodChsExtractor
    {
        private readonly DatePeriodExtractorChs extractor = new DatePeriodExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestDatePeriodChs_Extract()
        {
            BasicTest("时间从一月十日到十二日", 2, 9);
            BasicTest("时间从一月19到20日", 2, 9);
            BasicTest("从一月十日到20日", 0, 9);
            BasicTest("明年四月", 0, 4);
            BasicTest("我们去年5月见过", 2, 4);
            BasicTest("下周末", 0, 3);
            BasicTest("会议在下周", 3, 2);
            BasicTest("下个月完工", 0, 3);
            BasicTest("下周如何", 0, 2);
            BasicTest("明年", 0, 2);
            BasicTest("奥运会在2008年", 4, 5);
            BasicTest("十月的第一周是国庆节", 0, 6);
            BasicTest("三月二十八日到四月15日", 0, 12);
            BasicTest("前1周", 0, 3);
            BasicTest("上1月", 0, 3);
            BasicTest("下1年", 0, 3);
            BasicTest("下1天", 0, 3);
            BasicTest("今年夏天", 0, 4);
            BasicTest("今年第一季度", 0, 6);
        }
    }
}