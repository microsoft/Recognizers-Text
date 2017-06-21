using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateExtractorConfiguration
    {
        IEnumerable<Regex> DateRegexList { get; }
        IEnumerable<Regex> ImplicitDateList { get; }
        Regex OfMonth { get; }
        Regex MonthEnd { get; }
        Regex NonDateUnitRegex { get; }
        IExtractor IntegerExtractor { get; }
        IExtractor OrdinalExtractor { get; }
        IParser NumberParser { get; }
        IExtractor DurationExtractor { get; }

        bool GetAgoIndex(string text, out int index);
        bool GetLaterIndex(string text, out int index);
        bool GetInIndex(string text, out int index);
    }
}