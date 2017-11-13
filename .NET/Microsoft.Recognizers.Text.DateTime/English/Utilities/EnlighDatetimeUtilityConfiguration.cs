using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.English.Utilities
{
    public class EnlighDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly Regex AgoRegex = new Regex(DateTimeDefinitions.AgoRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LaterRegex = new Regex(DateTimeDefinitions.LaterRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex = new Regex(DateTimeDefinitions.InConnectorRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmDescRegex = new Regex(DateTimeDefinitions.AmDescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmDescRegex = new Regex(DateTimeDefinitions.PmDescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmPmDescRegex = new Regex(DateTimeDefinitions.AmPmDescRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex RangeUnitRegex = new Regex(DateTimeDefinitions.RangeUnitRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimeUtilityConfiguration.LaterRegex => LaterRegex;

        Regex IDateTimeUtilityConfiguration.AgoRegex => AgoRegex;

        Regex IDateTimeUtilityConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDateTimeUtilityConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimeUtilityConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimeUtilityConfiguration.AmPmDescRegex => AmPmDescRegex;

        Regex IDateTimeUtilityConfiguration.RangeUnitRegex => RangeUnitRegex;
    }
}