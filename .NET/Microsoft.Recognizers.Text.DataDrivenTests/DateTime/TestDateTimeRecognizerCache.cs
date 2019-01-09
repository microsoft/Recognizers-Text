using System.Linq;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DateTime.Tests
{
    [TestClass]
    public class TestDateTimeRecognizerCache
    {
        [TestInitialize]
        public void Initialization()
        {
            var recognizer = new DateTimeRecognizer();
            recognizer.GetInternalCache<DateTimeOptions>().Clear();
        }

        [TestMethod]
        public void WithLazyInitialization_CacheEmpty()
        {
            var recognizer = new DateTimeRecognizer(lazyInitialization: true);
            var internalCache = recognizer.GetInternalCache<DateTimeOptions>();
            Assert.AreEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitialization_CacheFull()
        {
            var recognizer = new DateTimeRecognizer(lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.AreNotEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitializationAndCulture_CacheWithCulture()
        {
            var recognizer = new DateTimeRecognizer(Culture.English, lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.IsTrue(internalCache.All(kv => kv.Key.culture == Culture.English));
        }
    }
}
