using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestDurationParser
    {
        readonly BaseDurationExtractor extractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
        readonly IDateTimeParser parser = new BaseDurationParser(new EnglishDurationParserConfiguration(new EnglishCommonDateTimeParserConfiguration()));

        public void BasicTest(string text, double value, string luisValue)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], new DateObject(2016, 11, 7));
            Assert.AreEqual(Constants.SYS_DATETIME_DURATION, pr.Type);
            Assert.AreEqual(value, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(luisValue, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        [TestMethod]
        public void TestDurationParse()
        {
            BasicTest("I'll leave for 3h", 10800, "PT3H");
            BasicTest("I'll leave for 3day", 259200, "P3D");
            BasicTest("I'll leave for 3.5years", 110376000, "P3.5Y");

            BasicTest("I'll leave for 3 h", 10800, "PT3H");
            BasicTest("I'll leave for 3 hours", 10800, "PT3H");
            BasicTest("I'll leave for 3 hrs", 10800, "PT3H");
            BasicTest("I'll leave for 3 hr", 10800, "PT3H");
            BasicTest("I'll leave for 3 day", 259200, "P3D");
            BasicTest("I'll leave for 3 months", 7776000, "P3M");
            BasicTest("I'll leave for 3 minutes", 180, "PT3M");
            BasicTest("I'll leave for 3 min", 180, "PT3M");
            BasicTest("I'll leave for 3.5 second ", 3.5, "PT3.5S");
            BasicTest("I'll leave for 123.45 sec", 123.45, "PT123.45S");
            BasicTest("I'll leave for two weeks", 1209600, "P2W");
            BasicTest("I'll leave for twenty min", 1200, "PT20M");
            BasicTest("I'll leave for twenty and four hours", 86400, "PT24H");

            BasicTest("I'll leave for all day", 86400, "P1D");
            BasicTest("I'll leave for all week", 604800, "P1W");
            BasicTest("I'll leave for all month", 2592000, "P1M");
            BasicTest("I'll leave for all year", 31536000, "P1Y");

            BasicTest("I'll leave for an hour", 3600, "PT1H");

            BasicTest("half year", 15768000, "P0.5Y");
            BasicTest("half an year", 15768000, "P0.5Y");

            BasicTest("I'll leave for 3-min", 180, "PT3M");
            BasicTest("I'll leave for 30-minutes", 1800, "PT30M");

            BasicTest("I'll leave for an hour and a half", 5400, "PT1.5H");
            BasicTest("I'll leave for an hour and half", 5400, "PT1.5H");
            BasicTest("I'll leave for half hour", 1800, "PT0.5H");

            BasicTest("I'll leave for two hour", 7200, "PT2H");
            BasicTest("I'll leave for two and a half hours", 9000, "PT2.5H");

            BasicTest("In a week and for an hour", 608400, "P1W1H");
            BasicTest("In a week and for one hour", 608400, "P1W1H");
            BasicTest("In a week and for two hour", 612000, "P1W2H");
            BasicTest("In a week and an hour", 608400, "P1W1H");
            BasicTest("In two week and half hour", 1211400, "P2W0.5H");
            BasicTest("In two week and half an hour", 1211400, "P2W0.5H");
            BasicTest("In two week and an hour and a half", 1215000, "P2W1.5H");
        }
    }
}