using System.Collections.Generic;
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
            base.TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-English.csv", "NumberModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModelPercentMode-English.csv", "NumberModelPercentMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModelPercentMode()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-English.csv", "OrdinalModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-English.csv", "PercentModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModelPercentMode-English.csv", "PercentModelPercentMode-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModelPercentMode()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-English.csv", "NumberRangeModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            base.TestNumber();
        }
    }
}
