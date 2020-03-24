using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Swedish;

namespace Microsoft.Recognizers.Text.Number.Swedish
{
    public class FractionExtractor : BaseNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), FractionExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), FractionExtractor>();

        private FractionExtractor(NumberMode mode, NumberOptions options)
            : base(options)
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
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.SWEDISH)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.SWEDISH)
                },
            };

            // Not add FractionPrepositionRegex when the mode is Unit to avoid wrong recognize cases like "$1000 over 3"
            if (mode != NumberMode.Unit)
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.SWEDISH));
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        public sealed override NumberOptions Options { get; }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        // "Fraction";
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION;

        public static FractionExtractor GetInstance(NumberMode mode = NumberMode.Default, NumberOptions options = NumberOptions.None)
        {
            var cacheKey = (mode, options);

            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new FractionExtractor(mode, options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}