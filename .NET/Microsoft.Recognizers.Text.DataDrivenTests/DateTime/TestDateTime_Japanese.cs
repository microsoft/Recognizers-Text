using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Japanese : TestBase
    {
        public static IDictionary<string, IDateTimeExtractor> Extractors { get; private set; }

        public static IDictionary<string, IDateTimeParser> Parsers { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Extractors = new Dictionary<string, IDateTimeExtractor>();
            Parsers = new Dictionary<string, IDateTimeParser>();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DatePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HolidayExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeZoneExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }
        */

        [NetCoreTestDataSource]
        [TestMethod]
        public void DurationExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void SetExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void MergedExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void MergedExtractorSkipFromTo(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }
        */

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DatePeriodParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimePeriodParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public new void DateTimeParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimePeriodParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HolidayParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeZoneParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }
        */

        [NetCoreTestDataSource]
        [TestMethod]
        public void DurationParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void SetParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void MergedParser(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeMergedParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeModel(TestModel testSpec)
        {
            TestDateTime(testSpec);
        }

    }
}
