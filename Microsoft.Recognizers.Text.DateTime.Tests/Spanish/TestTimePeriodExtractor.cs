using Microsoft.Recognizers.Text.DateTime.Extractors;
using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestTimePeriodExtractor
    {
        private readonly IExtractor extractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestTimePeriodExtract()
        {
            // basic match
            BasicTest("Estaré afuera de 5 a 6pm", 14, 10);
            BasicTest("Estaré afuera de las 5 a las 6pm", 14, 18);
            BasicTest("Estaré afuera desde las 5 a las 6pm", 14, 21);
            BasicTest("Estaré afuera desde las 5 hasta las 6pm", 14, 25);
            BasicTest("Estaré afuera de 5 a 6p.m.", 14, 12);
            BasicTest("Estaré afuera de 5 a 6 de la tarde", 14, 20);
            BasicTest("Estaré afuera desde las 5 hasta las 6p.m.", 14, 27);
            BasicTest("Estaré afuera entre las 5 y las 6p.m.", 14, 23);
            BasicTest("Estaré afuera entre las 5 y 6p.m.", 14, 19);
            BasicTest("Estaré afuera entre las 5 y las 6p.m.", 14, 23);
            BasicTest("Estaré afuera entre las 5 y las 6 de la mañana", 14, 32);
            BasicTest("Estaré afuera entre las 5 y las seis de la madrugada", 14, 38);

            // merge to time points
            BasicTest("Estaré fuera desde las 4pm hasta 5pm", 13, 23);
            BasicTest("Estaré fuera desde las 4:00 hasta 5pm", 13, 24);
            BasicTest("Estaré fuera desde las 4:00 hasta las 7 en punto", 13, 35);
            BasicTest("Estaré fuera de 3pm a siete y media", 13, 22);
            BasicTest("Estaré fuera 4pm-5pm", 13, 7);
            BasicTest("Estaré fuera 4pm - 5pm", 13, 9);
            BasicTest("Estaré fuera de tres menos veinte a cinco de la tarde", 13, 40);

            BasicTest("Estaré fuera de 4pm a 5pm", 13, 12);
            BasicTest("Estaré fuera de 4pm a cinco y media", 13, 22);
            BasicTest("Estaré fuera de 4pm a cinco treinta", 13, 22);
            BasicTest("Estaré fuera de 3 de la mañana hasta las 5pm", 13, 31);
            BasicTest("Estaré fuera desde las 3 de la madrugada hasta las cinco de la tarde", 13, 55);

            BasicTest("Estaré fuera entre las 4pm y las cinco y media", 13, 33);
            BasicTest("Estaré fuera entre las 3 de la mañana y las 5pm", 13, 34);

            BasicTest("Nos vemos en la mañana", 16, 6);
            BasicTest("Nos vemos por la tarde", 17, 5);
            BasicTest("Te veo a la noche", 12, 5);
            BasicTest("Nos vemos en la madrugada", 16, 9);
            BasicTest("Nos vemos de madrugada", 13, 9);
        }
    }
}