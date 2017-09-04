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

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        public TestDateParser()
        {
            refrenceDay = new DateObject(2016, 11, 7);
            parser = new BaseDateParser(new EnglishDateParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
            extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
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
    }
}