using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French.Utilities
{
    public class FrenchDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly Regex AgoRegex =
            new Regex(DateTimeDefinitions.AgoPrefixRegex, RegexFlags);

        public static readonly Regex LaterRegex =
            new Regex(DateTimeDefinitions.LaterRegex, RegexFlags);

        public static readonly Regex InConnectorRegex =
            new Regex(DateTimeDefinitions.InConnectorRegex, RegexFlags);

        public static readonly Regex SinceYearSuffixRegex =
            new Regex(DateTimeDefinitions.SinceYearSuffixRegex, RegexFlags);

        public static readonly Regex WithinNextPrefixRegex =
            new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexFlags);

        public static readonly Regex AmDescRegex =
            new Regex(DateTimeDefinitions.AmDescRegex, RegexFlags);

        public static readonly Regex PmDescRegex =
            new Regex(DateTimeDefinitions.PmDescRegex, RegexFlags);

        public static readonly Regex AmPmDescRegex =
            new Regex(DateTimeDefinitions.AmPmDescRegex, RegexFlags);

        public static readonly Regex RangeUnitRegex =
            new Regex(DateTimeDefinitions.RangeUnitRegex, RegexFlags);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags);

        public static readonly Regex DateUnitRegex =
            new Regex(DateTimeDefinitions.DateUnitRegex, RegexFlags);

        public static readonly Regex CommonDatePrefixRegex =
            new Regex(DateTimeDefinitions.CommonDatePrefixRegex, RegexFlags);

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
    }
}
