using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestMergedExtractor
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), DateTimeOptions.None);

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
        }

        public void BasicTestNone(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
        }

        public void BasicTestWithOptions(string text, int count, DateTimeOptions options = DateTimeOptions.None)
        {
            IExtractor extractorWithOptions = new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), options);
            var results = extractorWithOptions.Extract(text);
            Assert.AreEqual(count, results.Count);
        }

        [TestMethod]
        public void TestMergedExtract()
        {
            BasicTest("C'est 2 jours", 6, 7);

            BasicTest("C'est avant 16h", 6, 9);
            BasicTest("C'est avant 16h demain", 6, 16);
            BasicTest("C'est avant lendemain 16h ", 6, 19);

            BasicTest("C'est apres 16h", 6, 9);
            BasicTest("C'est apres 16h lendemain", 6, 19);
            BasicTest("C'est apres lendemain 16h ", 6, 19);

            BasicTest("Je reviendrai dans 5 minutes", 14, 14);
            BasicTest("derniere semaine", 0, 16);
            //BasicTest("dernier lundi", 0, 13);
            BasicTest("planifier un réunion dans 10 heures", 21, 14);
        }

        [TestMethod]
        public void TestMergedSkipFromTo()
        {
            BasicTestWithOptions("changer la reunion du 9am au 11am", 2, DateTimeOptions.SkipFromToMerge);
            BasicTestWithOptions("changer la reunion du Nov.19 au Nov.23th", 2, DateTimeOptions.SkipFromToMerge);

            BasicTestWithOptions("organiser une réunion du 9am à 11am", 1, DateTimeOptions.None); // Testing None.
            BasicTestWithOptions("organiser une reunion des 9 à 11", 1);
            BasicTestWithOptions("organiser une reunion des 9am a 11am demain", 1);

            BasicTestWithOptions("Changer 22 Juillet rencontre Bellevue a 22 Aout", 2); // No merge.
        }

        [TestMethod]
        public void TestAfterBefore()
        {
            BasicTest("apres 2/7 ", 0, 9);
            BasicTest("depuis 2/7 ", 0, 10);
            BasicTest("avant 2/7 ", 0, 9);
        }

        [TestMethod]
        public void TestDateWithTime()
        {
            BasicTest("06/06 12:15", 0, 11);
            BasicTest("06/06/12 15:15", 0, 14);
            BasicTest("06/06, 2015", 0, 11);
        }

        [TestMethod]
        public void TestNegativeExtract()
        {
            //Unit tests for text should not extract datetime

            //test cases provided may want to use words closer to date/time extracts
            BasicTestNone("quel courriel a reçu une réponse");
            BasicTestNone("il est souvent seul");
            BasicTestNone("souvent un oiseau");
            BasicTestNone("michigan heures");
        }
    }
}