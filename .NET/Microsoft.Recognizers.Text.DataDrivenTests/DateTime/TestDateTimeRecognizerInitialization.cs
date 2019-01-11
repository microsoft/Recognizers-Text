using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTimeRecognizerInitialization
    {
        private const string TestInput = "today and 18-7-8";

        private const string DefaultEnglishCulture = Culture.English;
        private const string SpanishCulture = Culture.Spanish;
        private const string InvalidCulture = "vo-id";
        private const string OtherEnglishCulture = "en-029";

        private readonly IModel defaultEnglishControlModel;
        private readonly IModel otherEnglishControlModel;

        public TestDateTimeRecognizerInitialization()
        {
            var config = new BaseOptionsConfiguration();
            var configEnableDmy = new BaseOptionsConfiguration(DateTimeOptions.None, true);

            defaultEnglishControlModel = new DateTimeModel(
                    new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(config)),
                    new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(config)));

            otherEnglishControlModel = new DateTimeModel(
                    new BaseMergedDateTimeParser(new EnglishMergedParserConfiguration(configEnableDmy)),
                    new BaseMergedDateTimeExtractor(new EnglishMergedExtractorConfiguration(configEnableDmy)));
        }

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(DefaultEnglishCulture);
            var testedModel = recognizer.GetDateTimeModel();

            TestDateTime(testedModel, defaultEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(SpanishCulture);
            var testedModel = recognizer.GetDateTimeModel(DefaultEnglishCulture);

            TestDateTime(testedModel, defaultEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(DefaultEnglishCulture);
            var testedModel = recognizer.GetDateTimeModel(InvalidCulture);

            TestDateTime(testedModel, defaultEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new DateTimeRecognizer();
            var testedModel = recognizer.GetDateTimeModel(InvalidCulture);

            TestDateTime(testedModel, defaultEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new DateTimeRecognizer();
            var testedModel = recognizer.GetDateTimeModel();

            TestDateTime(testedModel, defaultEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithUnrecordedEnglishCulture_FallbackToEnglishOthersCulture()
        {
            var recognizer = new DateTimeRecognizer(OtherEnglishCulture);
            var testedModel = recognizer.GetDateTimeModel(OtherEnglishCulture);

            TestDateTime(testedModel, otherEnglishControlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new DateTimeRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetDateTimeModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new DateTimeRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetDateTimeModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new DateTimeRecognizer(DefaultEnglishCulture, 5);
            Assert.IsTrue(recognizer.Options.HasFlag(DateTimeOptions.SkipFromToMerge | DateTimeOptions.CalendarMode));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DateTimeRecognizer(DefaultEnglishCulture, -1));
        }

        private void TestDateTime(IModel testedModel, IModel controlModel, string source)
        {
            var expectedResults = controlModel.Parse(source);
            var actualResults = testedModel.Parse(source);

            Assert.AreEqual(expectedResults.Count, actualResults.Count, source);
            Assert.IsTrue(expectedResults.Count > 0, source);

            foreach (var tuple in expectedResults.Zip(actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, source);
                Assert.AreEqual(expected.Text, actual.Text, source);
                if (expected.Start != 0)
                {
                    Assert.AreEqual(expected.Start, actual.Start, source);
                }

                if (expected.End != 0)
                {
                    Assert.AreEqual(expected.End, actual.End, source);
                }

                // Actual ValueSet types should not be modified as that's considered a breaking API change
                var listValues = actual.Resolution[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>;
                var actualValues = listValues.FirstOrDefault();

                var expectedObj = expected.Resolution[ResolutionKey.ValueSet] as IList<Dictionary<string, string>>;
                var expectedValues = expectedObj.FirstOrDefault();

                CollectionAssert.AreEqual(expectedValues, actualValues, source);
            }
        }
    }
}
