using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    [TestClass]
    public class TestDecimalAndThousandsSeparators
    {
        public void ParseTest(LongFormatType type, string query, string value)
        {
            char decimalSep = type.DecimalsMark, nonDecimalSep = type.ThousandsMark;

            var parser = AgnosticNumberParserFactory.GetParser(
                AgnosticNumberParserType.Double, new LongFormTestConfiguration(decimalSep, nonDecimalSep));

            var resultJson = parser.Parse(new ExtractResult()
            {
                Text = query, Start = 0, Length = query.Length, Type = "builtin.num.double", Data = "Num",
            });

            Assert.AreEqual(value, resultJson.ResolutionStr);
        }

        [TestMethod]
        public void TestArabicParse()
        {
            ParseTest(LongFormatType.DoubleNumBlankComma, "123 456 789,123", "123456789.123");
            ParseTest(LongFormatType.DoubleNumBlankDot, "123 456 789.123", "123456789.123");
            ParseTest(LongFormatType.DoubleNumCommaCdot, "123,456,789·123", "123456789.123");
            ParseTest(LongFormatType.DoubleNumCommaDot, "123,456,789.123", "123456789.123");
            ParseTest(LongFormatType.DoubleNumDotComma, "123.456.789,123", "123456789.123");
            ParseTest(LongFormatType.DoubleNumQuoteComma, "123'456'789,123", "123456789.123");
            ParseTest(LongFormatType.IntegerNumBlank, "123 456 789", "123456789");
            ParseTest(LongFormatType.IntegerNumComma, "123,456,789", "123456789");
            ParseTest(LongFormatType.IntegerNumDot, "123.456.789", "123456789");
            ParseTest(LongFormatType.IntegerNumQuote, "123'456'789", "123456789");
        }
    }
}
