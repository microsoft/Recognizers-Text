using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Number.German
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        private static readonly ConcurrentDictionary<string, OrdinalExtractor> Instances =
            new ConcurrentDictionary<string, OrdinalExtractor>();

        private OrdinalExtractor()
        {
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
                    new Regex(NumbersDefinitions.OrdinalGermanRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.GERMAN)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalRoundNumberRegex, RegexOptions.Singleline),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.GERMAN)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public static OrdinalExtractor GetInstance(string placeholder = "")
        {
            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new OrdinalExtractor();
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }
    }
}