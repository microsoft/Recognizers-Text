using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish.Extractors
{
    public class TemperatureExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public TemperatureExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public TemperatureExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => TemperatureSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_TEMPERATURE;

        public static readonly ImmutableDictionary<string, string> TemperatureSuffixList = new Dictionary<string, string>
        {
            {"Kelvin", "k|kelvin"},
            {"Rankine", "r|rankine"},
            {"Grado Celsius", "°c|grados c|grado celsius|grados celsius|celsius|grado centígrado|grados centígrados|centígrado|centígrados"},
            {"Grado Fahrenheit", "°f|grados f|grado fahrenheit|grados fahrenheit|fahrenheit"},
            {"Grado Réaumur", "°r|°re|grados r|grado réaumur|grados réaumur|réaumur"},
            {"Grado Delisle", "°d|grados d|grado delisle|grados delisle|delisle"},
            {"Grado", "°|grados|grado"},
        }.ToImmutableDictionary();
    }
}
