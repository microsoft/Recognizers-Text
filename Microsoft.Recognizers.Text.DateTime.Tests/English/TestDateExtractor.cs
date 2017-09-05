using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{

    [TestClass]
    public class TestDateExtractor
    {
        private readonly BaseDateExtractor extractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, results[0].Type);
        }

        [TestMethod]
        public void TestDateExtract()
        {
            BasicTest("I'll go back on 15", 16, 2);
            BasicTest("I'll go back April 22", 13, 8);
            BasicTest("I'll go back Jan-1", 13, 5);
            BasicTest("I'll go back Jan/1", 13, 5);
            BasicTest("I'll go back October. 2", 13, 10);
            BasicTest("I'll go back January 12, 2016", 13, 16);
            BasicTest("I'll go back January 12 of 2016", 13, 18);
            BasicTest("I'll go back Monday January 12th, 2016", 13, 25);
            BasicTest("I'll go back 02/22/2016", 13, 10);
            BasicTest("I'll go back 21/04/2016", 13, 10);
            BasicTest("I'll go back 21/04/16", 13, 8);
            BasicTest("I'll go back 9-18-15", 13, 7);
            BasicTest("I'll go back on 4.22", 16, 4);
            BasicTest("I'll go back on 4-22", 16, 4);
            BasicTest("I'll go back at 4.22", 16, 4);
            BasicTest("I'll go back in 4-22", 16, 4);
            BasicTest("I'll go back on    4/22", 19, 4);
            BasicTest("I'll go back on 22/04", 16, 5);
            BasicTest("I'll go back       4/22", 19, 4);
            BasicTest("I'll go back 22/04", 13, 5);
            BasicTest("I'll go back 2015/08/12", 13, 10);
            BasicTest("I'll go back 11/12,2016", 13, 10);
            BasicTest("I'll go back 11/12,16", 13, 8);
            BasicTest("I'll go back 1st Jan", 13, 7);
            BasicTest("I'll go back 1-Jan", 13, 5);
            BasicTest("I'll go back 28-Nov", 13, 6);
            BasicTest("I'll go back Wed, 22 of Jan", 13, 14);

            BasicTest("I'll go back Jan first", 13, 9);
            BasicTest("I'll go back May twenty-first", 13, 16);
            BasicTest("I'll go back May twenty one", 13, 14);
            BasicTest("I'll go back second of Aug", 13, 13);
            BasicTest("I'll go back twenty second of June", 13, 21);

            BasicTest("I'll go back on Friday", 16, 6);
            BasicTest("I'll go back Friday", 13, 6);
            BasicTest("I'll go back today", 13, 5);
            BasicTest("I'll go back tomorrow", 13, 8);
            BasicTest("I'll go back yesterday", 13, 9);
            BasicTest("I'll go back the day before yesterday", 13, 24);
            BasicTest("I'll go back the day after tomorrow", 13, 22);
            BasicTest("I'll go back the next day", 13, 12);
            BasicTest("I'll go back next day", 13, 8);
            BasicTest("I'll go back this Friday", 13, 11);
            BasicTest("I'll go back next Sunday", 13, 11);
            BasicTest("I'll go back last Sunday", 13, 11);
            BasicTest("I'll go back last day", 13, 8);
            BasicTest("I'll go back the last day", 13, 12);
            BasicTest("I'll go back the day", 13, 7);
            BasicTest("I'll go back this week Friday", 13, 16);
            BasicTest("I'll go back next week Sunday", 13, 16);
            BasicTest("I'll go back last week Sunday", 13, 16);
            BasicTest("I'll go back 15 June 2016", 13, 12);
            BasicTest("a baseball on may the eleventh", 14, 16);

            BasicTest("I'll go back the first friday of july", 13, 24);
            BasicTest("I'll go back the first friday in this month", 13, 30);

            BasicTest("I'll go back two weeks from now", 13, 18);

            BasicTest("I'll go back next week on Friday", 13, 19);
            BasicTest("I'll go back on Friday next week", 13, 19);

            BasicTest("past Monday", 0, 11);
        }

        [TestMethod]
        public void TestDateExtractAgoLater()
        {
            BasicTest("I went back two months ago", 12, 14);
            BasicTest("I'll go back two days later", 13, 14);
            BasicTest("who did i email a month ago", 16, 11);
        }

        [TestMethod]
        public void TestDateExtractForThe()
        {
            BasicTest("I went back for the 27", 12, 10);
            BasicTest("I went back for the 27th", 12, 12);
            BasicTest("I went back for the 27.", 12, 10);
            BasicTest("I went back for the 27!", 12, 10);
            BasicTest("I went back for the 27 .", 12, 10);
            BasicTest("I went back for the 21st", 12, 12);
            BasicTest("I went back for the 22nd", 12, 12);
            BasicTest("I went back for the second", 12, 14);
            BasicTest("I went back for the twenty second", 12, 21);
            BasicTest("I went back for the thirty first", 12, 20);
        }
    }
}