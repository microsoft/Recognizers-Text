using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequenceRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            // This test doesn't apply. Kept as documentation of purpose. Not marked as 'Ignore' to avoid permanent warning due to design.
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(InvalidCulture), recognizer.GetPhoneNumberModel());
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new SequenceRecognizer();
            Assert.AreEqual(recognizer.GetPhoneNumberModel(InvalidCulture), recognizer.GetPhoneNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void WithInvalidCultureAsTarget_AlwaysUseEnglish()
        {
            var recognizer = new SequenceRecognizer(InvalidCulture);
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(EnglishCulture));
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new SequenceRecognizer();
            Assert.AreEqual(recognizer.GetPhoneNumberModel(), recognizer.GetPhoneNumberModel(EnglishCulture));
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
