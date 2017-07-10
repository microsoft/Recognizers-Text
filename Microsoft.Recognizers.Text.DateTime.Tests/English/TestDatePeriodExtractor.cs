using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests.English
{
    [TestClass]
    public class TestDatePeriodExtractor
    {
        private readonly BaseDatePeriodExtractor extractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());

        public void BasicTest(string text, int start, int length)
        {
            var results = extractor.Extract(text);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(start, results[0].Start);
            Assert.AreEqual(length, results[0].Length);
            Assert.AreEqual(Constants.SYS_DATETIME_DATEPERIOD, results[0].Type);
        }

        private string[] shortMonths = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Sept", "Oct", "Nov", "Dec" };
        private string[] fullMonths = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };


        [TestMethod]
        public void TestDatePeriodExtract()
        {
            foreach (var month in shortMonths)
            {
                BasicTest($"I'll be out in {month}", 15, month.Length);
                BasicTest($"I'll be out this {month}", 12, 5 + month.Length);
                BasicTest($"I was missing {month} 2001", 14, 5 + month.Length);
                BasicTest($"I was missing {month}, 2001", 14, 6 + month.Length);
            }

            foreach (var month in fullMonths)
            {
                BasicTest($"I'll be out in {month}", 15, month.Length);
                BasicTest($"I'll be out this {month}", 12, 5 + month.Length);
                BasicTest($"I was missing {month} 2001", 14, 5 + month.Length);
                BasicTest($"I was missing {month}, 2001", 14, 6 + month.Length);
            }

            // test basic cases
            BasicTest("I'll be out from 4 to 22 this month", 12, 23);
            BasicTest("I'll be out from 4-23 in next month", 12, 23);
            BasicTest("I'll be out from 3 until 12 of Sept hahaha", 12, 23);
            BasicTest("I'll be out 4 to 23 next month", 12, 18);
            BasicTest("I'll be out 4 till 23 of this month", 12, 23);
            BasicTest("I'll be out between 4 and 22 this month", 12, 27);
            BasicTest("I'll be out between 3 and 12 of Sept hahaha", 12, 24);
            BasicTest("I'll be out from 4 to 22 January, 2017", 12, 26);
            BasicTest("I'll be out between 4-22 January, 2017", 12, 26);

            BasicTest("I'll be out on this week", 15, 9);
            BasicTest("I'll be out September", 12, 9);
            BasicTest("I'll be out this September", 12, 14);
            BasicTest("I'll be out last sept", 12, 9);
            BasicTest("I'll be out next june", 12, 9);
            BasicTest("I'll be out june 2016", 12, 9);
            BasicTest("I'll be out june next year", 12, 14);
            BasicTest("I'll be out this weekend", 12, 12);
            BasicTest("I'll be out the third week of this month", 12, 28);
            BasicTest("I'll be out the last week of july", 12, 21);

            BasicTest("I'll be out next 3 days", 12, 11);
            BasicTest("I'll be out next 3 months", 12, 13);
            BasicTest("I'll be out in 3 year", 12, 9);
            BasicTest("I'll be out past 3 weeks", 12, 12);
            BasicTest("I'll be out last 3year", 12, 10);
            BasicTest("I'll be out previous 3 weeks", 12, 16);

            // test merging two time points
            BasicTest("I'll be out Oct. 2 to October 22", 12, 20);
            BasicTest("I'll be out January 12, 2016 - 02/22/2016", 12, 29);
            BasicTest("I'll be out 1st Jan until Wed, 22 of Jan", 12, 28);
            BasicTest("I'll be out today till tomorrow", 12, 19);
            BasicTest("I'll be out today to October 22", 12, 19);
            BasicTest("I'll be out Oct. 2 until the day after tomorrow", 12, 35);
            BasicTest("I'll be out today until next Sunday", 12, 23);
            BasicTest("I'll be out this Friday until next Sunday", 12, 29);

            BasicTest("I'll be out from Oct. 2 to October 22", 12, 25);
            BasicTest("I'll be out from 2015/08/12 until October 22", 12, 32);
            BasicTest("I'll be out from today till tomorrow", 12, 24);
            BasicTest("I'll be out from this Friday until next Sunday", 12, 34);
            BasicTest("I'll be out between Oct. 2 and October 22", 12, 29);

            BasicTest("I'll be out November 19-20", 12, 14);
            BasicTest("I'll be out November 19 to 20", 12, 17);
            BasicTest("I'll be out November between 19 and 20", 12, 26);

            BasicTest("I'll be out the third quarter of 2016", 12, 25);
            BasicTest("I'll be out the third quarter this year", 12, 27);
            BasicTest("I'll be out 2016 the third quarter", 12, 22);

            BasicTest("I'll be out 2015.3", 12, 6);
            BasicTest("I'll be out 2015-3", 12, 6);
            BasicTest("I'll be out 2015/3", 12, 6);
            BasicTest("I'll be out 3/2015", 12, 6);

            BasicTest("I'll be out the third week of 2027", 12, 22);
            BasicTest("I'll be out the third week next year", 12, 24);

            BasicTest("I'll leave this summer", 11, 11);
            BasicTest("I'll leave next spring", 11, 11);
            BasicTest("I'll leave the summer", 11, 10);
            BasicTest("I'll leave summer", 11, 6);
            BasicTest("I'll leave summer 2016", 11, 11);
            BasicTest("I'll leave summer of 2016", 11, 14);

            //test week of and month of
            BasicTest("week of september.15th", 0, 22);
            BasicTest("month of september.15th", 0, 23);

        }
    }
}