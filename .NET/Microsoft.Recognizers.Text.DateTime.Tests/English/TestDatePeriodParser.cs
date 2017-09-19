using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
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
            extractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
            parser = new BaseDatePeriodParser(new EnglishDatePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
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
            BasicTestFuture(text, beginYear, beginMonth, beginDay, endYear, endMonth, endDay, referenceDay);
        }

        public void BasicTestFuture(string text, int beginYear, int beginMonth, int beginDay, int endYear, int endMonth,
            int endDay, DateObject refDateTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refDateTime);
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
            BasicTest(text, luisValueStr, referenceDay);
        }

        public void BasicTest(string text, string luisValueStr, DateObject refDateTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refDateTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestDatePeriodParse()
        {
            int year = 2016, month = 11;
            bool inclusiveEnd = parser.GetInclusiveEndPeriodFlag();

            // test basic cases
            BasicTestFuture("I'll be out from 4 to 22 this month", 4, 22, month, year);
            BasicTestFuture("I'll be out from 4-23 in next month", 4, 23, 12, year);
            BasicTestFuture("I'll be out from 3 until 12 of Sept hahaha", 3, 12, 9, year + 1);
            BasicTestFuture("I'll be out 4 to 23 next month", 4, 23, 12, year);
            BasicTestFuture("I'll be out 4 till 23 of this month", 4, 23, month, year);
            BasicTestFuture("I'll be out between 4 and 22 this month", 4, 22, month, year);
            BasicTestFuture("I'll be out between 3 and 12 of Sept hahaha", 3, 12, 9, year + 1);
            BasicTestFuture("I'll be out from 4 to 22 January, 1995", 4, 22, 1, 1995);
            BasicTestFuture("I'll be out between 4-22 January, 1995", 4, 22, 1, 1995);
            BasicTestFuture("I'll be out between september 4th through september 8th", 4, 8, 9, year+1);

            if (inclusiveEnd)
            {
                BasicTestFuture("I'll be out on this week", 7, 13, month, year);
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
                BasicTestFuture("I'll be out on this week", 7, 14, month, year);
                BasicTestFuture("I'll be out on current week", 7, 14, month, year);
                BasicTestFuture("I'll be out February", year + 1, 2, 1, year + 1, 3, 1);
                BasicTestFuture("I'll be out this September", year, 9, 1, year, 10, 1);
                BasicTestFuture("I'll be out last sept", year - 1, 9, 1, year - 1, 10, 1);
                BasicTestFuture("I'll be out next june", year + 1, 6, 1, year + 1, 7, 1);
                BasicTestFuture("I'll be out the third week of this month", 21, 28, month, year);
                BasicTestFuture("I'll be out the last week of july", year + 1, 7, 24, year + 1, 7, 31);
                BasicTestFuture("week of september.16th", 11, 18, 9, year + 1);
                BasicTestFuture("month of september.16th", year + 1, 9, 1, year + 1, 10, 1);
            }

            if (inclusiveEnd)
            {
                BasicTestFuture("I'll be out 2015.3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("I'll be out 2015-3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("I'll be out 2015/3", 2015, 3, 1, 2015, 3, 31);
                BasicTestFuture("I'll be out 3/2015", 2015, 3, 1, 2015, 3, 31);
            }
            else
            {
                BasicTestFuture("I'll be out 2015.3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("I'll be out 2015-3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("I'll be out 2015/3", 2015, 3, 1, 2015, 4, 1);
                BasicTestFuture("I'll be out 3/2015", 2015, 3, 1, 2015, 4, 1);
            }

        }

        [TestMethod]
        public void TestDatePeriodParseDuration()
        {
            int year = 2016, month = 11;
            bool inclusiveEnd = parser.GetInclusiveEndPeriodFlag();

            if (inclusiveEnd)
            {
                BasicTestFuture("scheduel a meeting in two weeks", 15, 21, month, year);
                BasicTestFuture("next 2 days", 8, 9, month, year);
                BasicTestFuture("past few days", 4, 6, month, year);
                BasicTestFuture("the week", 7, 13, month, year);
                BasicTestFuture("this week", 7, 13, month, year);
                BasicTestFuture("my week", 7, 13, month, year);
                BasicTestFuture("the weekend", 12, 13, month, year);
                BasicTestFuture("this weekend", 12, 13, month, year);
                BasicTestFuture("my weekend", 12, 13, month, year);

                BasicTestFuture("What is my schedule for the week?", 7, 13, month, year);
                BasicTestFuture("Show me my calendar for the week", 7, 13, month, year);
                BasicTestFuture("What's my day looking like?", 7, 7, month, year);
                BasicTestFuture("how does your day look like?", 7, 7, month, year);
            }
            else
            {
                BasicTestFuture("scheduel a meeting in two weeks", 15, 22, month, year);
                BasicTestFuture("next 2 days", 8, 10, month, year);
                BasicTestFuture("past few days", 4, 7, month, year);
                BasicTestFuture("the week", 7, 14, month, year);
                BasicTestFuture("this week", 7, 14, month, year);
                BasicTestFuture("my week", 7, 14, month, year);
                BasicTestFuture("the weekend", 12, 14, month, year);
                BasicTestFuture("this weekend", 12, 14, month, year);
                BasicTestFuture("my weekend", 12, 14, month, year);
            }
        }

        [TestMethod]
        public void TestDatePeriodMergeTwoTimepoints()
        {
            int year = 2016, month = 11;

            // test merging two time points
            BasicTestFuture("I'll be out Oct. 2 to October 22", 2, 22, 10, year + 1);
            BasicTestFuture("I'll be out January 12, 2016 - 01/22/2016", 12, 22, 1, year);
            BasicTestFuture("I'll be out 1st Jan until Wed, 22 of Jan", 1, 22, 1, year + 1);
            BasicTestFuture("I'll be out today till tomorrow", 7, 8, month, year);

            BasicTestFuture("I'll be out from Oct. 2 to October 22", 2, 22, 10, year + 1);
            BasicTestFuture("I'll be out between Oct. 2 and October 22", 2, 22, 10, year + 1);

            BasicTestFuture("I'll be out November 19-20", 19, 20, 11, year);
            BasicTestFuture("I'll be out November 19 to 20", 19, 20, 11, year);
            BasicTestFuture("I'll be out November between 19 and 20", 19, 20, 11, year);
        }

        [TestMethod]
        public void TestDatePeriodParseRestOf()
        {
            int year = 2016, month = 11, day = 7;
            BasicTestFuture("I'll be out rest of the week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest of week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest the week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest this week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest of my week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest of current week", year, month, 7, year, month, 13);
            BasicTestFuture("I'll be out rest of the month", year, month, 7, year, month, 30);
            BasicTestFuture("I'll be out rest of the year", year, month, 7, year, 12, 31);
            BasicTestFuture("I'll be out rest of my week", year, month, 13, year, month, 13, new DateObject(2016, 11, 13));
        }

        [TestMethod]
        public void TestDatePeriodParseLuis()
        {
            // test basic cases
            BasicTest("I'll be out from 4 to 22 this month", "(2016-11-04,2016-11-22,P18D)");
            BasicTest("I'll be out from 4-23 in next month", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("I'll be out from 3 until 12 of Sept hahaha", "(XXXX-09-03,XXXX-09-12,P9D)");
            BasicTest("I'll be out 4 to 23 next month", "(2016-12-04,2016-12-23,P19D)");
            BasicTest("I'll be out 4 till 23 of this month", "(2016-11-04,2016-11-23,P19D)");

            BasicTest("I'll be out on this week", "2016-W46");
            BasicTest("I'll be out on weekend", "2016-W46-WE");
            BasicTest("I'll be out on this weekend", "2016-W46-WE");
            BasicTest("I'll be out February", "XXXX-02");
            BasicTest("I'll be out this September", "2016-09");
            BasicTest("I'll be out last sept", "2015-09");
            BasicTest("I'll be out next june", "2017-06");
            BasicTest("I'll be out june 2016", "2016-06");
            BasicTest("I'll be out june next year", "2017-06");
            BasicTest("I'll be out next year", "2017");

            BasicTest("I'll be out next 3 days", "(2016-11-08,2016-11-11,P3D)");
            BasicTest("I'll be out next 3 months", "(2016-11-08,2017-02-08,P3M)");
            BasicTest("I'll be out in 3 year", "(2018-11-08,2019-11-08,P1Y)");
            BasicTest("I'll be out past 3 weeks", "(2016-10-17,2016-11-07,P3W)");
            BasicTest("I'll be out last 3year", "(2013-11-07,2016-11-07,P3Y)");
            BasicTest("I'll be out previous 3 weeks", "(2016-10-17,2016-11-07,P3W)");

            // test merging two time points
            BasicTest("I'll be out Oct. 2 to October 22", "(XXXX-10-02,XXXX-10-22,P20D)");
            BasicTest("I'll be out January 12, 2016 - 01/22/2016", "(2016-01-12,2016-01-22,P10D)");
            BasicTest("I'll be out today till tomorrow", "(2016-11-07,2016-11-08,P1D)");

            BasicTest("I'll be out from Oct. 2 to October 22", "(XXXX-10-02,XXXX-10-22,P20D)");

            BasicTest("the first week of Oct", "XXXX-10-W01");
            BasicTest("I'll be out the third week of 2027", "2027-01-W03");
            BasicTest("I'll be out the third week next year", "2017-01-W03");

            BasicTest("I'll be out November 19-20", "(XXXX-11-19,XXXX-11-20,P1D)");
            BasicTest("I'll be out November 19 to 20", "(XXXX-11-19,XXXX-11-20,P1D)");
            BasicTest("I'll be out November between 19 and 20", "(XXXX-11-19,XXXX-11-20,P1D)");

            BasicTest("I'll be out the third quarter of 2016", "(2016-07-01,2016-10-01,P3M)");
            BasicTest("I'll be out the third quarter this year", "(2016-07-01,2016-10-01,P3M)");
            BasicTest("I'll be out 2016 the third quarter", "(2016-07-01,2016-10-01,P3M)");

            BasicTest("I'll be out 2015.3", "2015-03");
            BasicTest("I'll be out 2015-3", "2015-03");
            BasicTest("I'll be out 2015/3", "2015-03");
            BasicTest("I'll be out 3/2015", "2015-03");

            BasicTest("I'll leave this summer", "2016-SU");
            BasicTest("I'll leave next spring", "2017-SP");
            BasicTest("I'll leave the summer", "SU");
            BasicTest("I'll leave summer", "SU");
            BasicTest("I'll leave summer 2016", "2016-SU");
            BasicTest("I'll leave summer of 2016", "2016-SU");

            //next and upcoming
            BasicTest("upcoming month holidays", "2016-12");
            BasicTest("next month holidays", "2016-12");
        }

        [TestMethod]
        public void TestDatePeriodRestOfLuis()
        {
            BasicTest("I'll be out rest of the week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest of week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest the week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest this week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest of my week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest of current week", "(2016-11-07,2016-11-13,P6D)");
            BasicTest("I'll be out rest of the month", "(2016-11-07,2016-11-30,P24D)");
            BasicTest("I'll be out rest of the year", "(2016-11-07,2016-12-31,P55D)");
            BasicTest("I'll be out rest of my week", "(2016-11-13,2016-11-13,P0D)", new DateObject(2016, 11, 13));
        }
    }
}