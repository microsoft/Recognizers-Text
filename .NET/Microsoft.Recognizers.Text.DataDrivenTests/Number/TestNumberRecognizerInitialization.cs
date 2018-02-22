using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberRecognizerInitialization
    {
        private readonly string DefaultCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseDefaultCulture()
        {
            var recognizer = new NumberRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetNumberModel(), recognizer.GetNumberModel(DefaultCulture));
        }

        [TestMethod]
        public void WithSpanishCulture_NotUseDefaultCulture()
        {
            var recognizer = new NumberRecognizer(DefaultCulture);
            Assert.AreNotEqual(recognizer.GetNumberModel(SpanishCulture), recognizer.GetNumberModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseDefaultCulture()
        {
            var recognizer = new NumberRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetNumberModel(InvalidCulture), recognizer.GetNumberModel());
        }

        [TestMethod]
        public void InitilaizationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new NumberRecognizer(DefaultCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(NumberOptions.None));
        }

        [TestMethod]
        public void InitilaizationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new NumberRecognizer(DefaultCulture, -1));
        }
    }
}
