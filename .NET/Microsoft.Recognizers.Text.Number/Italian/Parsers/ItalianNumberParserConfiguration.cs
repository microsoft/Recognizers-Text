using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class ItalianNumberParserConfiguration : BaseNumberParserConfiguration
    {
        public ItalianNumberParserConfiguration()
            : this(new CultureInfo(Culture.Italian))
        {
        }

        public ItalianNumberParserConfiguration(CultureInfo ci)
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

            this.CardinalNumberMap = NumbersDefinitions.CardinalNumberMap.ToImmutableDictionary();
            this.OrdinalNumberMap = NumberMapGenerator.InitOrdinalNumberMap(NumbersDefinitions.OrdinalNumberMap, NumbersDefinitions.PrefixCardinalMap, NumbersDefinitions.SuffixOrdinalMap);
            RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
        }

        public string NonDecimalSeparatorText { get; private set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public override long ResolveCompositeNumber(string numberStr)
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
