using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public class BaseIndianNumberParserConfiguration : IIndianNumberParserConfiguration
    {
        public ImmutableDictionary<string, long> CardinalNumberMap { get; set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; set; }

        public ImmutableDictionary<char, long> ZeroToNineMap { get; set; }

        public ImmutableDictionary<string, double> DecimalUnitsMap { get; set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceOffsetMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceRelativeToMap { get; set; }

        public INumberOptionsConfiguration Config { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public Regex DigitalNumberRegex { get; set; }

        public Regex FractionPrepositionRegex { get; set; }

        public Regex FractionPrepositionInverseRegex { get; set; }

        public string FractionMarkerToken { get; set; }

        public Regex HalfADozenRegex { get; set; }

        public string HalfADozenText { get; set; }

        public string LanguageMarker { get; set; }

        public char NonDecimalSeparatorChar { get; set; }

        public char DecimalSeparatorChar { get; set; }

        public string WordSeparatorToken { get; set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; set; }

        public Regex NegativeNumberSignRegex { get; set; }

        public bool IsCompoundNumberLanguage { get; set; }

        public bool IsMultiDecimalSeparatorCulture { get; set; }

        public Regex AdditionTermsRegex { get; set; }

        public virtual long ResolveCompositeNumber(string numberStr)
        {
            if (numberStr.Contains("-"))
            {
                var numbers = numberStr.Split('-');
                long ret = 0;
                foreach (var number in numbers)
                {
                    if (OrdinalNumberMap.ContainsKey(number))
                    {
                        ret += OrdinalNumberMap[number];
                    }
                    else if (CardinalNumberMap.ContainsKey(number))
                    {
                        ret += CardinalNumberMap[number];
                    }
                }

                return ret;
            }

            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }

            return 0;
        }

        public virtual IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            var fracWords = new List<string>();
            var tokenList = tokens.ToList();
            var tokenLen = tokenList.Count;

            for (var i = 0; i < tokenLen; i++)
            {
                if (tokenList[i].Contains("-"))
                {
                    var splitTokens = tokenList[i].Split('-');
                    if (splitTokens.Length == 2 && OrdinalNumberMap.ContainsKey(splitTokens[1]))
                    {
                        fracWords.Add(splitTokens[0]);
                        fracWords.Add(splitTokens[1]);
                    }
                    else
                    {
                        fracWords.Add(tokenList[i]);
                    }
                }
                else if (i < tokenLen - 2 && tokenList[i + 1] == "-")
                {
                    if (OrdinalNumberMap.ContainsKey(tokenList[i + 2]))
                    {
                        fracWords.Add(tokenList[i]);
                        fracWords.Add(tokenList[i + 2]);
                    }
                    else
                    {
                        fracWords.Add(tokenList[i] + tokenList[i + 1] + tokenList[i + 2]);
                    }

                    i += 2;
                }
                else
                {
                    fracWords.Add(tokenList[i]);
                }
            }

            return fracWords;
        }

        // Handle cases like "last", "next one", "previous one"
        public virtual string ResolveSpecificString(string numberStr)
        {
            if (this.RelativeReferenceMap.ContainsKey(numberStr))
            {
                return this.RelativeReferenceMap[numberStr];
            }

            return string.Empty;
        }

        // Used to parse regional Hindi cases like डेढ/सवा/ढाई
        // they are Indian Langauges specific cases and holds various meaning when prefixed with Number units.
        public virtual double ResolveUnitCompositeNumber(string numberStr)
        {
            if (this.DecimalUnitsMap.ContainsKey(numberStr))
            {
                return this.DecimalUnitsMap[numberStr];
            }

            return 0;
        }
    }
}
