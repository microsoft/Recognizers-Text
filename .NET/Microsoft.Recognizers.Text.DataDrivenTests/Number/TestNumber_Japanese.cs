using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Japanese : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Japanese.csv", "NumberModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Japanese.csv", "OrdinalModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Japanese.csv", "PercentModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            TestNumber();
        }

        /*
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-Japanese.csv", "NumberRangeModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModelExperimentalMode-Japanese.csv", "NumberRangeModelExperimentalMode-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModelExperimentalMode()
        {
            TestNumber();
        }
        */
    }
}
