﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class OrdinalExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    // だい一百五十四
                    new Regex(NumbersDefinitions.OrdinalRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.JAPANESE)
                },
                {
                    // だい２５６５
                    new Regex(NumbersDefinitions.OrdinalNumbersRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.JAPANESE)
                },
                {
                    // 2折 ２.５折
                    new Regex(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                },
                {
                    // 2折 ２.５折
                    new Regex(NumbersDefinitions.RelativeOrdinalRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.JAPANESE)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL;
    }
}
