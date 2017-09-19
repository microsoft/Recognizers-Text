using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestMergedExtractor
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), DateTimeOptions.None);

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        [TestMethod]
        public void TestMergedExtract()
        {
            BasicTest("esto es 2 dias en p ", 8, 6);

            BasicTest("esto es antes de las 4pm", 8, 16);
            BasicTest("esto es antes de las 4pm mañana", 8, 23);
            BasicTest("esto es antes de mañana a las 4pm ", 8, 25);
        }
        
        [TestMethod]
        public void TestAfterBeforeSince()
        {
            BasicTest("despues del 7/2 ", 0, 15);
            BasicTest("desde el 7/2 ", 0, 12);
            BasicTest("antes del  7/2 ", 0, 14);
        }
    }
}