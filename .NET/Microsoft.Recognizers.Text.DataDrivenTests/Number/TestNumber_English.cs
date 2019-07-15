using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_English : TestBase
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

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel()
        {
            TestNumber();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void OrdinalModelSuppressExtendedTypes()
        {
            TestNumber();
        }


        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelPercentMode(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelExperimentalMode(TestModel testSpec)
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

        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModelPercentMode(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModel(TestModel testSpec)
        {
            TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModelExperimentalMode(TestModel testSpec)
        {
            TestNumber(testSpec);
        }
    }
}
