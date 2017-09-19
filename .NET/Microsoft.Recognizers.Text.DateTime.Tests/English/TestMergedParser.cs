using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{

    [TestClass]
    public class TestMergedParser
    {
        private readonly IExtractor extractor = new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), DateTimeOptions.None);
        private readonly IDateTimeParser parser = new BaseMergedParser(new EnglishMergedParserConfiguration());

        readonly DateObject referenceDate;

        public TestMergedParser()
        {
            referenceDate = new DateObject(2016, 11, 7);
        }

        public void BasicTest(string text, string type)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], referenceDate);
            Assert.AreEqual(type, pr.Type.Replace("datetimeV2.",""));
        }

        public void BasicTestResolution(string text, string resolution)
        {
            BasicTestResolution(text, resolution, referenceDate);
        }

        public void BasicTestResolution(string text, string resolution, DateObject refDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refDate);
            var prValue = (List<Dictionary<string, string>>)(((SortedDictionary<string, object>)pr.Value).First().Value);
            Assert.AreEqual(resolution, prValue.First()["value"]);
        }

        public void BasicTestWithTwoResults(string text, string type1, string type2)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(2, er.Count);
            var pr = parser.Parse(er[0], referenceDate);
            Assert.AreEqual(type1, pr.Type.Replace("datetimeV2.", ""));
            pr = parser.Parse(er[1], referenceDate);
            Assert.AreEqual(type2, pr.Type.Replace("datetimeV2.", ""));
        }

        [TestMethod]
        public void TestMergedParse()
        {
            BasicTest("Set an appointment for Easter", Constants.SYS_DATETIME_DATE);
            BasicTest("day after tomorrow", Constants.SYS_DATETIME_DATE);
            BasicTest("day after tomorrow at 8am", Constants.SYS_DATETIME_DATETIME);
            BasicTest("on Friday in the afternoon", Constants.SYS_DATETIME_DATETIMEPERIOD);
            BasicTest("on Friday for 3 in the afternoon", Constants.SYS_DATETIME_DATETIME);

            BasicTest("Set appointment for tomorrow morning at 9 o'clock.", Constants.SYS_DATETIME_DATETIME);
        }

        [TestMethod]
        public void TestMergedParseResolution()
        {
            BasicTestResolution("Set an appointment for Easter", "not resolved");
        }

        [TestMethod]
        public void TestMergedParseResolutionWeekDayAndOrdinary()
        {
            BasicTestResolution("put make cable's wedding in my calendar for wednesday the thirty first", "not resolved", new DateObject(2017, 09, 15));
            BasicTestResolution("put make cable's wedding in my calendar for wednesday the thirty first", "not resolved", new DateObject(2017, 10, 15));
            BasicTestResolution("put make cable's wedding in my calendar for tuesday the thirty first", "2017-10-31", new DateObject(2017, 10, 15));
        }

        [TestMethod]
        public void TestMergedParseIn()
        {
            BasicTest("schedule a meeting in 8 minutes", Constants.SYS_DATETIME_DATETIME);
            BasicTest("schedule a meeting in 10 hours", Constants.SYS_DATETIME_DATETIME);
            BasicTest("schedule a meeting in 10 days", Constants.SYS_DATETIME_DATE);
            BasicTest("schedule a meeting in 3 weeks", Constants.SYS_DATETIME_DATEPERIOD);
            BasicTest("schedule a meeting in 3 months", Constants.SYS_DATETIME_DATEPERIOD);
            BasicTest("I'll be out in 3 year", Constants.SYS_DATETIME_DATEPERIOD);
        }

        [TestMethod]
        public void TestMergedParseAfterBeforeSince()
        {
            BasicTest("after 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
            BasicTest("before 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
            BasicTest("since 8pm", Constants.SYS_DATETIME_TIMEPERIOD);
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
            BasicTestWithTwoResults("add yoga to personal calendar on monday and wednesday at 3pm", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_DATETIME);

            BasicTestWithTwoResults("schedule a meeting at 8 am every week ", Constants.SYS_DATETIME_TIME,
                Constants.SYS_DATETIME_SET);

            BasicTestWithTwoResults("schedule second saturday of each month", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_SET);

            BasicTestWithTwoResults("Set an appointment for Easter Sunday", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_DATE);

            BasicTestWithTwoResults("block 1 hour on my calendar tomorrow morning", Constants.SYS_DATETIME_DURATION,
                Constants.SYS_DATETIME_DATETIMEPERIOD);

            BasicTestWithTwoResults("Change July 22nd meeting in Bellevue to August 22nd", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_DATE);

            BasicTestWithTwoResults("on Friday for 3 in Bellevue in the afternoon", Constants.SYS_DATETIME_DATE,
                Constants.SYS_DATETIME_TIMEPERIOD);
        }
    }
}
