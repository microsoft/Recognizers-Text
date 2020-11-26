using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public class SwedishNumberParserConfiguration : BaseNumberParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static ImmutableDictionary<string, long> swedishWrittenFractionLookupMap;

        public SwedishNumberParserConfiguration(INumberOptionsConfiguration config)
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
            this.OrdinalNumberMap = NumbersDefinitions.OrdinalNumberMap.ToImmutableDictionary();
            this.RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            this.RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexFlags);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags);

            swedishWrittenFractionLookupMap = NumbersDefinitions.SwedishWrittenFractionLookupMap.ToImmutableDictionary();
        }

        public string NonDecimalSeparatorText { get; private set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            var fracWords = new List<string>();
            var tokenList = tokens.ToList();
            var tokenLen = tokenList.Count;

            for (var i = 0; i < tokenLen; i++)
            {
                if ((i < tokenLen - 2) && tokenList[i + 1] == "-")
                {
                    fracWords.Add(tokenList[i] + tokenList[i + 1] + tokenList[i + 2]);
                    i += 2;
                }
                else
                {
                    fracWords.Add(tokenList[i]);
                }
            }

            return fracWords;
        }

        public override long ResolveCompositeNumber(string numberStr)
        {
            // Swedish Ordinals can't be used for denoting fractions as in other languages, e.g. English.
            // The default method uses the OrdinalNumberMap map to find a fraction expression.
            // When parsing swedish fractions, such as "en tjugoförstedel" (1/21) this method
            // fails to find the corresponding Ordinal since this doesn't exist.
            var resolvedNumber = base.ResolveCompositeNumber(numberStr);

            // So, if resolvedNumber == 0 we test for fractions and thus choose to
            // use the fallback swedishWrittenFractionLookupMap map to try to
            // find the corresponding value.
            if (resolvedNumber == 0)
            {
                // The swedishWrittenFractionLookupMap map contains the leading parts of all
                // tenths fractions, e.g.
                // 21: "tjugoförst" -> "tjugoförst(a|e)del(s|ar(na)?s?)"
                // 26: "tjugosjätted" -> "tjugosjätted(el(s|ar(na)?s?)"
                var tempResult = swedishWrittenFractionLookupMap.FirstOrDefault(k =>
                    {
                        // Try to find an entry in the map matching the start of numberStr
                        // E.g. "tjugoförstedel" starts w/ "tjugoförst" -> return 21
                        return numberStr.StartsWith(k.Key);
                    });

                resolvedNumber = tempResult.Value;
            }

            return resolvedNumber;

        }
    }
}
