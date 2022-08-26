// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class NumberRangeExtractor : BaseNumberRangeExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public NumberRangeExtractor(INumberOptionsConfiguration config)
            : base(NumberExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   OrdinalExtractor.GetInstance(new BaseNumberOptionsConfiguration(config.Culture, config.Options)),
                   new BaseNumberParser(new PortugueseNumberParserConfiguration(config)),
                   config)
        {

            var regexes = new Dictionary<Regex, string>()
            {
                {
                    // entre ...e ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex1, RegexFlags),
                    NumberRangeConstants.TWONUMBETWEEN
                },
                {
                    // mais que ... menos que ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex2, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // menos que ... mais que ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex3, RegexFlags),
                    NumberRangeConstants.TWONUM
                },
                {
                    // de ... a ...
                    new Regex(NumbersDefinitions.TwoNumberRangeRegex4, RegexFlags),
                    NumberRangeConstants.TWONUMTILL
                },
                {
                    // mais/maior que ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex1LB, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // 30 and/or greater/higher
                    new Regex(NumbersDefinitions.OneNumberRangeMoreRegex2, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // less/smaller/lower than ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex1LB, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // 30 e/ou mais/maior/maiores
                    new Regex(NumbersDefinitions.OneNumberRangeLessRegex2, RegexFlags),
                    NumberRangeConstants.LESS
                },
                {
                    // igual a ...
                    new Regex(NumbersDefinitions.OneNumberRangeEqualRegex, RegexFlags),
                    NumberRangeConstants.EQUAL
                },
                {
                    // igual a 30 ou mais, mais que 30 ou igual ...
                    new Regex(NumbersDefinitions.OneNumberRangeMoreSeparateRegex, RegexFlags),
                    NumberRangeConstants.MORE
                },
                {
                    // igual a 30 ou menos, menos que 30 ou igual ...
                    new Regex(NumbersDefinitions.OneNumberRangeLessSeparateRegex, RegexFlags),
                    NumberRangeConstants.LESS
                },
            };

            Regexes = regexes.ToImmutableDictionary();

            AmbiguousFractionConnectorsRegex =
                new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexFlags);
        }

        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        internal sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUMRANGE;
    }
}
