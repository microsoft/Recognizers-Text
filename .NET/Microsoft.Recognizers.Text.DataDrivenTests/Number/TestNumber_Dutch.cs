using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_Dutch : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelPercentMode(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelExperimentalMode(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }
        */

        [NetCoreTestDataSource]
        [TestMethod]
        public void OrdinalModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModelPercentMode(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModel(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }
        */

        /*
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModelExperimentalMode(TestModel testSpec)
        {
            TestSpec = testSpec;
            base.TestNumber();
        }
        */

    }
}
