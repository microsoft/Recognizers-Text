using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Sequence.Tests
{
    [TestClass]
    public class TestSequenceRecognizerInitialization
    {
        private const string TestInput = "1 (877) 609-2233";

        private const string EnglishCulture = Culture.English;
        private const string InvalidCulture = "vo-id";

        private readonly IModel controlModel;

        public TestSequenceRecognizerInitialization()
        {
            controlModel = new PhoneNumberModel(
                new English.PhoneNumberParser(),
                new English.PhoneNumberExtractor());
        }

        [TestMethod]
        public void WithoutCulture_UseTargetCulture()
        {
            var recognizer = new SequenceRecognizer(EnglishCulture);
            var testedModel = recognizer.GetPhoneNumberModel();

            TestPhoneNumber(testedModel, controlModel, TestInput);
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
            var testedModel = recognizer.GetPhoneNumberModel(InvalidCulture);

            TestPhoneNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCulture_AlwaysUseEnglish()
        {
            var recognizer = new SequenceRecognizer();
            var testedModel = recognizer.GetPhoneNumberModel(InvalidCulture);

            TestPhoneNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithoutTargetCultureAndWithoutCulture_FallbackToEnglishCulture()
        {
            var recognizer = new SequenceRecognizer();
            var testedModel = recognizer.GetPhoneNumberModel();

            TestPhoneNumber(testedModel, controlModel, TestInput);
        }

        [TestMethod]
        public void WithInvalidCultureAsTarget_AlwaysUseEnglish()
        {
            var recognizer = new SequenceRecognizer(InvalidCulture);
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

        private void TestPhoneNumber(IModel testedModel, IModel controlModel, string source)
        {
            var expectedResults = controlModel.Parse(source);
            var actualResults = testedModel.Parse(source);

            Assert.AreEqual(expectedResults.Count, actualResults.Count, source);
            Assert.IsTrue(expectedResults.Count > 0, source);

            foreach (var tuple in Enumerable.Zip(expectedResults, actualResults, Tuple.Create))
            {
                var expected = tuple.Item1;
                var actual = tuple.Item2;

                Assert.AreEqual(expected.TypeName, actual.TypeName, source);
                Assert.AreEqual(expected.Text, actual.Text, source);

                Assert.AreEqual(expected.Resolution[ResolutionKey.Value], actual.Resolution[ResolutionKey.Value], source);
            }
        }
    }
}
