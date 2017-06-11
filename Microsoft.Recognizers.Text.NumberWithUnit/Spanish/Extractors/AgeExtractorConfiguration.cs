using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class AgeExtractorConfiguration : SpanishNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public AgeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;

        public static readonly ImmutableDictionary<string, string> AgeSuffixList = new Dictionary<string, string>
        {
            {"Año", "años|año"},
            {"Mes", "meses|mes"},
            {"Semana", "semanas|semana"},
            {"Día", "dias|días|día|dia"}
        }.ToImmutableDictionary();
    }
}