using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitRecognizerInitialization
    {
        private readonly string DefaultCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseDefaultCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetCurrencyModel(), recognizer.GetCurrencyModel(DefaultCulture));
        }

        [TestMethod]
        public void WithSpanishCulture_NotUseDefaultCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(DefaultCulture);
            Assert.AreNotEqual(recognizer.GetCurrencyModel(SpanishCulture), recognizer.GetCurrencyModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseDefaultCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetCurrencyModel(InvalidCulture), recognizer.GetCurrencyModel());
        }

        [TestMethod]
        public void InitilaizationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new NumberWithUnitRecognizer(DefaultCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(NumberWithUnitOptions.None));
        }

        [TestMethod]
        public void InitilaizationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NumberWithUnitRecognizer(DefaultCulture, -1));
        }
    }
}
