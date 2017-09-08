using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateTimeExtractor
    {
        private readonly IExtractor extractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, results[0].Type);
            TestWriter.Write("Spa", extractor, text, results[0]);
        }

        [TestMethod]
        public void TestDateTimeExtract()
        {
            BasicTest("Voy a volver ahora", 13, 5);
            BasicTest("Voy a volver tan pronto como sea posible", 13, 27);
            BasicTest("Vamos a volver tan pronto como podamos", 15, 23);
            BasicTest("Voy a volver lo mas pronto posible", 13, 21);

            BasicTest("Voy a volver ahora mismo", 13, 11);
            BasicTest("Voy a volver justo ahora", 13, 11);
            BasicTest("Voy a volver en este momento", 13, 15);

            BasicTest("Voy a volver el 15 a las 8:00", 16, 13);
            BasicTest("Voy a volver el 15 a las 8:00:30", 16, 16);
            BasicTest("Voy a volver el 15, 8pm", 16, 7);
            BasicTest("Voy a volver el 04/21/2016, 8:00pm", 16, 18);
            BasicTest("Voy a volver el 04/21/2016, 8:00:13pm", 16, 21);
            BasicTest("Voy a volver el 23 de Oct a las siete", 16, 21);
            BasicTest("Voy a volver el 14 de Octubre 8:00am", 16, 20);
            BasicTest("Voy a volver el 14 de Octubre a las 8:00:00am", 16, 29);
            BasicTest("Voy a volver el 14 de Octubre, 8:00am", 16, 21);
            BasicTest("Voy a volver el 14 de Octubre, 8:00:01am", 16, 24);
            BasicTest("Voy a volver mañana 8:00am", 13, 13);
            BasicTest("Voy a volver mañana cerca de las 8:00am", 13, 26);
            BasicTest("Voy a volver mañana para las 8:00am", 13, 22);
            BasicTest("Voy a volver mañana a las 8:00:05am", 13, 22);
            BasicTest("Voy a volver el próximo viernes a las tres y media", 16, 34);
            BasicTest("Voy a volver el 5 de Mayo, 2016, 20 minutos pasados de las 8 de la tarde", 16, 56);

            BasicTest("Voy a volver 8pm del 15", 13, 10);
            BasicTest("Voy a volver a las siete del 15", 19, 12);
            BasicTest("Voy a volver 8pm del próximo domingo", 13, 23);
            BasicTest("Voy a volver a las 8pm de hoy", 19, 10);
            BasicTest("Voy a volver a las siete menos cuarto de mañana", 19, 28);
            BasicTest("Voy a volver 19:00, 2016-12-22", 13, 17);
            BasicTest("Voy a volver a las 7 en punto mañana", 19, 17);

            BasicTest("Voy a volver mañana por la mañana a las 7", 13, 28);
            BasicTest("Voy a volver 7:00 el domingo a la tarde", 13, 26);
            BasicTest("Voy a volver a las cinco y veinte mañana por la mañana", 19, 35);
            BasicTest("Voy a volver 14 de octubre 8:00, 14 de Octubre", 13, 18);
            BasicTest("Voy a volver a las 7, esta mañana", 19, 14);
            BasicTest("Voy a volver esta noche a las 8", 13, 18);

            BasicTest("Voy a volver a las 8pm de la tarde, Lunes", 19, 22);
            BasicTest("Voy a volver 8pm de la tarde, 1ro de Enero", 13, 29);
            BasicTest("Voy a volver 8pm de la tarde, 1 de Enero", 13, 27);
            BasicTest("Voy a volver a las 10pm de esta noche", 19, 18);
            BasicTest("Voy a volver a las 10pm esta noche", 19, 15);
            BasicTest("Voy a volver 8am de esta mañana", 13, 18);
            BasicTest("Voy a volver 8pm de esta tarde", 13, 17);

            BasicTest("Volví esta mañana a las 7", 6, 19);
            BasicTest("Volví esta mañana 7pm", 6, 15);
            BasicTest("Volví esta mañana a las siete", 6, 23);
            BasicTest("Volví esta mañana a las 7:00", 6, 22);
            BasicTest("Voy a volver esta noche a las 7", 13, 18);
            BasicTest("Volví esta noche a las 7", 6, 18);
            BasicTest("para dos personas esta noche a las 9:30 pm", 18, 24);
            BasicTest("para dos personas esta noche a las 9:30:31 pm", 18, 27);

            BasicTest("Volveré al final del día", 8, 16);
            BasicTest("Volveré al finalizar el día", 8, 19);
            BasicTest("Volveré al finalizar el día de mañana", 8, 29);
            BasicTest("Volveré mañana al finalizar el día", 8, 26);
            BasicTest("Volveré al finalizar el domingo", 8, 23);

            BasicTest("Voy a volver el 5 a las 4 a.m.", 16, 14);

            BasicTest("Voy a volver 2016-12-16T12:23:59", 13, 19);
        }
    }
}