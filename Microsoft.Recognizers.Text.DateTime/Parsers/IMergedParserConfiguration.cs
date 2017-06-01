using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public interface IMergedParserConfiguration : ICommonDateTimeParserConfiguration
    {
        Regex BeforeRegex { get; }
        Regex AfterRegex { get; }
        IDateTimeParser DatePeriodParser { get; }
        IDateTimeParser TimePeriodParser { get; }
        IDateTimeParser DateTimePeriodParser { get; }
        IDateTimeParser SetParser { get; }
        IDateTimeParser HolidayParser { get; }
    }
}
