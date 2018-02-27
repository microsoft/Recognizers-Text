using Microsoft.Recognizers.Text.Number;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Tests
{
    [TestClass]
    public class TestNumberRecognizer_English
    {
        [TestMethod]
        public void RecognizeNumber()
        {
            var actual = NumberRecognizer.RecognizeNumber("192", Culture.English);

            Assert.AreEqual("number", actual[0].TypeName);
            Assert.AreEqual("192", actual[0].Text);
            Assert.AreEqual("192", actual[0].Resolution["value"]);
        }

        [TestMethod]
        public void RecognizeOrdinal()
        {
            var input = "a hundred trillionth";

            var actual = NumberRecognizer.RecognizeOrdinal(input, Culture.English);

            Assert.AreEqual("ordinal", actual[0].TypeName);
            Assert.AreEqual("a hundred trillionth", actual[0].Text);
            Assert.AreEqual("100000000000000", actual[0].Resolution["value"]);
        }

        [TestMethod]
        public void RecognizePercentage()
        {
            var input = "100%";

            var actual = NumberRecognizer.RecognizePercentage(input, Culture.English);

            Assert.AreEqual("percentage", actual[0].TypeName);
            Assert.AreEqual("100%", actual[0].Text);
            Assert.AreEqual("100%", actual[0].Resolution["value"]);
        }

        [TestMethod]
        public void RecognizeNumberRange()
        {
            var input = "This number is larger than twenty and less or equal than thirty five.";

            var actual = NumberRecognizer.RecognizeNumberRange(input, Culture.English);

            Assert.AreEqual("numberrange", actual[0].TypeName);
            Assert.AreEqual("larger than twenty and less or equal than thirty five", actual[0].Text);
            Assert.AreEqual("(20,35]", actual[0].Resolution["value"]);
        }
    }
}
