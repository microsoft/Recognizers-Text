using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTimeRecognizerInitialization
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(), recognizer.GetDateTimeModel(EnglishCulture));
        }

        [TestMethod]
        public void WithOtherCulture_NotUseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreNotEqual(recognizer.GetDateTimeModel(SpanishCulture), recognizer.GetDateTimeModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseTargetCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(InvalidCulture), recognizer.GetDateTimeModel());
        }

        [TestMethod]
        public void WithInvalidCultureAndWithoutFallback_ThrowError()
        {
            var recognizer = new DateTimeRecognizer();
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetDateTimeModel(InvalidCulture, fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void WithInvalidCultureAsTargetAndWithoutFallback_ThrowError()
        {
            var recognizer = new DateTimeRecognizer(InvalidCulture);
            Assert.ThrowsException<ArgumentException>(() => recognizer.GetDateTimeModel(fallbackToDefaultCulture: false));
        }

        [TestMethod]
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture, DateTimeOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetDateTimeModel(), recognizer.GetDateTimeModel(EnglishCulture));
        }

        [TestMethod]
        public void InitializationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture, 5);
            Assert.IsTrue(recognizer.Options.HasFlag(DateTimeOptions.SkipFromToMerge | DateTimeOptions.CalendarMode));
        }

        [TestMethod]
        public void InitializationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DateTimeRecognizer(EnglishCulture, -1));
        }
    }
}
