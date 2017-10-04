using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.French.Tests
{
    [TestClass]
    public class TestSetParser
    {
        readonly BaseSetExtractor extractor = new BaseSetExtractor(new FrenchSetExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseSetParser(new FrenchSetParserConfiguration(new FrenchCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, string value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], System.DateTime.Now);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult)pr.Value).FutureValue);
            Assert.AreEqual(luisValue, pr.TimexStr);
        }

        [TestMethod]
        public void TestSetParse()
        {
            BasicTest("Je vais partir chaque semaine", "Set: P1W", "P1W");
            BasicTest("Je vais partir bihebdomadaire", "Set: P2W", "P2W");
            BasicTest("Je vais partir hebdomadaire", "Set: P1W", "P1W");
            BasicTest("Je vais partir quotidien", "Set: P1D", "P1D");
            BasicTest("Je vais partir journellement", "Set: P1D", "P1D");

            BasicTest("Je vais partir chaque mois", "Set: P1M", "P1M");
            BasicTest("Je vais partir annuellement", "Set: P1Y", "P1Y");
            BasicTest("Je vais partir annuel", "Set: P1Y", "P1Y");

            BasicTest("Je vais partir chaque deux jours", "Set: P2D", "P2D");
            BasicTest("Je vais partir chaque trois semaine", "Set: P3W", "P3W");

            BasicTest("Je vais partir 3pm tous les jour", "Set: T15", "T15");  // EachDayRegex
            BasicTest("Je vais partir 3pm tous les jours", "Set: T15", "T15");
            BasicTest("Je vais partir 3pm chaque jours", "Set: T15", "T15");

            BasicTest("Je vais partir chaque 15/4", "Set: XXXX-04-15", "XXXX-04-15");
            BasicTest("Je vais partir chaque Lundi", "Set: XXXX-WXX-1", "XXXX-WXX-1");
            BasicTest("Je vais partir chaque Lundi 4pm", "Set: XXXX-WXX-1T16", "XXXX-WXX-1T16");

            BasicTest("Je vais partir chaque matin", "Set: TMO", "TMO");
        }
    }
}