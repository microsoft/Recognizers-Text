using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Number.German
{
    public class FractionExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<string, FractionExtractor> Instances =
         new ConcurrentDictionary<string, FractionExtractor>();

        private FractionExtractor()
        {
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
                    new Regex(NumbersDefinitions.FractionNounRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.GERMAN)
                },
                {
                    new Regex(NumbersDefinitions.FractionNounWithArticleRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.GERMAN)
                },
                {
                    new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.GERMAN)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        // "Fraction";
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION;

        public static FractionExtractor GetInstance(string placeholder = "")
        {
            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new FractionExtractor();
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }
    }
}