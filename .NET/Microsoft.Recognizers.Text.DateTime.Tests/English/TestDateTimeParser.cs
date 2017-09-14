using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestDateTimeParser
    {
        readonly BaseDateTimeExtractor extractor;
        readonly IDateTimeParser parser;
        readonly DateObject referenceTime;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Eng", typeof(BaseDateTimeParser));
        }

        public TestDateTimeParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 0, 0, 0);
            extractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
            parser = new BaseDateTimeParser(new EnglishDateTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Eng", parser, referenceTime, text, pr);
        }

        public void BasicTest(string text, DateObject futreTime, DateObject pastTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(futreTime, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(pastTime, ((DateTimeResolutionResult) pr.Value).PastValue);
            TestWriter.Write("Eng", parser, referenceTime, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Eng", parser, referenceTime, text, pr);
        }

        [TestMethod]
        public void TestDateTimeParse()
        {
            int year = 2016, month = 11, day = 7, hour = 0, min = 0, second = 0;
            BasicTest("I'll go back now", new DateObject(2016, 11, 7, 0, 0, 0));
            BasicTest("I'll go back as soon as possible", new DateObject(year, month, day, hour, min, second));
            BasicTest("I'll go back on 15 at 8:00", new DateObject(year, month, 15, 8, 0, second),
                new DateObject(year, month - 1, 15, 8, 0, second));
            BasicTest("I'll go back on 15 at 8:00:20", new DateObject(year, month, 15, 8, 0, 20),
                new DateObject(year, month - 1, 15, 8, 0, 20));
            BasicTest("I'll go back on 15, 8pm", new DateObject(year, month, 15, 20, min, second),
                new DateObject(year, month - 1, 15, 20, min, second));
            BasicTest("I'll go back at 5th at 4 a.m.", new DateObject(year, month + 1, 5, 4, min, second),
                new DateObject(year, month, 5, 4, min, second));
            BasicTest("I'll go back 04/21/2016, 8:00pm", new DateObject(2016, 4, 21, 20, 0, second));
            BasicTest("I'll go back 04/21/2016, 8:00:20pm", new DateObject(2016, 4, 21, 20, 0, 20));
            BasicTest("I'll go back Oct.23 at seven", new DateObject(year + 1, 10, 23, 7, min, second),
                new DateObject(year, 10, 23, 7, min, second));
            BasicTest("I'll go back October 14 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("I'll go back October 14 8:00:31am", new DateObject(year + 1, 10, 14, 8, 0, 31),
                new DateObject(year, 10, 14, 8, 0, 31));
            BasicTest("I'll go back October 14 around 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second), new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("I'll go back October 14 for 8:00:31am", new DateObject(year + 1, 10, 14, 8, 0, 31), new DateObject(year, 10, 14, 8, 0, 31));
            BasicTest("I'll go back October 14, 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("I'll go back October 14, 8:00:25am", new DateObject(year + 1, 10, 14, 8, 0, 25),
                new DateObject(year, 10, 14, 8, 0, 25));
            BasicTest("I'll go back May 5, 2016, 20 min past eight in the evening",
                new DateObject(2016, 5, 5, 20, 20, second));

            BasicTest("I'll go back 8pm on 15", new DateObject(year, month, 15, 20, min, second),
                new DateObject(year, month - 1, 15, 20, min, second));
            BasicTest("I'll go back 8pm on the 15", new DateObject(year, month, 15, 20, min, second),
                      new DateObject(year, month - 1, 15, 20, min, second));
            BasicTest("I'll go back at seven on 15", new DateObject(year, month, 15, 7, min, second),
                new DateObject(year, month - 1, 15, 7, min, second));
            BasicTest("I'll go back 8pm today", new DateObject(year, month, day, 20, min, second));
            BasicTest("I'll go back a quarter to seven tomorrow", new DateObject(year, month, 8, 6, 45, second));
            BasicTest("I'll go back 19:00, 2016-12-22", new DateObject(2016, 12, 22, 19, 0, second));

            BasicTest("I'll go back tomorrow 8:00am", new DateObject(year, month, 8, 8, 0, second));
            BasicTest("I'll go back tomorrow morning at 7", new DateObject(2016, 11, 8, 7, min, second));
            BasicTest("I'll go back tonight around 7", new DateObject(2016, 11, 7, 19, min, second));
            BasicTest("I'll go back 7:00 on next Sunday afternoon", new DateObject(2016, 11, 20, 19, min, second));
            BasicTest("I'll go back twenty minutes past five tomorrow morning",
                new DateObject(2016, 11, 8, 5, 20, second));
            BasicTest("I'll go back 7, this morning", new DateObject(year, month, day, 7, min, second));
            BasicTest("I'll go back 10, tonight", new DateObject(year, month, day, 22, min, second));

            BasicTest("I'll go back 8pm in the evening, Sunday", new DateObject(2016, 11, 13, 20, min, second),
                new DateObject(2016, 11, 6, 20, min, second));
            BasicTest("I'll go back 8pm in the evening, 1st Jan", new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));
            BasicTest("I'll go back 8pm in the evening, 1 Jan", new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));
            BasicTest("I'll go back 10pm tonight", new DateObject(2016, 11, 7, 22, min, second));
            BasicTest("I'll go back 8am this morning", new DateObject(2016, 11, 7, 8, min, second));
            BasicTest("I'll go back 8pm this evening", new DateObject(2016, 11, 7, 20, min, second));
            BasicTest("I'll go back the end of the day", new DateObject(2016, 11, 7, 23, 59, second));
            BasicTest("I'll go back end of tomorrow", new DateObject(2016, 11, 8, 23, 59, second));
            BasicTest("I'll go back end of the sunday", new DateObject(2016, 11, 13, 23, 59, second),
                new DateObject(2016, 11, 6, 23, 59, second));

            BasicTest("I'll go back in 5 hours", new DateObject(2016, 11, 7, hour + 5, min, second),
                new DateObject(2016, 11, 7, hour + 5, min, second));
        }

        [TestMethod]
        public void TestDateTimeLuis()
        {
            BasicTest("I'll go back as soon as possible", "FUTURE_REF");
            BasicTest("I'll go back on 15 at 8:00", "XXXX-XX-15T08:00");
            BasicTest("I'll go back on 15 at 8:00:24", "XXXX-XX-15T08:00:24");
            BasicTest("I'll go back on 15, 8pm", "XXXX-XX-15T20");
            BasicTest("I'll go back 04/21/2016, 8:00pm", "2016-04-21T20:00");
            BasicTest("I'll go back 04/21/2016, 8:00:24pm", "2016-04-21T20:00:24");
            BasicTest("I'll go back Oct.23 at seven", "XXXX-10-23T07");
            BasicTest("I'll go back October 14 8:00am", "XXXX-10-14T08:00");
            BasicTest("I'll go back October 14 8:00:13am", "XXXX-10-14T08:00:13");
            BasicTest("I'll go back October 14, 8:00am", "XXXX-10-14T08:00");
            BasicTest("I'll go back October 14, 8:00:25am", "XXXX-10-14T08:00:25");
            BasicTest("I'll go back May 5, 2016, 20 min past eight in the evening", "2016-05-05T20:20");

            BasicTest("I'll go back 8pm on 15", "XXXX-XX-15T20");
            BasicTest("I'll go back at seven on 15", "XXXX-XX-15T07");
            BasicTest("I'll go back 8pm today", "2016-11-07T20");
            BasicTest("I'll go back a quarter to seven tomorrow", "2016-11-08T06:45");
            BasicTest("I'll go back 19:00, 2016-12-22", "2016-12-22T19:00");
            BasicTest("I'll go back now", "PRESENT_REF");

            BasicTest("I'll go back tomorrow 8:00am", "2016-11-08T08:00");
            BasicTest("I'll go back tomorrow morning at 7", "2016-11-08T07");
            //BasicTestFuture("I'll go back Oct. 5 in the afternoon at 7", "XXXX-10-05T19");
            BasicTest("I'll go back 7:00 on next Sunday afternoon", "2016-11-20T19:00");
            BasicTest("I'll go back twenty minutes past five tomorrow morning", "2016-11-08T05:20");

            BasicTest("I'll go back 8pm in the evening, Sunday", "XXXX-WXX-7T20");
            BasicTest("I'll go back 8pm in the evening, 1st Jan", "XXXX-01-01T20");
            BasicTest("I'll go back 8pm in the evening, 1 Jan", "XXXX-01-01T20");
            BasicTest("I'll go back 10pm tonight", "2016-11-07T22");
            BasicTest("I'll go back 8am this morning", "2016-11-07T08");
            BasicTest("I'll go back 8pm this evening", "2016-11-07T20");

            BasicTest("I'll go back this morning at 7", "2016-11-07T07");
            BasicTest("I'll go back this morning at 7am", "2016-11-07T07");
            BasicTest("I'll go back this morning at seven", "2016-11-07T07");
            BasicTest("I'll go back this morning at 7:00", "2016-11-07T07:00");
            BasicTest("I'll go back this night at 7", "2016-11-07T19");
            BasicTest("I'll go back tonight at 7", "2016-11-07T19");

            BasicTest("I'll go back 2016-12-16T12:23:59", "2016-12-16T12:23:59");

            BasicTest("I'll go back in 5 hours", "2016-11-07T05:00:00");
        }
    }
}