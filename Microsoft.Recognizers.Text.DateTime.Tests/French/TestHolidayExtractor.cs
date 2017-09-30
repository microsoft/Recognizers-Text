using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestHolidayExtractor
    {
        private readonly BaseHolidayExtractor extractor = new BaseHolidayExtractor(new FrenchHolidayExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
        }

        [TestMethod]
        public void TestHolidayExtract()
        {
            BasicTest("Je reviendrai sur Yuandan", 18, 7);
            BasicTest("Je reviendrai sur jour de thanks giving", 18, 21);
            BasicTest("Je reviendrai sur fete de pere", 18, 12);
            BasicTest("Je reviendrai sur noel", 18, 4); // I will return on christmas
            BasicTest("Je reviendrai sur jour de noël", 26, 4);
            BasicTest("Je reviendrai sur fete des meres", 18, 14);
            BasicTest("Je reviendrai sur le fete du travail", 21, 15);

            // should recognize prochain/dernier "suffix" next/past modifier, but likely still doesn't extract date
            BasicTest("Je reviendrai sur halloween d'annee prochain", 18, 26);

            BasicTest("Je reviendrai sur thanksgiving", 18, 12);

            BasicTest("Je reviendrai sur Yuandan cette annee", 18, 19);
            BasicTest("Je reviendrai sur Yuandan de 2016", 18, 15);
            BasicTest("Je reviendrai sur Yuandan 2016", 18, 12);

            BasicTest("Je reviendrai sur le Vendredi Saint", 21, 14);
        }
    }
}