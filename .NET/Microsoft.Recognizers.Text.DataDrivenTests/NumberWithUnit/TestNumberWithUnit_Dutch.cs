using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_Dutch : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void AgeModel(TestModel testSpec)
        {
            base.TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void CurrencyModel(TestModel testSpec)
        {
            base.TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DimensionModel(TestModel testSpec)
        {
            base.TestNumberWithUnit(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TemperatureModel(TestModel testSpec)
        {
            base.TestNumberWithUnit(testSpec);
        }
    }
}
