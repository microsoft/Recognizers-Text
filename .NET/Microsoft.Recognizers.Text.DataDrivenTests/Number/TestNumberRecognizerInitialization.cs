using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new NumberRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetNumberModel(), recognizer.GetNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new NumberRecognizer(EnglishCulture);
            Assert.AreNotEqual(recognizer.GetNumberModel(SpanishCulture), recognizer.GetNumberModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new NumberRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetNumberModel(InvalidCulture), recognizer.GetNumberModel());
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
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new NumberRecognizer();
            Assert.AreEqual(recognizer.GetNumberModel(), recognizer.GetNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new NumberRecognizer(EnglishCulture, NumberOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetNumberModel(), recognizer.GetNumberModel(EnglishCulture));
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
    }
}
