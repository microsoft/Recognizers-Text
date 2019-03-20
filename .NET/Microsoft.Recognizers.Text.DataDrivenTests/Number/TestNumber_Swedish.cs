using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Swedish : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Swedish.csv", "NumberModel-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelPercentMode-Swedish.csv", "NumberModelPercentMode-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelPercentMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelExperimentalMode-Swedish.csv", "NumberModelExperimentalMode-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelExperimentalMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Swedish.csv", "OrdinalModel-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModelEnablePreview-Swedish.csv", "OrdinalModelEnablePreview-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModelEnablePreview()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Swedish.csv", "PercentModel-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModelPercentMode-Swedish.csv", "PercentModelPercentMode-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModelPercentMode()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-Swedish.csv", "NumberRangeModel-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModelExperimentalMode-Swedish.csv", "NumberRangeModelExperimentalMode-Swedish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModelExperimentalMode()
        {
            TestNumber();
        }
    }
}
