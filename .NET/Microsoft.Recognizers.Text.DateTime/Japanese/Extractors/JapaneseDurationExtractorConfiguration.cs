using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.NumberWithUnit.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{

    public class JapaneseDurationExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDurationExtractorConfiguration
    {

        public static readonly Regex YearRegex = new Regex(DateTimeDefinitions.DurationYearRegex, RegexFlags);

        public static readonly Regex DurationUnitRegex = new Regex(DateTimeDefinitions.DurationUnitRegex, RegexFlags);

        public static readonly Regex DurationConnectorRegex = new Regex(DateTimeDefinitions.DurationConnectorRegex, RegexFlags);

        public static readonly Regex AllRegex = new Regex(DateTimeDefinitions.DurationAllRegex, RegexFlags);

        public static readonly Regex HalfRegex = new Regex(DateTimeDefinitions.DurationHalfRegex, RegexFlags);

        public static readonly Regex RelativeDurationUnitRegex = new Regex(DateTimeDefinitions.DurationRelativeDurationUnitRegex, RegexFlags);

        public static readonly Regex DuringRegex = new Regex(DateTimeDefinitions.DurationDuringRegex, RegexFlags);

        public static readonly Regex SomeRegex = new Regex(DateTimeDefinitions.DurationSomeRegex, RegexFlags);

        public static readonly Regex MoreOrLessRegex = new Regex(DateTimeDefinitions.DurationMoreOrLessRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private readonly bool merge;

        public JapaneseDurationExtractorConfiguration(IDateTimeOptionsConfiguration config, bool merge = true)
            : base(config)
        {
            this.merge = merge;

            InternalExtractor = new NumberWithUnitExtractor(new DurationExtractorConfiguration());

            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToDictionary(k => k.Key, k => k.Value);
            UnitValueMap = DateTimeDefinitions.DurationUnitValueMap;
        }

        public IExtractor InternalExtractor { get; }

        public Dictionary<string, string> UnitMap { get; }

        public Dictionary<string, long> UnitValueMap { get; }

        Regex ICJKDurationExtractorConfiguration.DurationUnitRegex => DurationUnitRegex;

        Regex ICJKDurationExtractorConfiguration.DurationConnectorRegex => DurationConnectorRegex;

        Regex ICJKDurationExtractorConfiguration.YearRegex => YearRegex;

        Regex ICJKDurationExtractorConfiguration.AllRegex => AllRegex;

        Regex ICJKDurationExtractorConfiguration.HalfRegex => HalfRegex;

        Regex ICJKDurationExtractorConfiguration.RelativeDurationUnitRegex => RelativeDurationUnitRegex;

        Regex ICJKDurationExtractorConfiguration.DuringRegex => DuringRegex;

        Regex ICJKDurationExtractorConfiguration.SomeRegex => SomeRegex;

        Regex ICJKDurationExtractorConfiguration.MoreOrLessRegex => MoreOrLessRegex;

        internal class DurationExtractorConfiguration : JapaneseNumberWithUnitExtractorConfiguration
        {
            public static readonly ImmutableDictionary<string, string> DurationSuffixList = DateTimeDefinitions.DurationSuffixList.ToImmutableDictionary();

            public DurationExtractorConfiguration()
                : base(new CultureInfo(Text.Culture.Japanese))
            {
            }

            public override ImmutableDictionary<string, string> SuffixList => DurationSuffixList;

            public override ImmutableDictionary<string, string> PrefixList => null;

            public override string ExtractType => Constants.SYS_DATETIME_DURATION;

            public override ImmutableList<string> AmbiguousUnitList => DateTimeDefinitions.DurationAmbiguousUnits.ToImmutableList();
        }
    }
}