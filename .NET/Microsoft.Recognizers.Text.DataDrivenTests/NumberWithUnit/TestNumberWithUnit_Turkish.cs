using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_Turkish : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void AgeModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DimensionModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TemperatureModel(TestModel testSpec)
        {
            TestNumberWithUnit(testSpec);
        }
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Turkish.csv", "CurrencyModel-Turkish#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            TestCurrency();
        }

        /* TODO uncomment with the Turkish dimension changes
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-English.csv", "DimensionModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            TestNumberWithUnit();
        }*/

        /* TODO uncomment with the Turkish temprature changes
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-English.csv", "TemperatureModel-English#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            TestNumberWithUnit();
        }*/
    }
}