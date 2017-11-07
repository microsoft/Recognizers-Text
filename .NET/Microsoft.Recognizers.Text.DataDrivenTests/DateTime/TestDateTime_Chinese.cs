using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Chinese : TestBase
    {
        public static TestResources TestResources { get; private set; }
        public static IDictionary<string, IDateTimeExtractor> Extractors { get; private set; }
        public static IDictionary<string, IDateTimeParser> Parsers { get; private set; }
        public static IDictionary<string, IModel> Models { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Extractors = new Dictionary<string, IDateTimeExtractor>();
            Parsers = new Dictionary<string, IDateTimeParser>();
            Models = new Dictionary<string, IModel>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-Chinese.csv", "DateExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-Chinese.csv", "TimeExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-Chinese.csv", "DatePeriodExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-Chinese.csv", "TimePeriodExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-Chinese.csv", "DateTimeExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-Chinese.csv", "DateTimePeriodExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-Chinese.csv", "HolidayExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-Chinese.csv", "DurationExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-Chinese.csv", "SetExtractor-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-Chinese.csv", "DateParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Chinese.csv", "TimeParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-Chinese.csv", "DatePeriodParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-Chinese.csv", "TimePeriodParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-Chinese.csv", "DateTimeParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-Chinese.csv", "DateTimePeriodParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-Chinese.csv", "HolidayParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-Chinese.csv", "DurationParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-Chinese.csv", "SetParser-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeModel-Chinese.csv", "DateTimeModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeModel()
        {
            base.ModelInitialize(Models);
            base.TestDateTime();
        }
    }
}
