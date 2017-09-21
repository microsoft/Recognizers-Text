using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
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

            var actualResults = Model.Parse(TestSpec.Input);
            var expectedResults = TestSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName);
                Assert.AreEqual(expected.Text, actual.Text);
                Assert.AreEqual(expected.Resolution["value"], actual.Resolution["value"]);
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

            var actualResults = Model.Parse(TestSpec.Input);
            var expectedResults = TestSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName);
                Assert.AreEqual(expected.Text, actual.Text);
                Assert.AreEqual(expected.Resolution["value"], actual.Resolution["value"]);
                Assert.AreEqual(expected.Resolution["unit"], actual.Resolution["unit"]);
            }
        }

        public void TestDateTime()
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

            var actualResults = ((DateTimeModel)Model).Parse(TestSpec.Input, referenceDateTime);
            var expectedResults = TestSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName);
                Assert.AreEqual(expected.Text, actual.Text);
                
                var values = actual.Resolution as IDictionary<string, object>;
                var listValues = values["values"] as IList<Dictionary<string, string>>;
                var actualValues = listValues.FirstOrDefault();

                var expectedObj = JsonConvert.DeserializeObject<IList<Dictionary<string, string>>>(expected.Resolution["values"].ToString());
                var expectedValues = expectedObj.FirstOrDefault();

                CollectionAssert.AreEqual(expectedValues, actualValues);
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

            var actualResults = Extractor.Extract(TestSpec.Input);
            var expectedResults = TestSpec.CastResults<ExtractResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.Type, actual.Type);
                Assert.AreEqual(expected.Text, actual.Text);
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
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();

            var expectedResults = TestSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count());

            foreach(var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;
                Assert.AreEqual(expected.Type, actual.Type);
                Assert.AreEqual(expected.Text, actual.Text);

                var actualValue = actual.Value as DateTimeResolutionResult;
                var expectedValue = JsonConvert.DeserializeObject<DateTimeResolutionResult>(expected.Value.ToString());

                Assert.AreEqual(expectedValue.Timex, actualValue.Timex);
                CollectionAssert.AreEqual(expectedValue.FutureResolution, actualValue.FutureResolution);
                CollectionAssert.AreEqual(expectedValue.PastResolution, actualValue.PastResolution);
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
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();
            
            var expectedResults = TestSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count(), $"Input: {TestSpec.Input}");

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                var values = actual.Value as IDictionary<string, object>;
                var listValues = values["values"] as IList<Dictionary<string, string>>;
                var actualValues = listValues.FirstOrDefault();

                var expectedObj = JsonConvert.DeserializeObject<IDictionary<string, IList<Dictionary<string, string>>>>(expected.Value.ToString());
                var expectedValues = expectedObj["values"].FirstOrDefault();

                CollectionAssert.AreEqual(expectedValues, actualValues);
            }
        }
    }
}
