using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequenceRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(EnglishCulture));
        }

        [Ignore]
        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            Assert.AreNotEqual(recognizer.GetPhoneNumberModel(SpanishCulture), recognizer.GetPhoneNumberModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(InvalidCulture), recognizer.GetPhoneNumberModel());
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new SequenceRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetPhoneNumberModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new SequenceRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetPhoneNumberModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture, SequenceOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(SequenceOptions.None));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SequenceRecognizer(EnglishCulture, -1));
        }
    }
}
