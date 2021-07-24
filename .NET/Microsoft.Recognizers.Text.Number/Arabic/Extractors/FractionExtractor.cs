// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.Number.Arabic
{
    public class FractionExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.RightToLeft;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), FractionExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), FractionExtractor>();

        private FractionExtractor(BaseNumberOptionsConfiguration config)
            : base(config.Options)
        {

            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.FractionNotationWithSpacesRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNotationWithSpacesRegex2, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC)
                },
                {
                    new Regex(NumbersDefinitions.FractionWithOrdinalPrefix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC)
                },
                {
                    new Regex(NumbersDefinitions.FractionWithPartOfPrefix, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC)
                },
            };

            // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
            if (config.Mode != NumberMode.Unit)
            {
                if ((Options & NumberOptions.PercentageMode) != 0)
                {
                    regexes.Add(
                        new Regex(NumbersDefinitions.FractionPrepositionWithinPercentModeRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC));
                }
                else
                {
                    regexes.Add(
                        new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ARABIC));
                }
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public static FractionExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {
            var cacheKey = (config.Mode, config.Options);

            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new FractionExtractor(config);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}