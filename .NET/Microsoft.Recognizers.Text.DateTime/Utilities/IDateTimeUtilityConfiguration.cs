using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public interface IDateTimeUtilityConfiguration
    {
        Regex AgoRegex { get; }

        Regex LaterRegex { get; }

        Regex InConnectorRegex { get; }

        Regex SinceYearSuffixRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex RangeUnitRegex { get; }

        Regex TimeUnitRegex { get; }

        Regex DateUnitRegex { get; }

        Regex AmDescRegex { get; }

        Regex PmDescRegex { get; }

        Regex AmPmDescRegex { get; }

        Regex CommonDatePrefixRegex { get; }
    }
}
