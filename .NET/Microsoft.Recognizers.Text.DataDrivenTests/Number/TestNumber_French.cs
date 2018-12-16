using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_French : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void OrdinalModel(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModel(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }
    }
}
