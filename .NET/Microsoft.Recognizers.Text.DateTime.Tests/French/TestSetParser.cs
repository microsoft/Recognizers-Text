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

            BasicTest("Je vais partir chaque  15/4", "Set: XXXX-04-15", "XXXX-04-15");
            BasicTest("Je vais partir chaque Lundi", "Set: XXXX-WXX-1", "XXXX-WXX-1");
            BasicTest("Je vais partir chaque Lundi 4pm", "Set: XXXX-WXX-1T16", "XXXX-WXX-1T16");

            //BasicTest("Je vais partir 3pm chaque jours", "Set: T15", "T15");
            //BasicTest("Je vais partir 3pm tous les jours", "Set: T15", "T15");
            //BasicTest("Je vais partir 15 chaque jour", "Set: T15", "T15");  // EachDayRegex
            //BasicTest("Je vais partir 3pm chaque jours", "Set: T15", "T15");

            //BasicTest("Je vais partir chaque matin", "Set: TMO", "TMO");
        }

        [TestMethod]
        public void TestSetExtractMergeDate_Time()
        {
            // Note: uses DescRegex for PM/AM, FR typically uses 24:00 time so consider removing AM/PM

            BasicTest("Je vais partir a 9am Dimanches", "Set: XXXX-WXX-7T09", "XXXX-WXX-7T09");
            //BasicTest("Je vais partir a 9 Dimanches", "Set: XXXX-WXX-7T09", "XXXX-WXX-7T09");
        }

        [TestMethod]
        public void TestSetExtractDate()
        {
            BasicTest("Je vais partir a Lundis", "Set: XXXX-WXX-1", "XXXX-WXX-1");
            BasicTest("Je vais partir Dimanches", "Set: XXXX-WXX-7", "XXXX-WXX-7");
            BasicTest("Je vais partir tous les Dimanches", "Set: XXXX-WXX-7", "XXXX-WXX-7");
            BasicTest("Je vais partir chaque Dimanches", "Set: XXXX-WXX-7", "XXXX-WXX-7");
        }

        [TestMethod]
        public void TestSetParseTimePeriod_Time()
        {
            BasicTest("Je vais partir chaque de matin a 9am", "Set: T09", "T09");
            BasicTest("Je vais partir chaque apres-midi a 4pm", "Set: T16", "T16");
            BasicTest("Je vais partir chaque nuit a 9pm", "Set: T21", "T21");
            BasicTest("Je vais partir chaque nuit 9", "Set: T21", "T21");
            BasicTest("Je vais partir chaque nuit a 21", "Set: T21", "T21");
            BasicTest("Je vais partir chaque nuit 21", "Set: T21", "T21");
            BasicTest("Je vais partir tous les de matin 9am", "Set: T09", "T09"); // should just be 'matin', not 'de'
        }
    }
}