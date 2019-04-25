using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<string, OrdinalExtractor> Instances =
            new ConcurrentDictionary<string, OrdinalExtractor>();

        private OrdinalExtractor(NumberOptions options)
            : base(options)
        {
            AmbiguousFractionConnectorsRegex = new Regex(NumbersDefinitions.AmbiguousFractionConnectorsRegex, RegexOptions.Singleline);

            RelativeReferenceRegex = new Regex(NumbersDefinitions.RelativeOrdinalRegex, RegexOptions.Singleline);

            RelativeOrdinalFilterRegex = new Regex(NumbersDefinitions.RelativeOrdinalFilterRegex, RegexOptions.Singleline);

            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.OrdinalSuffixRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalNumericRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalEnglishRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.ENGLISH)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalRoundNumberRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.ENGLISH)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        protected sealed override Regex AmbiguousFractionConnectorsRegex { get; }

        protected sealed override Regex RelativeReferenceRegex { get; }

        protected sealed override Regex RelativeOrdinalFilterRegex { get; }

        public static OrdinalExtractor GetInstance(NumberOptions options = NumberOptions.None)
        {
            var cacheKey = options.ToString();
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new OrdinalExtractor(options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }
    }
}