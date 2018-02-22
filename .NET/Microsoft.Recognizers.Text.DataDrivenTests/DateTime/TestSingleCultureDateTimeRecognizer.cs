using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestSingleCultureDateTimeRecognizer
    {
        private readonly string EnglishCulture = Culture.English;
        private readonly string SpanishCulture = Culture.Spanish;
        private readonly string ChineseCulture = Culture.Chinese;
        private readonly string InvalidCulture = "no-va";

        [TestMethod]
        public void GetModelWithoutCultureShouldUseDefaultCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(), recognizer.GetDateTimeModel(EnglishCulture));
        }

        [TestMethod]
        public void GetModelWithCultureShoudUseCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreNotEqual(recognizer.GetDateTimeModel(SpanishCulture), recognizer.GetDateTimeModel());
        }

        [TestMethod]
        public void GetModelWithInvalidCultureShouldUseDefaultCulture()
        {
            var recognizer = new DateTimeRecognizer(EnglishCulture);
            Assert.AreEqual(recognizer.GetDateTimeModel(InvalidCulture), recognizer.GetDateTimeModel());
        }
    }
}
