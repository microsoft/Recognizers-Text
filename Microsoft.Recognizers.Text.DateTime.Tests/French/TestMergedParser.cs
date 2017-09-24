using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestMergedParser
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), DateTimeOptions.None);
        private readonly IDateTimeParser parser = new BaseMergedParser(new FrenchMergedParserConfiguration());

        readonly DateObject refrenceDate;

        public TestMergedParser()
        {
            refrenceDate = new DateObject(2016, 11, 7);
        }

        public void BasicTest(string text, string type)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDate);
            Assert.AreEqual(type, pr.Type.Replace("datetimeV2.", ""));
        }

        public void BasicTestWithTwoResults(string text, string type1, string type2)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(2, er.Count);
            var pr = parser.Parse(er[0], refrenceDate);
            Assert.AreEqual(type1, pr.Type.Replace("datetimeV2.", ""));
            pr = parser.Parse(er[1], refrenceDate);
            Assert.AreEqual(type2, pr.Type.Replace("datetimeV2.", ""));
        }

        [TestMethod]
        public void TestMergedParse()
        {
            BasicTest("Vendredi d'apres-midis", Constants.SYS_DATETIME_DATETIMEPERIOD);
            //BasicTest("vendredi 3 apres-midi", Constants.SYS_DATETIME_DATETIME);
        }

        [TestMethod]
        public void TestMergedParseIn()
        {
            // recognizes as Duration, should be datetime
            //BasicTest("organiser une réunion à 8 minute", Constants.SYS_DATETIME_DATETIME);
            //BasicTest("organiser une réunion à 10 jours", Constants.SYS_DATETIME_DATE);
            //BasicTest("organiser une réunion dans 10 jours", Constants.SYS_DATETIME_DATE);
           
            // heures in french is both 'o'clock' and time unit, resolves to time instead of datetime
            //BasicTest("organiser une réunion à 10 heures", Constants.SYS_DATETIME_DATETIME);
            //BasicTest("organiser une réunion dans 8 minutes", Constants.SYS_DATETIME_DATETIME);

            BasicTest("organiser une réunion dans 3 semaines", Constants.SYS_DATETIME_DATEPERIOD);
            BasicTest("organiser une réunion dans 3 mois", Constants.SYS_DATETIME_DATEPERIOD);
            BasicTest("organiser une réunion dans 3 annees", Constants.SYS_DATETIME_DATEPERIOD);
        }

        [TestMethod]
        public void TestMergedParseAfterBefore()
        {
            BasicTest("apres 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
            BasicTest("avant 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
            BasicTest("depuis 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
        }

        [TestMethod]
        public void TestMergedParseInvalidDatetime()
        {
            BasicTest("2016-2-30", Constants.SYS_DATETIME_DATE);
            //only 2015-1 is extracted
            BasicTest("2015-1-32", Constants.SYS_DATETIME_DATEPERIOD);
            //only 2017 is extracted
            BasicTest("2017-13-12", Constants.SYS_DATETIME_DATEPERIOD);
        }

        [TestMethod]
        public void TestMergedParseWithTwoResults()
        {
            BasicTestWithTwoResults("Changer 22 Juillet rencontre Bellevue a 22 Aout", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_DATE);
            BasicTestWithTwoResults("Vendredi pour 3 dans Bellevue dans l'apres-midi", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_TIMEPERIOD);
        }
    }
}
