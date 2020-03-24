using System.Collections.Concurrent;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    internal class MergedNumberExtractor : BaseMergedNumberExtractor
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor>();

        public MergedNumberExtractor(BaseNumberOptionsConfiguration config)
        {
            NumberExtractor = English.NumberExtractor.GetInstance(config);
            RoundNumberIntegerRegexWithLocks = new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks, RegexFlags);
            ConnectorRegex = new Regex(NumbersDefinitions.ConnectorRegex, RegexFlags);
        }

        public sealed override BaseNumberExtractor NumberExtractor { get; set; }

        public sealed override Regex RoundNumberIntegerRegexWithLocks { get; set; }

        public sealed override Regex ConnectorRegex { get; set; }

        public static MergedNumberExtractor GetInstance(BaseNumberOptionsConfiguration config)
        {

            var extractorKey = (config.Mode, config.Options);

            if (!Instances.ContainsKey(extractorKey))
            {
                var instance = new MergedNumberExtractor(config);
                Instances.TryAdd(extractorKey, instance);
            }

            return Instances[extractorKey];
        }
    }
}
