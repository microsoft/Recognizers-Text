﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number.Config;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class DoubleExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DoubleExtractor(BaseNumberOptionsConfiguration config, CJKNumberExtractorMode mode = CJKNumberExtractorMode.Default)
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // (-)2.5, can avoid cases like ip address xx.xx.xx.xx
                    new Regex(NumbersDefinitions.DoubleSpecialsCharsWithNegatives, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // (-).2
                    new Regex(NumbersDefinitions.SimpleDoubleSpecialsChars, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // えは九・二三二一三一二
                    new Regex(NumbersDefinitions.DoubleRoundNumberSpecialsChars, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // １５.２万
                    new Regex(NumbersDefinitions.DoubleWithThousandsRegex, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.JAPANESE)
                },
                {
                    // 2e6, 21.2e0
                    new Regex(NumbersDefinitions.DoubleExponentialNotationRegex, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.DoubleExponentialNotationKanjiRegex, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    // 2^5
                    new Regex(NumbersDefinitions.DoubleScientificNotationRegex, RegexFlags, RegexTimeOut),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.POWER_SUFFIX)
                },
                {
                    // １　２３４　５６７．８９
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumFullWidthBlankDot),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    // 1 234 567.89
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumBlankDot),
                    RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX)
                },
            };

            switch (mode)
            {
                case CJKNumberExtractorMode.Default:
                    // Uses an allow list to avoid extracting "西九条" from "九"
                    regexes.Add(
                        new Regex(NumbersDefinitions.DoubleSpecialsChars, RegexFlags, RegexTimeOut),
                        RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX));

                    // 1.0 K
                    regexes.Add(
                        new Regex(NumbersDefinitions.DoubleWithMultiplierRegex, RegexFlags, RegexTimeOut),
                        RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX));
                    break;

                case CJKNumberExtractorMode.ExtractAll:
                    // Uses no allow lists and extracts all potential numbers (useful in Units, for example).
                    regexes.Add(
                        new Regex(NumbersDefinitions.DoubleSpecialsCharsAggressive, RegexFlags, RegexTimeOut),
                        RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX));

                    // 1.0 K
                    regexes.Add(
                        new Regex(NumbersDefinitions.DoubleWithMultiplierAggressiveRegex, RegexFlags, RegexTimeOut),
                        RegexTagGenerator.GenerateRegexTag(Constants.DOUBLE_PREFIX, Constants.NUMBER_SUFFIX));
                    break;
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;
    }
}
