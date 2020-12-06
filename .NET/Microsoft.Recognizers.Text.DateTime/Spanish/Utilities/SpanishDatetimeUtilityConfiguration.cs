using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Utilities
{
    public class SpanishDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly Regex AgoRegex =
            RegexCache.Get(DateTimeDefinitions.AgoRegex, RegexFlags);

        public static readonly Regex LaterRegex =
            RegexCache.Get(DateTimeDefinitions.LaterRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            RegexCache.Get(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex SinceYearSuffixRegex =
            RegexCache.Get(DateTimeDefinitions.SinceYearSuffixRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            RegexCache.Get(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex AmDescRegex =
            RegexCache.Get(DateTimeDefinitions.AmDescRegex, RegexFlags);

        public static readonly Regex PmDescRegex =
            RegexCache.Get(DateTimeDefinitions.PmDescRegex, RegexFlags);

        public static readonly Regex AmPmDescRegex =
            RegexCache.Get(DateTimeDefinitions.AmPmDescRegex, RegexFlags);

        public static readonly Regex RangeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            RegexCache.Get(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            RegexCache.Get(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex CommonDatePrefixRegex =
            RegexCache.Get(DateTimeDefinitions.CommonDatePrefixRegex, RegexFlags);

        public static readonly Regex RangePrefixRegex =
            RegexCache.Get(DateTimeDefinitions.RangePrefixRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        Regex IDateTimeUtilityConfiguration.LaterRegex => LaterRegex;

        Regex IDateTimeUtilityConfiguration.AgoRegex => AgoRegex;

        Regex IDateTimeUtilityConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDateTimeUtilityConfiguration.SinceYearSuffixRegex => SinceYearSuffixRegex;

        Regex IDateTimeUtilityConfiguration.WithinNextPrefixRegex => WithinNextPrefixRegex;

        Regex IDateTimeUtilityConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimeUtilityConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimeUtilityConfiguration.AmPmDescRegex => AmPmDescRegex;

        Regex IDateTimeUtilityConfiguration.RangeUnitRegex => RangeUnitRegex;

        Regex IDateTimeUtilityConfiguration.TimeUnitRegex => TimeUnitRegex;

        Regex IDateTimeUtilityConfiguration.DateUnitRegex => DateUnitRegex;

        Regex IDateTimeUtilityConfiguration.CommonDatePrefixRegex => CommonDatePrefixRegex;

        Regex IDateTimeUtilityConfiguration.RangePrefixRegex => RangePrefixRegex;

        bool IDateTimeUtilityConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;
    }
}
