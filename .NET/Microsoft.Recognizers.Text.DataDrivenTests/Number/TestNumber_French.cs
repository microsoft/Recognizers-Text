using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Number
{
    [TestClass]
    public class TestNumber_French : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-French.csv", "NumberModel-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-French.csv", "OrdinalModel-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            base.TestNumber();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-French.csv", "PercentModel-French#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            base.TestNumber();
        }
    }
}
