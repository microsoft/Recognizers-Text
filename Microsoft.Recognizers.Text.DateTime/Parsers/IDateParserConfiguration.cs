using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateParserConfiguration
    {
        string DateTokenPrefix { get; }

        #region internalParsers

        IExtractor IntegerExtractor { get; }
        IExtractor OrdinalExtractor { get; }
        IParser NumberParser { get; }

        #endregion

        #region Regexes

        IEnumerable<Regex> DateRegexes { get; }
        Regex OnRegex { get; }
        Regex SpecialDayRegex { get; }
        Regex NextRegex { get; }
        Regex ThisRegex { get; }
        Regex LastRegex { get; }
        Regex StrictWeekDay { get; }
        Regex MonthRegex { get; }
        Regex WeekDayOfMonthRegex { get; }

        #endregion

        #region Dictionaries

        IImmutableDictionary<string, int> DayOfMonth { get; }
        IImmutableDictionary<string, int> DayOfWeek { get; }
        IImmutableDictionary<string, int> MonthOfYear { get; }
        IImmutableDictionary<string, int> CardinalMap { get; }

        #endregion

        int GetSwiftDay(string text);
        int GetSwiftMonth(string text);
        bool IsCardinalLast(string text);
    }
}
