using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsExtractor
    /// </summary>
    [TestClass]
    public class TestDateChsExtractor
    {
        private readonly DateExtractorChs extractor = new DateExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
        }

        [TestMethod]
        public void TestDateChs_Extract()
        {
            BasicTest("2010-01-29", 0, 10);
            BasicTest("2010.01.29", 0, 10);
            BasicTest("2010/01/29", 0, 10);
            BasicTest("2010 01 29", 0, 10);
            BasicTest("1987年1月11日", 0, 10);
            BasicTest("农历2015年十月初一", 0, 11);
            BasicTest("2015年农历正月初一是春节", 0, 11);
            BasicTest("我们定在三月初一", 4, 4);
            BasicTest("微软在正月三十有活动", 3, 4);
            BasicTest("大年初一", 0, 4);
            BasicTest("大年三十", 0, 4);
            BasicTest("快到1月19日了", 2, 5);
            BasicTest("1月19号", 0, 5);
            BasicTest("10月12号，星期一", 0, 10);
            BasicTest("2009年10月12号，星期一", 0, 15);
            BasicTest("明天可以吗", 0, 2);
            BasicTest("最近还好吗", 0, 2);
            BasicTest("星期一", 0, 3);
            BasicTest("上周一有考试", 0, 3);
            BasicTest("下次的12号", 3, 3);
            BasicTest("会议在这个星期一", 3, 5);
            BasicTest("两千零四年八月十五", 0, 9);
            BasicTest("两千年八月十五", 0, 7);
            BasicTest("二零零四年八月十五", 0, 9);
            BasicTest("去年本月十日", 0, 6);
            BasicTest("本月十日", 0, 4);
        }
    }
}