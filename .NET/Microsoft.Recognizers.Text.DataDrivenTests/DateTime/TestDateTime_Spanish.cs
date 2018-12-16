using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Spanish : TestBase
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
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DatePeriodExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimePeriodExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimePeriodExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HolidayExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DurationExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void SetExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void MergedExtractor(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DatePeriodParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimePeriodParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public new void DateTimeParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimePeriodParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HolidayParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DurationParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void SetParser(TestModel testSpec)
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeModel(TestModel testSpec)
        {
            base.TestDateTime(testSpec);
        }
    }
}
