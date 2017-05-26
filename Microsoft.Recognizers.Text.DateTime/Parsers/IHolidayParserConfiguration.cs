using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public interface IHolidayParserConfiguration
    {
        IImmutableDictionary<string, string> VariableHolidaysTimexDictionary { get; }
        IImmutableDictionary<string, Func<int, DateObject>> HolidayFuncDictionary { get; }

        IImmutableDictionary<string, IEnumerable<string>> HolidayNames { get; }
        
        IEnumerable<Regex> HolidayRegexList { get; }

        int GetSwiftYear(string text);

        string SanitizeHolidayToken(string holiday);
    }
}