using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Italian : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void OrdinalModel(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModel(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberRangeModel-Italian.csv", "NumberRangeModel-Italian#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberRangeModel()
        {
            TestNumber();
        }
    }
}