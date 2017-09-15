using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests
{
    [TestClass]
    public class TestBase
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public IModel Model { get; set; }
        public IExtractor Extractor { get; set; }
        public IDateTimeParser DateTimeParser { get; set; }
        public TestModel TestSpec { get; set; }

        public void TestSpecInitialize(TestResources resources)
        {
            TestSpec = resources.GetSpecForContext(TestContext);
        }

        public void ModelInitialize(IDictionary<string, IModel> models)
        {
            var key = TestContext.TestName;
            IModel model;
            if (!models.TryGetValue(key, out model))
            {
                model = TestContext.GetModel();
                models.Add(key, model);
            }
            Model = model;
        }

        public void ExtractorInitialize(IDictionary<string, IExtractor> extractors)
        {
            var key = TestContext.TestName;
            IExtractor extractor;
            if (!extractors.TryGetValue(key, out extractor))
            {
                extractor = TestContext.GetExtractor();
                extractors.Add(key, extractor);
            }
            Extractor = extractor;
        }

        public void ParserInitialize(IDictionary<string, IDateTimeParser> parsers)
        {
            var key = TestContext.TestName;
            IDateTimeParser parser;
            if (!parsers.TryGetValue(key, out parser))
            {
                parser = TestContext.GetDateTimeParser();
                parsers.Add(key, parser);
            }
            DateTimeParser = parser;
        }

        public void TestNumber()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }

            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }

            var result = Model.Parse(TestSpec.Input);

            Assert.AreEqual(TestSpec.Results.Count(), result.Count);
            if (TestSpec.Results.Count() > 0)
            {
                var expected = TestSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
            }
        }

        public void TestNumberWithUnit()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }

            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }

            var result = Model.Parse(TestSpec.Input);

            Assert.AreEqual(TestSpec.Results.Count(), result.Count);
            if (TestSpec.Results.Count() > 0)
            {
                var expected = TestSpec.CastResults<ModelResult>().FirstOrDefault();
                Assert.AreEqual(expected.TypeName, result.First().TypeName);
                Assert.AreEqual(expected.Resolution["value"], result.First().Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], result.First().Resolution["unit"]);
            }
        }

        public void TestDateTimeExtractor()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }
            var results = Extractor.Extract(TestSpec.Input);
            Assert.AreEqual(TestSpec.Results.Count(), results.Count);
            if (TestSpec.Results.Count() > 0)
            {
                var expected = TestSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        public void TestDateTimeParser()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }
            var referenceDateTime = TestSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(TestSpec.Input);
            var result = DateTimeParser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = TestSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (TestSpec.Results.Count() > 0)
            {
                var actualValue = result.Value as DateTimeResolutionResult;
                var expectedValue = JsonConvert.DeserializeObject<DateTimeResolutionResult>(expected.Value.ToString());

                Assert.AreEqual(expectedValue.Timex, actualValue.Timex);
                CollectionAssert.AreEqual(expectedValue.FutureResolution, actualValue.FutureResolution);
                CollectionAssert.AreEqual(expectedValue.PastResolution, actualValue.PastResolution);
            }
            else
            {
                Assert.AreEqual(null, result.Value);
            }
        }

        public void TestDateTimeMergedParser()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }
            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }
            var referenceDateTime = TestSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(TestSpec.Input);
            var result = DateTimeParser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = TestSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type, TestSpec.Input);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (TestSpec.Results.Count() > 0)
            {
                var values = result.Value as IDictionary<string, object>;
                var listValue = values["values"] as IList<Dictionary<string, string>>;
                var actualValue = listValue.FirstOrDefault();
                var expectedTemp = JsonConvert.DeserializeObject<IDictionary<string, IList<Dictionary<string, string>>>>(expected.Value.ToString());
                var expectedValue = expectedTemp["values"].FirstOrDefault();

                CollectionAssert.AreEqual(expectedValue, actualValue);
            }
            else
            {
                Assert.AreEqual(null, result.Value);
            }
        }
    }
}
