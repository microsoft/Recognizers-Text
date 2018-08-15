using System.Collections.Generic;

using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Italian : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-Italian.csv", "DateExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-Italian.csv", "TimeExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-Italian.csv", "DatePeriodExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-Italian.csv", "TimePeriodExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-Italian.csv", "DateTimeExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-Italian.csv", "DateTimePeriodExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-Italian.csv", "HolidayExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-Italian.csv", "DurationExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-Italian.csv", "SetExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-Italian.csv", "MergedExtractor-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractorSkipFromTo-Italian.csv", "MergedExtractorSkipFromTo-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractorSkipFromTo()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-Italian.csv", "DateParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Italian.csv", "TimeParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-Italian.csv", "DatePeriodParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-Italian.csv", "TimePeriodParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-Italian.csv", "DateTimeParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-Italian.csv", "DateTimePeriodParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-Italian.csv", "HolidayParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-Italian.csv", "DurationParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-Italian.csv", "SetParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedParser-Italian.csv", "MergedParser-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeMergedParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-Italian.csv", "DateTimeModel-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModel()
        {
            base.TestDateTime();
        }

        ////[DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModelSplitDateAndTime-Italian.csv", "DateTimeModelSplitDateAndTime-Italian#csv", DataAccessMethod.Sequential)]
        ////[TestMethod]
        ////public void DateTimeModelSplitDateAndTime()
        ////{
        ////    base.ModelInitialize(Models);
        ////    base.TestDateTime();
        ////}
    }
}
