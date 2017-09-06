using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Portuguese
{
    public class TemperatureExtractorConfiguration : PortugueseNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = new Dictionary<string, string>
        {
            {"Kelvin", "k|kelvin"},
            {"Grau Rankine", "r|°r|°ra|grau rankine|graus rankine| rankine"},
            {"Grau Celsius", "°c|grau c|grau celsius|graus c|graus celsius|celsius|grau centígrado|grau centrigrado|graus centígrados|graus centigrados|centígrado|centígrados|centigrado|centigrados"},
            {"Grau Fahrenheit", "°f|grau f|graus f|grau fahrenheit|graus fahrenheit|fahrenheit"},
            {"Grau", "°|graus|grau"},
        }.ToImmutableDictionary();
    }
}
