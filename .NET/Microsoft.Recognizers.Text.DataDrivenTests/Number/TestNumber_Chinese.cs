using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Chinese : TestBase
    {
        public static TestResources TestResources { get; protected set; }
        public static IDictionary<string, IModel> Models { get; protected set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Models = new Dictionary<string, IModel>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            base.TestSpecInitialize(TestResources);
            base.ModelInitialize(Models);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Chinese.csv", "NumberModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CustomNumberModel-Chinese.csv", "CustomNumberModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CustomNumberModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Chinese.csv", "OrdinalModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Chinese.csv", "PercentModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            base.TestNumber();
        }
    }
}
