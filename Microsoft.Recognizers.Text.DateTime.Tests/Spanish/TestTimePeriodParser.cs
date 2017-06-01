using System;
using Microsoft.Recognizers.Text.DateTime.Spanish.Extractors;
using Microsoft.Recognizers.Text.DateTime.Spanish.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Tests
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
            extractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
            parser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(new SpanishCommonDateTimeParserConfiguration()));
        }

        public void BasicTest(string text, DateObject beginDate, DateObject endDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(beginDate,
                ((Tuple<DateObject, DateObject>) ((DTParseResult) pr.Value).FutureValue).Item1);
            Assert.AreEqual(endDate,
                ((Tuple<DateObject, DateObject>) ((DTParseResult) pr.Value).FutureValue).Item2);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceTime);
            Assert.AreEqual(Constants.SYS_DATETIME_TIMEPERIOD, pr.Type);
            Assert.AreEqual(luisValueStr, ((DTParseResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestTimePeriodParse()
        {
            int year = 2016, month = 11, day = 7, min = 0, second = 0;

            // basic match
            BasicTest("Estaré afuera de 5 a 6pm",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("Estaré afuera de 5 a 6p.m.",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("Estaré afuera de 5 a siete de la mañana",
                new DateObject(year, month, day, 5, min, second),
                new DateObject(year, month, day, 7, min, second));
            BasicTest("Estaré afuera desde las 5 hasta las 6 pm",
                new DateObject(year, month, day, 17, min, second),
                new DateObject(year, month, day, 18, min, second));
            BasicTest("Estaré afuera entre las 5 y 6pm",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));
            BasicTest("Estaré afuera entre 5pm y 6pm",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));
            BasicTest("Estaré afuera entre las 5 y 6 de la tarde",
                new DateObject(year, 11, 7, 17, min, second),
                new DateObject(year, 11, 7, 18, min, second));


            // merge two time points
            BasicTest("Estaré fuera desde las 4pm hasta 5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Estaré fuera desde las 4:00 hasta las 7 en punto",
                new DateObject(year, month, day, 4, min, second),
                new DateObject(year, month, day, 7, min, second));

            BasicTest("Estaré fuera 4pm-5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Estaré fuera 4pm - 5pm",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Estaré fuera desde las 3 de la mañana hasta las 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Estaré fuera entre las 3 de la madrugada y las 5pm",
                new DateObject(year, month, day, 3, min, second),
                new DateObject(year, month, day, 17, min, second));

            BasicTest("Estaré fuera entre las 4pm y 5pm de hoy",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 17, min, second));


            BasicTest("Nos veamos a la madrugada",
                new DateObject(year, month, day, 4, min, second),
                new DateObject(year, month, day, 8, min, second));
            BasicTest("Nos veamos a la mañana",
                new DateObject(year, month, day, 8, min, second),
                new DateObject(year, month, day, 12, min, second));
            BasicTest("No vemos pasado el mediodia",
                new DateObject(year, month, day, 12, min, second),
                new DateObject(year, month, day, 16, min, second));
            BasicTest("Nos vemos en la noche",
                new DateObject(year, month, day, 20, min, second),
                new DateObject(year, month, day, 23, 59, 59));
            BasicTest("Nos vemos a la tarde",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));/*
            BasicTest("let's meet in the evenings",
                new DateObject(year, month, day, 16, min, second),
                new DateObject(year, month, day, 20, min, second));*/
        }

        [TestMethod]
        public void TestTimePeriodParseLuis()
        {

            // basic match
            BasicTest("Estaré afuera de 5 a 6pm", "(T17,T18,PT1H)");
            BasicTest("Estaré afuera de 5 a 6p.m.", "(T17,T18,PT1H)");
            BasicTest("Estaré afuera de 5 a siete de la mañana", "(T05,T07,PT2H)");
            BasicTest("Estaré afuera entre las 5 y 6pm", "(T17,T18,PT1H)");


            // merge two time points
            BasicTest("Estaré fuera desde las 4pm hasta 5pm", "(T16,T17,PT1H)");

            BasicTest("Estaré fuera desde las 4:00 hasta las 7 en punto", "(T04:00,T07,PT3H)");

            BasicTest("Estaré fuera 4pm-5pm", "(T16,T17,PT1H)");

            BasicTest("Estaré fuera 4pm - 5pm", "(T16,T17,PT1H)");

            BasicTest("Estaré fuera desde las 3 de la mañana hasta las 5pm", "(T03,T17,PT14H)");


            BasicTest("Nos vemos a la madrugada", "TDA");
            BasicTest("Nos vemos a la mañana", "TMO");
            BasicTest("Nos vemos pasado el mediodia", "TAF");
            BasicTest("Nos vemos en la noche", "TNI");
            BasicTest("Nos vemos a la tarde", "TEV");
        }
    }
}