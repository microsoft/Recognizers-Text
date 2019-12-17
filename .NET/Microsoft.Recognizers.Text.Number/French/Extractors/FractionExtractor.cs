using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FractionExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions, string), FractionExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions, string), FractionExtractor>();

        private FractionExtractor(NumberMode mode, NumberOptions options)
        {
            Options = options;

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
                    new Regex(NumbersDefinitions.FractionNounRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH)
                },
            };

            // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
            if (mode != NumberMode.Unit)
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.FRENCH));
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        public sealed override NumberOptions Options { get; }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public static FractionExtractor GetInstance(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None, string placeholder = "")
        {
            var cacheKey = (mode, options, placeholder);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new FractionExtractor(mode, options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}
