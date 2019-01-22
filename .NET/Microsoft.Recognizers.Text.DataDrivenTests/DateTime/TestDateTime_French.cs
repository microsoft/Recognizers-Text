using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_French : TestBase
    {
        public static TestResources TestResources { get; private set; }

        public static IDictionary<string, IDateTimeExtractor> Extractors { get; private set; }

        public static IDictionary<string, IDateTimeParser> Parsers { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Extractors = new Dictionary<string, IDateTimeExtractor>();
            Parsers = new Dictionary<string, IDateTimeParser>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-French.csv", "DateExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-French.csv", "TimeExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-French.csv", "DatePeriodExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-French.csv", "TimePeriodExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-French.csv", "DateTimeExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-French.csv", "DateTimePeriodExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-French.csv", "HolidayExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-French.csv", "DurationExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-French.csv", "SetExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-French.csv", "MergedExtractor-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractorSkipFromTo-French.csv", "MergedExtractorSkipFromTo-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractorSkipFromTo()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-French.csv", "DateParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-French.csv", "TimeParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-French.csv", "DatePeriodParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-French.csv", "TimePeriodParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-French.csv", "DateTimeParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-French.csv", "DateTimePeriodParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-French.csv", "HolidayParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-French.csv", "DurationParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-French.csv", "SetParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedParser-French.csv", "MergedParser-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeMergedParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-French.csv", "DateTimeModel-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModel()
        {
            TestDateTime();
        }

        ////[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelSplitDateAndTime-French.csv", "DateTimeModelSplitDateAndTime-French#csv", DataAccessMethod.Sequential)]
        ////[TestMethod]
        ////public void DateTimeModelSplitDateAndTime()
        ////{
        ////    ModelInitialize(Models);
        ////    TestDateTime();
        ////}
    }
}
