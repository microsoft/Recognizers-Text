using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataDrivenTests.NumberWithUnit
{
    [TestClass]
    public class TestNumberWithUnit_Eng : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Eng.csv", "AgeModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AgeModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Eng.csv", "CurrencyModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-Eng.csv", "DimensionModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-Eng.csv", "TemperatureModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            base.TestNumberWithUnit();
        }
    }
}
