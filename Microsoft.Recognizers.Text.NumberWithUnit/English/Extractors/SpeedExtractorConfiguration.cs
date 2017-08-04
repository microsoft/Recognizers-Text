using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class SpeedExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public SpeedExtractorConfiguration() : base(new CultureInfo(Culture.English)) { }

        public SpeedExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => SpeedSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_SPEED;

        public static readonly ImmutableDictionary<string, string> SpeedSuffixList = new Dictionary<string, string>
        {
            {"Meter per second", "meters / second|m/s|meters per second|metres per second|meter per second|metre per second"},
            {"Kilometer per hour", "km/h|kilometres per hour|kilometers per hour|kilometer per hour|kilometre per hour"},
            {"Kilometer per minute", "km/min|kilometers per minute|kilometres per minute|kilometer per minute|kilometre per minute"},
            {"Kilometer per second", "km/s|kilometers per second|kilometres per second|kilometer per second|kilometre per second"},
            {"Mile per hour", "mph|mile per hour|miles per hour|mi/h|mile / hour|miles / hour|miles an hour"},
            {"Knot", "kt|knot|kn"},
            {"Foot per second", "ft/s|foot/s|foot per second|feet per second|fps"},
            {"Foot per minute", "ft/min|foot/min|foot per minute|feet per minute"},
            {"Yard per minute", "yards per minute|yard per minute|yards / minute|yards/min|yard/min"},
            {"Yard per second", "yards per second|yard per second|yards / second|yards/s|yard/s"},
        }.ToImmutableDictionary();
    }
}
