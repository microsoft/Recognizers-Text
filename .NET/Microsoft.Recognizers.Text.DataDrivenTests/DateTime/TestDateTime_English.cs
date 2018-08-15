using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_English : TestBase
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
            base.TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-English.csv", "DateExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-English.csv", "TimeExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-English.csv", "DatePeriodExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-English.csv", "TimePeriodExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-English.csv", "DateTimeExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-English.csv", "DateTimePeriodExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-English.csv", "HolidayExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeZoneExtractor-English.csv", "TimeZoneExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeZoneExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-English.csv", "DurationExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-English.csv", "SetExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-English.csv", "MergedExtractor-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractorSkipFromTo-English.csv", "MergedExtractorSkipFromTo-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractorSkipFromTo()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-English.csv", "DateParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-English.csv", "TimeParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-English.csv", "DatePeriodParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-English.csv", "TimePeriodParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-English.csv", "DateTimeParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-English.csv", "DateTimePeriodParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-English.csv", "HolidayParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeZoneParser-English.csv", "TimeZoneParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeZoneParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-English.csv", "DurationParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-English.csv", "SetParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedParser-English.csv", "MergedParser-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeMergedParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-English.csv", "DateTimeModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModel()
        {
            base.TestDateTime();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelSplitDateAndTime-English.csv", "DateTimeModelSplitDateAndTime-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelSplitDateAndTime()
        {
            base.TestDateTime();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelCalendarMode-English.csv", "DateTimeModelCalendarMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelCalendarMode()
        {
            base.TestDateTimeAlt();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelExtendedTypes-English.csv", "DateTimeModelExtendedTypes-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelExtendedTypes()
        {
            base.TestDateTimeAlt();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelComplexCalendar-English.csv", "DateTimeModelComplexCalendar-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelComplexCalendar()
        {
            base.TestDateTimeAlt();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelExperimentalMode-English.csv", "DateTimeModelExperimentalMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModelExperimentalMode()
        {
            base.TestDateTimeAlt();
        }
    }
}
