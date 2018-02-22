using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTimeRecognizerInitialization
    {
        private readonly string DefaultCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string InvalidCulture = "vo-id";

        [TestMethod]
        public void WithoutCulture_UseDefaultCulture()
        {
            var recognizer = new DateTimeRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(), recognizer.GetDateTimeModel(DefaultCulture));
        }

        [TestMethod]
        public void WithSpanishCulture_NotUseDefaultCulture()
        {
            var recognizer = new DateTimeRecognizer(DefaultCulture);
            Assert.AreNotEqual(recognizer.GetDateTimeModel(SpanishCulture), recognizer.GetDateTimeModel());
        }

        [TestMethod]
        public void WithInvalidCulture_UseDefaultCulture()
        {
            var recognizer = new DateTimeRecognizer(DefaultCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(InvalidCulture), recognizer.GetDateTimeModel());
        }

        [TestMethod]
        public void InitializationNonLazy_CanGetModel()
        {
            var recognizer = new DateTimeRecognizer(DefaultCulture, DateTimeOptions.None, lazyInitialization: false);
            Assert.AreEqual(recognizer.GetDateTimeModel(), recognizer.GetDateTimeModel(DefaultCulture));
        }

        [TestMethod]
        public void InitilaizationWithIntOption_ResolveOptionsEnum()
        {
            var recognizer = new DateTimeRecognizer(DefaultCulture, 5);
            Assert.IsTrue(recognizer.Options.HasFlag(DateTimeOptions.SkipFromToMerge | DateTimeOptions.CalendarMode));
        }

        [TestMethod]
        public void InitilaizationWithInvalidOptions_ThrowError()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new DateTimeRecognizer(DefaultCulture, -1));
        }
    }
}
