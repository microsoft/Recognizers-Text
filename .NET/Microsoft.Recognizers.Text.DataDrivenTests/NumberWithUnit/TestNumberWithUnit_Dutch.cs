using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_Dutch : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Dutch.csv", "AgeModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AgeModel()
        {
            TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Dutch.csv", "CurrencyModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-Dutch.csv", "DimensionModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-Dutch.csv", "TemperatureModel-Dutch#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            TestNumberWithUnit();
        }
    }
}
