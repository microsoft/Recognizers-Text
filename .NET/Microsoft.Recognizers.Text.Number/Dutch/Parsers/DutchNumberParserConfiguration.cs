using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class DutchNumberParserConfiguration : INumberParserConfiguration
    {
        public DutchNumberParserConfiguration(NumberOptions options) : this()
        {
            this.Options = options;
        }

        public DutchNumberParserConfiguration() : this(new CultureInfo(Culture.Dutch)) { }

        public DutchNumberParserConfiguration(CultureInfo ci)
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
            this.RelativeReferenceMap = NumbersDefinitions.RelativeReferenceMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            // @TODO Change init to follow design in other languages
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
            this.FractionPrepositionRegex  = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public NumberOptions Options { get; }

        public CultureInfo CultureInfo { get; private set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public Regex FractionPrepositionRegex { get; }

        public Regex NegativeNumberSignRegex { get; private set; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public string NonDecimalSeparatorText { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, string> RelativeReferenceMap { get; private set; }

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
                if (tokenList[i].Contains("-"))
                {
                    var splitedTokens = tokenList[i].Split('-');
                    if (splitedTokens.Length == 2 && OrdinalNumberMap.ContainsKey(splitedTokens[1]))
                    {
                        fracWords.Add(splitedTokens[0]);
                        fracWords.Add(splitedTokens[1]);
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

        // Handle cases like "last", "next one", "previous one"
        public string ResolveSpecificString(string numberStr)
        {
            if (this.RelativeReferenceMap.ContainsKey(numberStr))
            {
                return this.RelativeReferenceMap[numberStr];
            }

            return string.Empty;
        }
    }
}
