﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{

    public class ChineseNumberParserConfiguration : BaseNumberParserConfiguration, ICJKNumberParserConfiguration
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseNumberParserConfiguration(INumberOptionsConfiguration config)
        {
            this.LanguageMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = new CultureInfo(config.Culture);
            this.Config = config;

            this.IsCompoundNumberLanguage = NumbersDefinitions.CompoundNumberLanguage;
            this.IsMultiDecimalSeparatorCulture = NumbersDefinitions.MultiDecimalSeparatorCulture;

            this.DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            this.FractionMarkerToken = NumbersDefinitions.FractionMarkerToken;
            this.NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            this.HalfADozenText = NumbersDefinitions.HalfADozenText;
            this.WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;
            this.ZeroChar = NumbersDefinitions.ZeroChar;
            this.PairChar = NumbersDefinitions.PairChar;

            this.WrittenDecimalSeparatorTexts = Enumerable.Empty<string>();
            this.WrittenGroupSeparatorTexts = Enumerable.Empty<string>();
            this.WrittenIntegerSeparatorTexts = Enumerable.Empty<string>();
            this.WrittenFractionSeparatorTexts = Enumerable.Empty<string>();

            this.CardinalNumberMap = new Dictionary<string, long>().ToImmutableDictionary();
            this.OrdinalNumberMap = new Dictionary<string, long>().ToImmutableDictionary();
            this.RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            this.RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            this.ZeroToNineMap = NumbersDefinitions.ZeroToNineMap.ToImmutableDictionary();
            this.RoundNumberMapChar = NumbersDefinitions.RoundNumberMapChar.ToImmutableDictionary();
            this.FullToHalfMap = NumbersDefinitions.FullToHalfMap.ToImmutableDictionary();
            this.TratoSimMap = NumbersDefinitions.TratoSimMap.ToImmutableDictionary();
            this.UnitMap = NumbersDefinitions.UnitMap.ToImmutableSortedDictionary(new StringLengthComparer());
            this.RoundDirectList = NumbersDefinitions.RoundDirectList.ToImmutableList();
            this.TenChars = NumbersDefinitions.TenChars.ToImmutableList();

            this.HalfADozenRegex = null;

            // @TODO Change init to follow design in other languages
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags);
            this.DigitNumRegex = new Regex(NumbersDefinitions.DigitNumRegex, RegexFlags);
            this.DozenRegex = new Regex(NumbersDefinitions.DozenRegex, RegexFlags);
            this.PercentageRegex = new Regex(NumbersDefinitions.PercentageRegex, RegexFlags);
            this.DoubleAndRoundRegex = new Regex(NumbersDefinitions.DoubleAndRoundRegex, RegexFlags);
            this.FracSplitRegex = new Regex(NumbersDefinitions.FracSplitRegex, RegexFlags);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags);
            this.PointRegex = new Regex(NumbersDefinitions.PointRegex, RegexFlags);
            this.SpeGetNumberRegex = new Regex(NumbersDefinitions.SpeGetNumberRegex, RegexFlags);
            this.PairRegex = new Regex(NumbersDefinitions.PairRegex, RegexFlags);
            this.RoundNumberIntegerRegex = new Regex(NumbersDefinitions.RoundNumberIntegerRegex, RegexFlags);
            this.PercentageNumRegex = new Regex(NumbersDefinitions.PercentageNumRegex, RegexFlags);
            this.FractionPrepositionRegex = null;
        }

        public string NonDecimalSeparatorText { get; private set; }

        public char ZeroChar { get; private set; }

        public char PairChar { get; private set; }

        public Regex DigitNumRegex { get; private set; }

        public Regex DozenRegex { get; private set; }

        public Regex PercentageRegex { get; private set; }

        public Regex PercentageNumRegex { get; private set; }

        public Regex DoubleAndRoundRegex { get; private set; }

        public Regex FracSplitRegex { get; private set; }

        public Regex PointRegex { get; private set; }

        public Regex SpeGetNumberRegex { get; private set; }

        public Regex PairRegex { get; private set; }

        public Regex RoundNumberIntegerRegex { get; private set; }

        public ImmutableDictionary<char, double> ZeroToNineMap { get; private set; }

        public ImmutableDictionary<char, long> RoundNumberMapChar { get; private set; }

        public ImmutableDictionary<char, char> FullToHalfMap { get; private set; }

        public ImmutableSortedDictionary<string, string> UnitMap { get; private set; }

        public ImmutableDictionary<char, char> TratoSimMap { get; private set; }

        public ImmutableList<char> RoundDirectList { get; private set; }

        public ImmutableList<char> TenChars { get; private set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public override long ResolveCompositeNumber(string numberStr)
        {
            return 0;
        }

        // Handle cases like "last", "next one", "previous one"
        public override string ResolveSpecificString(string numberStr)
        {
            if (this.RelativeReferenceMap.ContainsKey(numberStr))
            {
                return this.RelativeReferenceMap[numberStr];
            }

            return string.Empty;
        }

        private class StringLengthComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x.Length != y.Length)
                {
                    return x.Length - y.Length;
                }

                return x.CompareTo(y);
            }
        }
    }
}