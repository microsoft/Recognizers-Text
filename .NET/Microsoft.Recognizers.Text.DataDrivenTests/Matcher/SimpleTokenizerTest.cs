// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Recognizers.Text.Matcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.DataDrivenTests.Matcher
{
    [TestClass]
    public class SimpleTokenizerTest
    {
        private readonly ITokenizer tokenizer = new SimpleTokenizer();

        [TestMethod]
        public void EnglishTokenizedTest()
        {
            var text = "   Hi, could     you give me a beer, please?";
            var tokenizedText = tokenizer.Tokenize(text);

            Assert.AreEqual("Hi", tokenizedText[0].Text);
            Assert.AreEqual(11, tokenizedText.Count);
        }

        [TestMethod]
        public void ChineseTokenizedTest()
        {
            var text = "你好，请给我一杯啤酒！";
            var tokenizedText = tokenizer.Tokenize(text);

            Assert.AreEqual("你", tokenizedText[0].Text);
            Assert.AreEqual(11, tokenizedText.Count);
        }

        [TestMethod]
        public void MixedTokenizedTest()
        {
            var text = "Hello，请给我1杯beer谢谢！";
            var tokenizedText = tokenizer.Tokenize(text);

            Assert.AreEqual("Hello", tokenizedText[0].Text);
            Assert.AreEqual(11, tokenizedText.Count);
        }
    }
}
