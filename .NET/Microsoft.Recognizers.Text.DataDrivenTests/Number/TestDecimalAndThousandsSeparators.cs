using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    public class LongFormTestConfiguration : INumberParserConfiguration
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

        public LongFormTestConfiguration(char decimalSep, char nonDecimalSep)
        {
            this.DecimalSeparatorChar = decimalSep;
            this.NonDecimalSeparatorChar = nonDecimalSep;
            this.CultureInfo = new CultureInfo(Culture.English);
            this.CardinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.OrdinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.RoundNumberMap = ImmutableDictionary<string, long>.Empty;
            this.DigitalNumberRegex = new Regex(@"((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))",
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
        public void ParseTest(LongFormatType type, string query, string value)
        {
            char decimalSep = type.DecimalsMark, nonDecimalSep = type.ThousandsMark;

            var parser = AgnosticNumberParserFactory.GetParser(
            AgnosticNumberParserType.Double,
            new LongFormTestConfiguration(decimalSep, nonDecimalSep));
            var resultJson = parser.Parse(
                new ExtractResult() { Text = query, Start = 0, Length = query.Length, Type = "builtin.num.double", Data = "Num" });
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
