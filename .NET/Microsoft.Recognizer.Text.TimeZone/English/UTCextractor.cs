using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.TimeZone.English
{
    class UTCextractor : BaseTimeZoneExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_TIMEZONE_UTC; // "Fraction";

        private static readonly ConcurrentDictionary<string, UTCextractor> Instances = new ConcurrentDictionary<string, UTCextractor>();
        public static UTCextractor GetInstance(string placeholder = "")
        {

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new UTCextractor();
                Instances.TryAdd(placeholder, instance);
            }
            return Instances[placeholder];
        }
        private UTCextractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(TimeZoneDefinitions.DirectUTCRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "DirectUTC"
                },
                {
                    new Regex(TimeZoneDefinitions.AbbrRegex,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "Abbr"
                },
                {
                    new Regex(TimeZoneDefinitions.FullRegex,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "Full"
                }
            };
            Regexes = regexes.ToImmutableDictionary();
        }

    }
}
