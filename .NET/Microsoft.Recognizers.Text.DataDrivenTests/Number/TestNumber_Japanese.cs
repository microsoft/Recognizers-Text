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
            base.TestSpecInitialize(TestResources);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Japanese.csv", "NumberModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Japanese.csv", "OrdinalModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Japanese.csv", "PercentModel-Japanese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            base.TestNumber();
        }
    }
}
