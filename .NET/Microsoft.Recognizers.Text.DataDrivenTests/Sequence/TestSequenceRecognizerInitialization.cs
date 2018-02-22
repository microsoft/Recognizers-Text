using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequenceRecognizerInitialization
    {
        private readonly string DefaultCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseDefaultCulture()
        {
            var recognizer = new SequenceRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(DefaultCulture));
        }

        [Ignore]
        [TestMethod]
        public void WithSpanishCulture_NotUseDefaultCulture()
        {
            var recognizer = new SequenceRecognizer(DefaultCulture);
            Assert.AreNotEqual(recognizer.GetPhoneNumberModel(SpanishCulture), recognizer.GetPhoneNumberModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseDefaultCulture()
        {
            var recognizer = new SequenceRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(InvalidCulture), recognizer.GetPhoneNumberModel());
        }

        [TestMethod]
        public void InitilaizationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new SequenceRecognizer(DefaultCulture, 0);
            Assert.IsTrue(recognizer.Options.HasFlag(SequenceOptions.None));
        }

        [TestMethod]
        public void InitilaizationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new SequenceRecognizer(DefaultCulture, -1));
        }
    }
}
