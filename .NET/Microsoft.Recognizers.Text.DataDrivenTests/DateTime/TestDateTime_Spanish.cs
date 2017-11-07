using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Spanish : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateExtractor-Spanish.csv", "DateExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeExtractor-Spanish.csv", "TimeExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodExtractor-Spanish.csv", "DatePeriodExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodExtractor-Spanish.csv", "TimePeriodExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeExtractor-Spanish.csv", "DateTimeExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimeExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodExtractor-Spanish.csv", "DateTimePeriodExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayExtractor-Spanish.csv", "HolidayExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationExtractor-Spanish.csv", "DurationExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetExtractor-Spanish.csv", "SetExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "MergedExtractor-Spanish.csv", "MergedExtractor-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void MergedExtractor()
        {
            base.ExtractorInitialize(Extractors);
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateParser-Spanish.csv", "DateParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Spanish.csv", "TimeParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DatePeriodParser-Spanish.csv", "DatePeriodParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DatePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimePeriodParser-Spanish.csv", "TimePeriodParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimeParser-Spanish.csv", "DateTimeParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public new void DateTimeParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DateTimePeriodParser-Spanish.csv", "DateTimePeriodParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DateTimePeriodParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "HolidayParser-Spanish.csv", "HolidayParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void HolidayParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DurationParser-Spanish.csv", "DurationParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DurationParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "SetParser-Spanish.csv", "SetParser-Spanish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void SetParser()
        {
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
            base.TestDateTimeParser();
        }
    }
}
