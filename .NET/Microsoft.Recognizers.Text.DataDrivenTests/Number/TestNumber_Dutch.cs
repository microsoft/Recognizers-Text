using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Dutch : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Dutch.csv", "NumberModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelPercentMode-Dutch.csv", "NumberModelPercentMode-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelPercentMode()
        {
            TestNumber();
        }

        /*
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelExperimentalMode-Dutch.csv", "NumberModelExperimentalMode-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelExperimentalMode()
        {
            TestNumber();
        }
        */

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Dutch.csv", "OrdinalModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            TestNumber();
        }

        /*
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Dutch.csv", "PercentModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModelPercentMode-Dutch.csv", "PercentModelPercentMode-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModelPercentMode()
        {
            TestNumber();
        }
        */

        /*
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-Dutch.csv", "NumberRangeModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            TestNumber();
        }
        */

        /*
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModelExperimentalMode-Dutch.csv", "NumberRangeModelExperimentalMode-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModelExperimentalMode()
        {
            TestNumber();
        }
        */
    }
}
