using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.TimeZone.English
{
    public class TimeZoneExtractor : BaseTimeZoneExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_TIMEZONE; // "Number";

        private static readonly ConcurrentDictionary<string, TimeZoneExtractor> Instances = new ConcurrentDictionary<string, TimeZoneExtractor>();

        public static TimeZoneExtractor GetInstance(TimeZoneMode mode = TimeZoneMode.Default)
        {

            var placeholder = mode.ToString();

            if (!Instances.ContainsKey(placeholder))
            {
                var instance = new TimeZoneExtractor(mode);
                Instances.TryAdd(placeholder, instance);
            }

            return Instances[placeholder];
        }

        private TimeZoneExtractor(TimeZoneMode mode = TimeZoneMode.Default)
        {
            var builder = ImmutableDictionary.CreateBuilder<Regex, string>();

            //Add Fraction
            var utcExtract = UTCextractor.GetInstance();
            builder.AddRange(utcExtract.Regexes);
            Regexes = builder.ToImmutable();
        }
    }
}
