using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateTimePeriodExtractor
    {
        private readonly IExtractor extractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, results[0].Type);
        }

        [TestMethod]
        public void TestDateTimePeriodExtract()
        {
            // basic match
            BasicTest("Estaré fuera de cinco a siete hoy", 13, 20);
            BasicTest("Hoy estaré fuera de cinco a siete", 0, 33);
            BasicTest("Estaré fuera hoy de cinco a siete", 13, 20);
            BasicTest("Estaré fuera de cinco a siete mañana", 13, 23);
            BasicTest("Estaré fuera desde las 5 hasta las 6 el próximo domingo", 13, 42);
            BasicTest("Estaré fuera desde las 5 hasta las 6pm el próximo domingo", 13, 44);
            BasicTest("Estaré fuera desde las 5 hasta las 6pm del próximo domingo", 13, 45);

            BasicTest("Estaré afuera de 4pm a 5pm hoy", 14, 16);
            BasicTest("Estaré afuera de 4pm a 5pm de hoy", 14, 19);
            BasicTest("Estaré afuera de 4pm de hoy a 5pm de mañana", 14, 29);
            BasicTest("Estaré afuera de 4pm a 5pm de mañana", 14, 22);
            BasicTest("Estaré afuera de 4pm a 5pm del 2017-6-6", 14, 25);
            BasicTest("Estaré afuera de 4pm a 5pm el 5 de Mayo del 2018", 14, 34);
            BasicTest("Estaré afuera de 4:00 a 5pm 5 de Mayo, 2018", 14, 29);
            BasicTest("Estaré afuera de 4pm del 1 de Enero del 2016 a 5pm de hoy", 14, 43);
            BasicTest("Estaré afuera de 2:00pm, 2016-2-21 a 3:32, 04/23/2016", 14, 39);
            BasicTest("Estaré afuera desde hoy a las 4 hasta el miercoles siguiente a las 5", 14, 54);

            BasicTest("Estaré afuera entre las 4pm y 5pm de hoy", 14, 26);
            BasicTest("Estaré afuera entre las 4pm del 1 de enero del 2016 y las 5pm de hoy", 14, 54);

            BasicTest("Regresaré a la noche", 10, 10);
            BasicTest("Regresaré a la madrugada", 10, 14);
            BasicTest("Regresaré esta tarde", 10, 10);
            BasicTest("Regresaré esta mañana", 10, 11);
            BasicTest("Regresaré en la mañana", 12, 10);
            BasicTest("Regresaré a la mañana", 10, 11);
            BasicTest("Regresaré esta tarde", 10, 10);
            BasicTest("Regresaré la próxima noche", 13, 13);
            BasicTest("Regresé anoche", 8, 6);
            BasicTest("Regresaré mañana por la noche", 10, 19);
            BasicTest("Regresaré el próximo lunes a la tarde", 13, 24);
            BasicTest("Regresaré el 5 de mayo en la noche", 13, 21);

            BasicTest("Voy a retroceder los últimos 3 minutos", 21, 17);
            BasicTest("Voy a retroceder los pasados 3 minutos", 21, 17);
            BasicTest("Voy a retroceder los previos 3 minutos", 21, 17);
            BasicTest("Voy a retroceder los anteriores 3mins", 21, 16);
            BasicTest("Voy a volver en 3 horas", 13, 10);
            BasicTest("Voy a volver en 5 h", 13, 6);
            BasicTest("Voy a volver dentro de 5 horas", 13, 17);
            BasicTest("Voy a volver en el último minuto", 19, 13);
            BasicTest("Voy a volver en la próxima hora", 19, 12);
        }
    }
}