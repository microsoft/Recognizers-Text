using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.Number.Hindi
{
    public class FractionExtractor : BaseNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberOptions, string), FractionExtractor> Instances =
            new ConcurrentDictionary<(NumberOptions, string), FractionExtractor>();

        private FractionExtractor(NumberOptions options)
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
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI)
                },
                {
                    new Regex(NumbersDefinitions.NegativeCompoundNumberOrdinals, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI)
                },
            };

            if ((Options & NumberOptions.PercentageMode) != 0)
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionPrepositionWithinPercentModeRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI));
            }
            else
            {
                regexes.Add(
                    new Regex(NumbersDefinitions.FractionRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI));
                regexes.Add(
                   new Regex(NumbersDefinitions.FractionPrepositionInverseRegex, RegexFlags),
                   RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.HINDI));
            }

            Regexes = regexes.ToImmutableDictionary();
        }

        public sealed override NumberOptions Options { get; }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

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
