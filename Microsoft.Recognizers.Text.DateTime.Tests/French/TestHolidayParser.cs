using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestHolidayParser
    {
        readonly DateObject refrenceDay;
        readonly BaseHolidayParser parser;
        readonly BaseHolidayExtractor extractor;

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

        public TestHolidayParser()
        {
            refrenceDay = new DateObject(2016, 11, 7);
            parser = new BaseHolidayParser(new FrenchHolidayParserConfiguration());
            extractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());
        }

        [TestMethod]
        public void TestHolidayParse()
        {
            BasicTest("Je reveindrai sur réveillon de Nouvel an",
                new DateObject(2016, 12, 31),
                new DateObject(2015, 12, 31));

            BasicTest("Je reviendrai sur la saint-sylvestre",
                new DateObject(2016, 12, 31),
                new DateObject(2015, 12, 31));

            BasicTest("Je reviens sur noël",
                new DateObject(2016, 12, 25),
                new DateObject(2015, 12, 25));

            BasicTest("Je reviens sur yuandan",
                new DateObject(2017, 1, 1),
                new DateObject(2016, 1, 1));

            BasicTest("Je reviens sur nouvel an chinois",
                new DateObject(2017, 1, 1),
                new DateObject(2016, 1, 1));

            BasicTest("Je reviens sur le jour de thanks giving",
                new DateObject(2016, 11, 24),
                new DateObject(2015, 11, 26));

            BasicTest("Je reviens sur l'action de grace",
                new DateObject(2016, 11, 24),
                new DateObject(2015, 11, 26));

            BasicTest("Je reviens sur thanksgiving",
                new DateObject(2016, 11, 24),
                new DateObject(2015, 11, 26));

            BasicTest("Je reviens sur fete des peres",
                new DateObject(2017, 6, 18),
                new DateObject(2016, 6, 19));

            BasicTest("Je reviens sur fete des meres",
                new DateObject(2017, 5, 14),
                new DateObject(2016, 5, 8));

            // Note: Labor day US is 9/4, however Labour day france is May 1...
            BasicTest("Je reviens sur fete du travail",
                new DateObject(2017, 9, 4),
                new DateObject(2016, 9, 5));

            // NOTE: "yuandan prochain" - will extract properly, but won't parse due to 
            // suffix/prefix swap with prochain/dernier. Existing parser treats 'next/last' as prefix, but in french
            // it needs support for suffix
            //BasicTest("I'll go back on Yuandan of next year",
            //    new DateObject(2017, 1, 1),
            //    new DateObject(2017, 1, 1));

            BasicTest("Je reviens sur le jour de thanks giving 2010",
                new DateObject(2010, 11, 25),
                new DateObject(2010, 11, 25));

            BasicTest("je reviens sur fete des peres 2015",
                new DateObject(2015, 6, 21),
                new DateObject(2015, 6, 21));

            BasicTest("je reviens sur noel",
                new DateObject(2016, 12, 25),
                new DateObject(2015, 12, 25));

            BasicTest("je reviens sur la veille de noel",
                new DateObject(2016, 12, 24),
                new DateObject(2015, 12, 24));

            BasicTest("je reviens sur reveillon de noel",
                new DateObject(2016, 12, 24),
                new DateObject(2015, 12, 24));

            BasicTest("je reviens sur nouvel an",
                new DateObject(2017, 01, 01),
                new DateObject(2016, 01, 01));

            // TODO: Add more coverage
        }

        [TestMethod]
        public void TestHolidayParseLuis()
        {
            BasicTest("Je reviendrai sur Yuandan", "XXXX-01-01");
            BasicTest("Je reviendrai sur thanksgiving", "XXXX-11-WXX-4-4");
            BasicTest("Je reviendrai sur le jour de thanksgiving", "XXXX-11-WXX-4-4");
            BasicTest("Je reviendrai sur fete des peres", "XXXX-06-WXX-6-3");
            BasicTest("Je reviendrai sur fete des meres", "XXXX-05-WXX-7-2");
            BasicTest("Je reviendrai sur jour de thanks giving 2010", "2010-11-WXX-4-4");
            BasicTest("Je reviendrai sur noel", "XXXX-12-25");
            BasicTest("Je reviendrai sur la veille de noel", "XXXX-12-24");
            BasicTest("Je reviendrai sur reveillon de noel", "XXXX-12-24");
            BasicTest("Je reviendrai sur le nouvel an", "XXXX-01-01");
            // Next/Last current 'prefix', need to add next/last 'suffix' support
            //BasicTest("Je reviendrai sur Yuandan l'annee prochain", "2017-01-01");

            BasicTest("Je reviendrai sur fete des peres de 2015", "2015-06-WXX-6-3");
            BasicTest("Je reviendrai sur la saint-sylvestre", "XXXX-12-31");

        }
    }
}