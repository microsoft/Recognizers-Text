using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;


namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestEnglishModel
    {
        public void BasicTest(DateTimeModel model, DateObject baseDateTime, string text, string expectedType, string expectedString, string expectedTimex)
        {
            var results = model.Parse(text, baseDateTime);
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual(expectedString, result.Text);

            var values = result.Resolution["values"] as IEnumerable<Dictionary<string, string>>;
            Assert.AreEqual(expectedType, values.First()["type"]);
            Assert.AreEqual(expectedTimex, values.First()["timex"]);
        }

        public void BasicTest(DateTimeModel model, DateObject baseDateTime, string text, string expectedType, string expectedString, string expectedTimex, string expectedFuture, string expectedPast)
        {
            var results = model.Parse(text, baseDateTime);
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual(expectedString, result.Text);

            var values = result.Resolution["values"] as IEnumerable<Dictionary<string, string>>;

            Assert.AreEqual(expectedType, values.First()["type"]);
            Assert.AreEqual(expectedTimex, values.First()["timex"]);

            Assert.AreEqual(expectedFuture ?? values.Last()["value"], values.Last()["value"]);

            Assert.AreEqual(expectedPast ?? values.First()["value"], values.First()["value"]);
        }

        [TestMethod]
        public void TestDateTime_Date()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll go back Oct/2",
                Constants.SYS_DATETIME_DATE, "oct/2", "XXXX-10-02", "2017-10-02", "2016-10-02");

            BasicTest(model, reference,
                "I'll go back on 22/04",
                Constants.SYS_DATETIME_DATE, "22/04", "XXXX-04-22", "2017-04-22", "2016-04-22");

            BasicTest(model, reference,
                "I'll go back May twenty nine",
                Constants.SYS_DATETIME_DATE, "may twenty nine", "XXXX-05-29", "2017-05-29", "2016-05-29");

            BasicTest(model, reference,
                "I'll go back second of Aug.",
                Constants.SYS_DATETIME_DATE, "second of aug", "XXXX-08-02", "2017-08-02", "2016-08-02");

            BasicTest(model, reference,
                "I'll go back today",
                Constants.SYS_DATETIME_DATE, "today", "2016-11-07");

            BasicTest(model, reference,
                "I'll go back tomorrow",
                Constants.SYS_DATETIME_DATE, "tomorrow", "2016-11-08");

            BasicTest(model, reference,
                "I'll go back yesterday",
                Constants.SYS_DATETIME_DATE, "yesterday", "2016-11-06");

            BasicTest(model, reference,
                "I'll go back on Friday",
                Constants.SYS_DATETIME_DATE, "friday", "XXXX-WXX-5", "2016-11-11", "2016-11-04");
        }

        [TestMethod]
        public void TestDateTime_DatePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll be out from 4-23 in next month",
                Constants.SYS_DATETIME_DATEPERIOD, "from 4-23 in next month", "(2016-12-04,2016-12-23,P19D)");

            BasicTest(model, reference,
                "I'll be out between 3 and 12 of Sept hahaha",
                Constants.SYS_DATETIME_DATEPERIOD, "between 3 and 12 of sept", "(XXXX-09-03,XXXX-09-12,P9D)");

            BasicTest(model, reference,
                "I'll be out this September",
                Constants.SYS_DATETIME_DATEPERIOD, "this september", "2016-09");

            BasicTest(model, reference,
                "I'll be out January 12, 2016 - 01/22/2016",
                Constants.SYS_DATETIME_DATEPERIOD, "january 12, 2016 - 01/22/2016", "(2016-01-12,2016-01-22,P10D)");

            BasicTest(model, reference,
                "I'll be out next 3 days",
                Constants.SYS_DATETIME_DATEPERIOD, "next 3 days", "(2016-11-08,2016-11-11,P3D)");

            BasicTest(model, reference,
                "I'll be out the last week of july",
                Constants.SYS_DATETIME_DATEPERIOD, "the last week of july", "XXXX-07-W04");

            BasicTest(model, reference,
                "I'll be out 2015-3",
                Constants.SYS_DATETIME_DATEPERIOD, "2015-3", "2015-03");

            BasicTest(model, reference,
                "I'll leave this SUMMER",
                Constants.SYS_DATETIME_DATEPERIOD, "this summer", "2016-SU");

            BasicTest(model, reference,
                "I'll be out since tomorrow",
                Constants.SYS_DATETIME_DATEPERIOD, "since tomorrow", "2016-11-08");

            BasicTest(model, reference,
                "I'll be out since August",
                Constants.SYS_DATETIME_DATEPERIOD, "since august", "XXXX-08");

            BasicTest(model, reference,
                "I'll be out since this August",
                Constants.SYS_DATETIME_DATEPERIOD, "since this august", "2016-08");
        }

        [TestMethod]
        public void TestDateTime_DateTime()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll go back now",
                Constants.SYS_DATETIME_DATETIME, "now", "PRESENT_REF");
            
            BasicTest(model, reference,
                "I'll go back October 14 for 8:00:31am",
                Constants.SYS_DATETIME_DATETIME, "october 14 for 8:00:31am", "XXXX-10-14T08:00:31");

            BasicTest(model, reference,
                "I'll go back tomorrow 8:00am",
                Constants.SYS_DATETIME_DATETIME, "tomorrow 8:00am", "2016-11-08T08:00");

            BasicTest(model, reference,
                "I'll go back 10, tonight",
                Constants.SYS_DATETIME_DATETIME, "10, tonight", "2016-11-07T22");

            BasicTest(model, reference,
                "I'll go back 8am this morning",
                Constants.SYS_DATETIME_DATETIME, "8am this morning", "2016-11-07T08");

            BasicTest(model, reference,
                "I'll go back end of tomorrow",
                Constants.SYS_DATETIME_DATETIME, "end of tomorrow", "2016-11-08T23:59");

            BasicTest(model, reference,
                "I'll go back end of the sunday",
                Constants.SYS_DATETIME_DATETIME, "end of the sunday", "XXXX-WXX-7T23:59");

            BasicTest(model, reference,
                "I'll go back end of this sunday",
                Constants.SYS_DATETIME_DATETIME, "end of this sunday", "2016-11-13T23:59");
        }

        [TestMethod]
        public void TestDateTime_DateTimePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7, 16, 12, 0);

            BasicTest(model, reference,
                "I'll be out five to seven today",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "five to seven today", "(2016-11-07T05,2016-11-07T07,PT2H)");

            BasicTest(model, reference,
                "I'll be out from 5 to 6pm of April 22",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "from 5 to 6pm of april 22", "(XXXX-04-22T17,XXXX-04-22T18,PT1H)");

            BasicTest(model, reference,
                "I'll be out 3:00 to 4:00 tomorrow",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "3:00 to 4:00 tomorrow", "(2016-11-08T03:00,2016-11-08T04:00,PT1H)");

            BasicTest(model, reference,
                "I'll go back this evening",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "this evening", "2016-11-07TEV");

            BasicTest(model, reference,
                "I'll go back tomorrow night",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "tomorrow night", "2016-11-08TNI");

            BasicTest(model, reference,
                "I'll go back next monday afternoon",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "next monday afternoon", "2016-11-14TAF");

            BasicTest(model, reference,
                "I'll go back next hour",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "next hour", "(2016-11-07T16:12:00,2016-11-07T17:12:00,PT1H)");

            BasicTest(model, reference,
                "I'll go back tuesday in the morning",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "tuesday in the morning", "XXXX-WXX-2TMO");
        }

        [TestMethod]
        public void TestDateTime_Duration()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll leave for 3h",
                Constants.SYS_DATETIME_DURATION, "3h", "PT3H");

            BasicTest(model, reference,
                "I'll leave for 3.5years",
                Constants.SYS_DATETIME_DURATION, "3.5years", "P3.5Y");

            BasicTest(model, reference,
                "I'll leave for 3 minutes",
                Constants.SYS_DATETIME_DURATION, "3 minutes", "PT3M");

            BasicTest(model, reference,
                "I'll leave for 123.45 sec",
                Constants.SYS_DATETIME_DURATION, "123.45 sec", "PT123.45S");

            BasicTest(model, reference,
                "I'll leave for all day",
                Constants.SYS_DATETIME_DURATION, "all day", "P1D");

            BasicTest(model, reference,
                "I'll leave for twenty and four hours",
                Constants.SYS_DATETIME_DURATION, "twenty and four hours", "PT24H");

            BasicTest(model, reference,
                "I'll leave for all month",
                Constants.SYS_DATETIME_DURATION, "all month", "P1M");

            BasicTest(model, reference,
                "I'll leave for an hour",
                Constants.SYS_DATETIME_DURATION, "an hour", "PT1H");

            BasicTest(model, reference,
                "I'll leave for few hours",
                Constants.SYS_DATETIME_DURATION, "few hours", "PT3H");

            BasicTest(model, reference,
                "I'll leave for a few minutes",
                Constants.SYS_DATETIME_DURATION, "a few minutes", "PT3M");

            BasicTest(model, reference,
                "I'll leave for some days",
                Constants.SYS_DATETIME_DURATION, "some days", "P3D");

            BasicTest(model, reference,
                "I'll leave for several weeks",
                Constants.SYS_DATETIME_DURATION, "several weeks", "P3W");
        }

        [TestMethod]
        public void TestDateTime_Set()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll leave weekly",
                Constants.SYS_DATETIME_SET, "weekly", "P1W");

            BasicTest(model, reference,
                "I'll leave every day",
                Constants.SYS_DATETIME_SET, "every day", "P1D");

            BasicTest(model, reference,
                "I'll leave annually",
                Constants.SYS_DATETIME_SET, "annually", "P1Y");

            BasicTest(model, reference,
                "I'll leave each two days",
                Constants.SYS_DATETIME_SET, "each two days", "P2D");

            BasicTest(model, reference,
                "I'll leave every three week",
                Constants.SYS_DATETIME_SET, "every three week", "P3W");

            BasicTest(model, reference,
                "I'll leave 3pm each day",
                Constants.SYS_DATETIME_SET, "3pm each day", "T15");

            BasicTest(model, reference,
                "I'll leave every monday",
                Constants.SYS_DATETIME_SET, "every monday", "XXXX-WXX-1");

            BasicTest(model, reference,
                "I'll leave each monday at 4pm",
                Constants.SYS_DATETIME_SET, "each monday at 4pm", "XXXX-WXX-1T16");
        }

        [TestMethod]
        public void TestDateTime_Time()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "I'll be back 7:56:30 pm",
                Constants.SYS_DATETIME_TIME, "7:56:30 pm", "T19:56:30");

            BasicTest(model, reference,
                "It's half past seven o'clock",
                Constants.SYS_DATETIME_TIME, "half past seven o'clock", "T07:30");

            BasicTest(model, reference,
                "It's 20 min past eight in the evening",
                Constants.SYS_DATETIME_TIME, "20 min past eight in the evening", "T20:20");

            BasicTest(model, reference,
                "I'll be back in the morning at 7",
                Constants.SYS_DATETIME_TIME, "in the morning at 7", "T07");

            BasicTest(model, reference,
                "I'll be back in the afternoon at 7",
                Constants.SYS_DATETIME_TIME, "in the afternoon at 7", "T19");

            BasicTest(model, reference,
                "I'll be back noonish",
                Constants.SYS_DATETIME_TIME, "noonish", "T12");

            BasicTest(model, reference,
                "I'll be back 11ish",
                Constants.SYS_DATETIME_TIME, "11ish", "T11");

            BasicTest(model, reference,
                "I'll be back 1140 a.m.",
                Constants.SYS_DATETIME_TIME, "1140 a.m.", "T11:40");

            BasicTest(model, reference,
                      "12 noon",
                      Constants.SYS_DATETIME_TIME, "12 noon", "T12");
        }

        [TestMethod]
        public void TestDateTime_TimePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.English);
            var reference = new DateObject(2016, 11, 7, 16, 12, 0);

            BasicTest(model, reference,
                "I'll be out 5 to 6pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "5 to 6pm", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "I'll be out 5 to seven in the morning",
                Constants.SYS_DATETIME_TIMEPERIOD, "5 to seven in the morning", "(T05,T07,PT2H)");

            BasicTest(model, reference,
                "I'll be out between 5 and 6 in the afternoon",
                Constants.SYS_DATETIME_TIMEPERIOD, "between 5 and 6 in the afternoon", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "I'll be out 4:00 to 7 oclock",
                Constants.SYS_DATETIME_TIMEPERIOD, "4:00 to 7 oclock", "(T04:00,T07,PT3H)");

            BasicTest(model, reference,
                "I'll be out from 3 in the morning until 5pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "from 3 in the morning until 5pm", "(T03,T17,PT14H)");

            BasicTest(model, reference,
                "I'll be out between 4pm and 5pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "between 4pm and 5pm", "(T16,T17,PT1H)");

            BasicTest(model, reference,
                "let's meet in the morning",
                Constants.SYS_DATETIME_TIMEPERIOD, "in the morning", "TMO");

            BasicTest(model, reference,
                "let's meet in the evening",
                Constants.SYS_DATETIME_TIMEPERIOD, "in the evening", "TEV");
        }
    }
}
