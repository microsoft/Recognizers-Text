using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestDateTimeChsExtractor
    {
        private readonly DateTimeExtractorChs extractor = new DateTimeExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, results[0].Type);
        }

        [TestMethod]
        public void TestDateTimeExtractChs()
        {
            BasicTest("2010-01-29早上七点", 0, 14);
            BasicTest("2010.01.29晚上六点", 0, 14);
            BasicTest("2010/01/29中午十二点", 0, 15);
            BasicTest("2010 01 29五点", 0, 12);
            BasicTest("1987年1月11日八点", 0, 12);
            BasicTest("农历2015年十月初一早上九点二十", 0, 17);
            BasicTest("1月19号下午5:00", 0, 11);
            BasicTest("明天下午5:00", 0, 8);

            BasicTest("今晚6点", 0, 4);
            BasicTest("昨晚6点", 0, 4);
            BasicTest("今晨5点", 0, 4);
            BasicTest("今早8点十五分", 0, 7);
        }
    }
}