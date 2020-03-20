using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Italian;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Number.Portuguese;
using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.Recognizers.Text.Number.Turkish;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestParserFactory
    {
        [TestMethod]
        public void TestEnglishParser()
        {
            var config = new EnglishNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.English, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parserPercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parserNumber is BaseNumberParser);
            Assert.IsTrue(parserCardinal is BaseNumberParser);
            Assert.IsTrue(parserPercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestSpanishParser()
        {
            var config = new SpanishNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Spanish, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parserPercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parserNumber is BaseNumberParser);
            Assert.IsTrue(parserCardinal is BaseNumberParser);
            Assert.IsTrue(parserPercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestPortugueseParser()
        {
            var config = new PortugueseNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Portuguese, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parserPercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parserNumber is BaseNumberParser);
            Assert.IsTrue(parserCardinal is BaseNumberParser);
            Assert.IsTrue(parserPercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestChineseParser()
        {
            var config = new ChineseNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Chinese, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parserPercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
            Assert.IsTrue(parserCardinal is BaseCJKNumberParser);
            Assert.IsTrue(parserPercentage is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestJapaneseParser()
        {
            var config = new JapaneseNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Japanese, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parserPercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
            Assert.IsTrue(parserCardinal is BaseCJKNumberParser);
            Assert.IsTrue(parserPercentage is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestKoreanParser()
        {
            var config = new KoreanNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Korean, NumberOptions.None));

            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestFrenchParser()
        {
            var config = new FrenchNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.French, NumberOptions.None));

            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestGermanParser()
        {
            var config = new GermanNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.German, NumberOptions.None));

            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestItalianParser()
        {
            var config = new ItalianNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Italian, NumberOptions.None));

            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestTurkishParser()
        {
            var config = new TurkishNumberParserConfiguration(new BaseNumberOptionsConfiguration(Culture.Turkish, NumberOptions.None));

            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, config);
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, config);
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, config);

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        private ExtractResult GetNumberToParse(string number, string data)
        {
            return new ExtractResult
            {
                Type = Constants.SYS_NUM,
                Data = data,
                Text = number,
            };
        }
    }
}
