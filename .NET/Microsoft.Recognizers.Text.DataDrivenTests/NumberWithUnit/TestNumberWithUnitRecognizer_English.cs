using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Tests
{
    [TestClass]
    public class TestNumberWithUnitRecognizer_English
    {
        [TestMethod]
        public void RecognizeCurrency()
        {
            var input = "montgomery county , md . - - $ 75 million of general obligation , series b , consolidated public improvement bonds of 1989 , through a manufacturers hanover trust co . group .";
            var actual = NumberWithUnitRecognizer.RecognizeCurrency(input, Culture.English);

            Assert.AreEqual("currency", actual[0].TypeName);
            Assert.AreEqual("$ 75 million", actual[0].Text);
            Assert.AreEqual("75000000", actual[0].Resolution["value"]);
            Assert.AreEqual("Dollar", actual[0].Resolution["unit"]);
        }

        [TestMethod]
        public void RecognizeTemperature()
        {
            var input = "the temperature outside is 40 deg celsius";

            var actual = NumberWithUnitRecognizer.RecognizeTemperature(input, Culture.English);

            Assert.AreEqual("temperature", actual[0].TypeName);
            Assert.AreEqual("40 deg celsius", actual[0].Text);
            Assert.AreEqual("40", actual[0].Resolution["value"]);
            Assert.AreEqual("C", actual[0].Resolution["unit"]);
        }

        [TestMethod]
        public void RecognizeDimension()
        {
            var input = "75ml";

            var actual = NumberWithUnitRecognizer.RecognizeDimension(input, Culture.English);

            Assert.AreEqual("dimension", actual[0].TypeName);
            Assert.AreEqual("75ml", actual[0].Text);
            Assert.AreEqual("75", actual[0].Resolution["value"]);
            Assert.AreEqual("Milliliter", actual[0].Resolution["unit"]);
        }

        [TestMethod]
        public void RecognizeAge()
        {
            var input = "When she was five years old, she learned to ride a bike.";

            var actual = NumberWithUnitRecognizer.RecognizeAge(input, Culture.English);

            Assert.AreEqual("age", actual[0].TypeName);
            Assert.AreEqual("five years old", actual[0].Text);
            Assert.AreEqual("5", actual[0].Resolution["value"]);
            Assert.AreEqual("Year", actual[0].Resolution["unit"]);
        }
    }
}
