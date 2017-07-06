using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestSetParser
    {
        readonly BaseSetExtractor extractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseSetParser(new EnglishSetParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, string value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], System.DateTime.Now);
            Assert.AreEqual(Constants.SYS_DATETIME_SET, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(luisValue, pr.TimexStr);
        }

        [TestMethod]
        public void TestSetParse()
        {
            BasicTest("I'll leave weekly", "Set: P1W", "P1W");
            BasicTest("I'll leave biweekly", "Set: P2W", "P2W");
            BasicTest("I'll leave daily", "Set: P1D", "P1D");
            BasicTest("I'll leave every day", "Set: P1D", "P1D");
            BasicTest("I'll leave each month", "Set: P1M", "P1M");
            BasicTest("I'll leave annually", "Set: P1Y", "P1Y");
            BasicTest("I'll leave annual", "Set: P1Y", "P1Y");

            BasicTest("I'll leave each two days", "Set: P2D", "P2D");
            BasicTest("I'll leave every three week", "Set: P3W", "P3W");

            BasicTest("I'll leave 3pm every day", "Set: T15", "T15");
            BasicTest("I'll leave 3pm each day", "Set: T15", "T15");

            BasicTest("I'll leave each 4/15", "Set: XXXX-04-15", "XXXX-04-15");
            BasicTest("I'll leave every monday", "Set: XXXX-WXX-1", "XXXX-WXX-1");
            BasicTest("I'll leave each monday 4pm", "Set: XXXX-WXX-1T16", "XXXX-WXX-1T16");

            BasicTest("I'll leave every morning", "Set: TMO", "TMO");
        }
    }
}