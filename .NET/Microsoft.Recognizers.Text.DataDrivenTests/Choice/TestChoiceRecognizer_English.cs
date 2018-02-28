using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Choice;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoiceRecognizer_English
    {
        [TestMethod]
        public void RecognizeBoolean()
        {
            var input = "I don't thing so. no.";

            var actual = ChoiceRecognizer.RecognizeBoolean(input, Culture.English);

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("boolean", actual[0].TypeName);
            Assert.AreEqual("no", actual[0].Text);

            Assert.AreEqual(false, actual[0].Resolution["value"]);
            Assert.AreEqual(0.5, actual[0].Resolution["score"]);
        }
    }
}
