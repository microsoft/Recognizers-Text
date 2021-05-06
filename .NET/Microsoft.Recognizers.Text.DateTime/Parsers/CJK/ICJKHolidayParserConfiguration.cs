using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKHolidayParserConfiguration : IDateTimeOptionsConfiguration
    {
        IExtractor IntegerExtractor { get; }

        IParser NumberParser { get; }

        Dictionary<string, Func<int, DateObject>> FixedHolidaysDict { get; }

        Dictionary<string, Func<int, DateObject>> HolidayFuncDict { get; }

        Dictionary<string, string> NoFixedTimex { get; }

        IEnumerable<Regex> HolidayRegexList { get; }

        Regex LunarHolidayRegex { get; }

        int GetSwiftYear(string text);

        string SanitizeYearToken(string holiday);
    }
}