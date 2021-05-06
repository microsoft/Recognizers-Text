using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public void TestNumber(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new[] { ResolutionKey.Value, ResolutionKey.SubType });
        }

        public void TestNumberWithUnit(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new[] { ResolutionKey.Unit, ResolutionKey.SubType });
        }

        public void TestCurrency(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new[] { ResolutionKey.Unit, ResolutionKey.IsoCurrency });
        }

        public void TestDateTime(TestModel testSpec)
        {
            TestPreValidation(testSpec);

            var actualResults = TestContext.GetModelParseResults(testSpec);
            var expectedResults = testSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(testSpec));

            try
            {

                foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
                {
                    var expected = tuple.Item1;
                    var actual = tuple.Item2;

                    Assert.AreEqual(expected.Text, actual.Text, GetMessage(testSpec));
                    Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(testSpec));
                    Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                    Assert.AreEqual(expected.End, actual.End, GetMessage(testSpec));

                    if (testSpec.IgnoreResolution)
                    {
                        Assert.Inconclusive(GetMessage(testSpec) + ". Resolution not validated.");
                    }
                    else
                    {
                        var values = actual.Resolution as IDictionary<string, object>;

                        // Actual ValueSet types should not be modified as that's considered a breaking API change
                        var actualValues = ((List<Dictionary<string, string>>)values[ResolutionKey.ValueSet]).ToList();
                        var expectedValues =
                            JsonConvert.DeserializeObject<IList<Dictionary<string, string>>>(expected.Resolution[ResolutionKey.ValueSet].ToString());

                        Assert.AreEqual(expectedValues.Count, actualValues.Count, GetMessage(testSpec));

                        foreach (var resolutionValues in expectedValues.Zip(actualValues, Tuple.Create))
                        {
                            Assert.AreEqual(resolutionValues.Item1.Count, resolutionValues.Item2.Count,
                                            GetMessage(testSpec));

                            var expectedResolution = resolutionValues.Item1.OrderBy(o => o.Key).ToImmutableDictionary();
                            var actualResolution = resolutionValues.Item2.OrderBy(o => o.Key).ToImmutableDictionary();

                            for (int i = 0; i < expectedResolution.Count; i++)
                            {
                                var expectedKey = expectedResolution.ElementAt(i).Key;
                                Assert.AreEqual(expectedKey, actualResolution.ElementAt(i).Key, GetMessage(testSpec));

                                var expectedValue = expectedResolution[expectedKey];
                                var actualValue = actualResolution[expectedKey];

                                Assert.AreEqual(expectedValue, actualValue, GetMessage(testSpec));
                            }

                        }
                    }
                }

            }
            catch (NullReferenceException nre)
            {
                throw new ApplicationException(GetMessage(testSpec), nre);
            }
        }

        public void TestDateTimeAlt(TestModel testSpec)
        {
            TestPreValidation(testSpec);

            var actualResults = TestContext.GetModelParseResults(testSpec);
            var expectedResults = testSpec.CastResults<ExtendedModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(testSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(testSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(testSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                Assert.AreEqual(expected.End, actual.End, GetMessage(testSpec));

                if (expected.ParentText != null)
                {
                    Assert.AreEqual(expected.ParentText, ((ExtendedModelResult)actual).ParentText, GetMessage(testSpec));
                }

                if (testSpec.IgnoreResolution)
                {
                    Assert.Inconclusive(GetMessage(testSpec) + ". Resolution not validated.");
                }
                else
                {
                    // Actual ValueSet types should not be modified as that's considered a breaking API change
                    var actualValues =
                        ((IDictionary<string, object>)actual.Resolution)[ResolutionKey.ValueSet] as
                        IList<Dictionary<string, string>>;

                    var expectedValues =
                        JsonConvert.DeserializeObject<IList<Dictionary<string, string>>>(expected
                            .Resolution[ResolutionKey.ValueSet].ToString());

                    Assert.AreEqual(expectedValues.Count, actualValues.Count, GetMessage(testSpec));

                    foreach (var value in expectedValues.Zip(actualValues, Tuple.Create))
                    {
                        Assert.AreEqual(value.Item1.Count, value.Item2.Count, GetMessage(testSpec));
                        CollectionAssert.AreEqual(value.Item1.OrderBy(o => o.Key).ToImmutableDictionary(),
                            value.Item2.OrderBy(o => o.Key).ToImmutableDictionary(), GetMessage(testSpec));
                    }
                }
            }
        }

        public void TestDateTimeExtractor(TestModel testSpec)
        {
            TestPreValidation(testSpec);

            var referenceDateTime = testSpec.GetReferenceDateTime();

            var actualResults = Extractor.Extract(testSpec.Input.ToLowerInvariant(), referenceDateTime);
            var expectedResults = testSpec.CastResults<ExtractResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(testSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;
                var ignoreResultCase = true;

                Assert.AreEqual(expected.Type, actual.Type, GetMessage(testSpec));
                Assert.AreEqual(expected.Text, actual.Text, ignoreResultCase, GetMessage(testSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(testSpec));
            }
        }

        public void TestDateTimeParser(TestModel testSpec)
        {
            TestPreValidation(testSpec);

            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input.ToLowerInvariant(), referenceDateTime);
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();

            var expectedResults = testSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count(), GetMessage(testSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;
                var ignoreResultCase = true;

                Assert.AreEqual(expected.Type, actual.Type, GetMessage(testSpec));
                Assert.AreEqual(expected.Text, actual.Text, ignoreResultCase, GetMessage(testSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(testSpec));

                if (testSpec.IgnoreResolution)
                {
                    Assert.Inconclusive(GetMessage(testSpec) + ". Resolution not validated.");
                }
                else
                {
                    var actualValue = actual.Value as DateTimeResolutionResult;
                    var expectedValue = JsonConvert.DeserializeObject<DateTimeResolutionResult>(expected.Value.ToString());

                    Assert.IsNotNull(actualValue, GetMessage(testSpec));
                    Assert.AreEqual(expectedValue.Timex, actualValue.Timex, GetMessage(testSpec));
                    if (expectedValue.Mod != null || actualValue.Mod != null)
                    {
                        Assert.IsNotNull(expectedValue.Mod, GetMessage(testSpec));
                        Assert.IsNotNull(actualValue.Mod, GetMessage(testSpec));
                        Assert.AreEqual(expectedValue.Mod, actualValue.Mod, GetMessage(testSpec));
                    }

                    CollectionAssert.AreEqual(expectedValue.FutureResolution, actualValue.FutureResolution, GetMessage(testSpec));
                    CollectionAssert.AreEqual(expectedValue.PastResolution, actualValue.PastResolution, GetMessage(testSpec));

                    if (expectedValue.TimeZoneResolution != null || actualValue.TimeZoneResolution != null)
                    {
                        Assert.IsNotNull(actualValue.TimeZoneResolution, GetMessage(testSpec));
                        Assert.IsNotNull(expectedValue.TimeZoneResolution, GetMessage(testSpec));
                        Assert.AreEqual(expectedValue.TimeZoneResolution.Value, actualValue.TimeZoneResolution.Value, GetMessage(testSpec));
                        Assert.AreEqual(expectedValue.TimeZoneResolution.UtcOffsetMins, actualValue.TimeZoneResolution.UtcOffsetMins, GetMessage(testSpec));
                    }
                }
            }
        }

        public void TestDateTimeMergedParser(TestModel testSpec)
        {
            TestPreValidation(testSpec);

            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input.ToLowerInvariant(), referenceDateTime);
            var actualResults = extractResults.Select(o => DateTimeParser.Parse(o, referenceDateTime)).ToArray();

            var expectedResults = testSpec.CastResults<DateTimeParseResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Length, GetMessage(testSpec));

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;
                var ignoreResultCase = true;

                Assert.AreEqual(expected.Type, actual.Type, GetMessage(testSpec));
                Assert.AreEqual(expected.Text, actual.Text, ignoreResultCase, GetMessage(testSpec));
                Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                Assert.AreEqual(expected.Length, actual.Length, GetMessage(testSpec));

                if (testSpec.IgnoreResolution)
                {
                    Assert.Inconclusive(GetMessage(testSpec) + ". Resolution not validated.");
                }
                else
                {
                    if (actual.Value is IDictionary<string, object> values)
                    {
                        // Actual ValueSet types should not be modified as that's considered a breaking API change
                        var actualValues = values[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>;

                        var expectedObj = JsonConvert.DeserializeObject<IDictionary<string, IList<Dictionary<string, string>>>>(expected.Value.ToString());
                        var expectedValues = expectedObj[ResolutionKey.ValueSet];

                        Assert.AreEqual(expectedValues.Count, actualValues?.Count, GetMessage(testSpec));

                        foreach (var (item1, item2) in expectedValues.Zip(actualValues, Tuple.Create))
                        {
                            Assert.AreEqual(item1.Count, item2.Count, GetMessage(testSpec));
                            CollectionAssert.AreEqual(item1.OrderBy(o => o.Key).ToImmutableDictionary(),
                                item2.OrderBy(o => o.Key).ToImmutableDictionary(), GetMessage(testSpec));
                        }
                    }
                }
            }
        }

        public void TestIpAddress(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new List<string> { ResolutionKey.Type });
        }

        public void TestPhoneNumber(TestModel testSpec)
        {
            var testResolutionKeys = new List<string> { "score" };
            TestPreValidation(testSpec);
            ValidateResults(testSpec, testResolutionKeys);
        }

        public void TestMention(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec);
        }

        public void TestHashtag(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec);
        }

        public void TestQuotedText(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec);
        }

        public void TestEmail(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec);
        }

        public void TestURL(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec);
        }

        public void TestGUID(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new string[] { ResolutionKey.Score });
        }

        public void TestChoice(TestModel testSpec)
        {
            TestPreValidation(testSpec);
            ValidateResults(testSpec, new string[] { ResolutionKey.Value, ResolutionKey.Score });
        }

        private static string GetMessage(TestModel spec)
        {
            return $"Input: \"{spec.Input}\"";
        }

        private void TestPreValidation(TestModel testSpec)
        {
            if (TestUtils.EvaluateSpec(testSpec, out string message))
            {
                Assert.Inconclusive(message);
            }

            if (Debugger.IsAttached && testSpec.Debug)
            {
                Debugger.Break();
            }
        }

        private void ValidateResults(TestModel testSpec, IEnumerable<string> testResolutionKeys = null)
        {

            var actualResults = TestContext.GetModelParseResults(testSpec);
            var expectedResults = testSpec.CastResults<ModelResult>();

            Assert.AreEqual(expectedResults.Count(), actualResults.Count, GetMessage(testSpec));

            foreach (var (expected, actual) in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {

                Assert.AreEqual(expected.TypeName, actual.TypeName, GetMessage(testSpec));
                Assert.AreEqual(expected.Text, actual.Text, GetMessage(testSpec));

                // Number and NumberWithUnit are supported currently.
                if (expected.Start != Constants.InvalidIndex)
                {
                    Assert.AreEqual(expected.Start, actual.Start, GetMessage(testSpec));
                }

                // Number and NumberWithUnit are supported currently.
                if (expected.End != Constants.InvalidIndex)
                {
                    Assert.AreEqual(expected.End, actual.End, GetMessage(testSpec));
                }

                if (testSpec.IgnoreResolution)
                {
                    Assert.Inconclusive(GetMessage(testSpec) + ". Resolution not validated.");
                }
                else
                {

                    if (expected.TypeName.Contains(Number.Constants.MODEL_ORDINAL))
                    {
                        if (!expected.TypeName.Equals(Number.Constants.MODEL_ORDINAL_RELATIVE))
                        {
                            Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value],
                                            GetMessage(testSpec));
                        }

                        Assert.AreEqual(expected.Resolution[ResolutionKey.Offset], actual.Resolution[ResolutionKey.Offset],
                                        GetMessage(testSpec));

                        Assert.AreEqual(expected.Resolution[ResolutionKey.RelativeTo], actual.Resolution[ResolutionKey.RelativeTo],
                                        GetMessage(testSpec));
                    }
                    else
                    {
                        Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value],
                                        GetMessage(testSpec));
                    }

                    foreach (var key in testResolutionKeys ?? Enumerable.Empty<string>())
                    {
                        if (!actual.Resolution.ContainsKey(key) && !expected.Resolution.ContainsKey(key))
                        {
                            continue;
                        }

                        Assert.AreEqual(expected.Resolution[key].ToString(), actual.Resolution[key].ToString(),
                                        GetMessage(testSpec));
                    }
                }
            }
        }
    }
}