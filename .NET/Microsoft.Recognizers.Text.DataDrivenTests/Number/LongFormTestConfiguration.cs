using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Tests
{
    public class LongFormTestConfiguration : INumberParserConfiguration
    {
        public LongFormTestConfiguration(char decimalSep, char nonDecimalSep)
        {
            this.DecimalSeparatorChar = decimalSep;
            this.NonDecimalSeparatorChar = nonDecimalSep;
            this.CultureInfo = new CultureInfo(Culture.English);
            this.CardinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.OrdinalNumberMap = ImmutableDictionary<string, long>.Empty;
            this.RoundNumberMap = ImmutableDictionary<string, long>.Empty;
            this.DigitalNumberRegex = new Regex(
                @"((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))", RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; }

        public ImmutableDictionary<string, string> RelativeReferenceMap { get; private set; }

        public ImmutableDictionary<string, string> RelativeReferenceOffsetMap { get; private set; }

        public ImmutableDictionary<string, string> RelativeReferenceRelativeToMap { get; private set; }

        public INumberOptionsConfiguration Config { get; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; }

        public CultureInfo CultureInfo { get; }

        public Regex DigitalNumberRegex { get; }

        public Regex FractionPrepositionRegex { get; }

        public Regex RoundMultiplierRegex { get; }

        public string FractionMarkerToken { get; }

        public Regex HalfADozenRegex { get; }

        public string HalfADozenText { get; }

        public string LanguageMarker { get; } = "SelfDefined";

        public char NonDecimalSeparatorChar { get; }

        public char DecimalSeparatorChar { get; }

        public string WordSeparatorToken { get; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; }

        // Test-specific initialization: the Regex matches nothing.
        public Regex NegativeNumberSignRegex { get; } = new Regex(@"[^\s\S]");

        public bool IsCompoundNumberLanguage { get; }

        public bool IsMultiDecimalSeparatorCulture { get; }

        public IEnumerable<string> NonStandardSeparatorVariants => Enumerable.Empty<string>();

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            throw new NotImplementedException();
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            throw new NotImplementedException();
        }

        public (bool isRelevant, double value) GetLangSpecificIntValue(List<string> matchStrs)
        {
            return (false, double.MinValue);
        }

    }
}
