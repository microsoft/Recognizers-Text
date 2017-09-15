using Microsoft.Recognizers.Text.DateTime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DataDrivenTests.DateTime
{
    [TestClass]
    public class TestDateTime_Eng : TestBase
    {
        public static TestResources TestResources { get; private set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestResources = new TestResources();
            TestResources.InitFromTestContext(context);
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateExtractor-Eng.csv", "BaseDateExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimeExtractor-Eng.csv", "BaseTimeExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodExtractor-Eng.csv", "BaseDatePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodExtractor-Eng.csv", "BaseTimePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeExtractor-Eng.csv", "BaseDateTimeExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodExtractor-Eng.csv", "BaseDateTimePeriodExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayExtractor-Eng.csv", "BaseHolidayExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationExtractor-Eng.csv", "BaseDurationExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetExtractor-Eng.csv", "BaseSetExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractor-Eng.csv", "BaseMergedExtractor-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count, testSpec.Input);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedExtractorSkipFromTo-Eng.csv", "BaseMergedExtractorSkipFromTo-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void BaseMergedExtractorSkipFromTo()
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
            var extractor = TestContext.GetExtractor();

            var results = extractor.Extract(testSpec.Input);
            Assert.AreEqual(testSpec.Results.Count(), results.Count, testSpec.Input);
            if (testSpec.Results.Count() > 0)
            {
                var expected = testSpec.CastResults<ExtractResult>().FirstOrDefault();
                Assert.AreEqual(expected.Type, results.First().Type);
                Assert.AreEqual(expected.Start, results.First().Start);
                Assert.AreEqual(expected.Length, results.First().Length);
            }
        }
        
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateParser-Eng.csv", "BaseDateParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "TimeParser-Eng.csv", "TimeParser-Eng#csv", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TimeParser()
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDatePeriodParser-Eng.csv", "BaseDatePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseTimePeriodParser-Eng.csv", "BaseTimePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimeParser-Eng.csv", "BaseDateTimeParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDateTimePeriodParser-Eng.csv", "BaseDateTimePeriodParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseHolidayParser-Eng.csv", "BaseHolidayParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseDurationParser-Eng.csv", "BaseDurationParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseSetParser-Eng.csv", "BaseSetParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "BaseMergedParser-Eng.csv", "BaseMergedParser-Eng#csv", DataAccessMethod.Sequential)]
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
            var extractor = TestContext.GetExtractor();
            var parser = TestContext.GetDateTimeParser();
            var referenceDateTime = testSpec.GetReferenceDateTime();

            var extractResults = extractor.Extract(testSpec.Input);
            var result = parser.Parse(extractResults.FirstOrDefault(), referenceDateTime);

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
