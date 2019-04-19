using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_English : TestBase
    {
        public static TestResources TestResources { get; protected set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-English.csv", "NumberModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelPercentMode-English.csv", "NumberModelPercentMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelPercentMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelExperimentalMode-English.csv", "NumberModelExperimentalMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelExperimentalMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-English.csv", "OrdinalModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-English.csv", "PercentModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModelPercentMode-English.csv", "PercentModelPercentMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModelPercentMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-English.csv", "NumberRangeModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModelExperimentalMode-English.csv", "NumberRangeModelExperimentalMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModelExperimentalMode()
        {
            TestNumber();
        }
    }
}
