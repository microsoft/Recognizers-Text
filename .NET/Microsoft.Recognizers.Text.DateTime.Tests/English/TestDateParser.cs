using Microsoft.VisualStudio.TestTools.UnitTesting;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{

    [TestClass]
    public class TestDateParser
    {
        readonly DateObject refrenceDay;
        readonly IDateTimeParser parser;
        readonly BaseDateExtractor extractor;

        public void BasicTest(string text, DateObject futureDate, DateObject pastDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(futureDate, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(pastDate, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        public void BasicTest(string text, DateObject date, bool now = false)
        {
            var refDay = refrenceDay;
            if (now) refDay = DateObject.Now;

            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        public void BasicTest(string text, string luisValueStr, bool now = false)
        {
            var refDay = refrenceDay;
            if (now) refDay = DateObject.Now;

            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        public TestDateParser()
        {
            refrenceDay = new DateObject(2016, 11, 7);
            parser = new BaseDateParser(new EnglishDateParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
            extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
        }

        // test using DateObject.Now as a reference time
        public void BasicTestDateNow(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], DateObject.Now);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        // use to generate the test cases sentences inside TestDateParserWeekDayAndDayOfMonth function
        // return a day of current week which the parameter refer to
        public System.Tuple<string, string> GenWeekDaynDayMonthTest(int dayOfMonth)
        {
            var weekDay = "None";
            var now = DateObject.Now;
            var date = new DateObject(now.Year, now.Month, dayOfMonth);
            if (dayOfMonth >= 1 && dayOfMonth <= 31)
            {
                weekDay = date.DayOfWeek.ToString();
            }

            var sentence = "I went back " + weekDay;
            var dateStr = $"{now.Year}-{now.Month.ToString().PadLeft(2, '0')}-{dayOfMonth.ToString().PadLeft(2, '0')}";
            return new System.Tuple<string, string>(sentence, dateStr);
        }

        // use to generate the answers to the test cases in TestDateParseRelativeDayOfWeek function
        public System.Tuple<DateObject, string> GenRelativeWeekDayAnswer(int ordinalNum, int wantedWeekDay, DateObject refDate)
        {
            var firstDate = DateObject.MinValue.SafeCreateFromValue(refDate.Year, refDate.Month, 1);
            var firstWeekDay = (int)firstDate.DayOfWeek;
            var firstWantedWeekDay = firstDate.AddDays(wantedWeekDay > firstWeekDay ? wantedWeekDay - firstWeekDay : wantedWeekDay - firstWeekDay + 7);
            var wantedDate = firstWantedWeekDay.Day + ((ordinalNum - 1) * 7);
            var answerDate = DateObject.MinValue.SafeCreateFromValue(refDate.Year, refDate.Month, wantedDate);
            var answerDateStr = $"{refDate.Year}-{refDate.Month.ToString().PadLeft(2, '0')}-{wantedDate.ToString().PadLeft(2, '0')}";
            return new System.Tuple<DateObject, string>(answerDate, answerDateStr);
        }

        [TestMethod]
        public void TestDateParse()
        {
            int tYear = 2016, tMonth = 11, tDay = 7;
            BasicTest("I'll go back on 15", new DateObject(tYear, tMonth, 15), new DateObject(tYear, tMonth - 1, 15));
            BasicTest("I'll go back Oct. 2", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("I'll go back Oct-2", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("I'll go back Oct/2", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("I'll go back October. 2", new DateObject(tYear + 1, 10, 2), new DateObject(tYear, 10, 2));
            BasicTest("I'll go back January 12, 2016", new DateObject(2016, 1, 12), new DateObject(2016, 1, 12));
            BasicTest("I'll go back Monday January 12th, 2016", new DateObject(2016, 1, 12));
            BasicTest("I'll go back 02/22/2016", new DateObject(2016, 2, 22));
            BasicTest("I'll go back 21/04/2016", new DateObject(2016, 4, 21));
            BasicTest("I'll go back 21/04/16", new DateObject(2016, 4, 21));
            BasicTest("I'll go back 21-04-2016", new DateObject(2016, 4, 21));
            BasicTest("I'll go back on 4.22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back on 4-22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back in 4.22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back at 4-22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back on    4/22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back on 22/04", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back     4/22", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back 22/04", new DateObject(tYear + 1, 4, 22), new DateObject(tYear, 4, 22));
            BasicTest("I'll go back 2015/08/12", new DateObject(2015, 8, 12));
            BasicTest("I'll go back 08/12,2015", new DateObject(2015, 8, 12));
            BasicTest("I'll go back 08/12,15", new DateObject(2015, 8, 12));
            BasicTest("I'll go back 1st Jan", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("I'll go back Jan-1", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("I'll go back Wed, 22 of Jan", new DateObject(tYear + 1, 1, 22), new DateObject(tYear, 1, 22));

            BasicTest("I'll go back Jan first", new DateObject(tYear + 1, 1, 1), new DateObject(tYear, 1, 1));
            BasicTest("I'll go back May twenty-first", new DateObject(tYear + 1, 5, 21), new DateObject(tYear, 5, 21));
            BasicTest("I'll go back May twenty one", new DateObject(tYear + 1, 5, 21), new DateObject(tYear, 5, 21));
            BasicTest("I'll go back second of Aug.", new DateObject(tYear + 1, 8, 2), new DateObject(tYear, 8, 2));
            BasicTest("I'll go back twenty second of June", new DateObject(tYear + 1, 6, 22),
                new DateObject(tYear, 6, 22));

            // cases below change with reference day
            BasicTest("I'll go back on Friday", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("I'll go back |Friday", new DateObject(2016, 11, 11), new DateObject(2016, 11, 4));
            BasicTest("I'll go back today", new DateObject(2016, 11, 7));
            BasicTest("I'll go back tomorrow", new DateObject(2016, 11, 8));
            BasicTest("I'll go back yesterday", new DateObject(2016, 11, 6));
            BasicTest("I'll go back the day before yesterday", new DateObject(2016, 11, 5));
            BasicTest("I'll go back the day after tomorrow", new DateObject(2016, 11, 9));
            BasicTest("The day after tomorrow", new DateObject(2016, 11, 9));
            BasicTest("I'll go back the next day", new DateObject(2016, 11, 8));
            BasicTest("I'll go back next day", new DateObject(2016, 11, 8));
            BasicTest("I'll go back this Friday", new DateObject(2016, 11, 11));
            BasicTest("I'll go back next Sunday", new DateObject(2016, 11, 20));
            BasicTest("I'll go back last Sunday", new DateObject(2016, 11, 6));
            BasicTest("I'll go back this week Friday", new DateObject(2016, 11, 11));
            BasicTest("I'll go back next week Sunday", new DateObject(2016, 11, 20));
            BasicTest("I'll go back last week Sunday", new DateObject(2016, 11, 6));
            BasicTest("I'll go back last day", new DateObject(2016, 11, 6));
            BasicTest("I'll go back the last day", new DateObject(2016, 11, 6));
            BasicTest("I'll go back the day", new DateObject(tYear, tMonth, tDay));
            BasicTest("I'll go back 15 June 2016", new DateObject(2016, 6, 15));

            BasicTest("I'll go back the first friday of july", new DateObject(2017, 7, 7), new DateObject(2016, 7, 1));
            BasicTest("I'll go back the first friday in this month", new DateObject(2016, 11, 4));

            BasicTest("I'll go back next week on Friday", new DateObject(2016, 11, 18));
            BasicTest("I'll go back on Friday next week", new DateObject(2016, 11, 18));
        }

        [TestMethod]
        public void TestDateParse_TheDay()
        {
            int tYear = 2016, tMonth = 11, tDay = 7;
            
            BasicTest("I'll go back next day", new DateObject(2016, 11, 8));
            BasicTest("I'll go back the day", new DateObject(2016, 11, 7));
            BasicTest("I'll go back my day", new DateObject(2016, 11, 7));
            BasicTest("I'll go back this day", new DateObject(2016, 11, 7));
            BasicTest("I'll go back last day", new DateObject(2016, 11, 6));
            BasicTest("I'll go back past day", new DateObject(2016, 11, 6));
        }

        [TestMethod]
        public void TestDateParseAgoLater()
        {
            BasicTest("I'll go back two weeks from now", new DateObject(2016, 11, 21));
            BasicTest("who did I email a month ago", new DateObject(2016, 10, 7));
            BasicTest("who did I email few month ago", new DateObject(2016, 8, 7));
            BasicTest("who did I email a few day ago", new DateObject(2016, 11, 4));
        }

        [TestMethod]
        public void TestDateParseForThe()
        {
            BasicTest("I went back for the 27", new DateObject(2016, 11, 27));
            BasicTest("I went back for the 27th", new DateObject(2016, 11, 27));
            BasicTest("I went back for the 27.", new DateObject(2016, 11, 27));
            BasicTest("I went back for the 27!", new DateObject(2016, 11, 27));
            BasicTest("I went back for the 27 .", new DateObject(2016, 11, 27));
            BasicTest("I went back for the 21st", new DateObject(2016, 11, 21));
            BasicTest("I went back for the 22nd", new DateObject(2016, 11, 22));
            BasicTest("I went back for the second", new DateObject(2016, 11, 2));
            BasicTest("I went back for the twenty second", new DateObject(2016, 11, 22));
            BasicTest("I went back for the thirty", new DateObject(2016, 11, 30));
        }

        [TestMethod]
        public void TestDateParseWeekDayAndDayOfMonth()
        {
            int y = DateObject.Now.Year, m = DateObject.Now.Month;
            BasicTest(GenWeekDaynDayMonthTest(21).Item1 + " the 21st", new DateObject(y, m, 21), true);
            BasicTest(GenWeekDaynDayMonthTest(22).Item1 + " the 22nd", new DateObject(y, m, 22), true);
            BasicTest(GenWeekDaynDayMonthTest(23).Item1 + " the 23rd", new DateObject(y, m, 23), true);
            BasicTest(GenWeekDaynDayMonthTest(15).Item1 + " the 15th", new DateObject(y, m, 15), true);
            BasicTest(GenWeekDaynDayMonthTest(21).Item1 + " the twenty first", new DateObject(y, m, 21), true);
            BasicTest(GenWeekDaynDayMonthTest(22).Item1 + " the twenty second", new DateObject(y, m, 22), true);
            BasicTest(GenWeekDaynDayMonthTest(15).Item1 + " the fifteen", new DateObject(y, m, 15), true);
        }

        [TestMethod]
        public void TestDateParseRelativeDayOfWeek()
        {
            BasicTest("I'll go back second Sunday", GenRelativeWeekDayAnswer(2, 0, DateObject.Now).Item1, true);
            BasicTest("I'll go back first Sunday", GenRelativeWeekDayAnswer(1, 0, DateObject.Now).Item1, true);
            BasicTest("I'll go back third Tuesday", GenRelativeWeekDayAnswer(3, 2, DateObject.Now).Item1, true);
            BasicTest("I'll go back third Tuesday", GenRelativeWeekDayAnswer(3, 2, DateObject.Now).Item1, true);
            // Negative case
            BasicTest("I'll go back fifth Sunday", GenRelativeWeekDayAnswer(5, 0, DateObject.Now).Item1, true);
        }

        [TestMethod]
        public void TestDateParseOdNumRelativeMonth()
        {
            BasicTest("I went back 20th of next month", new DateObject(2016, 12, 20));
            // Negative cases
            BasicTest("I went back 31st of this month", new DateObject(0001, 1, 1));
        }

        [TestMethod]
        public void TestDateParseLuis()
        {
            BasicTest("I'll go back on 15", "XXXX-XX-15");
            BasicTest("I'll go back Oct. 2", "XXXX-10-02");
            BasicTest("I'll go back Oct/2", "XXXX-10-02");
            BasicTest("I'll go back January 12, 2018", "2018-01-12");
            BasicTest("I'll go back 21/04/2016", "2016-04-21");
            BasicTest("I'll go back on 4.22", "XXXX-04-22");
            BasicTest("I'll go back on 4-22", "XXXX-04-22");
            BasicTest("I'll go back on    4/22", "XXXX-04-22");
            BasicTest("I'll go back on 22/04", "XXXX-04-22");
            BasicTest("I'll go back 21/04/16", "2016-04-21");
            BasicTest("I'll go back 9-18-15", "2015-09-18");
            BasicTest("I'll go back 2015/08/12", "2015-08-12");
            BasicTest("I'll go back 2015/08/12", "2015-08-12");
            BasicTest("I'll go back 08/12,2015", "2015-08-12");
            BasicTest("I'll go back 1st Jan", "XXXX-01-01");
            BasicTest("I'll go back Wed, 22 of Jan", "XXXX-01-22");

            BasicTest("I'll go back Jan first", "XXXX-01-01");
            BasicTest("I'll go back May twenty-first", "XXXX-05-21");
            BasicTest("I'll go back May twenty one", "XXXX-05-21");
            BasicTest("I'll go back second of Aug.", "XXXX-08-02");
            BasicTest("I'll go back twenty second of June", "XXXX-06-22");

            // cases below change with reference day
            BasicTest("I'll go back on Friday", "XXXX-WXX-5");
            BasicTest("I'll go back |Friday", "XXXX-WXX-5");
            BasicTest("I'll go back today", "2016-11-07");
            BasicTest("I'll go back tomorrow", "2016-11-08");
            BasicTest("I'll go back yesterday", "2016-11-06");
            BasicTest("I'll go back the day before yesterday", "2016-11-05");
            BasicTest("I'll go back the day after tomorrow", "2016-11-09");
            BasicTest("The day after tomorrow", "2016-11-09");
            BasicTest("I'll go back the next day", "2016-11-08");
            BasicTest("I'll go back next day", "2016-11-08");
            BasicTest("I'll go back this Friday", "2016-11-11");
            BasicTest("I'll go back next Sunday", "2016-11-20");
            BasicTest("I'll go back the day", "2016-11-07");
            BasicTest("I'll go back 15 June 2016", "2016-06-15");
            BasicTest("I went back two days ago", "2016-11-05");
            BasicTest("I went back two years ago", "2014-11-07");

            BasicTest("I'll go back two weeks from now", "2016-11-21");

            BasicTest("I'll go back next week on Friday", "2016-11-18");
            BasicTest("I'll go back on Friday next week", "2016-11-18");

        }

        [TestMethod]
        public void TestDateParseForTheLuis()
        {
            BasicTest("I went back for the 27", "XXXX-XX-27");
            BasicTest("I went back for the 27th", "XXXX-XX-27");
            BasicTest("I went back for the 27.", "XXXX-XX-27");
            BasicTest("I went back for the 27!", "XXXX-XX-27");
            BasicTest("I went back for the 27 .", "XXXX-XX-27");
            BasicTest("I went back for the 21st", "XXXX-XX-21");
            BasicTest("I went back for the 22nd", "XXXX-XX-22");
            BasicTest("I went back for the second", "XXXX-XX-02");
            BasicTest("I went back for the twenty second", "XXXX-XX-22");
            BasicTest("I went back for the thirty", "XXXX-XX-30");
        }

        [TestMethod]
        public void TestDateParseWeekDayAndDayOfMonthLuis()
        {
            int y = DateObject.Now.Year, m = DateObject.Now.Month;
            BasicTest(GenWeekDaynDayMonthTest(21).Item1 + " the 21st", GenWeekDaynDayMonthTest(21).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(22).Item1 + " the 22nd", GenWeekDaynDayMonthTest(22).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(23).Item1 + " the 23rd", GenWeekDaynDayMonthTest(23).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(15).Item1 + " the 15th", GenWeekDaynDayMonthTest(15).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(21).Item1 + " the twenty first", GenWeekDaynDayMonthTest(21).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(22).Item1 + " the twenty second", GenWeekDaynDayMonthTest(22).Item2, true);
            BasicTest(GenWeekDaynDayMonthTest(15).Item1 + " the fifteen", GenWeekDaynDayMonthTest(15).Item2, true);
        }

        [TestMethod]
        public void TestDateParseRelativeDayOfWeekLuis()
        {
            BasicTest("I'll go back second Sunday", GenRelativeWeekDayAnswer(2, 0, DateObject.Now).Item2, true);
            BasicTest("I'll go back first Sunday", GenRelativeWeekDayAnswer(1, 0, DateObject.Now).Item2, true);
            BasicTest("I'll go back third Tuesday", GenRelativeWeekDayAnswer(3, 2, DateObject.Now).Item2, true);
            // Negative case
            BasicTest("I'll go back fifth Sunday", GenRelativeWeekDayAnswer(5, 0, DateObject.Now).Item2, true);
        }

        [TestMethod]
        public void TestDateParseOdNumRelativeMonthLuis()
        {
            BasicTest("I went back 20th of next month", "2016-12-20");
            // Negative cases
            BasicTest("I went back 31st of this month", "2016-11-31");
        }
    }
}