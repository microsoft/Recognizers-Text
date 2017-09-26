using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    /// <summary>
    /// Summary description for TestDateChsExtractor
    /// </summary>
    [TestClass]
    public class TestHolidayChsExtractor
    {
        private readonly BaseHolidayExtractor extractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
            TestWriter.Write(TestCulture.Chinese, extractor, text, results);
        }
        
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.Chinese, typeof(BaseHolidayExtractor));
        }

        [TestMethod]
        public void TestHolidayChs_Extract()
        {
            BasicTest("明天元旦去哪里", 2, 2);
            BasicTest("明天元旦节去哪里", 2, 3);
            BasicTest("明天教师节去哪里", 2, 3);
            BasicTest("明天青年节去哪里", 2, 3);
            BasicTest("明天儿童节去哪里", 2, 3);
            BasicTest("明天妇女节去哪里", 2, 3);
            BasicTest("明天植树节去哪里", 2, 3);
            BasicTest("明天情人节去哪里", 2, 3);
            BasicTest("明天圣诞节去哪里", 2, 3);
            BasicTest("明天新年去哪里", 2, 2);
            BasicTest("明天愚人节去哪里", 2, 3);
            BasicTest("明天五一去哪里", 2, 2);
            BasicTest("明天劳动节去哪里", 2, 3);
            BasicTest("明天万圣节去哪里", 2, 3);
            BasicTest("明天中秋节去哪里", 2, 3);
            BasicTest("明天中秋去哪里", 2, 2);
            BasicTest("明天春节去哪里", 2, 2);
            BasicTest("明天除夕去哪里", 2, 2);
            BasicTest("明天元宵节去哪里", 2, 3);
            BasicTest("明天清明节去哪里", 2, 3);
            BasicTest("明天清明去哪里", 2, 2);
            BasicTest("明天端午节去哪里", 2, 3);
            BasicTest("明天端午去哪里", 2, 2);
            BasicTest("明天国庆节去哪里", 2, 3);
            BasicTest("明天建军节去哪里", 2, 3);
            BasicTest("明天女生节去哪里", 2, 3);
            BasicTest("明天光棍节去哪里", 2, 3);
            BasicTest("明天双十一去哪里", 2, 3);
            BasicTest("明天重阳节去哪里", 2, 3);
            BasicTest("明天父亲节去哪里", 2, 3);
            BasicTest("明天母亲节去哪里", 2, 3);
            BasicTest("明天感恩节去哪里", 2, 3);
        }
    }
}