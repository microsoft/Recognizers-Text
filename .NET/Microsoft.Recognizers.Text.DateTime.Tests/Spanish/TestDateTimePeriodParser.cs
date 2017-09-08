using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateTimePeriodParser
    {
        readonly BaseDateTimePeriodExtractor extractor;
        readonly IDateTimeParser parser;

        readonly DateObject referenceTime;

        public TestDateTimePeriodParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 16, 12, 0);
            extractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());
            parser = new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));
        }

        public void BasicTestFuture(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item2);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Spa", parser, text, pr);
        }

        [TestMethod]
        public void TestDateTimePeriodParse()
        {
            int year = 2016, month = 11, day = 7, min = 0, second = 0;

            // basic match
            BasicTestFuture("Estaré fuera de cinco a siete hoy",
                new DateObject(year, month, day, 5, min, second),
                new DateObject(year, month, day, 7, min, second));
            BasicTestFuture("Estaré fuera de 5 a 6 del 4/22/2016",
                new DateObject(2016, 4, 22, 5, min, second),
                new DateObject(2016, 4, 22, 6, min, second));
            BasicTestFuture("Estaré fuera de 5 a 6 del 22 de Abril",
                new DateObject(year + 1, 4, 22, 5, min, second),
                new DateObject(year + 1, 4, 22, 6, min, second));
            BasicTestFuture("Estaré fuera de 5 a 6pm del 22 de Abril",
                new DateObject(year + 1, 4, 22, 17, min, second),
                new DateObject(year + 1, 4, 22, 18, min, second));
            BasicTestFuture("Estaré fuera de 5 a 6 del 1ro de Ene",
                new DateObject(year + 1, 1, 1, 5, min, second),
                new DateObject(year + 1, 1, 1, 6, min, second));


            // merge two time points
            BasicTestFuture("Estaré afuera de 3pm a 4pm mañana",
                new DateObject(year, month, 8, 15, min, second),
                new DateObject(year, month, 8, 16, min, second));

            BasicTestFuture("Estaré afuera de 3:00 a 4:00 de mañana",
                new DateObject(year, month, 8, 3, min, second),
                new DateObject(year, month, 8, 4, min, second));

            BasicTestFuture("Estaré afuera de siete y media a 4pm mañana",
                new DateObject(year, month, 8, 7, 30, second),
                new DateObject(year, month, 8, 16, min, second));

            BasicTestFuture("Estaré afuera desde las 4pm de hoy hasta las 5pm de mañana",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, 8, 17, min, second));

            BasicTestFuture("Estaré afuera de 2:00pm, 2016-2-21 a 3:32, 04/23/2016",
                new DateObject(2016, 2, 21, 14, min, second),
                new DateObject(2016, 4, 23, 3, 32, second));
            BasicTestFuture("Estaré afuera entre las 4pm a 5pm hoy",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTestFuture("Estaré afuera entre las 4pm del 1 Ene, 2016 y las 5pm de hoy",
                new DateObject(2016, 1, 1, 16, min, second),
                new DateObject(year, month, day, 17, min, second));


            BasicTestFuture("Regresaré a la noche",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));
            BasicTestFuture("Regresaré esta noche",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));
            BasicTestFuture("Regresaré esta tarde",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));
            BasicTestFuture("Regresé esta mañana",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 12, min, second));
            BasicTestFuture("Regresaré pasado el mediodia de hoy",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));
            BasicTestFuture("Regresaré mañana a la noche",
                new DateObject(year, month, day + 1, 20, min, second),
                new DateObject(year, month, day + 1, 23, 59, 59));
            BasicTestFuture("Regresé anoche",
                new DateObject(year, month, day - 1, 20, min, second),
                new DateObject(year, month, day - 1, 23, 59, 59));
            BasicTestFuture("Regresaré mañana por la noche",
                new DateObject(year, month, day + 1, 20, min, second),
                new DateObject(year, month, day + 1, 23, 59, 59));
            BasicTestFuture("Regresaré el próximo lunes a la tarde",
                new DateObject(year, month, 14, 16, min, second),
                new DateObject(year, month, 14, 20, min, second));

            BasicTestFuture("Voy a retroceder los últimos 3 minutos",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));
            BasicTestFuture("Voy a retroceder los pasados 3 minutos",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));
            BasicTestFuture("Voy a retroceder los previos 3 minutos",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));
            BasicTestFuture("Voy a retroceder los anteriores 3mins",
                new DateObject(year, month, day, 16, 9, second),
                new DateObject(year, month, day, 16, 12, second));
            BasicTestFuture("Voy a volver en 3 horas",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 19, 12, second));
            BasicTestFuture("Voy a volver en 5 h",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 21, 12, second));
            BasicTestFuture("Voy a volver dentro de 5 horas",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 21, 12, second));
            BasicTestFuture("Voy a volver en el último minuto",
                new DateObject(year, month, day, 16, 11, second),
                new DateObject(year, month, day, 16, 12, second));
            BasicTestFuture("Voy a volver en la próxima hora",
                new DateObject(year, month, day, 16, 12, second),
                new DateObject(year, month, day, 17, 12, second));
        }

        [TestMethod]
        public void TestDateTimePeriodParseLuis()
        {
            // basic match
            BasicTest("Estaré fuera de cinco a siete hoy", "(2016-11-07T05,2016-11-07T07,PT2H)");
            BasicTest("Estaré fuera de 5 a 6 del 4/22/2016", "(2016-04-22T05,2016-04-22T06,PT1H)");
            BasicTest("Estaré fuera de 5 a 6 del 22 de Abril", "(XXXX-04-22T05,XXXX-04-22T06,PT1H)");
            BasicTest("Estaré fuera de 5 a 6 del 1ro de Ene", "(XXXX-01-01T05,XXXX-01-01T06,PT1H)");


            // merge two time points
            BasicTest("Estaré afuera de 3pm a 4pm mañana", "(2016-11-08T15,2016-11-08T16,PT1H)");
            BasicTest("Estaré afuera de 3:00 a 4:00 mañana", "(2016-11-08T03:00,2016-11-08T04:00,PT1H)");
            BasicTest("Estaré afuera de 2:00pm, 2016-2-21 a 3:32, 04/23/2016", "(2016-02-21T14:00,2016-04-23T03:32,PT1478H)");

            BasicTest("Regresaré a la noche", "2016-11-07TNI");
            BasicTest("Regresaré esta noche", "2016-11-07TNI");
            BasicTest("Regresaré esta tarde", "2016-11-07TEV");
            BasicTest("Regresé esta mañana", "2016-11-07TMO");
            BasicTest("Regresaré pasado el mediodia de hoy", "2016-11-07TAF");
            BasicTest("Regresaré mañana a la mañana", "2016-11-08TMO");
            BasicTest("Regresaré mañana a la noche", "2016-11-08TNI");
            BasicTest("Regresé anoche", "2016-11-06TNI");
            BasicTest("Regresaré mañana por la noche", "2016-11-08TNI");
            BasicTest("Regresaré el próximo lunes a la tarde", "2016-11-14TEV");

            BasicTest("Voy a retroceder los últimos 3 minutos", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Voy a retroceder los pasados 3 minutos", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Voy a retroceder los previos 3 minutos", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Voy a retroceder los anteriores 3mins", "(2016-11-07T16:09:00,2016-11-07T16:12:00,PT3M)");
            BasicTest("Voy a volver en 3 horas", "(2016-11-07T16:12:00,2016-11-07T19:12:00,PT3H)");
            BasicTest("Voy a volver en 5 h", "(2016-11-07T16:12:00,2016-11-07T21:12:00,PT5H)");
            BasicTest("Voy a volver dentro de 5 horas", "(2016-11-07T16:12:00,2016-11-07T21:12:00,PT5H)");
            BasicTest("Voy a volver en el último minuto", "(2016-11-07T16:11:00,2016-11-07T16:12:00,PT1M)");
            BasicTest("Voy a volver en la próxima hora", "(2016-11-07T16:12:00,2016-11-07T17:12:00,PT1H)");
        }
    }
}