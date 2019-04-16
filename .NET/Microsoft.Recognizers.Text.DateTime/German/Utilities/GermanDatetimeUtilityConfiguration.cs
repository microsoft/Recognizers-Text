using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.German.Utilities
{
    public class GermanDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly Regex AgoRegex = new Regex(DateTimeDefinitions.AgoRegex, RegexOptions.Singleline);

        public static readonly Regex LaterRegex = new Regex(DateTimeDefinitions.LaterRegex, RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex = new Regex(DateTimeDefinitions.InConnectorRegex, RegexOptions.Singleline);

        public static readonly Regex SinceYearSuffixRegex = new Regex(DateTimeDefinitions.SinceYearSuffixRegex, RegexOptions.Singleline);

        public static readonly Regex WithinNextPrefixRegex = new Regex(DateTimeDefinitions.WithinNextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex AmDescRegex = new Regex(DateTimeDefinitions.AmDescRegex, RegexOptions.Singleline);

        public static readonly Regex PmDescRegex = new Regex(DateTimeDefinitions.PmDescRegex, RegexOptions.Singleline);

        public static readonly Regex AmPmDescRegex = new Regex(DateTimeDefinitions.AmPmDescRegex, RegexOptions.Singleline);

        public static readonly Regex RangeUnitRegex = new Regex(DateTimeDefinitions.RangeUnitRegex, RegexOptions.Singleline);

        public static readonly Regex TimeUnitRegex = new Regex(DateTimeDefinitions.TimeUnitRegex, RegexOptions.Singleline);

        public static readonly Regex DateUnitRegex = new Regex(DateTimeDefinitions.DateUnitRegex, RegexOptions.Singleline);

        public static readonly Regex CommonDatePrefixRegex =
            new Regex(DateTimeDefinitions.CommonDatePrefixRegex, RegexOptions.Singleline);

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