using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestSetExtractor
    {
        private readonly IExtractor extractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, results[0].Type);
        }

        [TestMethod]
        public void TestSetExtract()
        {
            BasicTest("saldré semanalmente", 7, 12);
            BasicTest("saltré diariamente", 7, 11);
            BasicTest("saltré a diario", 7, 8);
            BasicTest("saldré todos los dias", 7, 14);
            BasicTest("saldré cada mes", 7, 8);
            BasicTest("saldré todos los meses", 7, 15);
            BasicTest("saldré todos las semanas", 7, 17);
            BasicTest("saldré mensualmente", 7, 12);
            BasicTest("saldré anualmente", 7, 10);
            BasicTest("saldré todos los años", 7, 14);

            BasicTest("me iré cada dos dias", 7, 13);
            BasicTest("me iré cada tres semanas", 7, 17);
            BasicTest("me iré cada 3 semanas", 7, 14);

            BasicTest("me iré a las 3pm todos los dias", 13, 18);
            BasicTest("me iré todos los dias a las 3pm", 7, 24);

            BasicTest("saldré cada 15/4", 7, 9);
            BasicTest("saldré todos los lunes", 7, 15);
            BasicTest("saldré cada lunes a las 4pm", 7, 20);
        }
    }
}