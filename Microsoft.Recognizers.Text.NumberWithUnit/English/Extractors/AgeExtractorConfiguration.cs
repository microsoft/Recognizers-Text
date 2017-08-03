using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Microsoft.Recognizers.Text.NumberWithUnit.English
{
    public class AgeExtractorConfiguration : EnglishNumberWithUnitExtractorConfiguration
    {
        public AgeExtractorConfiguration() : this(new CultureInfo(Culture.English)) { }

        public AgeExtractorConfiguration(CultureInfo ci) : base(ci) { }

        public override ImmutableDictionary<string, string> SuffixList => AgeSuffixList;

        public override ImmutableDictionary<string, string> PrefixList => null;

        public override ImmutableList<string> AmbiguousUnitList => null;

        public override string ExtractType => Constants.SYS_UNIT_AGE;

        public static readonly ImmutableDictionary<string, string> AgeSuffixList = new Dictionary<string, string>
        {
            {"Year", "years old|year old|year-old|years-old|-year-old|-years-old|years of age|year of age"},
            {"Month", "months old|month old|month-old|months-old|-month-old|-months-old|month of age|months of age"},
            {"Week", "weeks old|week old|week-old|weeks-old|-week-old|-weeks-old|week of age|weeks of age"},
            {"Day", "days old|day old|day-old|days-old|-day-old|-days-old|day of age|days of age"}
        }.ToImmutableDictionary();
    }
}