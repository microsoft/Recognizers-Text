using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class FractionExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<(NumberOptions, string), FractionExtractor> Instances =
            new ConcurrentDictionary<(NumberOptions, string), FractionExtractor>();

        private FractionExtractor(NumberOptions options)
        {
            Options = options;

            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.FractionNotationWithSpacesRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.FractionNotationRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(
                        NumbersDefinitions.FractionNounRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH)
                },
            };

            if ((Options & NumberOptions.PercentageMode) != 0)
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionPrepositionWithinPercentModeRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH));
            }
            else
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.DUTCH));
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override NumberOptions Options { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public static FractionExtractor GetInstance(NumberOptions options = NumberOptions.None, string placeholder = "")
        {
            var cacheKey = (options, placeholder);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new FractionExtractor(options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}