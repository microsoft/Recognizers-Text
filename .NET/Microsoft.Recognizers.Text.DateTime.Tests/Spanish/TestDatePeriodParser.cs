using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestDatePeriodParser
    {
        readonly BaseDatePeriodParser parser;
        readonly BaseDatePeriodExtractor extractor;
        readonly DateObject referenceDay;

        public TestDatePeriodParser()
        {
            referenceDay = new DateObject(2016, 11, 7);
            extractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());
            parser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));
        }

        public void BasicTestFuture(string text, int beginDay, int endDay, int month, int year)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            var beginDate = new DateObject(year, month, beginDay);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item1);
            var endDate = new DateObject(year, month, endDay);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item2);
        }

        public void BasicTestFuture(string text, int beginYear, int beginMonth, int beginDay, int endYear, int endMonth,
            int endDay)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            var beginDate = new DateObject(beginYear, beginMonth, beginDay);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item1);
            var endDate = new DateObject(endYear, endMonth, endDay);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestDatePeriodParse()
        {
            int year = 2016, month = 11;

            bool inclusiveEnd=parser.GetInclusiveEndPeriodFlag();


            //TODO: add tests for week of and month of

            // test basic cases
            BasicTestFuture("Estare afuera desde el 4 hasta el 22 de este mes", 4, 22, month, year);
            BasicTestFuture("Estare afuera desde 4-23 del proximo mes", 4, 23, 12, year);
            BasicTestFuture("Estare afuera desde el 3 hasta el 12 de Sept jajaja", 3, 12, 9, year + 1);
            BasicTestFuture("Estare afuera 4 hasta 23 del proximo mes", 4, 23, 12, year);
            BasicTestFuture("Estare afuera desde el 4 hasta el 23 de este mes", 4, 23, month, year);
            BasicTestFuture("Estare afuera entre 4 y 22 este mes", 4, 22, month, year);
            BasicTestFuture("Estare afuera entre el 3 y el 12 de Set jajaja", 3, 12, 9, year + 1);
            BasicTestFuture("Estare afuera del 4 al 22 de enero, 1995", 4, 22, 1, 1995);
            BasicTestFuture("Estare afuera entre 4-22 enero,  1995", 4, 22, 1, 1995);

            if (inclusiveEnd)
            {
                BasicTestFuture("Estare afuera esta semana", 7, 13, month, year);
                BasicTestFuture("Estare afuera en Febrero", year + 1, 2, 1, year + 1, 2, 28);
                BasicTestFuture("Estare afuera este Septiembre", year, 9, 1, year, 9, 30);
                BasicTestFuture("Estare afuera el ultimo sept", year - 1, 9, 1, year - 1, 9, 30);
                BasicTestFuture("Estare afuera el proximo junio", year + 1, 6, 1, year + 1, 6, 30);
                BasicTestFuture("Estare afuera la tercera semana de este mes", 21, 28, month, year);
                BasicTestFuture("Estare afuera la ultima semana de julio", year + 1, 7, 24, year + 1, 7, 31);
            }
            else
            {
                BasicTestFuture("Estare afuera esta semana", 7, 14, month, year);
                BasicTestFuture("Estare afuera en Febrero", year + 1, 2, 1, year + 1, 3, 1);
                BasicTestFuture("Estare afuera este Septiembre", year, 9, 1, year, 10, 1);
                BasicTestFuture("Estare afuera el ultimo sept", year - 1, 9, 1, year - 1, 10, 1);
                BasicTestFuture("Estare afuera el proximo junio", year + 1, 6, 1, year + 1, 7, 1);
                BasicTestFuture("Estare afuera la tercera semana de este mes", 21, 28, month, year);
                BasicTestFuture("Estare afuera la ultima semana de julio", year + 1, 7, 24, year + 1, 7, 31);
            }
            

            // test merging two time points
            BasicTestFuture("Estare afuera el 2 de Oct hasta 22 de Octubre", 2, 22, 10, year + 1);
            BasicTestFuture("Estare afuera el 12 de Enero, 2016 - 22/01/2016", 12, 22, 1, year);
            BasicTestFuture("Estare afuera el 1ro de Ene hasta Mi, 22 de Ene", 1, 22, 1, year + 1);
            BasicTestFuture("Estare afuera hoy hasta mañana", 7, 8, month, year);

            BasicTestFuture("Estare afuera desde 2 de Oct hasta 22 de Octubre", 2, 22, 10, year + 1);
            BasicTestFuture("Estare afuera entre 2 de Oct y 22 de Octubre", 2, 22, 10, year + 1);

            BasicTestFuture("Estare afuera 19-20 de Noviembre", 19, 20, 11, year);
            BasicTestFuture("Estare afuera 19 hasta 20 de Noviembre", 19, 20, 11, year);
            BasicTestFuture("Estare afuera entre 19 y 20 de Noviembre", 19, 20, 11, year);

            if (inclusiveEnd)
            {
                BasicTestFuture("Estare afuera 2015.3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Estare afuera 2015-3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Estare afuera 2015/3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Estare afuera 3/2015", 2015, 3, 1, 2015, 3, 31);
            }
            else
            {
                BasicTestFuture("Estare afuera 2015.3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Estare afuera 2015-3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Estare afuera 2015/3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Estare afuera 3/2015", 2015, 3, 1, 2015, 4, 1);
            }
            

            //BasicTestFuture("I'll leave this summer", 2016, 6, 1, 2016, 9, 1);
            //BasicTestFuture("I'll leave in summer", 2017, 6, 1, 2017, 9, 1);
            //BasicTestFuture("I'll leave in winter", 2016, 12, 1, 2017, 3, 1);
            //BasicTestFuture("I'll leave in winter, 2017", 2017, 12, 1, 2018, 3, 1);
        }

        [TestMethod]
        public void TestDatePeriodParseLuis()
        {
            // test basic cases
            BasicTest("Estare afuera desde el 4 hasta el 22 de este mes", "(2016-11-04,2016-11-22,P18D)");
            BasicTest("Estare afuera desde 4-23 del proximo mes", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("Estare afuera desde el 3 hasta el 12 de Sept jajaja", "(XXXX-09-03,XXXX-09-12,P9D)");
            BasicTest("Estare afuera 4 hasta 23 del proximo mes", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("Estare afuera desde el 4 hasta el 23 de este mes", "(2016-11-04,2016-11-23,P19D)");

            BasicTest("Estare afuera esta semana", "2016-W46");
            BasicTest("Estare afuera el fin de semana", "2016-W46-WE");
            BasicTest("Estare afuera este fin de semana", "2016-W46-WE");
            BasicTest("Estare afuera en Febrero", "XXXX-02");
            BasicTest("Estare afuera este Septiembre", "2016-09");
            BasicTest("Estare afuera el ultimo sept", "2015-09");
            BasicTest("Estare afuera el proximo junio", "2017-06");
            BasicTest("Estare afuera en junio 2016", "2016-06");
            BasicTest("Estare afuera en junio del proximo año", "2017-06");
            BasicTest("Estare afuera el próximo año", "2017");

            BasicTest("Estare afuera los próximos 3 dias", "(2016-11-08,2016-11-11,P3D)");
            BasicTest("Estare afuera los proximos 3 meses", "(2016-11-08,2017-02-08,P3M)");
            BasicTest("Estare afuera en 3 años", "(2016-11-08,2019-11-08,P3Y)");
            BasicTest("Estuve afuera las pasadas 3 semanas", "(2016-10-17,2016-11-07,P3W)");
            BasicTest("Estuve afuera los ultimos 3años", "(2013-11-07,2016-11-07,P3Y)");
            BasicTest("Estuve afuera las anteriores 3 semanas", "(2016-10-17,2016-11-07,P3W)");

            // test merging two time points
            BasicTest("Estare afuera el 2 de Oct hasta 22 de Octubre", "(XXXX-10-02,XXXX-10-22,P20D)");
            BasicTest("Estare afuera el 12 de Enero, 2016 - 22/01/2016", "(2016-01-12,2016-01-22,P10D)");
            BasicTest("Estare afuera hoy hasta mañana", "(2016-11-07,2016-11-08,P1D)");
            BasicTest("Estare afuera desde hoy hasta mañana", "(2016-11-07,2016-11-08,P1D)");

            BasicTest("Estare afuera desde 2 de Oct hasta 22 de Octubre", "(XXXX-10-02,XXXX-10-22,P20D)");

            BasicTest("la primer semana de Oct", "XXXX-10-W01");
            BasicTest("Estare afuera la tercera semana del 2027", "2027-01-W03");
            BasicTest("Estare afuera la tercer semana del próximo año", "2017-01-W03");

            BasicTest("Estare afuera 19-20 de Noviembre", "(XXXX-11-19,XXXX-11-20,P1D)");
            BasicTest("Estare afuera 19 hasta 20 de Noviembre", "(XXXX-11-19,XXXX-11-20,P1D)");
            BasicTest("Estare afuera entre 19 y 20 de Noviembre", "(XXXX-11-19,XXXX-11-20,P1D)");

            BasicTest("Estare afuera el tercer cuatrimestre de 2016", "(2016-07-01,2016-10-01,P3M)");
            BasicTest("Estare afuera el tercer cuatrimestre de este año", "(2016-07-01,2016-10-01,P3M)");
            BasicTest("Estare afuera 2016 el tercer cuatrimestre", "(2016-07-01,2016-10-01,P3M)");

            BasicTest("Estare afuera 2015.3", "2015-03");
            BasicTest("Estare afuera 2015-3", "2015-03");
            BasicTest("Estare afuera 2015/3", "2015-03");
            BasicTest("Estare afuera 3/2015", "2015-03");

            BasicTest("Estare afuera este verano", "2016-SU");
            BasicTest("Estare afuera la próxima primavera", "2017-SP");
            BasicTest("Estare afuera el verano", "SU");
            BasicTest("Estare afuera verano", "SU");
            BasicTest("Estare afuera verano 2016", "2016-SU");
            BasicTest("Estare afuera verano del 2016", "2016-SU");
        }
    }
}