using System.Linq;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Tests
{
    [TestClass]
    public class TestNumberWithUnitRecognizerCache
    {
        [TestInitialize]
        public void Initialization()
        {
            var recognizer = new NumberWithUnitRecognizer();
            recognizer.GetInternalCache().Clear();
        }

        [TestMethod]
        public void WithLazyInitialization_CacheEmpty()
        {
            var recognizer = new NumberWithUnitRecognizer(lazyInitialization: true);
            var internalCache = recognizer.GetInternalCache();
            Assert.AreEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitialization_CacheFull()
        {
            var recognizer = new NumberWithUnitRecognizer(lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.AreNotEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitializationAndCulture_CacheWithCulture()
        {
            var recognizer = new NumberWithUnitRecognizer(Culture.English, lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.IsTrue(internalCache.All(kv => kv.Key.culture == Culture.English));
        }
    }
}
