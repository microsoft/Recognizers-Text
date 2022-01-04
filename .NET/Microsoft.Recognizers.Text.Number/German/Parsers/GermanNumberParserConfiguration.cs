// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Number.German
{
    public class GermanNumberParserConfiguration : BaseNumberParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FractionHalfRegex =
            new Regex(NumbersDefinitions.FractionHalfRegex, RegexFlags);

        private static readonly Regex FractionUnitsRegex =
            new Regex(NumbersDefinitions.FractionUnitsRegex, RegexFlags);

        private static readonly string[] OneHalfTokens = NumbersDefinitions.OneHalfTokens;

        public GermanNumberParserConfiguration(INumberOptionsConfiguration config)
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
            this.RoundMultiplierRegex = new Regex(NumbersDefinitions.RoundMultiplierRegex, RegexFlags);
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

            // The following piece of code is needed to compute the fraction pattern number+'einhalb'
            // e.g. 'zweieinhalb' ('two and a half').
            fracWords.RemoveAll(item => item == "/");
            for (int i = fracWords.Count - 1; i >= 0; i--)
            {
                if (FractionHalfRegex.IsMatch(fracWords[i]))
                {
                    fracWords[i] = fracWords[i].Substring(0, fracWords[i].Length - 7);
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(0));
                    fracWords.Insert(i + 2, OneHalfTokens[0]);
                    fracWords.Insert(i + 3, OneHalfTokens[1]);
                }
                else if (FractionUnitsRegex.Match(fracWords[i]).Groups["onehalf"].Success)
                {
                    fracWords[i] = OneHalfTokens[0];
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(0));
                    fracWords.Insert(i + 2, OneHalfTokens[0]);
                    fracWords.Insert(i + 3, OneHalfTokens[1]);
                }
                else if (FractionUnitsRegex.Match(fracWords[i]).Groups["quarter"].Success)
                {
                    var tempWord = fracWords[i];
                    fracWords[i] = tempWord.Substring(0, 4);
                    fracWords.Insert(i + 1, this.WrittenFractionSeparatorTexts.ElementAt(0));
                    fracWords.Insert(i + 2, tempWord.Substring(4, 5));
                }
            }

            return fracWords;
        }
    }
}
