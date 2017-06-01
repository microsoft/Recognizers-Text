using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Extractors
{
    public interface IDateExtractorConfiguration
    {
        IEnumerable<Regex> DateRegexList { get; }
        IEnumerable<Regex> ImplicitDateList { get; }
        Regex OfMonth { get; }
        Regex MonthEnd { get; }
        IExtractor IntegerExtractor { get; }
        IExtractor OrdinalExtractor { get; }
        IParser NumberParser { get; }
    }
}