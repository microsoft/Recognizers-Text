using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests.NumberWithUnit
{
    [TestClass]
    public class TestNumberWithUnit_Spa : TestBase
    {
        public static TestResources TestResources { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "AgeModel-Spa.csv", "AgeModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void AgeModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            if (TestUtils.EvaluateSpec(testSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && testSpec.Debug)
            {
                Debugger.Break();
            }
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), result.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], result.First().Resolution["unit"]);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "CurrencyModel-Spa.csv", "CurrencyModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void CurrencyModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            if (TestUtils.EvaluateSpec(testSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && testSpec.Debug)
            {
                Debugger.Break();
            }
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), result.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], result.First().Resolution["unit"]);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "DimensionModel-Spa.csv", "DimensionModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void DimensionModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            if (TestUtils.EvaluateSpec(testSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && testSpec.Debug)
            {
                Debugger.Break();
            }
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), result.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], result.First().Resolution["unit"]);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TemperatureModel-Spa.csv", "TemperatureModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TemperatureModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            if (TestUtils.EvaluateSpec(testSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && testSpec.Debug)
            {
                Debugger.Break();
            }
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), result.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], result.First().Resolution["unit"]);
            }
        }
    }
}
