using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDateTimeParser
    {
        readonly BaseDateTimeExtractor extractor;
        readonly IDateTimeParser parser;
        readonly DateObject referenceTime;

        public TestDateTimeParser()
        {
            referenceTime = new DateObject(2016, 11, 7, 0, 0, 0);
            extractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            parser = new BaseDateTimeParser(new FrenchDateTimeParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        public void BasicTest(string text, DateObject futreTime, DateObject pastTime)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(futreTime, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(pastTime, ((DateTimeResolutionResult)pr.Value).PastValue);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_DATETIME, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestDateTimeParse()
        {
            int year = 2016, month = 11, day = 7, hour = 0, min = 0, second = 0;
            BasicTest("Je reviendrai maintenant", new DateObject(2016, 11, 7, 0, 0, 0));
            BasicTest("Je reviendrai dès que possible", new DateObject(year, month, day, hour, min, second));
            BasicTest("Je reviendrai dqp", new DateObject(year, month, day, hour, min, second));

            BasicTest("Je reviendrai 21/04/2016, 8:00pm", new DateObject(2016, 4, 21, 20, 0, second));
            BasicTest("Je reviendrai 21/04/2016, 8:00:20pm", new DateObject(2016, 4, 21, 20, 0, 20));
            //BasicTest("Je reviendrai Oct 23 a sept", new DateObject(year + 1, 10, 23, 7, min, second), new DateObject(year, 10, 23, 7, min, second));
            BasicTest("Je reviendrai 14 Octobre 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("Je reviendrai 14 Octobre 8:00:31am", new DateObject(year + 1, 10, 14, 8, 0, 31),
                new DateObject(year, 10, 14, 8, 0, 31));
            //BasicTest("Je reviendrai 14 Octobre vers 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second), new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("Je reviendrai 14 Octobre pour 8:00:31am", new DateObject(year + 1, 10, 14, 8, 0, 31), new DateObject(year, 10, 14, 8, 0, 31));
            BasicTest("Je reviendrai 14 Octobre, 8:00am", new DateObject(year + 1, 10, 14, 8, 0, second),
                new DateObject(year, 10, 14, 8, 0, second));
            BasicTest("Je reviendrai 14 Octobre, 8:00:25am", new DateObject(year + 1, 10, 14, 8, 0, 25),
                new DateObject(year, 10, 14, 8, 0, 25));
            //BasicTest("reviendrai 5 Mai 2016, 20 min apres huit dans la nuit",
            //    new DateObject(2016, 5, 5, 20, 20, second));

            BasicTest("Je reviendrai 8pm en 15", new DateObject(year, month, 15, 20, min, second),
                new DateObject(year, month - 1, 15, 20, min, second));
            //BasicTest("Je reviendrai sept en 15", new DateObject(year, month, 15, 7, min, second),
            //    new DateObject(year, month - 1, 15, 7, min, second));
            BasicTest("Je reviendrai 8pm aujourd'hui", new DateObject(year, month, day, 20, min, second));
            //BasicTest("Je reviendrai sept heures et quart lendemain", new DateObject(year, month, 8, 6, 45, second));
            BasicTest("Je reviendrai 19:00, 2016-12-22", new DateObject(2016, 12, 22, 19, 0, second));

            BasicTest("Je reviendrai lendemain 8:00am", new DateObject(year, month, 8, 8, 0, second));
            BasicTest("Je reviendrai demain matin a 7", new DateObject(2016, 11, 8, 7, min, second));
            BasicTest("Je reviendrai ce soir vers 7", new DateObject(2016, 11, 7, 19, min, second));
            BasicTest("Je reviendrai 19:00 sur dimanche prochain d'apres-midi", new DateObject(2016, 11, 20, 19, min, second));
            BasicTest("Je reviendrai 9:00 sur dimanche prochain", new DateObject(2016, 11, 20, 9, min, second));
            BasicTest("Je reviendrai 9:00 sur dimanche derniere", new DateObject(2016, 11, 6, 9, min, second));
            //BasicTest("Je reviendrai vingt minutes dernier cinq lendemain matin",
            //    new DateObject(2016, 11, 8, 5, 20, second));
            BasicTest("Je reviendrai 7, ce matin", new DateObject(year, month, day, 7, min, second));
            BasicTest("Je reviendrai 10, ce soir", new DateObject(year, month, day, 22, min, second));
            BasicTest("Je reviendrai 23, ce soir", new DateObject(year, month, day, 23, min, second));
            BasicTest("Je reviendrai 11, du soir", new DateObject(year, month, day, 23, min, second));
            BasicTest("Je reviendrai 8pm dans la soiree, Dimanche", new DateObject(2016, 11, 13, 20, min, second),
                new DateObject(2016, 11, 6, 20, min, second));
            BasicTest("Je reviendrai 8pm dans la soiree, 1 Jan", new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));

            //BasicTest("Je reviendrai 10pm ce soir", new DateObject(2016, 11, 7, 22, min, second));
            //BasicTest("Je reviendrai 8am ce matin", new DateObject(2016, 11, 7, 8, min, second));
            //BasicTest("Je reviendrai 8pm dans la soiree", new DateObject(2016, 11, 7, 20, min, second));
            BasicTest("Je reviendrai la fin de la journee", new DateObject(2016, 11, 7, 23, 59, second));
            BasicTest("Je reviendrai la fin demain", new DateObject(2016, 11, 8, 23, 59, second));
            BasicTest("Je reviendrai la fin de dimanche", new DateObject(2016, 11, 13, 23, 59, second),
                new DateObject(2016, 11, 6, 23, 59, second));

            BasicTest("Je reviendrai dans 5 heures", new DateObject(2016, 11, 7, hour + 5, min, second),
                new DateObject(2016, 11, 7, hour + 5, min, second));

            // Note: '1er' doesn't seem to work..
            //BasicTest("Je reviendrai 8pm dans la soiree, 1er Jan", new DateObject(2017, 1, 1, 20, min, second),
            //    new DateObject(2016, 1, 1, 20, min, second));
            BasicTest("Je reviendrai 8pm dans la soiree, 1 Jan", new DateObject(2017, 1, 1, 20, min, second),
                new DateObject(2016, 1, 1, 20, min, second));

            //BasicTest("Je reviendrai sur 15 a 8:00", new DateObject(year, month, 15, 8, 0, second),
            //    new DateObject(year, month - 1, 15, 8, 0, second));
            //BasicTest("Je reviendrai on 15 at 8:00:20", new DateObject(year, month, 15, 8, 0, 20),
            //    new DateObject(year, month - 1, 15, 8, 0, 20));
            //BasicTest("Je reviendrai on 15, 8pm", new DateObject(year, month, 15, 20, min, second),
            //    new DateObject(year, month - 1, 15, 20, min, second));
            //BasicTest("Je reviendrai a 5 au 4 a.m.", new DateObject(year, month + 1, 5, 4, min, second),
            //    new DateObject(year, month, 5, 4, min, second));
        }

        [TestMethod]
        public void TestDateTimeLuis()
        {
            BasicTest("Je reviendrai dès que possible", "FUTURE_REF");
            BasicTest("Je reviendrai 21/04/2016, 8:00pm", "2016-04-21T20:00");
            BasicTest("Je reviendrai 21/04/2016, 20:00", "2016-04-21T20:00");
            BasicTest("Je reviendrai 20:00, 21/04/2016", "2016-04-21T20:00");
            BasicTest("Je reviendrai 21/04/2016, 8:00:24pm", "2016-04-21T20:00:24");
            BasicTest("Je reviendrai 21/04/2016, 20:00:24", "2016-04-21T20:00:24");
            BasicTest("Je reviendrai 20:00:24, 21/04/2016", "2016-04-21T20:00:24");
            //BasicTest("Je reviendrai Oct.23 sur sept", "XXXX-10-23T07");
            BasicTest("Je reviendrai Octobre 14 8:00am", "XXXX-10-14T08:00");
            BasicTest("Je reviendrai Octobre 14 8:00:13am", "XXXX-10-14T08:00:13");
            BasicTest("Je reviendrai Octobre 14, 8:00am", "XXXX-10-14T08:00");
            BasicTest("Je reviendrai Octobre 14, 8:00:25am", "XXXX-10-14T08:00:25");
            //BasicTest("Je reviendrai 5 Mai, 2016, 20 min past eight in the evening", "2016-05-05T20:20");

            BasicTest("Je reviendrai 20:00 sur 15", "XXXX-XX-15T20:00");
            BasicTest("Je reviendrai 20:00 sur la 15", "XXXX-XX-15T20:00");
            BasicTest("Je reviendrai a sept sur 15", "XXXX-XX-15T07");
            BasicTest("Je reviendrai 8pm aujourd'hui", "2016-11-07T20");
            BasicTest("Je reviendrai a huit ce soir aujourd'hui", "2016-11-07T20");
           // BasicTest("Je reviendrai a quart et sept lendemain", "2016-11-08T06:45");
            BasicTest("Je reviendrai 19:00, 2016-12-22", "2016-12-22T19:00");
            BasicTest("Je reviendrai maintenant", "PRESENT_REF");

            BasicTest("Je reviendrai lendemain 8:00", "2016-11-08T08:00");
            BasicTest("Je reviendrai demain du matin a 7", "2016-11-08T07");
            //BasicTestFuture("Je reviendrai Oct. 5 in the afternoon at 7", "XXXX-10-05T19");
            BasicTest("Je reviendrai 7:00 du soiree dimanche prochain", "2016-11-20T19:00");
            //BasicTest("Je reviendrai cinq heures et quart de matin lendemain", "2016-11-08T05:20");

            BasicTest("Je reviendrai 8 dans la soiree, dimanche", "XXXX-WXX-7T20");
            BasicTest("Je reviendrai 8 dans la soiree, 1 Jan", "XXXX-01-01T20");
            BasicTest("Je reviendrai 8pm ce soir, 1 Jan", "XXXX-01-01T20");
            BasicTest("Je reviendrai 22 ce soir", "2016-11-07T22");
            BasicTest("Je reviendrai 8 du matin", "2016-11-07T08");

            BasicTest("Je reviendrai ce matin a 7", "2016-11-07T07");
            BasicTest("Je reviendrai cette matin a 7am", "2016-11-07T07");
            BasicTest("Je reviendrai ce matin a sept", "2016-11-07T07");
           // BasicTest("Je reviendrai ce matin a 7:00", "2016-11-07T07:00"); // resolves to 2016-11-07T07
            BasicTest("Je reviendrai ce soir a 7", "2016-11-07T19");

            BasicTest("Je reviendrai 2016-12-16T12:23:59", "2016-12-16T12:23:59");

            BasicTest("Je reviendrai en 5 heures", "2016-11-07T05:00:00");

            //BasicTest("Je reviendrai sur 15 a 8:00", "XXXX-XX-15T08:00");
            //BasicTest("Je reviendrai sur 15 a 8:00:24", "XXXX-XX-15T08:00:24");
            //BasicTest("Je reviendrai sur 15, 8pm", "XXXX-XX-15T20");
            // BasicTest("Je reviendrai 8pm dans la soiree", "2016-11-07T20");
        }
    }
}