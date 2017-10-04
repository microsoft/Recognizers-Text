using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;


namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestFrenchModel
    {
        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestWriter.Close(TestCulture.French, typeof(DateTimeModel));
        }

        public void BasicTest(DateTimeModel model, DateObject baseDateTime, string text, string expectedType, string expectedString, string expectedTimex)
        {
            var results = model.Parse(text, baseDateTime);
            Assert.AreEqual(1, results.Count);
            var result = results.First();
            Assert.AreEqual(expectedString, result.Text);

            var values = result.Resolution["values"] as IEnumerable<Dictionary<string, string>>;
            Assert.AreEqual(expectedType, values.First()["type"]);
            Assert.AreEqual(expectedTimex, values.First()["timex"]);
            TestWriter.Write(TestCulture.French, model, baseDateTime, text, results);
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
            TestWriter.Write(TestCulture.French, model, baseDateTime, text, results);
        }

        [TestMethod]
        public void TestDateTime_Date()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je reviendrai Oct/2",
                Constants.SYS_DATETIME_DATE, "oct/2", "XXXX-10-02", "2017-10-02", "2016-10-02");

            BasicTest(model, reference,
                "Je reviendrai sur 22/04",
                Constants.SYS_DATETIME_DATE, "22/04", "XXXX-04-22", "2017-04-22", "2016-04-22");

            BasicTest(model, reference,
                "Je reviendrai Mai vingt-neuf",
                Constants.SYS_DATETIME_DATE, "mai vingt-neuf", "XXXX-05-29", "2017-05-29", "2016-05-29");

            //BasicTest(model, reference,
            //    "Je reviendrai seconde de Aout",
            //    Constants.SYS_DATETIME_DATE, "seconde de aout", "XXXX-08-02", "2017-08-02", "2016-08-02");

            BasicTest(model, reference,
                "Je reviendrai aujourd'hui",
                Constants.SYS_DATETIME_DATE, "aujourd'hui", "2016-11-07");

            BasicTest(model, reference,
                "Je reviendrai lendemain",
                Constants.SYS_DATETIME_DATE, "lendemain", "2016-11-08");

            BasicTest(model, reference,
                "Je reviendrai hier",
                Constants.SYS_DATETIME_DATE, "hier", "2016-11-06");

            BasicTest(model, reference,
                "Je reviendrai vendredi",
                Constants.SYS_DATETIME_DATE, "vendredi", "XXXX-WXX-5", "2016-11-11", "2016-11-04");
        }

        [TestMethod]
        public void TestDateTime_DatePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je reviendrai de 4-23 mois prochain",
                Constants.SYS_DATETIME_DATEPERIOD, "4-23 mois prochain", "(2016-12-04,2016-12-23,P19D)");

            BasicTest(model, reference,
                "I'll be out entre 3 et 12 de Sept hahaha",
                Constants.SYS_DATETIME_DATEPERIOD, "entre 3 et 12 de sept", "(XXXX-09-03,XXXX-09-12,P9D)");

            BasicTest(model, reference,
                "Je vais sortir cette septembre",
                Constants.SYS_DATETIME_DATEPERIOD, "cette septembre", "2016-09");

            BasicTest(model, reference,
                "Je vais sortir 2015-3",
                Constants.SYS_DATETIME_DATEPERIOD, "2015-3", "2015-03");

            BasicTest(model, reference,
                "Je vais sortir cette été",
                Constants.SYS_DATETIME_DATEPERIOD, "cette été", "2016-SU");

            // Correct time, however extra blank space
            //BasicTest(model, reference,
            //    "Je vais sortir 21 janvier, 2016 - 22/01/2016",
            //    Constants.SYS_DATETIME_DATEPERIOD, "21 janvier, 2016 - 22/01/2016", "(2016-01-12,2016-01-22,P10D)");

            //BasicTest(model, reference,
            //    "Je vais sortir les 3 jours prochain",
            //    Constants.SYS_DATETIME_DATEPERIOD, "3 jours prochain", "(2016-11-08,2016-11-11,P3D)");

            //BasicTest(model, reference,
            //    "Je vais sortir la fin juillet",
            //    Constants.SYS_DATETIME_DATEPERIOD, "la fin juillet", "XXXX-07-W04");

        }

        [TestMethod]
        public void TestDateTime_DateTime()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je reviendrai maintenant",
                Constants.SYS_DATETIME_DATETIME, "maintenant", "PRESENT_REF");

            BasicTest(model, reference,
                "Je reviendrai 14 Octobre 8:00:31am",
                Constants.SYS_DATETIME_DATETIME, "14 octobre 8:00:31am", "XXXX-10-14T08:00:31");

            BasicTest(model, reference,
                "Je reviendrai lendemain 8:00am",
                Constants.SYS_DATETIME_DATETIME, "lendemain 8:00am", "2016-11-08T08:00");

            BasicTest(model, reference,
                "Je reviendrai 10, ce soir",
                Constants.SYS_DATETIME_DATETIME, "10, ce soir", "2016-11-07T22");

            // resolves 2 matches
            //BasicTest(model, reference,
            //    "Je reviendrai 8 dans du matin",
            //    Constants.SYS_DATETIME_DATETIME, "8 dans du matin", "2016-11-07T08");

            BasicTest(model, reference,
                "Je reviendrai fin de demain", // end of tomorrow
                Constants.SYS_DATETIME_DATETIME, "fin de demain", "2016-11-08T23:59");

            BasicTest(model, reference,
                "Je reviendrai fin de dimanche",
                Constants.SYS_DATETIME_DATETIME, "fin de dimanche", "XXXX-WXX-7T23:59");

            BasicTest(model, reference,
                "Je reviendrai fin de cette dimanche",
                Constants.SYS_DATETIME_DATETIME, "fin de cette dimanche", "2016-11-13T23:59");
        }

        [TestMethod]
        public void TestDateTime_DateTimePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7, 16, 12, 0);

            BasicTest(model, reference,
                "Je serai de 5 a 7 aujourd'hui",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "de 5 a 7 aujourd'hui", "(2016-11-07T05,2016-11-07T07,PT2H)");

            BasicTest(model, reference,
                "Je serai de 5 a 6pm 22 Avril",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "de 5 a 6pm 22 avril", "(XXXX-04-22T17,XXXX-04-22T18,PT1H)");

            BasicTest(model, reference,
                "Je serai de 3:00 au 4:00 lendemain",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "de 3:00 au 4:00 lendemain", "(2016-11-08T03:00,2016-11-08T04:00,PT1H)");

            BasicTest(model, reference,
                "Je reviendrai demain nuit",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "demain nuit", "2016-11-08TNI");

            BasicTest(model, reference,
                "Je reviendrai mardi dans le matin",
                Constants.SYS_DATETIME_DATETIMEPERIOD, "mardi dans le matin", "XXXX-WXX-2TMO");

            //BasicTest(model, reference,
            //    "Je reviendrai heure prochaine",
            //    Constants.SYS_DATETIME_DATETIMEPERIOD, "heure prochaine", "(2016-11-07T16:12:00,2016-11-07T17:12:00,PT1H)");

            // These two tests resolves to 'timerange' instead of 'datetimerange'
            //BasicTest(model, reference,
            //    "Je reviendrai ce soir",
            //    Constants.SYS_DATETIME_DATETIMEPERIOD, "ce soir", "2016-11-07TEV");

            //BasicTest(model, reference,
            //    "Je reviendrai cette lundi apres-midi",
            //    Constants.SYS_DATETIME_DATETIMEPERIOD, "cette lundi apres-midi", "2016-11-14TAF");
        }

        [TestMethod]
        public void TestDateTime_Duration()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je vais partir pour 3heures",
                Constants.SYS_DATETIME_DURATION, "3heures", "PT3H");

            BasicTest(model, reference,
                "Je vais partir pour 3.5ans",
                Constants.SYS_DATETIME_DURATION, "3.5ans", "P3.5Y");

            BasicTest(model, reference,
                "je vais partir pour 3 minutes",
                Constants.SYS_DATETIME_DURATION, "3 minutes", "PT3M");

            BasicTest(model, reference,
                "JE vais partir pour 123,45 sec",
                Constants.SYS_DATETIME_DURATION, "123,45 sec", "PT123.45S");

            BasicTest(model, reference,
                "Je vais partir toute le jour",
                Constants.SYS_DATETIME_DURATION, "toute le jour", "P1D");

            BasicTest(model, reference,
                "Je vais partir pour vingt-quatre heures",
                Constants.SYS_DATETIME_DURATION, "vingt-quatre heures", "PT24H");

            BasicTest(model, reference,
                "Je vais partir toute le mois",
                Constants.SYS_DATETIME_DURATION, "toute le mois", "P1M");

            // resolves to 'time' not 'duration'
            //BasicTest(model, reference,
            //    "je vais partir pour un heure",
            //    Constants.SYS_DATETIME_DURATION, "un heure", "PT1H");

            BasicTest(model, reference,
                "Je vais partir pour quelques heures",
                Constants.SYS_DATETIME_DURATION, "quelques heures", "PT3H");

            BasicTest(model, reference,
                "Je vais partir pour quel ques minutes",
                Constants.SYS_DATETIME_DURATION, "quel ques minutes", "PT3M");

            BasicTest(model, reference,
                "Je vais partir pour quelques jours",
                Constants.SYS_DATETIME_DURATION, "quelques jours", "P3D");

            BasicTest(model, reference,
                "Je vais partir pour quelques semaines",
                Constants.SYS_DATETIME_DURATION, "quelques semaines", "P3W");
        }

        [TestMethod]
        public void TestDateTime_Set()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je vais partir hebdomadaire",
                Constants.SYS_DATETIME_SET, "hebdomadaire", "P1W");

            BasicTest(model, reference,
                "Je vais partir tous les jours",
                Constants.SYS_DATETIME_SET, "tous les jours", "P1D");

            BasicTest(model, reference,
                "Je vais partir annuellement",
                Constants.SYS_DATETIME_SET, "annuellement", "P1Y");

            BasicTest(model, reference,
                "Je vais partir chaque deux jours",
                Constants.SYS_DATETIME_SET, "chaque deux jours", "P2D");

            BasicTest(model, reference,
                "Je vais partir toutes les trois semaines",
                Constants.SYS_DATETIME_SET, "toutes les trois semaines", "P3W");

            BasicTest(model, reference,
                "Je vais partir chaque lundi",
                Constants.SYS_DATETIME_SET, "chaque lundi", "XXXX-WXX-1");

            BasicTest(model, reference,
                "Je vais partir 4pm chaque lundi",
                Constants.SYS_DATETIME_SET, "4pm chaque lundi", "XXXX-WXX-1T16");

            BasicTest(model, reference,
                "Je vais partir 16 chaque lundi",
                Constants.SYS_DATETIME_SET, "16 chaque lundi", "XXXX-WXX-1T16");

            // Returns P1D, instead of T15
            //BasicTest(model, reference,
            //    "Je vais partir 15 tous les jour",
            //    Constants.SYS_DATETIME_SET, "15 tous les jour", "T15");

            //BasicTest(model, reference,
            //    "Je vais partir 15 chaque jour",
            //    Constants.SYS_DATETIME_SET, "15 chaque jour", "T15");
        }

        [TestMethod]
        public void TestDateTime_Time()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7);

            BasicTest(model, reference,
                "Je retournerai 7:56:30 pm",
                Constants.SYS_DATETIME_TIME, "7:56:30 pm", "T19:56:30");

            BasicTest(model, reference,
                "Je retournerai 19:56:30",
                Constants.SYS_DATETIME_TIME, "19:56:30", "T19:56:30");

            BasicTest(model, reference,
                "C'est sept et demie heures",
                Constants.SYS_DATETIME_TIME, "sept et demie heures", "T07:30");

            BasicTest(model, reference,
                "C'est 8 h et vingt minute dans la soiree",
                Constants.SYS_DATETIME_TIME, "8 h et vingt minute dans la soiree", "T20:20");

            BasicTest(model, reference,
                "Je retournerai dans le matin a 7",
                Constants.SYS_DATETIME_TIME, "dans le matin a 7", "T07");

            //BasicTest(model, reference,
            //    "Je retournerai a 7 d'apres midi",
            //    Constants.SYS_DATETIME_TIME, "7 d'apres midi", "T19");

            //BasicTest(model, reference,
            //    "Je retournerai peu près midi",
            //    Constants.SYS_DATETIME_TIME, "peu près midi", "T12");

            //BasicTest(model, reference,
            //    "Je retournerai peu pres a 11",
            //    Constants.SYS_DATETIME_TIME, "peu pres a 11", "T11");

            BasicTest(model, reference,
                "Je retournerai 1140 a.m.",
                Constants.SYS_DATETIME_TIME, "1140 a.m.", "T11:40");
        }

        [TestMethod]
        public void TestDateTime_TimePeriod()
        {
            var model = DateTimeRecognizer.GetInstance().GetDateTimeModel(Culture.French);
            var reference = new DateObject(2016, 11, 7, 16, 12, 0);

            BasicTest(model, reference,
                "Je vais partir 5 a 6pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "5 a 6pm", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "Je vais partir 17 a 18",
                Constants.SYS_DATETIME_TIMEPERIOD, "17 a 18", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "Je vais partir 5 a sept du matin",
                Constants.SYS_DATETIME_TIMEPERIOD, "5 a sept du matin", "(T05,T07,PT2H)");

            BasicTest(model, reference,
                "Je vais partir entre 5pm et 6 apres-midi",
                Constants.SYS_DATETIME_TIMEPERIOD, "entre 5pm et 6 apres-midi", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "Je vais partir entre 17 et 18 apres-midi",
                Constants.SYS_DATETIME_TIMEPERIOD, "entre 17 et 18 apres-midi", "(T17,T18,PT1H)");

            BasicTest(model, reference,
                "Je vais partir entre 5 du matin et 6 apres-midi",
                Constants.SYS_DATETIME_TIMEPERIOD, "5 du matin et 6 apres-midi", "(T05,T18,PT13H)");

            BasicTest(model, reference,
                "Je vais partir 4:00 a 7 heures",
                Constants.SYS_DATETIME_TIMEPERIOD, "4:00 a 7 heures", "(T04:00,T07,PT3H)");

            BasicTest(model, reference,
                "Je vais partir du 3 matin jusqu'a 5pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "3 matin jusqu'a 5pm", "(T03,T17,PT14H)");

            BasicTest(model, reference,
                "Je vais partir entre 4pm et 5pm",
                Constants.SYS_DATETIME_TIMEPERIOD, "entre 4pm et 5pm", "(T16,T17,PT1H)");

            BasicTest(model, reference,
                "rencontrons-nous dans le matin",
                Constants.SYS_DATETIME_TIMEPERIOD, "dans le matin", "TMO");

            BasicTest(model, reference,
                "rencontrons-nouse ce soir",
                Constants.SYS_DATETIME_TIMEPERIOD, "ce soir", "TEV");

            BasicTest(model, reference,
                "rencontrons-nouse dans la soiree",
                Constants.SYS_DATETIME_TIMEPERIOD, "dans la soiree", "TEV");
        }
    }
}
