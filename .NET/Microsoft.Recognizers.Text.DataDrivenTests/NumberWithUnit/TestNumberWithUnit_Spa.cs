using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataDrivenTests.NumberWithUnit
{
    [TestClass]
    public class TestNumberWithUnit_Spa : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Spa.csv", "AgeModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AgeModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Spa.csv", "CurrencyModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-Spa.csv", "DimensionModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-Spa.csv", "TemperatureModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            base.TestNumberWithUnit();
        }
    }
}
