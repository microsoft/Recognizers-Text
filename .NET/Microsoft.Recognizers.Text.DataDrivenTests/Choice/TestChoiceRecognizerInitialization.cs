using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Choice.Tests
{
    [TestClass]
    public class TestChoiceRecognizerInitialization
    {
        private const string TestInput = "true";

        private const string EnglishCulture = Culture.English;

        // private const string SpanishCulture = Culture.Spanish;
        private const string InvalidCulture = "vo-id";

        private IModel controlModel;

        public TestChoiceRecognizerInitialization()
        {
            controlModel = new BooleanModel(
                new BooleanParser(),
                new BooleanExtractor(new English.EnglishBooleanExtractorConfiguration()));
        }

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            var testedModel = recognizer.GetBooleanModel();

            TestChoice(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            // This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning due to design.
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            var testedModel = recognizer.GetBooleanModel(InvalidCulture);

            TestChoice(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new ChoiceRecognizer();
            var testedModel = recognizer.GetBooleanModel(InvalidCulture);

            TestChoice(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new ChoiceRecognizer();
            var testedModel = recognizer.GetBooleanModel();

            TestChoice(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new ChoiceRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetBooleanModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new ChoiceRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetBooleanModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new ChoiceRecognizer(EnglishCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(ChoiceOptions.None));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ChoiceRecognizer(EnglishCulture, -1));
        }

        private void TestChoice(IModel testedModel, IModel controlModel, string source)
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
                Assert.AreEqual(expected.Resolution[ResolutionKey.Score], actual.Resolution[ResolutionKey.Score], source);
            }
        }
    }
}
