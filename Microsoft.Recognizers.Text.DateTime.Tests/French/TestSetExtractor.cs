using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestSetExtractor
    {
        private readonly BaseSetExtractor extractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, results[0].Type);
        }

        [TestMethod]
        public void TestSetExtract()
        {
            BasicTest("Je vais partir chaque semaine", 15, 14); // I will leave every week
            BasicTest("Je vais partir tous les jours", 15, 14); // I will leave every day

            BasicTest("Je vais partir quotidien", 15, 9); // I will leave daily
            BasicTest("Je vais partir hebdomadaire", 15, 12); // I will leave weekly
            BasicTest("Je vais partir mensuel", 15, 7); // I will leave monthly
            BasicTest("Je vais partir bihebdomadaire", 15, 14); // I will leave biweekly

            BasicTest("Je vais partir chaque mois", 15, 11); // I will leave every month
            BasicTest("Je vais partir annuellement", 15, 12); // I will leave annually

            BasicTest("Je vais partir chaque deux jours", 15, 17); // I will leave every two days
            BasicTest("Je vais partir chaque trois semaine", 15, 20); // I will leave every three weeks

            BasicTest("Je vais partir 3pm tous les jours", 15, 18);

            BasicTest("Je vais partir chaque 15/4", 15, 11);
            BasicTest("Je vais partir chaque lundi", 15, 12);
            BasicTest("Je vais partir chaque lundi 4pm", 15, 16);

            BasicTest("Je vais partir tous les matins", 15, 15); // I will leave every morning -Eachprefixregex
        }
    }
}