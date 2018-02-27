using Microsoft.Recognizers.Text.Sequence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Sequence
{
    [TestClass]
    public class TestSequenceRecognizer_English
    {
        [TestMethod]
        public void RecognizePhoneNumber()
        {
            var input = "My phone number is 1 (877) 609-2233.";

            var actual = SequenceRecognizer.RecognizePhoneNumber(input, Culture.English);

            Assert.AreEqual("phonenumber", actual[0].TypeName);
            Assert.AreEqual("1 (877) 609-2233", actual[0].Text);
            Assert.AreEqual("1 (877) 609-2233", actual[0].Resolution["value"]);
        }
    }
}
