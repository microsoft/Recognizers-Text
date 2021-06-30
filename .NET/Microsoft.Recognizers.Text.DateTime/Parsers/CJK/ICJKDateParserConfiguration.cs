using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateParserConfiguration : IDateTimeOptionsConfiguration
    {
        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeExtractor DateExtractor { get; }

        IEnumerable<Regex> DateRegexList { get; }

        Regex SpecialDate { get; }

        Regex NextRe { get; }

        Regex LastRe { get; }

        Regex SpecialDayRegex { get; }

        Regex StrictWeekDayRegex { get; }

        Regex LunarRegex { get; }

        Regex UnitRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        Regex DynastyYearRegex { get; }

        ImmutableDictionary<string, int> DynastyYearMap { get; }

        string DynastyStartYear { get; }

        Regex NextRegex { get; }

        Regex ThisRegex { get; }

        Regex LastRegex { get; }

        Regex WeekDayOfMonthRegex { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        IImmutableDictionary<string, int> DayOfMonth { get; }

        IImmutableDictionary<string, int> DayOfWeek { get; }

        IImmutableDictionary<string, int> MonthOfYear { get; }

        IImmutableDictionary<string, int> CardinalMap { get; }

        string LastWeekDayToken { get; }

        string NextMonthToken { get; }

        string LastMonthToken { get; }

        List<int> MonthMaxDays { get; }

        int GetSwiftDay(string text);
    }
}
