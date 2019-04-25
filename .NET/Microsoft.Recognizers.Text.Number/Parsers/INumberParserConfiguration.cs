using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public interface INumberParserConfiguration
    {
        ImmutableDictionary<string, long> CardinalNumberMap { get; }

        ImmutableDictionary<string, long> OrdinalNumberMap { get; }

        ImmutableDictionary<string, long> RoundNumberMap { get; }

        ImmutableDictionary<string, string> RelativeReferenceMap { get; }

        ImmutableDictionary<string, string> RelativeReferenceOffsetMap { get; }

        ImmutableDictionary<string, string> RelativeReferenceRelativeToMap { get; }

        NumberOptions Options { get; }

        CultureInfo CultureInfo { get; }

        Regex DigitalNumberRegex { get; }

        Regex FractionPrepositionRegex { get; }

        string FractionMarkerToken { get; }

        Regex HalfADozenRegex { get; }

        string HalfADozenText { get; }

        string LangMarker { get; }

        char NonDecimalSeparatorChar { get; }

        char DecimalSeparatorChar { get; }

        string WordSeparatorToken { get; }

        IEnumerable<string> WrittenDecimalSeparatorTexts { get; }

        IEnumerable<string> WrittenGroupSeparatorTexts { get; }

        IEnumerable<string> WrittenIntegerSeparatorTexts { get; }

        IEnumerable<string> WrittenFractionSeparatorTexts { get; }

        Regex NegativeNumberSignRegex { get; }

        /// <summary>
        /// Used when requiring to normalize a token to a valid expression supported by the ImmutableDictionaries (language dictionaries).
        /// </summary>
        /// <param name="tokens">list of tokens to normalize.</param>
        /// <param name="context">context of the call.</param>
        /// <returns>list of normalized tokens.</returns>
        IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context);

        /// <summary>
        /// Used when requiring to convert a string to a valid number supported by the language.
        /// </summary>
        /// <param name="numberStr">composite number.</param>
        /// <returns>value of the string.</returns>
        long ResolveCompositeNumber(string numberStr);
    }

    public class BaseNumberParserConfiguration : INumberParserConfiguration
    {
        public ImmutableDictionary<string, long> CardinalNumberMap { get; set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceOffsetMap { get; set; }

        public ImmutableDictionary<string, string> RelativeReferenceRelativeToMap { get; set; }

        public NumberOptions Options { get; set; }

        public CultureInfo CultureInfo { get; set; }

        public Regex DigitalNumberRegex { get; set; }

        public Regex FractionPrepositionRegex { get; set; }

        public string FractionMarkerToken { get; set; }

        public Regex HalfADozenRegex { get; set; }

        public string HalfADozenText { get; set; }

        public string LangMarker { get; set; }

        public char NonDecimalSeparatorChar { get; set; }

        public char DecimalSeparatorChar { get; set; }

        public string WordSeparatorToken { get; set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; set; }

        public Regex NegativeNumberSignRegex { get; set; }

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

        // Handle cases like "last", "next one", "previous one"
        public virtual string ResolveSpecificString(string numberStr)
        {
            if (this.RelativeReferenceMap.ContainsKey(numberStr))
            {
                return this.RelativeReferenceMap[numberStr];
            }

            return string.Empty;
        }
    }
}