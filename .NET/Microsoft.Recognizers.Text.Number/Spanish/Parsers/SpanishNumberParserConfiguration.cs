using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Spanish;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class SpanishNumberParserConfiguration : BaseNumberParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public SpanishNumberParserConfiguration(INumberOptionsConfiguration config)
        {

            this.Config = config;
            this.LanguageMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = new CultureInfo(config.Culture);

            this.IsCompoundNumberLanguage = NumbersDefinitions.CompoundNumberLanguage;
            this.IsMultiDecimalSeparatorCulture = NumbersDefinitions.MultiDecimalSeparatorCulture;

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
            this.OrdinalNumberMap = NumberMapGenerator.InitOrdinalNumberMap(
                NumbersDefinitions.OrdinalNumberMap,
                NumbersDefinitions.PrefixCardinalMap,
                NumbersDefinitions.SuffixOrdinalMap);

            this.RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            this.RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexFlags);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags);
        }

        public string NonDecimalSeparatorText { get; private set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            var result = new List<string>();

            foreach (var token in tokens)
            {
                var tempWord = token.Trim('s');
                if (this.OrdinalNumberMap.ContainsKey(tempWord))
                {
                    result.Add(tempWord);
                    continue;
                }

                if (tempWord.EndsWith("avo") || tempWord.EndsWith("ava"))
                {
                    var origTempWord = tempWord;
                    var newLength = origTempWord.Length;
                    tempWord = origTempWord.Remove(newLength - 3);
                    if (this.CardinalNumberMap.ContainsKey(tempWord))
                    {
                        result.Add(tempWord);
                        continue;
                    }
                    else
                    {
                        tempWord = origTempWord.Remove(newLength - 2);
                        if (this.CardinalNumberMap.ContainsKey(tempWord))
                        {
                            result.Add(tempWord);
                            continue;
                        }
                    }
                }

                result.Add(token);
            }

            return result;
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