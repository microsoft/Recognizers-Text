using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    public class ArabicNumberTestConfiguration : INumberParserConfiguration
    {
        public ImmutableDictionary<string, long> CardinalNumberMap { get; }
        public ImmutableDictionary<string, long> OrdinalNumberMap { get; }
        public ImmutableDictionary<string, long> RoundNumberMap { get; }
        public CultureInfo CultureInfo { get; }
        public Regex DigitalNumberRegex { get; }
        public string FractionMarkerToken { get; }
        public Regex HalfADozenRegex { get; }
        public string HalfADozenText { get; }

        public string LangMarker { get; } = "SelfDefined";

        public char NonDecimalSeparatorChar { get; }

        public char DecimalSeparatorChar { get; }
        public string WordSeparatorToken { get; }
        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; }
        public IEnumerable<string> WrittenGroupSeparatorTexts { get; }
        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; }
        public IEnumerable<string> WrittenFractionSeparatorTexts { get; }

        public ArabicNumberTestConfiguration(char decimalSep, char nonDecimalSep)
        {
            this.DecimalSeparatorChar = decimalSep;
            this.NonDecimalSeparatorChar = nonDecimalSep;
            this.CultureInfo = new CultureInfo(Culture.English);
            this.CardinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.OrdinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.RoundNumberMap = ImmutableDictionary<string, long>.Empty;
            this.DigitalNumberRegex = new Regex(
        @"((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))",
         RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Singleline);

        }

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            throw new NotImplementedException();
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class TestDecimalAndThousandsSeparators
    {
        public void ParseTest(ArabicType type, string query, string value)
        {
            char decimalSep = '.', nonDecimalSep = ',';
            switch (type)
            {
                case ArabicType.DoubleNumBlankComma:
                case ArabicType.DoubleNumBlankDot:
                case ArabicType.IntegerNumBlank:
                    nonDecimalSep = ' ';
                    break;
                case ArabicType.DoubleNumCommaCdot:
                case ArabicType.DoubleNumCommaDot:
                case ArabicType.IntegerNumComma:
                    nonDecimalSep = ',';
                    break;
                case ArabicType.DoubleNumDotComma:
                case ArabicType.IntegerNumDot:
                    nonDecimalSep = '.';
                    break;
                case ArabicType.DoubleNumQuoteComma:
                case ArabicType.IntegerNumQuote:
                    nonDecimalSep = '\'';
                    break;
            }
            switch (type)
            {
                case ArabicType.DoubleNumQuoteComma:
                case ArabicType.DoubleNumBlankComma:
                case ArabicType.DoubleNumDotComma:
                    decimalSep = ',';
                    break;
                case ArabicType.DoubleNumBlankDot:
                case ArabicType.DoubleNumCommaDot:
                    decimalSep = '.';
                    break;
                case ArabicType.DoubleNumCommaCdot:
                    decimalSep = '·';
                    break;
            }

            var parser = AgnosticNumberParserFactory.GetParser(
            AgnosticNumberParserType.Double,
            new ArabicNumberTestConfiguration(decimalSep, nonDecimalSep));
            var resultJson =
                parser.Parse(
                    new ExtractResult() { Text = query, Start = 0, Length = query.Length, Type = "builtin.num.double", Data = "Num" });
            Assert.AreEqual(value, resultJson.ResolutionStr);
        }

        [TestMethod]
        public void TestArabicParse()
        {
            ParseTest(ArabicType.DoubleNumBlankComma, "123 456 789,123", "123456789.123");
            ParseTest(ArabicType.DoubleNumBlankDot, "123 456 789.123", "123456789.123");
            ParseTest(ArabicType.DoubleNumCommaCdot, "123,456,789·123", "123456789.123");
            ParseTest(ArabicType.DoubleNumCommaDot, "123,456,789.123", "123456789.123");
            ParseTest(ArabicType.DoubleNumDotComma, "123.456.789,123", "123456789.123");
            ParseTest(ArabicType.DoubleNumQuoteComma, "123'456'789,123", "123456789.123");
            ParseTest(ArabicType.IntegerNumBlank, "123 456 789", "123456789");
            ParseTest(ArabicType.IntegerNumComma, "123,456,789", "123456789");
            ParseTest(ArabicType.IntegerNumDot, "123.456.789", "123456789");
            ParseTest(ArabicType.IntegerNumQuote, "123'456'789", "123456789");
        }
    }
}
