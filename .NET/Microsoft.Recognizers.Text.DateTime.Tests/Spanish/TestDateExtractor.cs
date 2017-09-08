using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateExtractor
    {
        private readonly BaseDateExtractor extractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
            TestWriter.Write("Spa", extractor, text, results[0]);
        }

        [TestMethod]
        public void TestDateExtract()
        {
            BasicTest("Volvere el 15", 11, 2);
            BasicTest("Volvere el 22 de Abril", 11, 11);
            BasicTest("Volvere el 1-Ene", 11, 5);
            BasicTest("Volvere el 1/Ene", 11, 5);
            BasicTest("Volvere el 2. Octubre", 11, 10);
            BasicTest("Volvere el 12 de enero, 2016", 11, 17);
            BasicTest("Volvere el 12 de Enero de 2016", 11, 19);
            BasicTest("Volvere el Lunes 12 de enero, 2016", 11, 23);
            BasicTest("Volvere el 02/22/2016", 11, 10);
            BasicTest("Volvere el 21/04/2016", 11, 10);
            BasicTest("Volvere el 21/04/16", 11, 8);
            BasicTest("Volvere el 9-18-15", 11, 7);
            BasicTest("Volvere el 4.22", 11, 4);
            BasicTest("Volvere el 4-22", 11, 4);
            BasicTest("Volvere el 4.22", 11, 4);
            BasicTest("Volvere el 4-22", 11, 4);

            BasicTest("Volvere el    4/22", 14, 4);
            BasicTest("Volvere el 22/04", 11, 5);
            BasicTest("Volvere       4/22", 14, 4);
            BasicTest("Volvere 22/04", 8, 5);
            BasicTest("Volvere el 2015/08/12", 11, 10);
            BasicTest("Volvere el 11/12,2016", 11, 10);
            BasicTest("Volvere el 11/12,16", 11, 8);
            BasicTest("Volvere el 1ro de Ene", 11, 10);
            BasicTest("Volvere el 1-Ene", 11, 5);
            BasicTest("Volvere el 28-Nov", 11, 6);
            BasicTest("Volvere el Mi, 22 de Ene", 11, 13);

            BasicTest("Volvere el primero de Ene", 11, 14);
            BasicTest("Volvere el veintiuno de Mayo", 11, 17);
            BasicTest("Volvere en Mayo veintiuno", 11, 14);
            BasicTest("Volvere el segundo de Ago", 11, 14);
            BasicTest("Volvere el vigesimo segundo de Junio", 11, 25);

            BasicTest("Volvere el viernes", 11, 7);
            BasicTest("Volvere viernes", 8, 7);
            BasicTest("Volvere los viernes", 12, 7);
            BasicTest("Volvere los sabados", 12, 7);
            BasicTest("Volvere hoy", 8, 3);
            BasicTest("Volvere mañana", 8, 6);
            BasicTest("Volvi ayer", 6, 4);
            BasicTest("Volvi el dia antes de ayer", 6, 20);
            BasicTest("Volvi anteayer", 6, 8);
            BasicTest("Volvere el dia despues de mañana", 8, 24);
            BasicTest("Volvere pasado mañana", 8, 13);
            BasicTest("Volvere el dia siguiente", 8, 16);
            BasicTest("Volvere el proximo dia", 8, 14);
            BasicTest("Volvere el dia siguiente", 8, 16);
            BasicTest("Volvere este viernes", 8, 12);
            BasicTest("Volvere el proximo domingo", 11, 15);
            BasicTest("Volvere el siguiente domingo", 11, 17);
            BasicTest("Volvere el ultimo domingo", 11, 14);
            BasicTest("Volvere ultimo dia", 8, 10);
            BasicTest("Volvere el ultimo dia", 8, 13);
            BasicTest("Volvere el dia", 8, 6);

            BasicTest("Volvere el viernes de esta semana", 11, 22);
            BasicTest("Volvere el domingo de la siguiente semana", 11, 30);
            BasicTest("Volvere el domingo de la ultima semana", 11, 27);

            BasicTest("Volvere el 15 Junio 2016", 11, 13);
            BasicTest("Volvere el undecimo de mayo", 11, 16);

            BasicTest("Volvere el primer viernes de julio", 8, 26);
            BasicTest("Volvere el primer viernes de este mes", 8, 29);
        }
    }
}
