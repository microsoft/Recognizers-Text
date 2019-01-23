using System;
using System.Linq;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberRecognizerInitialization
    {
        private const string TestInput = "one";

        private const string EnglishCulture = Culture.English;
        private const string SpanishCulture = Culture.Spanish;
        private const string InvalidCulture = "vo-id";

        private readonly IModel controlModel;

        public TestNumberRecognizerInitialization()
        {
            controlModel = new NumberModel(
                    AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration()),
                    NumberExtractor.GetInstance(NumberMode.PureNumber));
        }

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new NumberRecognizer(EnglishCulture);
            var testedModel = recognizer.GetNumberModel();

            TestNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new NumberRecognizer(SpanishCulture);
            var testedModel = recognizer.GetNumberModel(EnglishCulture);

            TestNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new NumberRecognizer(EnglishCulture);
            var testedModel = recognizer.GetNumberModel(InvalidCulture);

            TestNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new NumberRecognizer();
            var testedModel = recognizer.GetNumberModel(InvalidCulture);

            TestNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new NumberRecognizer();
            var testedModel = recognizer.GetNumberModel();

            TestNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new NumberRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetNumberModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new NumberRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetNumberModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new NumberRecognizer(EnglishCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(NumberOptions.None));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NumberRecognizer(EnglishCulture, -1));
        }

        private void TestNumber(IModel testedModel, IModel controlModel, string source)
        {
            var expectedResults = controlModel.Parse(source);
            var actualResults = testedModel.Parse(source);

            Assert.AreEqual(expectedResults.Count, actualResults.Count, source);
            Assert.IsTrue(expectedResults.Count > 0, source);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, source);
                Assert.AreEqual(expected.Text, actual.Text, source);

                Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value], source);
            }
        }
    }
}
