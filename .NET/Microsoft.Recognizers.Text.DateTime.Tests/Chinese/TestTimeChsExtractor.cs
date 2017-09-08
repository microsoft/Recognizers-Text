using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Tests
{
    [TestClass]
    public class TestTimeChsExtractor
    {
        private readonly TimeExtractorChs extractor = new TimeExtractorChs();

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, results[0].Type);
            TestWriter.Write("Chs", extractor, text, results[0]);
        }

        public void NullTest(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
            TestWriter.Write("Chs", extractor, text);
        }

        [TestMethod]
        public void TestTimeChs()
        {
            BasicTest("今天晚上9:30,微软大厦门口见", 2, 6);
            BasicTest("今天晚上19:30,微软大厦门口见", 2, 7);
            BasicTest("今天下午十一点半,微软大厦门口见", 2, 6);
            BasicTest("今天大约十点以后,微软大厦门口见", 2, 6);
            BasicTest("今天大约早上十点左右,微软大厦门口见", 2, 8);
            BasicTest("今天大约晚上十点以后,微软大厦门口见", 2, 8);
            BasicTest("今天早上11点,微软大厦门口见", 2, 5);
            BasicTest("今天晚2点半,微软大厦门口见", 2, 4);
            BasicTest("今天零点,微软大厦门口见", 2, 2);
            BasicTest("今天零点整,微软大厦门口见", 2, 3);
            BasicTest("今天零点一刻,微软大厦门口见", 2, 4);
            BasicTest("今天11点3刻,微软大厦门口见", 2, 5);
            NullTest("今天第1时");
        }
    }
}