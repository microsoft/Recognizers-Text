using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
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
            extractor = new BaseTimePeriodExtractor(new FrenchTimePeriodExtractorConfiguration());
            parser = new BaseTimePeriodParser(new FrenchTimePeriodParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate, ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate, ((Tuple<DateObject, DateObject>)((DateTimeResolutionResult)pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestTimePeriodParse()
        {
            int year = 2016, month = 11, day = 7, min = 0, second = 0;

            // basic match
            BasicTest("Je vais dehors 5 au 6pm",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));

            BasicTest("Je suis sorti de 5 au 6 p.m",   // i'm out from 5 to 6 pm
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));

            BasicTest("Je suis sorti 5 au sept dans le matin",
                new DateObject(year, month, day, 5, min, second),
                new DateObject(year, month, day, 7, min, second));

            BasicTest("Je suis sorti 5 au 6 ce soir",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));

            BasicTest("Je suis sorti entre 5pm et 6pm",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));

            // between(entre) recognizes proper date, but not time
            //BasicTest("Je suis sorti entre 5 et 6pm",
            //    new DateObject(year, 11, 7, 17, min, second),
            //    new DateObject(year, 11, 7, 18, min, second));

            // Correct date, doesn't recognize proper time without am/pm in this context
            //BasicTest("Je suis sorti entre 5 et 6 dans l'apres midi",
            //    new DateObject(year, 11, 7, 17, min, second),
            //    new DateObject(year, 11, 7, 18, min, second));

            BasicTest("Je suis sorti de 1am au 5pm",
                new DateObject(year, month, day, 1, min, second),
                new DateObject(year, month, day, 17, min, second));

            // merge two time points
            BasicTest("Je reviendrai 4pm jusqu'a 5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Je vais retourner 4:00 au 7 heures",
                new DateObject(year, month, day, 4, min, second),
                new DateObject(year, month, day, 7, min, second));

            BasicTest("Je vais retourner 4pm-5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Je vais retourner 16:00 - 17:00",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Je suis sorti 3 du matin au 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Je suis sorti 3 du matin et 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Je vais retourner entre 4pm au 5pm aujourd'hui",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            // in the period_of_day
            BasicTest("recontrons nous de la matin",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 12, min, second));

            BasicTest("retrouvons nous apres midi",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));

            BasicTest("retrouvons nous l'apres-midi",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));

            BasicTest("recontrons nous dans la nuit",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));

            BasicTest("recontrons nous ce soir",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));

            BasicTest("recontrons nous dans la soiree",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));

            BasicTest("recontrons nous tot matin",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 10, min, second));

            BasicTest("recontrons nous fin de matin",
                new DateObject(year, month, day, 10, min, second),
                new DateObject(year, month, day, 12, min, second));

            BasicTest("recontrons nous tard matinee",
                new DateObject(year, month, day, 10, min, second),
                new DateObject(year, month, day, 12, min, second));

            BasicTest("recontrons nous debut matin",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 10, min, second));

            BasicTest("recontrons nous fin de matinee",
                new DateObject(year, month, day, 10, min, second),
                new DateObject(year, month, day, 12, min, second));

            BasicTest("recontrons nous tôt après midi",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 14, min, second));

            BasicTest("recontrons nous tôt d'après-midi",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 14, min, second));

            BasicTest("recontrons nous le tard d'apres midi",
                new DateObject(year, month, day, 14, min, second),
                new DateObject(year, month, day, 16, min, second));

            BasicTest("recontrons nous tôt soirée",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 18, min, second));

            BasicTest("recontrons nous le tard soiree",
                new DateObject(year, month, day, 18, min, second),
                new DateObject(year, month, day, 20, min, second));

            BasicTest("recontrons nous le tot nuit",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 22, min, second));

            BasicTest("recontrons nous le tard nuit",
                new DateObject(year, month, day, 22, min, second),
                new DateObject(year, month, day, 23, 59, 59));

            BasicTest("recontrons nous debut nuit",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 22, min, second));

            BasicTest("recontrons nous le fin de nuit",
                new DateObject(year, month, day, 22, min, second),
                new DateObject(year, month, day, 23, 59, 59));
        }

        [TestMethod]
        public void TestTimePeriodParseLuis()
        {

            // basic match
            BasicTest("Je vais dehors 5 au 6pm", "(T17,T18,PT1H)");
            BasicTest("Je suis sorti de 5 au 6 p.m", "(T17,T18,PT1H)");
            BasicTest("Je suis sorti 5 au sept dans le matin", "(T05,T07,PT2H)");
            BasicTest("Je suis sorti 5 au 6 ce soir", "(T17,T18,PT1H)");
            BasicTest("Je suis sorti entre 5pm et 6pm", "(T17,T18,PT1H)");

            // merge two time points
            BasicTest("Je reviendrai 4pm jusqu'a 5pm", "(T16,T17,PT1H)");

            BasicTest("Je vais retourner 4:00 au 7 heures", "(T04:00,T07,PT3H)");

            BasicTest("Je vais retourner 4pm-5pm", "(T16,T17,PT1H)");

            BasicTest("Je vais retourner 4pm - 5pm", "(T16,T17,PT1H)");

            BasicTest("Je suis sorti 3 du matin au 5pm", "(T03,T17,PT14H)");
            BasicTest("Je suis sorti 3 du matin jusqu'a 5pm", "(T03,T17,PT14H)"); // I'll be out from 3 in the morning till 5 pm

            // in the period_of_day
            BasicTest("recontrons nous de la matin", "TMO");
            BasicTest("retrouvons nous l'apres-midi", "TAF");
            BasicTest("recontrons nous dans la nuit", "TNI");
            BasicTest("recontrons nous ce soir", "TEV");

            BasicTest("recontrons nous tot matin", "TMO");
            BasicTest("recontrons nous tard matinee", "TMO");
            BasicTest("recontrons nous debut de matin", "TMO");
            BasicTest("recontrons nous le tard d'apres midi", "TAF");
            BasicTest("recontrons nous tôt soirée", "TEV");
            BasicTest("recontrons nous le tard soiree", "TEV");
            BasicTest("recontrons nous le tot nuit", "TNI");
            BasicTest("recontrons nous le tard nuit", "TNI");
            BasicTest("recontrons nous debut nuit", "TNI");
            BasicTest("recontrons nous le fin de nuit", "TNI");
        }
    }
}