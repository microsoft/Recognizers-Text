
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English.Extractors
{
    public class TemperatureExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = new Dictionary<string, string>
        {
            {
                "F",
                "degrees fahrenheit|degree fahrenheit|fahrenheit|°f|degrees farenheit|degree farenheit|degrees f|degree f|deg f|farenheit|f"
            },
            {"K", "k|kelvin"},
            {"R", "rankine|°r"},
            {"D", "delisle|°de"},
            {
                "C",
                "degrees celsius|degree celsius|celsius|degrees celcius|degree celcius|celcius|degrees centigrade|degree centigrade|centigrade|degrees centigrate|degree centigrate|centigrate|degrees c|degree c|deg c|°c|c"
            },
            {"Degree", "degree|degrees|deg.|deg|°"}
        }.ToImmutableDictionary();
    }
}
