using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Dutch : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-Dutch.csv", "DateExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-Dutch.csv", "TimeExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void TimeExtractor()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-Dutch.csv", "DatePeriodExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DatePeriodExtractor()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-Dutch.csv", "TimePeriodExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-Dutch.csv", "DateTimeExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-Dutch.csv", "DateTimePeriodExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimePeriodExtractor()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-Dutch.csv", "HolidayExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeZoneExtractor-Dutch.csv", "TimeZoneExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void TimeZoneExtractor()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-Dutch.csv", "DurationExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-Dutch.csv", "SetExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-Dutch.csv", "MergedExtractor-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void MergedExtractor()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractorSkipFromTo-Dutch.csv", "MergedExtractorSkipFromTo-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void MergedExtractorSkipFromTo()
        // {
        //    ExtractorInitialize(Extractors);
        //    TestDateTimeExtractor();
        // }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-Dutch.csv", "DateParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Dutch.csv", "TimeParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-Dutch.csv", "DatePeriodParser-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DatePeriodParser()
        // {
        //    ExtractorInitialize(Extractors);
        //    ParserInitialize(Parsers);
        //    TestDateTimeParser();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-Dutch.csv", "TimePeriodParser-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void TimePeriodParser()
        // {
        //    ExtractorInitialize(Extractors);
        //    ParserInitialize(Parsers);
        //    TestDateTimeParser();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-Dutch.csv", "DateTimeParser-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public new void DateTimeParser()
        // {
        //    ExtractorInitialize(Extractors);
        //    ParserInitialize(Parsers);
        //    TestDateTimeParser();
        // }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-Dutch.csv", "DateTimePeriodParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-Dutch.csv", "HolidayParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeZoneParser-Dutch.csv", "TimeZoneParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeZoneParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-Dutch.csv", "DurationParser-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            ExtractorInitialize(Extractors);
            ParserInitialize(Parsers);
            TestDateTimeParser();
        }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-Dutch.csv", "SetParser-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void SetParser()
        // {
        //    ExtractorInitialize(Extractors);
        //    ParserInitialize(Parsers);
        //    TestDateTimeParser();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedParser-Dutch.csv", "MergedParser-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void MergedParser()
        // {
        //    ExtractorInitialize(Extractors);
        //    ParserInitialize(Parsers);
        //    TestDateTimeMergedParser();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-Dutch.csv", "DateTimeModel-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModel()
        // {
        //    TestDateTime();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelSplitDateAndTime-Dutch.csv", "DateTimeModelSplitDateAndTime-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModelSplitDateAndTime()
        // {
        //    TestDateTime();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelCalendarMode-Dutch.csv", "DateTimeModelCalendarMode-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModelCalendarMode()
        // {
        //    TestDateTimeAlt();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelExtendedTypes-Dutch.csv", "DateTimeModelExtendedTypes-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModelExtendedTypes()
        // {
        //    TestDateTimeAlt();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelComplexCalendar-Dutch.csv", "DateTimeModelComplexCalendar-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModelComplexCalendar()
        // {
        //    TestDateTimeAlt();
        // }

        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelExperimentalMode-Dutch.csv", "DateTimeModelExperimentalMode-Dutch#csv", DataAccessMethod.Sequential)]
        // [TestMethod]
        // public void DateTimeModelExperimentalMode()
        // {
        //    TestDateTimeAlt();
        // }
    }
}
