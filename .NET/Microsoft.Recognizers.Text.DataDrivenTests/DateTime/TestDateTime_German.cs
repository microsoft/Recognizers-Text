using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_German : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-German.csv", "DateExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-German.csv", "TimeExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-German.csv", "DatePeriodExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-German.csv", "TimePeriodExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-German.csv", "DateTimeExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-German.csv", "DateTimePeriodExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-German.csv", "HolidayExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-German.csv", "DurationExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-German.csv", "SetExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-German.csv", "MergedExtractor-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractorSkipFromTo-German.csv", "MergedExtractorSkipFromTo-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractorSkipFromTo()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-German.csv", "DateParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-German.csv", "TimeParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-German.csv", "DatePeriodParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-German.csv", "TimePeriodParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-German.csv", "DateTimeParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-German.csv", "DateTimePeriodParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-German.csv", "HolidayParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-German.csv", "DurationParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-German.csv", "SetParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedParser-German.csv", "MergedParser-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeMergedParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-German.csv", "DateTimeModel-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModel()
        {
            TestDateTime();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelSplitDateAndTime-German.csv", "DateTimeModelSplitDateAndTime-German#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelSplitDateAndTime()
        {
            TestDateTime();
        }
    }
}
