using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedParserConfiguration : ICommonDateTimeParserConfiguration
    {

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex SinceRegex { get; }

        Regex YearAfterRegex { get; }

        Regex YearRegex { get; }

        IDateTimeParser GetParser { get; }

        IDateTimeParser HolidayParser { get; }

        IDateTimeParser TimeZoneParser { get; }
    }
}
