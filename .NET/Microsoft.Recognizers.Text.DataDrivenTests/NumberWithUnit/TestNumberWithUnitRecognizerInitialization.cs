using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetCurrencyModel(), recognizer.GetCurrencyModel(EnglishCulture));
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            Assert.AreNotEqual(recognizer.GetCurrencyModel(SpanishCulture), recognizer.GetCurrencyModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetCurrencyModel(InvalidCulture), recognizer.GetCurrencyModel());
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
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new NumberWithUnitRecognizer(EnglishCulture, NumberWithUnitOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetCurrencyModel(), recognizer.GetCurrencyModel(EnglishCulture));
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
    }
}
