using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnit_Chinese : TestBase
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Chinese.csv", "AgeModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AgeModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Chinese.csv", "CurrencyModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-Chinese.csv", "DimensionModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            base.TestNumberWithUnit();
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-Chinese.csv", "TemperatureModel-Chinese#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            base.TestNumberWithUnit();
        }
    }
}
