using Microsoft.VisualStudio.TestTools.UnitTesting;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.English.Tests
{
    [TestClass]
    public class TestHolidayParser
    {
        readonly DateObject refrenceDay;
        readonly BaseHolidayParser parser;
        readonly BaseHolidayExtractor extractor;

        public void BasicTest(string text, DateObject futureDate, DateObject pastDate)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(futureDate, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(pastDate, ((DateTimeResolutionResult) pr.Value).PastValue);
        }

        public void BasicTest(string text, DateObject date)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).FutureValue);
            Assert.AreEqual(date, ((DateTimeResolutionResult) pr.Value).PastValue);
        }

        public void BasicTest(string text, string luisValueStr)
        {
            var er = extractor.Extract(text);
            Assert.AreEqual(1, er.Count);
            var pr = parser.Parse(er[0], refrenceDay);
            Assert.AreEqual(Constants.SYS_DATETIME_DATE, pr.Type);
            Assert.AreEqual(luisValueStr, ((DateTimeResolutionResult) pr.Value).Timex);
        }

        public TestHolidayParser()
        {
            refrenceDay = new DateObject(2016, 11, 7);
            parser = new BaseHolidayParser(new EnglishHolidayParserConfiguration());
            extractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        }

        [TestMethod]
        public void TestHolidayParse()
        {
            BasicTest("I'll go back on christmas",
                new DateObject(2016, 12, 25),
                new DateObject(2015, 12, 25));

            BasicTest("I'll go back on Yuandan", 
                new DateObject(2017, 1, 1), 
                new DateObject(2016, 1, 1));

            BasicTest("I'll go back on thanks giving day", 
                new DateObject(2016, 11, 24), 
                new DateObject(2015, 11, 26));

            BasicTest("I'll go back on thanksgiving",
                new DateObject(2016, 11, 24),
                new DateObject(2015, 11, 26));

            BasicTest("I'll go back on father's day",
                new DateObject(2017, 6, 18),
                new DateObject(2016, 6, 19));

            BasicTest("I'll go back on Yuandan of next year",
                new DateObject(2017, 1, 1), 
                new DateObject(2017, 1, 1));

            BasicTest("I'll go back on thanks giving day 2010", 
                new DateObject(2010, 11, 25),
                new DateObject(2010, 11, 25));

            BasicTest("I'll go back on father's day of 2015",
                new DateObject(2015, 6, 21),
                new DateObject(2015, 6, 21));
        }

        [TestMethod]
        public void TestHolidayParseLuis()
        {
            BasicTest("I'll go back on Yuandan", "XXXX-01-01");
            BasicTest("I'll go back on thanks giving day", "XXXX-11-WXX-4-4");
            BasicTest("I'll go back on father's day", "XXXX-06-WXX-6-3");

            BasicTest("I'll go back on Yuandan of next year", "2017-01-01");
            BasicTest("I'll go back on thanks giving day 2010", "2010-11-WXX-4-4");
            BasicTest("I'll go back on father's day of 2015", "2015-06-WXX-6-3");
        }
    }
}