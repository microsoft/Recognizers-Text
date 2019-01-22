using System.Linq;
using Microsoft.Recognizers.Text.DataDrivenTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestNumberRecognizerCache
    {
        [TestInitialize]
        public void Initialization()
        {
            var recognizer = new NumberRecognizer();
            recognizer.GetInternalCache().Clear();
        }

        [TestMethod]
        public void WithLazyInitialization_CacheEmpty()
        {
            var recognizer = new NumberRecognizer(lazyInitialization: true);
            var internalCache = recognizer.GetInternalCache();
            Assert.AreEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitialization_CacheFull()
        {
            var recognizer = new NumberRecognizer(lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.AreNotEqual(0, internalCache.Count);
        }

        [TestMethod]
        public void WithoutLazyInitializationAndCulture_CacheWithCulture()
        {
            var recognizer = new NumberRecognizer(Culture.English, lazyInitialization: false);
            var internalCache = recognizer.GetInternalCache();
            Assert.IsTrue(internalCache.All(kv => kv.Key.culture == Culture.English));
        }
    }
}
