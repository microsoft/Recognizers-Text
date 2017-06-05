using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class TemperatureExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => ambiguousUnitList;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = new Dictionary<string, string>
        {
            {
                "F",
                "degrees fahrenheit|degree fahrenheit|deg fahrenheit|degs fahrenheit|fahrenheit|°f|degrees farenheit|degree farenheit|deg farenheit|degs farenheit|degrees f|degree f|deg f|degs f|farenheit|f"
            },
            {"K", "k|kelvin"},
            {"R", "rankine|°r"},
            {"D", "delisle|°de"},
            {
                "C",
                "degrees celsius|degree celsius|deg celsius|degs celsius|celsius|degrees celcius|degree celcius|celcius|deg celcius|degs celcius|degrees centigrade|degree centigrade|centigrade|degrees centigrate|degree centigrate|degs centigrate|deg centigrate|centigrate|degrees c|degree c|deg c|degs c|°c|c"
            },
            {"Degree", "degree|degrees|deg.|deg|°"}
        }.ToImmutableDictionary();

        private static readonly ImmutableList<string> ambiguousUnitList = new List<string>
        {
            "c",
            "f",
            "k",
        }.ToImmutableList();
    }
}
