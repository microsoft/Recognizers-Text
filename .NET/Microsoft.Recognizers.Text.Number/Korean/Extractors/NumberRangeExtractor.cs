// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;

namespace Microsoft.Recognizers.Text.Number.Korean
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(new NumberExtractor(new BaseNumberOptionsConfiguration(config)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseCJKNumberParser(new KoreanNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>
            {
                {
                    // ...과...사이
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // 이상...이하...

                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUM
                },
                {
                    // 이하...이상...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUM
                },
                {
                    // 이십보다 크고 삼십오보다 작다
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex7, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUM
                },
                {
                    // ...에서..., 20~30
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex5, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex6, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // ...이상|초과|많|높|크|더많|더높|더크|>
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                     // ...이상|초과|많|높|크|더많|더높|더크|>
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex5, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                   // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegexFraction, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
                {
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.TWONUMCLOSED
                },
                {
                    // 까지최소|<|≤...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex3, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.EQUAL
                },
                {
                    // >|≥...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex2, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.EQUAL
                },
                {
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.MORE
                },
                {
                    // 700에 달하는
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex4, RegexFlags, RegexTimeOut),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}