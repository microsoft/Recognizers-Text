using Microsoft.Recognizers.Text.Choice;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Choice
{
    [TestClass]
    public class TestChoiceRecognizer_Japanese
    {
        [TestMethod]
        public void RecognizeBoolean()
        {
            var input = "この場所ですが？いいえ";

            var actual = ChoiceRecognizer.RecognizeBoolean(input, Culture.Japanese);

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("boolean", actual[0].TypeName);
            Assert.AreEqual("いいえ", actual[0].Text);

            Assert.AreEqual(false, actual[0].Resolution["value"]);
            Assert.AreEqual(0.5, actual[0].Resolution["score"]);
        }
    }
}
