using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FrenchNumberParserConfiguration : INumberParserConfiguration
    {
        public FrenchNumberParserConfiguration(): this(new CultureInfo(Culture.French)) { }

        public FrenchNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            this.FractionMarkerToken = NumbersDefinitions.FractionMarkerToken; 
            this.NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            this.HalfADozenText = NumbersDefinitions.HalfADozenText;
            this.WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;

            this.WrittenDecimalSeparatorTexts = NumbersDefinitions.WrittenDecimalSeparatorTexts;
            this.WrittenGroupSeparatorTexts = NumbersDefinitions.WrittenGroupSeparatorTexts;
            this.WrittenIntegerSeparatorTexts = NumbersDefinitions.WrittenIntegerSeparatorTexts;
            this.WrittenFractionSeparatorTexts = NumbersDefinitions.WrittenFractionSeparatorTexts;

            foreach (var sufix in NumbersDefinitions.SufixOrdinalDictionary)
            {
                foreach (var prefix in NumbersDefinitions.PrefixCardinalDictionary)
                {
                    if (!NumbersDefinitions.OrdinalNumberMap.ContainsKey(prefix.Key + sufix.Key))
                    {
                        NumbersDefinitions.OrdinalNumberMap.Add(prefix.Key + sufix.Key, prefix.Value * sufix.Value);
                    }
                }
            }

            this.CardinalNumberMap = NumbersDefinitions.CardinalNumberMap.ToImmutableDictionary();
            this.OrdinalNumberMap = NumbersDefinitions.OrdinalNumberMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public CultureInfo CultureInfo { get; set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public string NonDecimalSeparatorText { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; private set; }

        public string WordSeparatorToken { get; private set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; private set; }

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }

            long value = 0;
            long finalValue = 0;
            var strBuilder = new StringBuilder();
            int lastGoodChar = 0;
            for (int i = 0; i < numberStr.Length; i++)
            {
                strBuilder.Append(numberStr[i]);

                if (this.CardinalNumberMap.ContainsKey(strBuilder.ToString()) && this.CardinalNumberMap[strBuilder.ToString()] > value)
                {
                    lastGoodChar = i;
                    value = this.CardinalNumberMap[strBuilder.ToString()];
                }

                if ((i + 1) == numberStr.Length)
                {
                    finalValue += value;
                    strBuilder.Clear();
                    i = lastGoodChar++;
                    value = 0;
                }
            }

            return finalValue;
        }
    }
}
