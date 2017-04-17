using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Recognizers.Text.Number.Extractors;
using Microsoft.Recognizers.Text.Number.Parsers;
using Microsoft.Recognizers.Text.Number.Chinese.Parsers;
using Microsoft.Recognizers.Text.Number.English.Parsers;
using Microsoft.Recognizers.Text.Number.Spanish.Parsers;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestParserFactory
    {
        private ExtractResult getNumberToParse(string number, string data)
        {
            return new ExtractResult
            {
                Type = Constants.SYS_NUM,
                Data = data,
                Text = number
            };
        }

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

            Assert.IsTrue(parserNumber is ChineseNumberParser);
            Assert.IsTrue(parserCardinal is ChineseNumberParser);
            Assert.IsTrue(parserPercentaje is ChineseNumberParser);
        }
    }
}
