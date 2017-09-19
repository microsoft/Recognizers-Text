using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{

    [TestClass]
    public class TestTimeParser
    {
        readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        readonly IDateTimeParser parser = new TimeParser(new EnglishTimeParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestTimeParseWithTwoNumbers()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, second = 0;

            BasicTest("set an alarm for eight forty", new DateObject(year, month, day, 8, 40, second));
            BasicTest("set an alarm for eight forty am", new DateObject(year, month, day, 8, 40, second));
            BasicTest("set an alarm for eight forty pm", new DateObject(year, month, day, 20, 40, second));
            BasicTest("set an alarm for ten forty five", new DateObject(year, month, day, 10, 45, second));
            BasicTest("set an alarm for fifteen fifteen p m", new DateObject(year, month, day, 15 , 15, second));
            BasicTest("set an alarm for fifteen thirty p m", new DateObject(year, month, day, 15, 30, second));
            BasicTest("set an alarm for ten ten", new DateObject(year, month, day, 10, 10, second));
            BasicTest("set an alarm for ten fifty five p. m.", new DateObject(year, month, day, 22, 55, second));
        }

        [TestMethod]
        public void TestTimeParse()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, min = 0, second = 0;
            BasicTest("I'll be back at 7ampm", new DateObject(year, month, day, 7, min, second));
            BasicTest("I'll be back at 7", new DateObject(year, month, day, 7, min, second));
            BasicTest("I'll be back at seven", new DateObject(year, month, day, 7, min, second));
            BasicTest("I'll be back 7pm", new DateObject(year, month, day, 19, min, second));
            BasicTest("I'll be back 7:56pm", new DateObject(year, month, day, 19, 56, second));
            BasicTest("I'll be back 7:56:30pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("I'll be back 7:56:30 pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("I'll be back 12:34", new DateObject(year, month, day, 12, 34, second));
            BasicTest("I'll be back 12:34:25 ", new DateObject(year, month, day, 12, 34, 25));

            BasicTest("It's 7 o'clock", new DateObject(year, month, day, 7, min, second));
            BasicTest("It's seven o'clock", new DateObject(year, month, day, 7, min, second));
            BasicTest("It's 8 in the morning", new DateObject(year, month, day, 8, min, second));
            BasicTest("It's 8 in the night", new DateObject(year, month, day, 20, min, second));

            BasicTest("It's half past eight", new DateObject(year, month, day, 8, 30, second));
            BasicTest("It's half past 8pm", new DateObject(year, month, day, 20, 30, second));
            BasicTest("It's 30 mins past eight", new DateObject(year, month, day, 8, 30, second));
            BasicTest("It's a quarter past eight", new DateObject(year, month, day, 8, 15, second));
            BasicTest("It's quarter past eight", new DateObject(year, month, day, 8, 15, second));
            BasicTest("It's three quarters past 9pm", new DateObject(year, month, day, 21, 45, second));
            BasicTest("It's three minutes to eight", new DateObject(year, month, day, 7, 57, second));

            BasicTest("It's half past seven o'clock", new DateObject(year, month, day, 7, 30, second));
            BasicTest("It's half past seven afternoon", new DateObject(year, month, day, 19, 30, second));
            BasicTest("It's half past seven in the morning", new DateObject(year, month, day, 7, 30, second));
            BasicTest("It's a quarter to 8 in the morning", new DateObject(year, month, day, 7, 45, second));
            BasicTest("It's 20 min past eight in the evening", new DateObject(year, month, day, 20, 20, second));

            BasicTest("I'll be back in the afternoon at 7", new DateObject(year, month, day, 19, 0, second));
            BasicTest("I'll be back afternoon at 7", new DateObject(year, month, day, 19, 0, second));
            BasicTest("I'll be back afternoon 7:00", new DateObject(year, month, day, 19, 0, second));
            BasicTest("I'll be back afternoon 7:00:05", new DateObject(year, month, day, 19, 0, 05));
            BasicTest("I'll be back afternoon seven pm", new DateObject(year, month, day, 19, 0, second));

            BasicTest("I'll go back seven thirty pm", new DateObject(year, month, day, 19, 30, second));
            BasicTest("I'll go back seven thirty five pm", new DateObject(year, month, day, 19, 35, second));
            BasicTest("I'll go back eleven twenty pm", new DateObject(year, month, day, 23, 20, second));

            BasicTest("I'll be back noonish", new DateObject(year, month, day, 12, 0, second));
            BasicTest("I'll be back 12 noon", new DateObject(year, month, day, 12, 0, second));
            BasicTest("I'll be back 11ish", new DateObject(year, month, day, 11, 0, second));
            BasicTest("I'll be back 11-ish", new DateObject(year, month, day, 11, 0, second));

            BasicTest("I'll be back 340pm", new DateObject(year, month, day, 15, 40, second));
            BasicTest("I'll be back 1140 a.m.", new DateObject(year, month, day, 11, 40, second));
            BasicTest("I'll be back 1140 a.m.", new DateObject(year, month, day, 11, 40, second));

            BasicTest("midnight", new DateObject(year, month, day, 0, 0, second));
            BasicTest("mid-night", new DateObject(year, month, day, 0, 0, second));
            BasicTest("mid night", new DateObject(year, month, day, 0, 0, second));
            BasicTest("midmorning", new DateObject(year, month, day, 10, 0, second));
            BasicTest("mid-morning", new DateObject(year, month, day, 10, 0, second));
            BasicTest("mid morning", new DateObject(year, month, day, 10, 0, second));
            BasicTest("midafternoon", new DateObject(year, month, day, 14, 0, second));
            BasicTest("mid-afternoon", new DateObject(year, month, day, 14, 0, second));
            BasicTest("mid afternoon", new DateObject(year, month, day, 14, 0, second));
            BasicTest("midday", new DateObject(year, month, day, 12, 0, second));
            BasicTest("mid-day", new DateObject(year, month, day, 12, 0, second));
            BasicTest("mid day", new DateObject(year, month, day, 12, 0, second));
            BasicTest("noon", new DateObject(year, month, day, 12, 0, second));
        }

        [TestMethod]
        public void TestTimeParseMealTime()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, min = 0, second = 0;
            BasicTest("I'll be back 12 lunchtime", new DateObject(year, month, day, 12, 0, 0));
            BasicTest("I'll be back 12 midnight", new DateObject(year, month, day, 0, 0, 0));
            BasicTest("I'll be back 12 in the night", new DateObject(year, month, day, 0, 0, 0));
            BasicTest("I'll be back 1 o'clock midnight", new DateObject(year, month, day, 1, 0, 0));
            BasicTest("I'll be back 12 o'clock lunchtime", new DateObject(year, month, day, 12, 0, 0));
            BasicTest("I'll be back 11 o'clock lunchtime", new DateObject(year, month, day, 11, 0, 0));
            BasicTest("I'll be back 1 o'clock lunchtime", new DateObject(year, month, day, 13, 0, 0));
            BasicTest("I'll be back 12 o'clock lunch", new DateObject(year, month, day, 12, 0, 0));
            BasicTest("I'll be back 8 o'clock dinner", new DateObject(year, month, day, 20, 0, 0));
            BasicTest("I'll be back 8 o'clock breakfast", new DateObject(year, month, day, 8, 0, 0));
            BasicTest("I'll be back at lunch 12 o'clock", new DateObject(year, month, day, 12, 0, 0));
            BasicTest("I'll be back at lunchtime 11 o'clock", new DateObject(year, month, day, 11, 0, 0));
        }

        [TestMethod]
        public void TestTimeParseLuis()
        {
            BasicTest("I'll be back at 7", "T07");
            BasicTest("I'll be back at seven", "T07");
            BasicTest("I'll be back 7pm", "T19");
            BasicTest("I'll be back 7:56pm", "T19:56");
            BasicTest("I'll be back 7:56:30pm", "T19:56:30");
            BasicTest("I'll be back 7:56:13 pm", "T19:56:13");
            BasicTest("I'll be back 12:34", "T12:34");
            BasicTest("I'll be back 12:34:45 ", "T12:34:45");

            BasicTest("It's 7 o'clock", "T07");
            BasicTest("It's seven o'clock", "T07");
            BasicTest("It's 8 in the morning", "T08");
            BasicTest("It's 8 in the night", "T20");

            BasicTest("It's half past eight", "T08:30");
            BasicTest("It's half past 8pm", "T20:30");
            BasicTest("It's 30 mins past eight", "T08:30");
            BasicTest("It's a quarter past eight", "T08:15");
            BasicTest("It's three quarters past 9pm", "T21:45");
            BasicTest("It's three minutes to eight", "T07:57");

            BasicTest("It's half past seven o'clock", "T07:30");
            BasicTest("It's half past seven afternoon", "T19:30");
            BasicTest("It's half past seven in the morning", "T07:30");
            BasicTest("It's a quarter to 8 in the morning", "T07:45");
            BasicTest("It's 20 min past eight in the evening", "T20:20");

            BasicTest("I'll be back in the afternoon at 7", "T19");
            BasicTest("I'll be back afternoon at 7", "T19");
            BasicTest("I'll be back afternoon 7:00", "T19:00");
            BasicTest("I'll be back afternoon 7:00:25", "T19:00:25");
            BasicTest("I'll be back afternoon seven pm", "T19");

            BasicTest("I'll go back seven thirty am", "T07:30");
            BasicTest("I'll go back seven thirty five pm", "T19:35");
            BasicTest("I'll go back at eleven five", "T11:05");
            BasicTest("I'll go back three mins to five thirty ", "T05:27");
            BasicTest("I'll go back five thirty in the night", "T17:30");
            BasicTest("I'll go back in the night five thirty", "T17:30");

            BasicTest("I'll be back noonish", "T12");
            BasicTest("I'll be back noon", "T12");
            BasicTest("I'll be back 12 noon", "T12");
            BasicTest("I'll be back 11ish", "T11");
            BasicTest("I'll be back 11-ish", "T11");

            //TODO: discussion on the definition
            //Default time period definition for now
            //LUIS Time Resolution Spec address: https://microsoft.sharepoint.com/teams/luis_core/_layouts/15/WopiFrame2.aspx?sourcedoc=%7B852DBAAF-911B-4CCC-9401-996505EC9B67%7D&file=LUIS%20Time%20Resolution%20Spec.docx&action=default
            //a.Morning: 08:00:00 - 12:00:00
            //b.Afternoon: 12:00:00 – 16:00:00
            //c.Evening: 16:00:00 – 20:00:00
            //d.Night: 20:00:00 – 23:59:59
            //e.Daytime: 08:00:00 – 16:00:00(morning + afternoon)
            BasicTest("midnight", "T00");
            BasicTest("mid-night", "T00");
            BasicTest("mid night", "T00");
            BasicTest("midmorning", "T10");
            BasicTest("mid-morning", "T10");
            BasicTest("mid morning", "T10");
            BasicTest("midafternoon", "T14");
            BasicTest("mid-afternoon", "T14");
            BasicTest("mid afternoon", "T14");
            BasicTest("midday", "T12");
            BasicTest("mid-day", "T12");
            BasicTest("mid day", "T12");
            BasicTest("noon", "T12");
        }

        [TestMethod]
        public void TestTimeParseMealTimeLuis()
        {
            BasicTest("I'll be back 12 o'clock lunchtime", "T12");
            BasicTest("I'll be back 12 o'clock lunch", "T12");
            BasicTest("I'll be back 8 o'clock dinner", "T20");
            BasicTest("I'll be back 8 o'clock breakfast", "T08");
            BasicTest("I'll be back at lunch 12 o'clock", "T12");
            BasicTest("I'll be back at lunchtime 12 o'clock", "T12");
        }
    }
}