using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDurationExtractor
    {
        private readonly IExtractor extractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Spa", typeof(BaseDurationExtractor));
        }


        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, results[0].Type);
            TestWriter.Write("Spa", extractor, text, results);
        }

        [TestMethod]
        public void TestDurationExtract()
        {
            BasicTest("me voy por 3h", 11, 2);
            BasicTest("me voy por 3 dias", 11, 6);
            BasicTest("me voy por 3,5 años", 11, 8);

            BasicTest("me voy por 3 h", 11, 3);
            BasicTest("me voy por 3 horas", 11, 7);
            BasicTest("me voy 3 dias", 7, 6);
            BasicTest("me voy por 3 meses", 11, 7);
            BasicTest("me voy por 3 minutos", 11, 9);
            BasicTest("me voy por 3 min", 11, 5);
            BasicTest("me voy por 3,5 segundos ", 11, 12);
            BasicTest("me voy por 123,45 seg", 11, 10);
            BasicTest("me voy por dos semanas", 11, 11);
            BasicTest("me voy 20 minutos", 7, 10);
            BasicTest("me voy por veinticuatro horas", 11, 18);

            BasicTest("me voy todo el dia", 7, 11);
            BasicTest("me voy toda la semana", 7, 14);
            BasicTest("me voy por todo el mes", 11, 11);
            BasicTest("me voy por todo el año", 11, 11);

            BasicTest("me voy por una hora", 11, 8);
            BasicTest("me voy por un año", 11, 6);
        }
    }
}