using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateParserConfiguration
    {
        string DateTokenPrefix { get; }

        #region internalParsers

        IExtractor IntegerExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        #endregion

        #region Regexes

        IEnumerable<Regex> DateRegexes { get; }
        Regex OnRegex { get; }
        Regex SpecialDayRegex { get; }
        Regex NextRegex { get; }
        Regex ThisRegex { get; }
        Regex LastRegex { get; }
        Regex UnitRegex { get; }
        Regex WeekDayRegex { get; }
        Regex MonthRegex { get; }
        Regex WeekDayOfMonthRegex { get; }
        Regex ForTheRegex { get; }
        Regex WeekDayAndDayOfMothRegex { get; }
        Regex RelativeMonthRegex { get; }

        #endregion

        #region Dictionaries
        IImmutableDictionary<string, string> UnitMap { get; }
        IImmutableDictionary<string, int> DayOfMonth { get; }
        IImmutableDictionary<string, int> DayOfWeek { get; }
        IImmutableDictionary<string, int> MonthOfYear { get; }
        IImmutableDictionary<string, int> CardinalMap { get; }

        #endregion

        int GetSwiftDay(string text);

        int GetSwiftMonth(string text);

        bool IsCardinalLast(string text);

        IDateTimeUtilityConfiguration UtilityConfiguration { get; }
    }
}
