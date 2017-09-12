using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDateParser
    {
        readonly DateObject refrenceDay;
        readonly IDateTimeParser parser;
        readonly BaseDateExtractor extractor;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Spa", typeof(BaseDateParser));
        }


        public void BasicTest(string text, DateObject futureDate, DateObject pastDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(futureDate, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(pastDate, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public TestDateParser()
        {
            refrenceDay = new DateObject(2016, 11, 7);
            parser = new BaseDateParser(new SpanishDateParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));
            extractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
        }

        [TestMethod]
        public void TestDateParse()
        {
            int tYear = 2016, tMonth = 11, tDay = 7;
            BasicTest("Volvere el 15", new DateObject(tYear, tMonth, 15), new DateObject(tYear, tMonth - 1, 15));
            BasicTest("volvere el 2 Oct", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("volvere el 2-Oct", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("volvere el 2/Oct", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("volveré el 2 Octubre", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("volveré el 12 de enero, 2016", new DateObject(2016, 1, 12), new DateObject(2016, 1, 12));
            BasicTest("volveré el lunes 12 de enero, 2016", new DateObject(2016, 1, 12));
            BasicTest("volveré el 22/02/2016", new DateObject(2016, 2, 22));
            BasicTest("volveré el 21/04/2016", new DateObject(2016, 4, 21));
            BasicTest("volveré el 21/04/16", new DateObject(2016, 4, 21));
            BasicTest("volveré el 21-04-2016", new DateObject(2016, 4, 21));
            BasicTest("volveré el 4.22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré el 4-22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré el 4.22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré el 4-22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré el     4/22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré el 22/04", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré     4/22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré 22/04", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("volveré 2015/08/12", new DateObject(2015, 8, 12));
            BasicTest("volveré 08/12,2015", new DateObject(2015, 8, 12));
            BasicTest("volveré 08/12,15", new DateObject(2015, 8, 12));
            BasicTest("volveré 1ro de Ene", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("volveré el 1-Ene", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("volveré el Mi, 22 de Ene", new DateObject(tYear + 1, 1, 22), new DateObject(tYear, 1, 22));

            BasicTest("volveré el primero de Ene", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("volveré el veintiuno de Mayo", new DateObject(tYear + 1, 5, 21), new DateObject(tYear, 5, 21));
            BasicTest("volveré en Mayo veintiuno", new DateObject(tYear + 1, 5, 21), new DateObject(tYear, 5, 21));
            BasicTest("volveré el segundo de Ago.", new DateObject(tYear + 1, 8, 2), new DateObject(tYear, 8, 2));
            BasicTest("volveré el vigesimo segundo de Junio", new DateObject(tYear + 1, 6, 22), new DateObject(tYear, 6, 22));

            // cases below change with reference day
            BasicTest("volveré en viernes", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("volveré en |viernes", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("volveré los viernes", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("volveré |Viernes", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("volveré hoy", new DateObject(2016, 11, 7));
            BasicTest("volveré mañana", new DateObject(2016, 11, 8));
            BasicTest("volví ayer", new DateObject(2016, 11, 6));
            BasicTest("volví anteayer", new DateObject(2016, 11, 5));
            BasicTest("volveré pasado mañana", new DateObject(2016, 11, 9));
            BasicTest("volveré el día despues de mañana", new DateObject(2016, 11, 9));
            BasicTest("volveré el próximo dia", new DateObject(2016, 11, 8));
            BasicTest("volveré el día siguiente", new DateObject(2016, 11, 8));
            BasicTest("volveré el este viernes", new DateObject(2016, 11, 11));
            BasicTest("volveré el proximo domingo", new DateObject(2016, 11, 20));
            BasicTest("volveré el ultimo domingo", new DateObject(2016, 11, 6));
            BasicTest("volveré el viernes de esta semana", new DateObject(2016, 11, 11));
            BasicTest("volveré el domingo de la siguiente semana", new DateObject(2016, 11, 20));
            BasicTest("volveré el domingo de la última semana", new DateObject(2016, 11, 6));
            BasicTest("volveré el último dia", new DateObject(2016, 11, 6));
            BasicTest("volveré en el último día", new DateObject(2016, 11, 6));
            BasicTest("volveré en el día", new DateObject(tYear, tMonth, tDay));
            BasicTest("volveré el 15 Junio 2016", new DateObject(2016, 6, 15));

            BasicTest("volveré el primer viernes de julio", new DateObject(2017, 7, 7), new DateObject(2016, 7, 1));
            BasicTest("volveré el primer viernes de este mes", new DateObject(2016, 11, 4));
        }

        [TestMethod]
        public void TestDateParseLuis()
        {
            BasicTest("Volvere el 15", "XXXX-XX-15");
            BasicTest("volvere el 2 Oct", "XXXX-10-02");
            BasicTest("volvere el 2/Oct", "XXXX-10-02");
            BasicTest("volveré el 12 de enero, 2018", "2018-01-12");
            BasicTest("volveré el 21/04/2016", "2016-04-21");
            BasicTest("volveré el 4.22", "XXXX-04-22");
            BasicTest("volveré el 4-22", "XXXX-04-22");
            BasicTest("volveré el     4/22", "XXXX-04-22");
            BasicTest("volveré el 22/04", "XXXX-04-22");
            BasicTest("volveré el 21/04/16", "2016-04-21");
            BasicTest("volveré el 9-18-15", "2015-09-18");
            BasicTest("volveré el 2015/08/12", "2015-08-12");
            BasicTest("volveré el 2015/08/12", "2015-08-12");
            BasicTest("volveré el 08/12,2015", "2015-08-12");
            BasicTest("volveré el 1ro de Ene", "XXXX-01-01");
            BasicTest("volveré el Mi, 22 de Ene", "XXXX-01-22");

            BasicTest("Volveré el primero de Ene", "XXXX-01-01");
            BasicTest("Volveré el veintiuno de Mayo", "XXXX-05-21");
            BasicTest("Volveré el Mayo veintiuno", "XXXX-05-21");
            BasicTest("Volveré el segundo de Ago.", "XXXX-08-02");
            BasicTest("Volveré el vigesimo segundo de Junio", "XXXX-06-22");

            // cases below change with reference day
            BasicTest("Volveré el viernes", "XXXX-WXX-5");
            BasicTest("volveré en |viernes", "XXXX-WXX-5");
            BasicTest("volveré hoy", "2016-11-07");
            BasicTest("volveré mañana", "2016-11-08");
            BasicTest("volví ayer", "2016-11-06");
            BasicTest("Volvi el dia antes de ayer", "2016-11-05");
            BasicTest("Volveré pasado mañana", "2016-11-09");
            BasicTest("El día despues de mañana", "2016-11-09");
            BasicTest("volveré el día siguiente", "2016-11-08");
            BasicTest("volveré el proximo día", "2016-11-08");
            BasicTest("volveré el este viernes", "2016-11-11");
            BasicTest("volveré el próximo domingo", "2016-11-20");
            BasicTest("volveré en el día", "2016-11-07");
            BasicTest("volveré el 15 Junio 2016", "2016-06-15");
        }
    }
}