using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTime_Japanese : TestBase
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
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimeExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DatePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TimePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DateTimePeriodExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void HolidayExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DurationExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void SetExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void MergedExtractor(TestModel testSpec)
        {
            ExtractorInitialize(Extractors);
            TestDateTimeExtractor(testSpec);
        }

        /*[NetCoreTestDataSource]
        [TestMethod]
        public void DateTimeModel(TestModel testSpec)
        {
            TestDateTime(testSpec);
        }*/
    }
}
