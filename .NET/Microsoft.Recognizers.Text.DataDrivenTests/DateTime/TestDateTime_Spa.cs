using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Recognizers.Text.DataDrivenTests.DateTime
{
    [TestClass]
    public class TestDateTime_Spa : TestBase
    {
        public static TestResources TestResources { get; private set; }
        public static IDictionary<string, IExtractor> Extractors { get; private set; }
        public static IDictionary<string, IDateTimeParser> Parsers { get; private set; }

        public IExtractor Extractor { get; set; }
        public IDateTimeParser Parser { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
            Extractors = new Dictionary<string, IExtractor>();
            Parsers = new Dictionary<string, IDateTimeParser>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var key = TestContext.TestName;
            IExtractor extractor;
            IDateTimeParser parser;
            if (!Extractors.TryGetValue(key, out extractor))
            {
                extractor = TestContext.GetExtractor();
                Extractors.Add(key, extractor);
            }
            Extractor = extractor;

            if (!Parsers.TryGetValue(key, out parser))
            {
                parser = TestContext.GetDateTimeParser();
                Parsers.Add(key, parser);
            }
            Parser = parser;
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateExtractor-Spa.csv", "BaseDateExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeExtractor-Spa.csv", "BaseTimeExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimeExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodExtractor-Spa.csv", "BaseDatePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodExtractor-Spa.csv", "BaseTimePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeExtractor-Spa.csv", "BaseDateTimeExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodExtractor-Spa.csv", "BaseDateTimePeriodExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayExtractor-Spa.csv", "BaseHolidayExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationExtractor-Spa.csv", "BaseDurationExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetExtractor-Spa.csv", "BaseSetExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractor-Spa.csv", "BaseMergedExtractor-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedExtractor()
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
            var results = Extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count, testSpec.Input);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateParser-Spa.csv", "BaseDateParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeParser-Spa.csv", "BaseTimeParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimeParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodParser-Spa.csv", "BaseDatePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDatePeriodParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodParser-Spa.csv", "BaseTimePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseTimePeriodParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeParser-Spa.csv", "BaseDateTimeParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimeParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodParser-Spa.csv", "BaseDateTimePeriodParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDateTimePeriodParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayParser-Spa.csv", "BaseHolidayParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseHolidayParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationParser-Spa.csv", "BaseDurationParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseDurationParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetParser-Spa.csv", "BaseSetParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseSetParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedParser-Spa.csv", "BaseMergedParser-Spa#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedParser()
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
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = Extractor.Extract(testSpec.Input);
            var result = Parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

            var expected = testSpec.CastResults<DateTimeParseResult>().FirstOrDefault();

            Assert.AreEqual(expected.Type, result.Type, testSpec.Input);
            Assert.AreEqual(expected.Start, result.Start);
            Assert.AreEqual(expected.Length, result.Length);

            if (testSpec.Results.Count() > 0)
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
