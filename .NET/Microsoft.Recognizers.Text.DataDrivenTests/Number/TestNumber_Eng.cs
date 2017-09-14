using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Number
{
    [TestClass]
    public class TestNumber_Eng : TestBase
    {
        public static TestResources TestResources { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Eng.csv", "NumberModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
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
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Eng.csv", "OrdinalModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
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
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Eng.csv", "PercentModel-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
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
            }
        }
    }
}
