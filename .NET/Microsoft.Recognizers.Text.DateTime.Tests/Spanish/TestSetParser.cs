using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestSetParser
    {
        readonly BaseSetExtractor extractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseSetParser(new SpanishSetParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, string value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], System.DateTime.Now);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(luisValue, pr.TimexStr);
        }

        [TestMethod]
        public void TestSetParse()
        {
            BasicTest("Saldré semanalmente", "Set: P1W", "P1W");
            BasicTest("Saldré quincenalmente", "Set: P2W", "P2W");
            BasicTest("Saldré diariamente", "Set: P1D", "P1D");
            BasicTest("Saldré a diario", "Set: P1D", "P1D");
            BasicTest("Saldré todos los dias", "Set: P1D", "P1D");
            BasicTest("Saldré cada mes", "Set: P1M", "P1M");
            BasicTest("Saldré todos los meses", "Set: P1M", "P1M");
            BasicTest("Saldré todas las semanas", "Set: P1W", "P1W");
            BasicTest("Saldré anualmente", "Set: P1Y", "P1Y");
            BasicTest("Saldré todos los años", "Set: P1Y", "P1Y");

            BasicTest("Me iré cada dos dias", "Set: P2D", "P2D");
            BasicTest("Me iré cada tres semanas", "Set: P3W", "P3W");
            BasicTest("Me iré cada 3 semanas", "Set: P3W", "P3W");

            BasicTest("Me iré a las 3pm todos los dias", "Set: T15", "T15");
            BasicTest("Me iré todos los dias a las 3pm", "Set: T15", "T15");

            BasicTest("Saldré cada 15/4", "Set: XXXX-04-15", "XXXX-04-15");
            BasicTest("Saldré todos los lunes", "Set: XXXX-WXX-1", "XXXX-WXX-1");
            BasicTest("Saldré cada lunes a las 4pm", "Set: XXXX-WXX-1T16", "XXXX-WXX-1T16");
        }
    }
}