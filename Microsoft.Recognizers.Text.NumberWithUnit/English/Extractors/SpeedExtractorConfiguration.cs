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
            {
                "Meter per second",
                "meters / second|m/s|meters per second|metres per second|meter per second|metre per second"
            },
            {"Kilometer per hour", "km/h|kilometres per hour|kilometers per hour|kilometer per hour|kilometers / hour"},
            {"Kilometer per minute", "km/min|kilometers per minute|kilometres per minute|kilometer per minute"},
            {"Kilometer per second", "km/s|kilometers per second|kilometres per second"},
            {"Mile per hour", "mph|miles per hour|mi/h|mile / hour"},
            {"Knot", "kt|knot|kn"},
            {"Foot per second", "ft/s|foot/s|feet per second|fps"},
            {"Foot per minute", "ft/min|foot/min|feet per minute"},
            {"Yard per minute", "yards per minute|yards / minute|yards/min"},
            {"Yard per second", "yards per second|yards / second|yards/s"}
        }.ToImmutableDictionary();
    }
}
