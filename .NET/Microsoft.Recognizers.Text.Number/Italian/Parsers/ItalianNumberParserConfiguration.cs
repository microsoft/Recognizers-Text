﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Italian;

namespace Microsoft.Recognizers.Text.Number.Italian
{
    public class ItalianNumberParserConfiguration : BaseNumberParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ItalianNumberParserConfiguration(INumberOptionsConfiguration config)
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

            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexFlags, RegexTimeOut);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags, RegexTimeOut);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags, RegexTimeOut);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags, RegexTimeOut);
            this.OneToNineOrdinalRegex = new Regex(NumbersDefinitions.OneToNineOrdinalRegex, RegexFlags, RegexTimeOut);
            this.RoundMultiplierRegex = new Regex(NumbersDefinitions.RoundMultiplierRegex, RegexFlags, RegexTimeOut);
        }

        public string NonDecimalSeparatorText { get; private set; }

        public Regex OneToNineOrdinalRegex { get; }

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

            // The following piece of code is needed in Italian to correctly compute some fraction patterns
            // e.g. 'due milioni duemiladuecento quinti' (=2002200/5) which is otherwise interpreted as
            // 2000000/2205 (in Italian, isolated ordinals <10 have a different form respect to when
            // they are concatenated to other numbers, so the following lines try to keep them isolated
            // by concatenating the two previous numbers)
            var fracLen = fracWords.Count;
            if (fracLen > 2 && this.OneToNineOrdinalRegex.Match(fracWords[fracLen - 1]).Success)
            {
                if (fracWords[fracLen - 3] != NumbersDefinitions.WordSeparatorToken && fracWords[fracLen - 2] != NumbersDefinitions.WordSeparatorToken)
                {
                    fracWords[fracLen - 3] += fracWords[fracLen - 2];
                    fracWords.RemoveAt(fracLen - 2);
                }
            }

            // The following piece of code is needed to compute the fraction pattern number+'e mezzo'
            // e.g. 'due e mezzo' ('two and a half') where the numerator is omitted in Italian.
            // It works by inserting the numerator 'un' ('a') in the list fracWords
            // so that the pattern is correctly processed.
            fracLen = fracWords.Count;
            if (fracLen > 2)
            {
                if (fracWords[fracLen - 1] == NumbersDefinitions.OneHalfTokens[1] && fracWords[fracLen - 2] == NumbersDefinitions.WordSeparatorToken)
                {
                    fracWords.Insert(fracLen - 1, NumbersDefinitions.OneHalfTokens[0]);
                }
            }

            return fracWords;
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
            long prevValue = 0;

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
                    if (prevValue > 0 && value > prevValue)
                    {
                        value = (prevValue * value) - prevValue;
                    }

                    if (prevValue < 1000)
                    {
                        prevValue = value + prevValue;
                    }
                    else
                    {
                        prevValue = value;
                    }

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
