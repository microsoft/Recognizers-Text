using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
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
            extractor = new BaseDatePeriodExtractor(new FrenchDatePeriodExtractorConfiguration());
            parser = new BaseDatePeriodParser(new FrenchDatePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));
        }

        public void BasicTestFuture(string text, int beginDay, int endDay, int month, int year)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            var beginDate = new DateObject(year, month, beginDay);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item1);
            var endDate = new DateObject(year, month, endDay);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item2);
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
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item1);
            var endDate = new DateObject(endYear, endMonth, endDay);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestDatePeriodParse()
        {
            int year = 2016, month = 11;
            bool inclusiveEnd = parser.GetInclusiveEndPeriodFlag();

            // test basic cases
            BasicTestFuture("Je serai dehors 4 au 22 cette mois", 4, 22, month, year);
//          BasicTestFuture("Je serai dehors 4-23 in next month", 4, 23, 12, year);
            BasicTestFuture("Je serai dehors 3 jusqu'a 12 de Sept hahaha", 3, 12, 9, year + 1);
            BasicTestFuture("Je serai dehors 4 au 23 mois prochain", 4, 23, 12, year);
            BasicTestFuture("Je serai dehors 4 jusqu'a 23 cette mois", 4, 23, month, year);
            BasicTestFuture("Je serai dehors 4 et 22 cette mois", 4, 22, month, year);
            BasicTestFuture("Je serai dehors 3 et 12 de Sept hahaha", 3, 12, 9, year + 1);
            BasicTestFuture("Je serai dehors 4 au 22 Janv, 1995", 4, 22, 1, 1995);
 //         BasicTestFuture("Je serai dehors 4.22 Janvier, 1995", 4, 22, 1, 1995);   //returns 1/1/1995, doens't read 4.22
            BasicTestFuture("Je serai dehors entre septembre 4 jusqu'a septembre 8h", 4, 8, 9, year + 1);

            if (inclusiveEnd)
            {
                BasicTestFuture("Je serais dehors cette mois", 7, 13, month, year);
                BasicTestFuture("I'll be out on current week", 7, 13, month, year);
                BasicTestFuture("I'll be out February", year + 1, 2, 1, year + 1, 2, 28);
                BasicTestFuture("I'll be out this September", year, 9, 1, year, 9, 30);
                BasicTestFuture("I'll be out last sept", year - 1, 9, 1, year - 1, 9, 30);
                BasicTestFuture("I'll be out next june", year + 1, 6, 1, year + 1, 6, 30);
                BasicTestFuture("I'll be out the third week of this month", 21, 27, month, year);
                BasicTestFuture("I'll be out the last week of july", year + 1, 7, 24, year + 1, 7, 30);
                BasicTestFuture("week of september.16th", 11, 17, 9, year + 1);
                BasicTestFuture("month of september.16th", year, 9, 1, year, 9, 30);
            }
            else
            {
                BasicTestFuture("Je serais dehors cette semaine", 7, 14, month, year);
                BasicTestFuture("Je serais dehors Fevrier", year + 1, 2, 1, year + 1, 3, 1);
                BasicTestFuture("Je serais dehors cette Septembre", year, 9, 1, year, 10, 1);
              //BasicTestFuture("Je serais dehors Sept dernier", year - 1, 9, 1, year - 1, 10, 1); TODO: Fix/Add Month + PastSuffix/NextSuffix
                BasicTestFuture("Je serais dehors prochain Juin", year + 1, 6, 1, year + 1, 7, 1);  // This one works?
              //BasicTestFuture("Je serais dehors le troisieme semaine de cette mois", 21, 28, month, year);  // returns 11/1/2016 instead of 11/21/2016, not third week
              //BasicTestFuture("Je serais dehors le fin semaine de juillet", year + 1, 7, 24, year + 1, 7, 31);
                BasicTestFuture("semaine de septembre.16", 11, 18, 9, year + 1);
                BasicTestFuture("mois de septembre.16", year + 1, 9, 1, year + 1, 10, 1);
            }

            if (inclusiveEnd)
            {
                BasicTestFuture("Je serais dehors 2015.3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Je serais dehors 2015-3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Je serais dehors 2015/3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("Je serais dehors 3/2015", 2015, 3, 1, 2015, 3, 31);
            }
            else
            {
                BasicTestFuture("Je serais dehors 2015.3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Je serais dehors 2015-3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Je serais dehors 2015/3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("Je serais dehors 3/2015", 2015, 3, 1, 2015, 4, 1);
            }

        }

        [TestMethod]
        public void TestDatePeriodParseDuration()
        {
            int year = 2016, month = 11;
            bool inclusiveEnd = parser.GetInclusiveEndPeriodFlag();

            if (inclusiveEnd)
            {
                BasicTestFuture("organiser une reunion en deux semaines", 15, 21, month, year);
                BasicTestFuture("cette 2 jours", 8, 9, month, year);
                BasicTestFuture("quelques jours passes", 4, 6, month, year);
            }
            else
            {
                BasicTestFuture("organiser une reunion en deux semaines", 15, 22, month, year);
                // TODO: FIX - 'les deux prochain jours"
//              BasicTestFuture("cette deux jours", 8, 10, month, year); 
//              BasicTestFuture("quelques jours passes", 4, 7, month, year);
            }
        }

        [TestMethod]
        public void TestDatePeriodMergeTwoTimepoints()
        {
            int year = 2016, month = 11;

            // test merging two time points
            BasicTestFuture("Je serais dehors depuis 2 Oct a 22 Octobre", 2, 22, 10, year + 1);
            BasicTestFuture("Je serais dehors 12 Janvier, 2016 - 22/01/2016", 12, 22, 1, year);
            BasicTestFuture("Je serais dehors 1 Jan jusqu'a Mer, 22 Janv", 1, 22, 1, year + 1);
            BasicTestFuture("Je serais dehors depuis aujourd'hui jusqu'a demain", 7, 8, month, year);

            BasicTestFuture("Je serais dehors depuis Oct. 2 au Octobre 22", 2, 22, 10, year + 1);
            BasicTestFuture("Je serais sorti depuis Oct. 2 et Oct 22", 2, 22, 10, year + 1);
            BasicTestFuture("Je serais dehors 19-20 Novembre", 19, 20, 11, year);
            BasicTestFuture("Je serais sorti Novembre 19 au 20", 19, 20, 11, year);
            BasicTestFuture("Je serais sorti Novembre entre 19 et 20", 19, 20, 11, year);

            // TODO: Fix '1er'
            //BasicTestFuture("Je serais dehors 1er Jan jusqu'a Mer, 22 Janv", 1, 22, 1, year + 1);
        }

        [TestMethod]
        public void TestDatePeriodParseLuis()
        {
            // test basic cases
            BasicTest("Je serais sorti depuis 4 au 22 cette mois", "(2016-11-04,2016-11-22,P18D)");
            BasicTest("Je serais sorti depuis 4-23 mois prochain", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("Je serais sorti depuis 3 jusqu'a 12 de Sept hahaha", "(XXXX-09-03,XXXX-09-12,P9D)");
            BasicTest("Je serais sorti 4 au 23 mois prochain", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("Je serais sorti 4 jusqu'a 23 de cette mois", "(2016-11-04,2016-11-23,P19D)");

            BasicTest("Je serais sorti cette semaine", "2016-W46");
            BasicTest("Je vais sortir le weekend", "2016-W46-WE");
            BasicTest("Je serais dehors le weekend", "2016-W46-WE");
            BasicTest("Je serais dehors fevrier", "XXXX-02");
            BasicTest("Je serais cette Septembre", "2016-09");
            
            BasicTest("Je serais dehors juin 2016", "2016-06");
            //BasicTest("JE serais dehors juin annee prochain", "2017-06"); // returns XXXX-06
            //BasicTest("Je serais l'année prochaine", "2017"); // nothing in dictionary?

            // test merging two time points
            BasicTest("Je serais dehors Oct. 2 a Octobre 22", "(XXXX-10-02,XXXX-10-22,P20D)");
            BasicTest("Je serais dehors 12 Janvier, 2016 - 22/01/2016", "(2016-01-12,2016-01-22,P10D)");
            BasicTest("Je serais dehors aujourd'hui jusqu'a lendemain", "(2016-11-07,2016-11-08,P1D)"); // I will be out today until tomorrow

            BasicTest("Je serais dehors depuis Oct. 2 a Octobre 22", "(XXXX-10-02,XXXX-10-22,P20D)");

            //BasicTest("le premier semaine octobre", "XXXX-10-W01"); // first week of october, fix
            BasicTest("Je serais dehors le troisieme semaine de 2027", "2027-01-W03");
            //BasicTest("I'll be out the third week next year", "2017-01-W03");

            BasicTest("Je serais dehors Novembre 19-20", "(XXXX-11-19,XXXX-11-20,P1D)");
            BasicTest("Je serais dehors Novembre 19 au 20", "(XXXX-11-19,XXXX-11-20,P1D)");
            //BasicTest("Je serais dehors Novembre depuis 19 au 20", "(XXXX-11-19,XXXX-11-20,P1D)");

            BasicTest("Je serais dehors le troisieme quart de 2016", "(2016-07-01,2016-10-01,P3M)");
            BasicTest("Je serai dehors derniere 3 semaines", "(2016-10-17,2016-11-07,P3W)");
            
            BasicTest("Je serais sorti 2015.3", "2015-03");
            BasicTest("Je serais sorti 2015-3", "2015-03");
            BasicTest("Je serais sorti 2015/3", "2015-03");
            BasicTest("Je serais sorti 3/2015", "2015-03");

            BasicTest("je partirai cette été", "2016-SU");
            //BasicTest("I'll leave printemps prochain", "2017-SP"); // fix season + relative suffix
            BasicTest("je partirai l'été", "SU");
            BasicTest("je partirai été", "SU");
            BasicTest("je partirai l'été 2016", "2016-SU");
            BasicTest("je pars l'été 2016", "2016-SU");

            //next and upcoming
            //BasicTest("mois prochain vacances", "2016-12"); // returns 2016-11...
            //BasicTest("next month holidays", "2016-12");

            //BasicTest("Je serais septembre derniere", "2015-09"); // doesn't recognize year, returns XXXX-09
            //BasicTest("Je serais dehors juin prochain", "2017-06"); // doesn't recognize year, returns XXXX-06
            //BasicTest("Je serais dehors le troisieme quart de cette l'annee", "(2016-07-01,2016-10-01,P3M)");
            //BasicTest("Je serais dehors 2016 le troisieme quart", "(2016-07-01,2016-10-01,P3M)");
            //BasicTest("Je serais dehors les 3 prochain jours", "(2016-11-08,2016-11-11,P3D)");
            //BasicTest("Je serais dehors les trois prochains mois", "(2016-11-08,2017-02-08,P3M)");
            //BasicTest("Je serai dehors 3 ans", "(2018-11-08,2019-11-08,P1Y)");

            //BasicTest("I'll be out last 3year", "(2013-11-07,2016-11-07,P3Y)");
            //BasicTest("I'll be out previous 3 weeks", "(2016-10-17,2016-11-07,P3W)");
            //BasicTest("Je serai dehors 3 semaines dernier", "(2016-10-17,2016-11-07,P3W)");
        }
    }
}