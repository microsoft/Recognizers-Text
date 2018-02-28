using System;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoiceRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        //private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new ChoiceRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetBooleanModel(), recognizer.GetBooleanModel(EnglishCulture));
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
            Assert.AreEqual(recognizer.GetBooleanModel(InvalidCulture), recognizer.GetBooleanModel());
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
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new ChoiceRecognizer();
            Assert.AreEqual(recognizer.GetBooleanModel(), recognizer.GetBooleanModel(EnglishCulture));
        }

        [TestMethod]
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new ChoiceRecognizer(EnglishCulture, ChoiceOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetBooleanModel(), recognizer.GetBooleanModel(EnglishCulture));
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
    }
}
