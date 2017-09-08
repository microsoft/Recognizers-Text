using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateTimeParser
    {
        readonly BaseDateTimeExtractor extractor;
        readonly IDateTimeParser parser;
        readonly DateObject referenceTime;

        public TestDateTimeParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 0, 0, 0);
            extractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            parser = new BaseDateTimeParser(new SpanishDateTimeParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, DateObject futreTime, DateObject pastTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(futreTime, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(pastTime, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Spa", parser, text, pr);
        }

        [TestMethod]
        public void TestDateTimeParse()
        {
            int year = 2016, month = 11, day = 7, hour = 0, min = 0, second = 0;

            BasicTest("Voy a volver ahora",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver tan pronto como sea posible",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Vamos a volver tan pronto como podamos",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver lo mas pronto posible",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver ahora mismo",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver justo ahora",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver en este momento",
                new DateObject(year, month, day, hour, min, second));

            BasicTest("Voy a volver el 15 a las 8:00",
                new DateObject(year, month, 15, 8, 0, second),
                new DateObject(year, month - 1, 15, 8, 0, second));

            BasicTest("Voy a volver el 15 a las 8:00:20", 
                new DateObject(year, month, 15, 8, 0, 20),
                new DateObject(year, month - 1, 15, 8, 0, 20));

            BasicTest("Voy a volver el 15, 8pm",
                new DateObject(year, month, 15, 20, min, second),
                new DateObject(year, month - 1, 15, 20, min, second));

            BasicTest("Voy a volver el cinco de mayo a las 4 a.m.",
                new DateObject(year + 1, 5, 5, 4, min, second),
                new DateObject(year, 5, 5, 4, min, second));

            BasicTest("Voy a volver el 04/21/2016, 8:00pm", 
                new DateObject(2016, 4, 21, 20, 0, second));

            BasicTest("Voy a volver el 04/21/2016, 8:00:13pm",
                new DateObject(2016, 4, 21, 20, 0, 13));

            BasicTest("Voy a volver el 23 de Oct a las siete", 
                new DateObject(year + 1, 10, 23, 7, min, second),
                new DateObject(year, 10, 23, 7, min, second));

            BasicTest("Voy a volver el 14 de Octubre 8:00am", 
                new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));

            BasicTest("Voy a volver el 14 de Octubre 8:00:31am", 
                new DateObject(year + 1, 10, 14, 8, 0, 31),
                new DateObject(year, 10, 14, 8, 0, 31));

            BasicTest("Voy a volver el 14 de Octubre, cerca de las 8:00am",
                new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));

            BasicTest("Voy a volver el 14 de Octubre a las 8:00:31am", 
                new DateObject(year + 1, 10, 14, 8, 0, 31), 
                new DateObject(year, 10, 14, 8, 0, 31));

            BasicTest("Voy a volver el 14 de Octubre, 8:00am", 
                new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));

            BasicTest("Voy a volver el 14 de Octubre, 8:00:26am",
                new DateObject(year + 1, 10, 14, 8, 0, 26),
                new DateObject(year, 10, 14, 8, 0, 26));

            BasicTest("Voy a volver el 5 de Mayo, 2016, 20 minutos pasados de las 8 de la tarde",
                new DateObject(2016, 5, 5, 20, 20, second));

            BasicTest("Voy a volver 8pm del 15", 
                new DateObject(year, month, 15, 20, min, second),
                new DateObject(year, month - 1, 15, 20, min, second));

            BasicTest("Voy a volver a las siete del 15",
                new DateObject(year, month, 15, 7, min, second),
                new DateObject(year, month - 1, 15, 7, min, second));

            BasicTest("Voy a volver a las 8pm de hoy",
                new DateObject(year, month, day, 20, min, second));

            BasicTest("Voy a volver a las siete menos cuarto de mañana",
                new DateObject(year, month, 8, 6, 45, second));

            BasicTest("Voy a volver 19:00, 2016-12-22", 
                new DateObject(2016, 12, 22, 19, 0, second));

            BasicTest("Voy a volver mañana 8:00am",
                new DateObject(year, month, 8, 8, 0, second));

            BasicTest("Voy a volver mañana por la mañana a las 7", 
                new DateObject(2016, 11, 8, 7, min, second));
            
            BasicTest("Voy a volver 7:00 el próximo domingo a la tarde",
                new DateObject(2016, 11, 20, 19, min, second));

            BasicTest("Voy a volver a las cinco y veinte mañana por la mañana",
                new DateObject(2016, 11, 8, 5, 20, second));

            BasicTest("Voy a volver a las 7 esta mañana",
                new DateObject(year, month, day, 7, min, second));

            BasicTest("Voy a volver a las 10 esta noche", 
                new DateObject(year, month, day, 22, min, second));

            BasicTest("Voy a volver esta noche a las 8", 
                new DateObject(year, month, day, 20, min, second));

            BasicTest("Voy a volver 8pm de la tarde, Domingo",
                new DateObject(2016, 11, 13, 20, min, second),
                new DateObject(2016, 11, 6, 20, min, second));

            BasicTest("Voy a volver 8pm de la tarde, primero de Ene",
                new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));

            BasicTest("Voy a volver 8pm de la tarde, 1 Ene", 
                new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));

            BasicTest("Voy a volver a las 10pm de esta noche", 
                new DateObject(2016, 11, 7, 22, min, second));
            
            BasicTest("Voy a volver 8am de hoy", 
                new DateObject(2016, 11, 7, 8, min, second));

            BasicTest("Voy a volver 8pm de esta tarde",
                new DateObject(2016, 11, 7, 20, min, second));

            BasicTest("Volveré al final del día",
                new DateObject(2016, 11, 7, 23, 59, second));

            BasicTest("Volveré al finalizar el día",
                new DateObject(2016, 11, 7, 23, 59, second));

            BasicTest("Volveré al finalizar el día de mañana", 
                new DateObject(2016, 11, 8, 23, 59, second));

            BasicTest("Volveré al finalizar el domingo", 
                new DateObject(2016, 11, 13, 23, 59, second),
                new DateObject(2016, 11, 6, 23, 59, second));
        }

        [TestMethod]
        public void TestDateTimeLuis()
        {
            BasicTest("Voy a volver tan pronto como sea posible", "FUTURE_REF");
            BasicTest("Vamos a volver tan pronto como podamos", "FUTURE_REF");
            BasicTest("Voy a volver lo mas pronto posible", "FUTURE_REF");
            BasicTest("Voy a volver el 15 a las 8:00", "XXXX-XX-15T08:00");
            BasicTest("Voy a volver el 15 a las 8:00:24", "XXXX-XX-15T08:00:24");
            BasicTest("Voy a volver el 15, 8pm", "XXXX-XX-15T20");
            BasicTest("Voy a volver 04/21/2016, 8:00pm", "2016-04-21T20:00");
            BasicTest("Voy a volver 04/21/2016, 8:00:24pm", "2016-04-21T20:00:24");
            BasicTest("Voy a volver el 23 de Oct a las siete", "XXXX-10-23T07");
            BasicTest("Voy a volver el 14 de Octubre 8:00am", "XXXX-10-14T08:00");
            BasicTest("Voy a volver el 14 de Octubre 8:00:13am", "XXXX-10-14T08:00:13");
            BasicTest("Voy a volver el 14 de Octubre, 8:00am", "XXXX-10-14T08:00");
            BasicTest("Voy a volver el 14 de Octubre, 8:00:25am", "XXXX-10-14T08:00:25");
            BasicTest("Voy a volver el 5 de Mayo, 2016, 20 minutos pasados de las 8 de la tarde", "2016-05-05T20:20");

            BasicTest("Voy a volver 8pm del 15", "XXXX-XX-15T20");
            BasicTest("Voy a volver a las siete del 15", "XXXX-XX-15T07");
            BasicTest("Voy a volver 8pm de hoy", "2016-11-07T20");
            BasicTest("Voy a volver 8pm hoy", "2016-11-07T20");
            BasicTest("Voy a volver a las siete menos cuarto de mañana", "2016-11-08T06:45");
            BasicTest("Voy a volver 19:00, 2016-12-22", "2016-12-22T19:00");
            BasicTest("Voy a volver ahora", "PRESENT_REF");
            BasicTest("Voy a volver ahora mismo", "PRESENT_REF");
            BasicTest("Voy a volver justo ahora", "PRESENT_REF");
            BasicTest("Voy a volver en este momento", "PRESENT_REF");

            BasicTest("Voy a volver mañana 8:00am", "2016-11-08T08:00");
            BasicTest("Voy a volver mañana por la mañana a las 7", "2016-11-08T07");
            BasicTest("Voy a volver 7:00 del próximo domingo a la tarde", "2016-11-20T19:00");
            BasicTest("Voy a volver a las cinco y veinte mañana por la mañana", "2016-11-08T05:20");

            BasicTest("Voy a volver 8pm de la tarde, Domingo", "XXXX-WXX-7T20");
            BasicTest("Voy a volver 8pm de la tarde, 1ro de Ene", "XXXX-01-01T20");
            BasicTest("Voy a volver 8pm de la tarde, 1 Ene", "XXXX-01-01T20");
            BasicTest("Voy a volver a las 10pm de esta noche", "2016-11-07T22");
            BasicTest("Voy a volver 4am de esta madrugada", "2016-11-07T04");
            BasicTest("Voy a volver 4pm de esta tarde", "2016-11-07T16");

            BasicTest("Volví esta mañana a las 7", "2016-11-07T07");
            BasicTest("Volví esta mañana a las 7am", "2016-11-07T07");
            BasicTest("Volví esta mañana a las siete", "2016-11-07T07");
            BasicTest("Volví esta mañana a las 7:00", "2016-11-07T07:00");
            BasicTest("Voy a volver esta noche a las 7", "2016-11-07T19");
            BasicTest("Volví anoche a las 7", "2016-11-07T19");

            BasicTest("Voy a volver 2016-12-16T12:23:59", "2016-12-16T12:23:59");
        }
    }
}