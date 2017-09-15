using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests.DateTime
{
    [TestClass]
    public class TestDateTime_Spa : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateExtractor-Spa.csv", "BaseDateExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeExtractor-Spa.csv", "BaseTimeExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimeExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodExtractor-Spa.csv", "BaseDatePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodExtractor-Spa.csv", "BaseTimePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeExtractor-Spa.csv", "BaseDateTimeExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodExtractor-Spa.csv", "BaseDateTimePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayExtractor-Spa.csv", "BaseHolidayExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationExtractor-Spa.csv", "BaseDurationExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetExtractor-Spa.csv", "BaseSetExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetExtractor()
        {
            base.TestDateTimeExtractor();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractor-Spa.csv", "BaseMergedExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedExtractor()
        {
            base.TestDateTimeExtractor();
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateParser-Spa.csv", "BaseDateParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeParser-Spa.csv", "BaseTimeParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimeParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodParser-Spa.csv", "BaseDatePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodParser-Spa.csv", "BaseTimePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeParser-Spa.csv", "BaseDateTimeParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodParser-Spa.csv", "BaseDateTimePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayParser-Spa.csv", "BaseHolidayParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationParser-Spa.csv", "BaseDurationParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetParser-Spa.csv", "BaseSetParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetParser()
        {
            base.TestDateTimeParser();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedParser-Spa.csv", "BaseMergedParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedParser()
        {
            base.TestDateTimeMergedParser();
        }
    }
}
