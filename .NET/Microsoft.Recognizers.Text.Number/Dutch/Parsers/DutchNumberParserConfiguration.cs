// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class DutchNumberParserConfiguration : BaseNumberParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FractionHalfRegex =
            new Regex(NumbersDefinitions.FractionHalfRegex, RegexFlags);

        private static readonly Regex FractionUnitsRegex =
            new Regex(NumbersDefinitions.FractionUnitsRegex, RegexFlags);

        private static readonly string[] OneHalfTokens = NumbersDefinitions.OneHalfTokens;

        public DutchNumberParserConfiguration(INumberOptionsConfiguration config)
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

            // @TODO Change init to follow design in other languages
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexFlags);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags);
            this.RoundMultiplierRegex = new Regex(NumbersDefinitions.RoundMultiplierRegex, RegexFlags);
        }

        public string NonDecimalSeparatorText { get; private set; }

        // Same behavior as the base but also handles numbers such as tweeënhalf and tweeëneenhalf
        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
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

            // The following piece of code is needed to compute the fraction pattern number+'ënhalf'
            // e.g. 'tweeënhalf' ('two and a half'). Similarly for "ëneenhalf", e.g. tweeëneenhalf.
            int len = 2;
            fracWords.RemoveAll(item => item == "/");
            for (int i = fracWords.Count - 1; i >= 0; i--)
            {
                if (FractionHalfRegex.IsMatch(fracWords[i]))
                {
                    fracWords[i] = fracWords[i].Substring(0, fracWords[i].Length - 6);
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(3));
                    fracWords.Insert(i + 2, OneHalfTokens[0]);
                    fracWords.Insert(i + 3, OneHalfTokens[1]);
                    len = 4;
                }
                else if (FractionUnitsRegex.Match(fracWords[i]).Groups["onehalf"].Success)
                {
                    fracWords[i] = OneHalfTokens[0];
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(3));
                    fracWords.Insert(i + 2, OneHalfTokens[0]);
                    fracWords.Insert(i + 3, OneHalfTokens[1]);
                    len = 4;
                }
                else if (FractionUnitsRegex.Match(fracWords[i]).Groups["quarter"].Success)
                {
                    var tempWord = fracWords[i];
                    fracWords[i] = tempWord.Substring(0, 4);
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(3));
                    fracWords.Insert(i + 2, tempWord.Substring(4, 5));
                    len = 3;
                }
            }

            // In Dutch, only the last two numbers in fracWords must be considered as fraction
            var fracLen = fracWords.Count;
            if (fracLen > len && fracWords[fracLen - len - 1] != NumbersDefinitions.WordSeparatorToken)
            {
                fracWords.Insert(fracLen - len, NumbersDefinitions.WordSeparatorToken);
            }

            return fracWords;
        }
    }
}
