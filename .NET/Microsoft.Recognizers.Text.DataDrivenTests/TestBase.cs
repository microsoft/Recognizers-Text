using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

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
        
        public IDateTimeExtractor Extractor { get; set; }
        public IDateTimeParser DateTimeParser { get; set; }
        public TestModel TestSpec { get; set; }

        public void TestSpecInitialize(TestResources resources)
        {
            TestSpec = resources.GetSpecForContext(TestContext);
        }

        public void ExtractorInitialize(IDictionary<string, IDateTimeExtractor> extractors)
        {
            var key = TestContext.TestName;
            if (!extractors.TryGetValue(key, out IDateTimeExtractor extractor))
            {
                extractor = TestContext.GetExtractor();
                extractors.Add(key, extractor);
            }

            Extractor = extractor;
        }

        public void ParserInitialize(IDictionary<string, IDateTimeParser> parsers)
        {
            var key = TestContext.TestName;
            if (!parsers.TryGetValue(key, out IDateTimeParser parser))
            {
                parser = TestContext.GetDateTimeParser();
                parsers.Add(key, parser);
            }

            DateTimeParser = parser;
        }

        public void TestNumber()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestNumberWithUnit()
        {
            TestPreValidation();
            ValidateResults(new[] { ResolutionKey.Unit });
        }

        public void TestCurrency()
        {
            TestPreValidation();
            ValidateResults(new[] { ResolutionKey.Unit, ResolutionKey.IsoCurrency });
        }

        public void TestDateTime()
        {
            TestPreValidation();

            var actualResults = TestContext.GetModelParseResults(TestSpec);
            var expectedResults = TestSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(TestSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(TestSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(TestSpec));
                Assert.AreEqual(expected.End, actual.End, GetMessage(TestSpec));

                var values = actual.Resolution as IDictionary<string, object>;
                var actualValues = (values[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>).ToList();
                var expectedValues = JsonConvert.DeserializeObject<IList<Dictionary<string, string>>>(expected.Resolution[ResolutionKey.ValueSet].ToString());

                Assert.AreEqual(expectedValues.Count, actualValues.Count, GetMessage(TestSpec));
                foreach (var t in expectedValues.Zip(actualValues, Tuple.Create))
                {
                   CollectionAssert.AreEqual(t.Item1, t.Item2, GetMessage(TestSpec)); 
                }

            }
        }

        public void TestDateTimeAlt()
        {
            TestPreValidation();
            
            var actualResults = TestContext.GetModelParseResults(TestSpec);
            var expectedResults = TestSpec.CastResults<ExtendedModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(TestSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(TestSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(TestSpec));
                Assert.AreEqual(expected.End, actual.End, GetMessage(TestSpec));

                if (expected.ParentText != null)
                {
                    Assert.AreEqual(expected.ParentText, ((ExtendedModelResult)actual).ParentText, GetMessage(TestSpec));
                }

                var values = actual.Resolution as IDictionary<string, object>;
                var listValues = values[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>;
                var actualValues = listValues;

                var expectedObj = JsonConvert.DeserializeObject<IList<Dictionary<string, string>>>(expected.Resolution[ResolutionKey.ValueSet].ToString());
                var expectedValues = expectedObj;

                Assert.AreEqual(expectedValues.Count, actualValues.Count, GetMessage(TestSpec));

                foreach (var value in Enumerable.Zip(expectedValues, actualValues, Tuple.Create))
                {
                    CollectionAssert.AreEqual(value.Item1, value.Item2, GetMessage(TestSpec));
                }

            }
        }

        public void TestDateTimeExtractor()
        {
            TestPreValidation();

            var referenceDateTime = TestSpec.GetReferenceDateTime();

            var actualResults = Extractor.Extract(TestSpec.Input, referenceDateTime);
            var expectedResults = TestSpec.CastResults<ExtractResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(TestSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.Type, actual.Type, GetMessage(TestSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(TestSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(TestSpec));
            }

        }

        public void TestDateTimeParser()
        {
            TestPreValidation();

            var referenceDateTime = TestSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(TestSpec.Input, referenceDateTime);
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();

            var expectedResults = TestSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count(), GetMessage(TestSpec));

            foreach(var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;
                Assert.AreEqual(expected.Type, actual.Type, GetMessage(TestSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(TestSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(TestSpec));

                var actualValue = actual.Value as DateTimeResolutionResult;
                var expectedValue = JsonConvert.DeserializeObject<DateTimeResolutionResult>(expected.Value.ToString());

                Assert.IsNotNull(actualValue, GetMessage(TestSpec));
                Assert.AreEqual(expectedValue.Timex, actualValue.Timex, GetMessage(TestSpec));
                if (expectedValue.Mod != null || actualValue.Mod != null)
                {
                    Assert.IsNotNull(expectedValue.Mod, GetMessage(TestSpec));
                    Assert.IsNotNull(actualValue.Mod, GetMessage(TestSpec));
                    Assert.AreEqual(expectedValue.Mod, actualValue.Mod, GetMessage(TestSpec));
                }

                CollectionAssert.AreEqual(expectedValue.FutureResolution, actualValue.FutureResolution, GetMessage(TestSpec));
                CollectionAssert.AreEqual(expectedValue.PastResolution, actualValue.PastResolution, GetMessage(TestSpec));

                if (expectedValue.TimeZoneResolution != null || actualValue.TimeZoneResolution != null)
                {
                    Assert.IsNotNull(actualValue.TimeZoneResolution, GetMessage(TestSpec));
                    Assert.IsNotNull(expectedValue.TimeZoneResolution, GetMessage(TestSpec));
                    Assert.AreEqual(expectedValue.TimeZoneResolution.Value, actualValue.TimeZoneResolution.Value, GetMessage(TestSpec));
                    Assert.AreEqual(expectedValue.TimeZoneResolution.UtcOffsetMins, actualValue.TimeZoneResolution.UtcOffsetMins, GetMessage(TestSpec));
                }

            }
        }

        public void TestDateTimeMergedParser()
        {
            TestPreValidation();

            var referenceDateTime = TestSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(TestSpec.Input, referenceDateTime);
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();
            
            var expectedResults = TestSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count(), GetMessage(TestSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));
                Assert.AreEqual(expected.Type, actual.Type, GetMessage(TestSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(TestSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(TestSpec));

                var values = actual.Value as IDictionary<string, object>;
                if (values != null)
                {
                    var actualValues = values[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>;

                    var expectedObj = JsonConvert.DeserializeObject<IDictionary<string, IList<Dictionary<string, string>>>>(expected.Value.ToString());
                    var expectedValues = expectedObj[ResolutionKey.ValueSet];

                    foreach (var results in Enumerable.Zip(expectedValues, actualValues, Tuple.Create))
                    {
                        CollectionAssert.AreEqual(results.Item1, results.Item2, GetMessage(TestSpec));
                    }

                }
            }
        }
        
        public void TestIpAddress()
        {
            TestPreValidation();
            ValidateResults(new List<string>() { ResolutionKey.Type });
        }

        public void TestPhoneNumber()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestMention()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestHashtag()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestEmail()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestURL()
        {
            TestPreValidation();
            ValidateResults();
        }

        public void TestChoice()
        {
            TestPreValidation();
            ValidateResults(new string[] { ResolutionKey.Value, ResolutionKey.Score });
        }

        private void TestPreValidation()
        {
            if (TestUtils.EvaluateSpec(TestSpec, out string message))
            {
                Assert.Inconclusive(message);
            }

            if (Debugger.IsAttached && TestSpec.Debug)
            {
                Debugger.Break();
            }
        }

        private void ValidateResults(IEnumerable<string> testResolutionKeys = null)
        {
            var actualResults = TestContext.GetModelParseResults(TestSpec);
            var expectedResults = TestSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(TestSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(TestSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(TestSpec));

                Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value], GetMessage(TestSpec));
                
                foreach (var key in testResolutionKeys ?? Enumerable.Empty<string>())
                {
                    if (!actual.Resolution.ContainsKey(key) && !expected.Resolution.ContainsKey(key))
                    {
                        continue;
                    }

                    Assert.AreEqual(expected.Resolution[key], actual.Resolution[key], GetMessage(TestSpec));
                }

            }
        }

        private static string GetMessage(TestModel spec)
        {
            return $"Input: \"{spec.Input}\"";
        }

    }
}
