using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitRecognizerInitialization
    {
        private const string TestInput = "two dollars";

        private const string EnglishCulture = Culture.English;
        private const string SpanishCulture = Culture.Spanish;
        private const string InvalidCulture = "vo-id";

        private readonly IModel controlModel;

        public TestNumberWithUnitRecognizerInitialization()
        {
            controlModel = new CurrencyModel(new Dictionary<IExtractor, IParser>
            {
                {
                    new NumberWithUnitExtractor(new English.CurrencyExtractorConfiguration()),
                    new NumberWithUnitParser(new English.CurrencyParserConfiguration())
                },
            });
        }

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            var testedModel = recognizer.GetCurrencyModel();

            TestNumberWithUnit(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(SpanishCulture);
            var testedModel = recognizer.GetCurrencyModel(EnglishCulture);

            TestNumberWithUnit(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            var testedModel = recognizer.GetCurrencyModel(InvalidCulture);

            TestNumberWithUnit(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new NumberWithUnitRecognizer();
            var testedModel = recognizer.GetCurrencyModel(InvalidCulture);

            TestNumberWithUnit(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new NumberWithUnitRecognizer();
            var testedModel = recognizer.GetCurrencyModel();

            TestNumberWithUnit(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new NumberWithUnitRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetCurrencyModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new NumberWithUnitRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetCurrencyModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(NumberWithUnitOptions.None));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NumberWithUnitRecognizer(EnglishCulture, -1));
        }

        private void TestNumberWithUnit(IModel testedModel, IModel controlModel, string source)
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
                Assert.AreEqual(expected.Resolution[ResolutionKey.Unit], actual.Resolution[ResolutionKey.Unit], source);
            }
        }
    }
}
