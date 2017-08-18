using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestTimePeriodParser
    {
        readonly BaseTimePeriodExtractor extractor;
        readonly IDateTimeParser parser;

        readonly DateObject referenceTime;

        public TestTimePeriodParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 16, 12, 0);
            extractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
            parser = new BaseTimePeriodParser(new EnglishTimePeriodParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DateTimeResolutionResult) pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestTimePeriodParse()
        {
            int year = 2016, month = 11, day = 7, min = 0, second = 0;

            // basic match
            BasicTest("I'll be out 5 to 6pm",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("I'll be out 5 to 6 p.m",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("I'll be out 5 to seven in the morning",
                new DateObject(year, month, day, 5, min, second),
                new DateObject(year, month, day, 7, min, second));
            BasicTest("I'll be out from 5 to 6 pm",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("I'll be out between 5 and 6pm",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));
            BasicTest("I'll be out between 5pm and 6pm",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));
            BasicTest("I'll be out between 5 and 6 in the afternoon",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));

            BasicTest("I'll be out from 1am to 5pm",
                new DateObject(year, month, day, 1, min, second),
                new DateObject(year, month, day, 17, min, second));

            // merge two time points
            BasicTest("I'll be out 4pm till 5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("I'll be out 4:00 to 7 oclock",
                new DateObject(year, month, day, 4, min, second),
                new DateObject(year, month, day, 7, min, second));

            BasicTest("I'll be out 4pm-5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("I'll be out 4pm - 5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("I'll be out from 3 in the morning until 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("I'll be out between 3 in the morning and 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("I'll be out between 4pm and 5pm today",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));


            BasicTest("let's meet in the morning",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 12, min, second));
            BasicTest("let's meet in the afternoon",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));
            BasicTest("let's meet in the night",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));
            BasicTest("let's meet in the evening",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));
            BasicTest("let's meet in the evenings",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));

            BasicTest("let's meet in the early-mornings",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 10, min, second));
            BasicTest("let's meet in the late-mornings",
                new DateObject(year, month, day, 10, min, second),
                new DateObject(year, month, day, 12, min, second));
            BasicTest("let's meet in the early-morning",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 10, min, second));
            BasicTest("let's meet in the late-morning",
                new DateObject(year, month, day, 10, min, second),
                new DateObject(year, month, day, 12, min, second));
            BasicTest("let's meet in the early-afternoon",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 14, min, second));
            BasicTest("let's meet in the late-afternoon",
                new DateObject(year, month, day, 14, min, second),
                new DateObject(year, month, day, 16, min, second));
            BasicTest("let's meet in the early-evening",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("let's meet in the late-evening",
                new DateObject(year, month, day, 18, min, second),
                new DateObject(year, month, day, 20, min, second));
            BasicTest("let's meet in the early-night",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 22, min, second));
            BasicTest("let's meet in the late-night",
                new DateObject(year, month, day, 22, min, second),
                new DateObject(year, month, day, 23, 59, 59));
            BasicTest("let's meet in the early night",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 22, min, second));
            BasicTest("let's meet in the late night",
                new DateObject(year, month, day, 22, min, second),
                new DateObject(year, month, day, 23, 59, 59));
        }

        [TestMethod]
        public void TestTimePeriodParseLuis()
        {

            // basic match
            BasicTest("I'll be out 5 to 6pm", "(T17,T18,PT1H)");
            BasicTest("I'll be out 5 to 6 p.m", "(T17,T18,PT1H)");
            BasicTest("I'll be out 5 to seven in the morning", "(T05,T07,PT2H)");
            BasicTest("I'll be out from 5 to 6 pm", "(T17,T18,PT1H)");
            BasicTest("I'll be out from 1am to 5pm", "(T01,T17,PT16H)");


            // merge two time points
            BasicTest("I'll be out 4pm till 5pm", "(T16,T17,PT1H)");

            BasicTest("I'll be out 4:00 to 7 oclock", "(T04:00,T07,PT3H)");

            BasicTest("I'll be out 4pm-5pm", "(T16,T17,PT1H)");

            BasicTest("I'll be out 4pm - 5pm", "(T16,T17,PT1H)");

            BasicTest("I'll be out from 3 in the morning until 5pm", "(T03,T17,PT14H)");


            BasicTest("let's meet in the morning", "TMO");
            BasicTest("let's meet in the afternoon", "TAF");
            BasicTest("let's meet in the night", "TNI");
            BasicTest("let's meet in the evening", "TEV");

            BasicTest("let's meet in the early-mornings", "TMO");
            BasicTest("let's meet in the late-mornings", "TMO");
            BasicTest("let's meet in the early-morning", "TMO");
            BasicTest("let's meet in the late-morning", "TMO");
            BasicTest("let's meet in the early-afternoon", "TAF");
            BasicTest("let's meet in the late-afternoon", "TAF");
            BasicTest("let's meet in the early-evening", "TEV");
            BasicTest("let's meet in the late-evening", "TEV");
            BasicTest("let's meet in the early-night", "TNI");
            BasicTest("let's meet in the late-night", "TNI");
            BasicTest("let's meet in the early night", "TNI");
            BasicTest("let's meet in the late night", "TNI");
        }
    }
}