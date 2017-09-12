using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
{
    [TestClass]
    public class TestTimeParser
    {
        readonly BaseTimeExtractor extractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseTimeParser(new SpanishTimeParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close("Spa", typeof(BaseTimeParser));
        }


        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            TestWriter.Write("Spa", parser, text, pr);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0]);
            Assert.AreEqual(Constants.SYS_DATETIME_TIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
            TestWriter.Write("Spa", parser, text, pr);
        }

        [TestMethod]
        public void TestTimeParse()
        {
            var today = DateObject.Today;
            int year = today.Year, month = today.Month, day = today.Day, min = 0, second = 0;
            BasicTest("Volvere a las 7", new DateObject(year, month, day, 7, min, second));
            BasicTest("Volvere a las siete", new DateObject(year, month, day, 7, min, second));
            BasicTest("Volvere a las 7pm", new DateObject(year, month, day, 19, min, second));
            BasicTest("Volvere a las 7:56pm", new DateObject(year, month, day, 19, 56, second));
            BasicTest("Volvere a las 7:56:30pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("Volvere a las 7:56:30 pm", new DateObject(year, month, day, 19, 56, 30));
            BasicTest("Volvere a las 12:34", new DateObject(year, month, day, 12, 34, second));
            BasicTest("Volvere a las 12:34:25 ", new DateObject(year, month, day, 12, 34, 25));

            BasicTest("Son las 7 en punto", new DateObject(year, month, day, 7, min, second));
            BasicTest("Son las siete en punto", new DateObject(year, month, day, 7, min, second));
            BasicTest("Son las 8 de la mañana", new DateObject(year, month, day, 8, min, second));
            BasicTest("Son las 8 de la tarde", new DateObject(year, month, day, 20, min, second));
            BasicTest("Son las 8 de la noche", new DateObject(year, month, day, 20, min, second));


            BasicTest("Son las ocho y media", new DateObject(year, month, day, 8, 30, second));
            BasicTest("Son las 8pm y media", new DateObject(year, month, day, 20, 30, second));
            BasicTest("Son 30 mins pasadas las ocho", new DateObject(year, month, day, 8, 30, second));
            BasicTest("Son las ocho y cuarto", new DateObject(year, month, day, 8, 15, second));
            BasicTest("Son cuarto pasadas las ocho", new DateObject(year, month, day, 8, 15, second));
            BasicTest("Son cuarto pasadas de las ocho", new DateObject(year, month, day, 8, 15, second));
            BasicTest("Son 9pm menos cuarto", new DateObject(year, month, day, 20, 45, second));
            BasicTest("Faltan 3 minutos para las ocho", new DateObject(year, month, day, 7, 57, second));

            BasicTest("Son siete y media en punto", new DateObject(year, month, day, 7, 30, second));
            BasicTest("Son siete y media de la tarde", new DateObject(year, month, day, 19, 30, second));
            BasicTest("Son siete y media de la mañana", new DateObject(year, month, day, 7, 30, second));
            BasicTest("Son ocho menos cuarto de la mañana", new DateObject(year, month, day, 7, 45, second));
            BasicTest("Son 20 min pasadas las ocho de la tarde", new DateObject(year, month, day, 20, 20, second));

            BasicTest("Volvere por la tarde a las 7", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Volvere a la tarde a las 7", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Volvere a la tarde a las 7:00", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Volvere a la tarde a las 7:00:14", new DateObject(year, month, day, 19, 0, 14));

            BasicTest("Volvere a la tarde a las siete pm", new DateObject(year, month, day, 19, 0, second));
            BasicTest("Volvere a las siete treinta pm", new DateObject(year, month, day, 19, 30, second));
            BasicTest("Volvere a las siete treinta y cinco pm", new DateObject(year, month, day, 19, 35, second));
            BasicTest("Volvere a las once y cinco pm", new DateObject(year, month, day, 23, 5, second));

            BasicTest("Volvere 340pm", new DateObject(year, month, day, 15, 40, second));
            BasicTest("Volvere 1140 a.m.", new DateObject(year, month, day, 11, 40, second));
        }

        [TestMethod]
        public void TestTimeParseLuis()
        {
            BasicTest("Volvere a las 7", "T07");
            BasicTest("Volvere a las siete", "T07");
            BasicTest("Volvere a las 7pm", "T19");
            BasicTest("Volvere a las 7:56pm", "T19:56");
            BasicTest("Volvere a las 7:56:30pm", "T19:56:30");
            BasicTest("Volvere a las 7:56:13 pm", "T19:56:13");
            BasicTest("Volvere a las 12:34", "T12:34");
            BasicTest("Volvere a las 12:34:45 ", "T12:34:45");

            BasicTest("Son las 7 en punto", "T07");
            BasicTest("Son las siete en punto", "T07");
            BasicTest("Son las 8 de la mañana", "T08");
            BasicTest("Son las 8 de la noche", "T20");
            
            BasicTest("Son las ocho y media", "T08:30");
            BasicTest("Son las 8pm y media", "T20:30");
            BasicTest("Son 30 mins pasadas las ocho", "T08:30");
            BasicTest("Son las ocho y cuarto", "T08:15");
            BasicTest("Son 9pm menos cuarto", "T20:45");
            BasicTest("Faltan 3 minutos para las ocho", "T07:57");

            BasicTest("Son siete y media en punto", "T07:30");
            BasicTest("Son siete y media de la tarde", "T19:30");
            BasicTest("Son siete y media de la mañana", "T07:30");
            BasicTest("Son ocho menos cuarto de la mañana", "T07:45");
            BasicTest("Son 20 min pasadas las ocho de la tarde", "T20:20");

            BasicTest("Volvere por la tarde a las 7", "T19");
            BasicTest("Volvere a la tarde a las 7", "T19");
            BasicTest("Volvere a la tarde a las 7:00", "T19:00");
            BasicTest("Volvere a la tarde a las 7:00:25", "T19:00:25");

            BasicTest("Volvere a la tarde a las siete pm", "T19");
            BasicTest("Volvere a las siete treinta am", "T07:30");
            BasicTest("Volvere a las siete treinta y cinco pm", "T19:35");
            BasicTest("Volvere a las once y cinco", "T11:05");
            BasicTest("Volvere en 3 min para las cinco treinta", "T05:27");
            BasicTest("Volvere a las cinco y media de la noche", "T17:30");
            BasicTest("Volvere en la tarde a las cinco treinta", "T17:30");
        }
    }
}