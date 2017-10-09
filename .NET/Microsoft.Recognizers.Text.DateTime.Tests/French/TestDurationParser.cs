using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDurationParser
    {
        readonly BaseDurationExtractor extractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseDurationParser(new FrenchDurationParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, double value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], new DateObject(2016, 11, 7));
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(luisValue, ((DateTimeResolutionResult)pr.Value).Timex);
        }

        [TestMethod]
        public void TestDurationParse()
        {
            BasicTest("je partirai pour 3h", 10800, "PT3H");
            BasicTest("je partirai pour 3jour", 259200, "P3D");
            BasicTest("je partirai pour 3,5ans", 110376000, "P3.5Y");

            BasicTest("je partirai pour 3 h", 10800, "PT3H");
            BasicTest("je partirai pour 3 heures", 10800, "PT3H");
            BasicTest("je partirai pour 3 hrs", 10800, "PT3H");
            BasicTest("je partirai pour 3 hr", 10800, "PT3H");
            BasicTest("je partirai pour 3 jour", 259200, "P3D");
            BasicTest("je partirai pour 3 mois", 7776000, "P3M");
            BasicTest("je partirai pour 3 minutes", 180, "PT3M");
            BasicTest("je partirai pour 3 min", 180, "PT3M");
            BasicTest("je partirai pour 3,5 seconde ", 3.5, "PT3.5S");
            BasicTest("je partirai pour 123,45 sec", 123.45, "PT123.45S");
            BasicTest("je partirai pour deux semaines", 1209600, "P2W");
            BasicTest("je partirai pour vingt min", 1200, "PT20M");
            BasicTest("je partirai pour vingt quatre heures", 86400, "PT24H");

            BasicTest("je partirai pour toute la journee", 86400, "P1D");
            BasicTest("je partirai pour toute la semaine", 604800, "P1W");
            BasicTest("je partirai pour toute le mois", 2592000, "P1M");
            BasicTest("je partirai pour toute l'annee", 31536000, "P1Y");

            BasicTest("je partirai pour une heure", 3600, "PT1H");

            BasicTest("demi ans", 15768000, "P0.5Y");

            BasicTest("je partirai pour 3-min", 180, "PT3M");
            BasicTest("je partirai pour 30-minutes", 1800, "PT30M");

            BasicTest("je partirai pour un et demi heures", 5400, "PT1.5H");
            BasicTest("je partirai pour une et demi heure", 5400, "PT1.5H");
            BasicTest("je partirai pour demi heure", 1800, "PT0.5H");

            BasicTest("je partirai pour deux heures", 7200, "PT2H");
            BasicTest("je partirai pour deux et demi heures", 9000, "PT2.5H");
        }
    }
}