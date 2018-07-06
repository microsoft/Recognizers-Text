using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    class MergedNumberExtractor : BaseMergedNumberExtractor
    {
        public sealed override BaseNumberExtractor NumberExtractor { get; set; }

        public sealed override Regex RoundNumberIntegerRegexWithLocks { get; set; }

        public sealed override Regex ConnectorRegex { get; set; }

        private static readonly ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor> Instances =
            new ConcurrentDictionary<(NumberMode, NumberOptions), MergedNumberExtractor>();


        public static MergedNumberExtractor GetInstance(NumberMode mode = NumberMode.Default,
            NumberOptions options = NumberOptions.None)
        {
            var cacheKey = (mode, options);
            if (!Instances.ContainsKey(cacheKey))
            {
                var instance = new MergedNumberExtractor(mode, options);
                Instances.TryAdd(cacheKey, instance);
            }

            return Instances[cacheKey];
        }

        public MergedNumberExtractor(NumberMode mode, NumberOptions options)
        {
            NumberExtractor = English.NumberExtractor.GetInstance(mode, options);
            RoundNumberIntegerRegexWithLocks = new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            ConnectorRegex = new Regex(NumbersDefinitions.ConnectorRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
    }
}
