using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class OrdinalExtractor : CachedNumberExtractor
    {

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<string, OrdinalExtractor> Instances = new ConcurrentDictionary<string, OrdinalExtractor>();

        private readonly string keyPrefix;

        private OrdinalExtractor(BaseNumberOptionsConfiguration config)
            : base(config.Options)
        {

            keyPrefix = string.Intern(ExtractType + "_" + config.Options.ToString() + "_" + config.Culture);

            var regexes = new Dictionary<Regex, TypeTag>
            {
                {
                    new Regex(NumbersDefinitions.OrdinalSuffixRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalNumericRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalDutchRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.DUTCH)
                },
                {
                    new Regex(NumbersDefinitions.OrdinalRoundNumberRegex, RegexFlags),
                    RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.DUTCH)
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal sealed override ImmutableDictionary<Regex, TypeTag> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public static OrdinalExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {

            var extractorKey = config.Options.ToString();

            if (!Instances.ContainsKey(extractorKey))
            {
                var instance = new OrdinalExtractor(config);
                Instances.TryAdd(extractorKey, instance);
            }

            return Instances[extractorKey];
        }

        protected override object GenKey(string input)
        {
            return (keyPrefix, input);
        }
    }
}