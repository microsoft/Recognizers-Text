using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests.DateTime
{
    [TestClass]
    public class TestDateTime_Eng : TestBase
    {
        public static TestResources TestResources { get; private set; }
        public static IDictionary<string, IExtractor> Extractors { get; private set; }
        public static IDictionary<string, IDateTimeParser> Parsers { get; private set; }
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Extractors = new Dictionary<string, IExtractor>();
            Parsers = new Dictionary<string, IDateTimeParser>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestSpecInitialize(TestResources);
            base.ExtractorInitialize(Extractors);
            base.ParserInitialize(Parsers);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateExtractor-Eng.csv", "BaseDateExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeExtractor-Eng.csv", "BaseTimeExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimeExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodExtractor-Eng.csv", "BaseDatePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodExtractor-Eng.csv", "BaseTimePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeExtractor-Eng.csv", "BaseDateTimeExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodExtractor-Eng.csv", "BaseDateTimePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayExtractor-Eng.csv", "BaseHolidayExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationExtractor-Eng.csv", "BaseDurationExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetExtractor-Eng.csv", "BaseSetExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractor-Eng.csv", "BaseMergedExtractor-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractorSkipFromTo-Eng.csv", "BaseMergedExtractorSkipFromTo-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedExtractorSkipFromTo()
        {
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateParser-Eng.csv", "BaseDateParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Eng.csv", "TimeParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodParser-Eng.csv", "BaseDatePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodParser-Eng.csv", "BaseTimePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeParser-Eng.csv", "BaseDateTimeParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodParser-Eng.csv", "BaseDateTimePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayParser-Eng.csv", "BaseHolidayParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationParser-Eng.csv", "BaseDurationParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetParser-Eng.csv", "BaseSetParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedParser-Eng.csv", "BaseMergedParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedParser()
        {
            base.TestDateTimeMergedParser();
        }
    }
}
