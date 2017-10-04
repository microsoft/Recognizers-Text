using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestDurationExtractor
    {
        private readonly BaseDurationExtractor extractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, results[0].Type);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        [TestMethod]
        public void TestDurationExtract()
        {
            BasicTest("je partirai pour 3h", 17, 2); // I will leave for 3 hours
            BasicTest("je partirai pour 3jour", 17, 5);
            BasicTest("je partirai pour 3,5ans", 17, 6);

            BasicTest("je partirai pour 3 h", 17, 3);
            BasicTest("je partirai pour 3 heures", 17, 8);
            BasicTest("je partirai pour 3 hrs", 17, 5);
            BasicTest("je partirai pour 3 hr", 17, 4);
            BasicTest("je partirai pour 3 jour", 17, 6);
            BasicTest("je partirai pour 3 mois", 17, 6);
            BasicTest("je partirai pour 3 minutes", 17, 9);
            BasicTest("je partirai pour 3 min", 17, 5);
            BasicTest("je partirai pour 3,5 seconde", 17, 11);
            BasicTest("je partirai pour 123,45 sec", 17, 10);
            BasicTest("je partirai pour deux semaines", 17, 13);
            BasicTest("je partirai pour vingt min", 17, 9);
            BasicTest("je partirai pour vingt quatre heures", 17, 19);

            BasicTest("Je partirai pour toute la journee", 17, 16);
            BasicTest("Je partirai pour toute la semaine", 17, 16);
            BasicTest("Je partirai pour toute le mois", 17, 13);
            BasicTest("Je partirai pour toute l'année", 17, 13);
            BasicTest("Je partirai pour toute l'annee", 17, 13);

            //BasicTest("Je partirai pour un semestre", 17, 11);
            BasicTest("Je partirai pour une heure", 17, 9);
            BasicTest("Je partirai pendant un ans", 20, 6);

            BasicTest("demi année", 0, 10);
            BasicTest("demi-annee", 0, 10);

            BasicTest("Je partirai pour 3-min", 17, 5);
            BasicTest("Je partirai pour 30-minutes", 17, 10);

            BasicTest("Je partirai pour demi heure", 17, 10);

            BasicTest("Je partirai pour une heure et demi", 17, 17);
            BasicTest("Je partirai pour un heure et demi", 17, 16);

            BasicTest("Je partirai pour une demi-heure", 17, 14);

            BasicTest("Je partirai pour deux heures", 17, 11);
            BasicTest("Je partirai pour deux et demi heures", 17, 19);

            BasicTest("dans une semaine", 5, 11);
            BasicTest("en un jour", 3, 7);
            BasicTest("pour un heure", 5, 8);
            BasicTest("pour un mois", 5, 7);

            BasicTest("Je partirai pour quel qués heures", 17, 16);
            BasicTest("Je partirai pour quelques heures", 17, 15);
            BasicTest("Je partirai pour quelqués minutes", 17, 16);
            BasicTest("Je partirai pour quelque jours", 17, 13);
            BasicTest("Je partirai pour plusieurs jours", 17, 15);
        }
    }
}