using Microsoft.Recognizers.Text.Number.Chinese;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Text.Number.French;
using Microsoft.Recognizers.Text.Number.German;
using Microsoft.Recognizers.Text.Number.Italian;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Number.Spanish;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestParserFactory
    {
        [TestMethod]
        public void TestEnglishParser()
        {
            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new EnglishNumberParserConfiguration());
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new EnglishNumberParserConfiguration());
            IParser parserPercentaje = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new EnglishNumberParserConfiguration());

            Assert.IsTrue(parserNumber is BaseNumberParser);
            Assert.IsTrue(parserCardinal is BaseNumberParser);
            Assert.IsTrue(parserPercentaje is BasePercentageParser);
        }

        [TestMethod]
        public void TestSpanishParser()
        {
            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new SpanishNumberParserConfiguration());
            IParser parserPercentaje = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new SpanishNumberParserConfiguration());

            Assert.IsTrue(parserNumber is BaseNumberParser);
            Assert.IsTrue(parserCardinal is BaseNumberParser);
            Assert.IsTrue(parserPercentaje is BasePercentageParser);
        }

        [TestMethod]
        public void TestChineseParser()
        {
            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ChineseNumberParserConfiguration());
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new ChineseNumberParserConfiguration());
            IParser parserPercentaje = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ChineseNumberParserConfiguration());

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
            Assert.IsTrue(parserCardinal is BaseCJKNumberParser);
            Assert.IsTrue(parserPercentaje is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestJapaneseParser()
        {
            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new JapaneseNumberParserConfiguration());
            IParser parserCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new JapaneseNumberParserConfiguration());
            IParser parserPercentaje = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new JapaneseNumberParserConfiguration());

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
            Assert.IsTrue(parserCardinal is BaseCJKNumberParser);
            Assert.IsTrue(parserPercentaje is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestKoreanParser()
        {
            IParser parserNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new KoreanNumberParserConfiguration());

            Assert.IsTrue(parserNumber is BaseCJKNumberParser);
        }

        [TestMethod]
        public void TestFrenchParser()
        {
            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new FrenchNumberParserConfiguration());
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new FrenchNumberParserConfiguration());
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new FrenchNumberParserConfiguration());

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestGermanParser()
        {
            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new GermanNumberParserConfiguration());
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new GermanNumberParserConfiguration());
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new GermanNumberParserConfiguration());

            Assert.IsTrue(parseNumber is BaseNumberParser);
            Assert.IsTrue(parseCardinal is BaseNumberParser);
            Assert.IsTrue(parsePercentage is BasePercentageParser);
        }

        [TestMethod]
        public void TestItalianParser()
        {
            IParser parseNumber = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new ItalianNumberParserConfiguration());
            IParser parseCardinal = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Cardinal, new ItalianNumberParserConfiguration());
            IParser parsePercentage = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Percentage, new ItalianNumberParserConfiguration());

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
