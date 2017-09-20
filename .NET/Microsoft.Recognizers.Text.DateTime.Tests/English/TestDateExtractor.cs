using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

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

        public void BasicTest(string text, string expectedOutput)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedOutput, results[0].Text);
        }

        public void BasicTestTwoOutputs(string text, string expectedOutput1, string expectedOutput2)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedOutput1, results[0].Text);
            Assert.AreEqual(expectedOutput2, results[1].Text);
        }

        public void BasicTestNone(string text)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(0, results.Count);
        }

        // use to generate the test cases sentences inside TestDateExtractWeekDayAndDayOfMonth function
        // return a day of current week which the parameter refer to
        public string CalculateWeekOfDay(int dayOfMonth)
        {
            var weekDay = "None";
            if (dayOfMonth >= 1 && dayOfMonth <= 31)
            {
                var referenceTime = DateObject.Now;
                var date = referenceTime.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, dayOfMonth);
                weekDay = date.DayOfWeek.ToString();
            }

            return weekDay;
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

            BasicTest("I'll go back the first friday of july", 13, 24);
            BasicTest("I'll go back the first friday in this month", 13, 30);

            BasicTest("I'll go back two weeks from now", 13, 18);

            BasicTest("I'll go back next week on Friday", 13, 19);
            BasicTest("I'll go back on Friday next week", 13, 19);

            BasicTest("past Monday", 0, 11);
        }

        [TestMethod]
        public void TestDateExtractDayOfWeek()
        {
            BasicTest("I'll go back on Tues.", 16, 4);
            BasicTest("I'll go back on Tues. good news.", 16, 4);
            BasicTest("I'll go back on Tues", 16, 4);
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
        }

        [TestMethod]
        public void TestDateExtractMonthDate()
        {
            BasicTest("I'll go back fourth of may", 13, 13);
            BasicTest("I'll go back 4th of march", 13, 12);
            BasicTest("I'll go back Jan first", 13, 9);
            BasicTest("I'll go back May twenty-first", 13, 16);
            BasicTest("I'll go back May twenty one", 13, 14);
            BasicTest("I'll go back second of Aug", 13, 13);
            BasicTest("I'll go back twenty second of June", 13, 21);
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

        [TestMethod]
        public void TestDateExtractOn()
        {
            BasicTest("I went back on the 27th", 12, 11);
            BasicTest("I went back on the 21st", 12, 11);
            BasicTest("I went back on 22nd", 12, 7);
            BasicTest("I went back on the second!", 12, 13);
            BasicTest("I went back on twenty second?", 12, 16);
        }

        [TestMethod]
        public void TestDateExtractForTheNegative()
        {
            BasicTestNone("the first prize");
            BasicTestNone("I'll go to the 27th floor");
            BasicTestNone("Commemorative Events for the 25th Anniversary of Diplomatic Relations between Singapore and China");
            BasicTestNone("Get tickets for the 17th Door Haunted Experience");
        }

        [TestMethod]
        public void TestDateExtractWeekDayAndDayOfMonthMerge()
        {
            //Need to calculate the DayOfWeek by the date
            //Example: What do I have on Wednesday the second?
            BasicTest("What do I have on " + CalculateWeekOfDay(2) + " the second",
                CalculateWeekOfDay(2) + " the second");
            BasicTest("A meeting for " + CalculateWeekOfDay(27) + " the 27th with Joe Smith",
                CalculateWeekOfDay(27) + " the 27th");
            BasicTest("I'll go back " + CalculateWeekOfDay(21) + " the 21st", CalculateWeekOfDay(21) + " the 21st");
            BasicTest("I'll go back " + CalculateWeekOfDay(22) + " the 22nd", CalculateWeekOfDay(22) + " the 22nd");
            BasicTest("I'll go back " + CalculateWeekOfDay(23) + " the 23rd", CalculateWeekOfDay(23) + " the 23rd");
            BasicTest("I'll go back " + CalculateWeekOfDay(15) + " the 15th", CalculateWeekOfDay(15) + " the 15th");
            BasicTest("I'll go back " + CalculateWeekOfDay(21) + " the twenty first", CalculateWeekOfDay(21) + " the twenty first");
            BasicTest("I'll go back " + CalculateWeekOfDay(22) + " the twenty second", CalculateWeekOfDay(22) + " the twenty second");
            BasicTest("I'll go back " + CalculateWeekOfDay(15) + " the fifteen", CalculateWeekOfDay(15) + " the fifteen");
            BasicTest("I'll go back " + CalculateWeekOfDay(7) + " the seventh", CalculateWeekOfDay(7) + " the seventh");
        }

        [TestMethod]
        public void TestDateExtractWeekDayAndDayOfMonthSeparate()
        {
            //comment this method since we output a merged one even the weekday and date is not matching

            ////Need to calculate the DayOfWeek by the date
            ////Example: What do I have on Wednesday the second?
            ////Should separate the Wednesday and the second to two outputs if the second of current month is not Wednesday
            //BasicTestTwoOutputs("What do I have on " + CalculateWeekOfDay(3) + " the second?", CalculateWeekOfDay(3),
            //    "second");
            //BasicTestTwoOutputs("What do I have on " + CalculateWeekOfDay(28) + " the 27th", CalculateWeekOfDay(28),
            //    "27th");
            //BasicTestTwoOutputs("What do I have on " + CalculateWeekOfDay(24) + " the 21st", CalculateWeekOfDay(24),
            //    "21st");
        }

        [TestMethod]
        public void TestDateExtractRelativeDayOfWeek()
        {
            BasicTest("I'll go back second Sunday", 13, 13);
            BasicTest("I'll go back first Sunday", 13, 12);
            BasicTest("I'll go back third Tuesday", 13, 13);
            BasicTest("I'll go back fifth Sunday", 13, 12);
        }

        [TestMethod]
        public void TestDateExtractRelativeDayOfWeekSingle()
        {
            // For ordinary number>5, only the DayOfWeek should be extracted
            BasicTest("I'll go back sixth Sunday", 19, 6);
            BasicTest("I'll go back tenth Monday", 19, 6);
        }

        [TestMethod]
        public void TestDateExtractOdNumRelativeMonth()
        {
            BasicTest("I'll go back 20th of next month", 13, 18);
            BasicTest("I'll go back 31st of this month", 13, 18);
        }
    }
}