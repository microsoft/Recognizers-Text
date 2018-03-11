using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class EnglishNumberParserConfiguration : INumberParserConfiguration
    {
        public EnglishNumberParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public EnglishNumberParserConfiguration(CultureInfo ci)
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
            this.OrdinalNumberMap = NumbersDefinitions.OrdinalNumberMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public CultureInfo CultureInfo { get; private set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public Regex NegativeNumberSignRegex { get; private set; }

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

        public long ResolveCompositeNumber(string numberStr)
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
    }
}
