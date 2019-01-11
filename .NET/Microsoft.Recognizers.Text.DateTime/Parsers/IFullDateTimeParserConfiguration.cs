using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IFullDateTimeParserConfiguration : IOptionsConfiguration
    {
        int TwoNumYear { get; }

        string LastWeekDayToken { get; }

        string NextMonthToken { get; }

        string LastMonthToken { get; }

        string DatePrefix { get; }

        IEnumerable<Regex> DateRegexList { get; }

        Regex NextRegex { get; }

        Regex ThisRegex { get; }

        Regex LastRegex { get; }

        Regex StrictWeekDayRegex { get; }

        Regex WeekDayOfMonthRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex UntilRegex { get; }

        Regex SincePrefixRegex { get; }

        Regex SinceSuffixRegex { get; }

        ImmutableDictionary<string, string> UnitMap { get; }

        ImmutableDictionary<string, long> UnitValueMap { get; }

        ImmutableDictionary<string, string> SeasonMap { get; }

        ImmutableDictionary<string, int> SeasonValueMap { get; }

        ImmutableDictionary<string, int> CardinalMap { get; }

        ImmutableDictionary<string, int> DayOfMonth { get; }

        ImmutableDictionary<string, int> DayOfWeek { get; }

        ImmutableDictionary<string, int> MonthOfYear { get; }

        // TODO we need to use number parser
        ImmutableDictionary<string, int> Numbers { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser DatePeriodParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DateTimePeriodParser { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeParser GetParser { get; }

        IDateTimeParser HolidayParser { get; }
    }
}