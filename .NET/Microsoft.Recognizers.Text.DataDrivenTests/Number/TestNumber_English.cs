using System.Collections.Generic;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumber_English : TestBase
    {
        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModel(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelPercentMode(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberModelExperimentalMode(TestModel testSpec)
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

        [NetCoreTestDataSource]
        [TestMethod]
        public void PercentModelPercentMode(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModel(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }

        [NetCoreTestDataSource]
        [TestMethod]
        public void NumberRangeModelExperimentalMode(TestModel testSpec)
        {
            base.TestNumber(testSpec);
        }
    }
}
