﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    [TestClass]
    public class TestNumber_Spa : TestBase
    {
        public static TestResources TestResources { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTextContext(context);
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "NumberModel-Spa.csv", "NumberModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void NumberModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.ResultsLength, result.Count);
            if (testSpec.Results != null)
            {
                var expected = JsonConvert.DeserializeObject<IEnumerable<ModelResult>>(testSpec.Results.ToString()).FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "OrdinalModel-Spa.csv", "OrdinalModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void OrdinalModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.ResultsLength, result.Count);
            if (testSpec.Results != null)
            {
                var expected = JsonConvert.DeserializeObject<IEnumerable<ModelResult>>(testSpec.Results.ToString()).FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "PercentModel-Spa.csv", "PercentModel-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void PercentModel()
        {
            var testSpec = TestResources.GetSpecForContext(TestContext);
            var model = TestContext.GetModel();

            var result = model.Parse(testSpec.Input);
            Assert.AreEqual(testSpec.ResultsLength, result.Count);
            if (testSpec.Results != null)
            {
                var expected = JsonConvert.DeserializeObject<IEnumerable<ModelResult>>(testSpec.Results.ToString()).FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
            }
        }
    }
}
