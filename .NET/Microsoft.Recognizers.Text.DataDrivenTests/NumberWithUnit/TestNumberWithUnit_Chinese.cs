using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_Chinese : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void AgeModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumberWithUnit();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void CurrencyModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestCurrency();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void DimensionModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumberWithUnit();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void TemperatureModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumberWithUnit();
        }
    }
}
