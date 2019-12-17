using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class FractionExtractor : BaseNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

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
                    new Regex(NumbersDefinitions.FractionNotationRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(
                        NumbersDefinitions.FractionNounRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH)
                },
            };

            // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
            if (config.Mode != NumberMode.Unit)
            {
                if ((Options & NumberOptions.PercentageMode) != 0)
                {
                    regexes.Add(
                        new Regex(NumbersDefinitions.FractionPrepositionWithinPercentModeRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH));
                }
                else
                {
                    regexes.Add(
                        new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags),
                        RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH));
                }
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        public sealed override NumberOptions Options { get; }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public static FractionExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {
            var extractorKey = (config.Mode, config.Options);

            if (!Instances.ContainsKey(extractorKey))
            {
                var instance = new FractionExtractor(config);
                Instances.TryAdd(extractorKey, instance);
            }

            return Instances[extractorKey];
        }
    }
}