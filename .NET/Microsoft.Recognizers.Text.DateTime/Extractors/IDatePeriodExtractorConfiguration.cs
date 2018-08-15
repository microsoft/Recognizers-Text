using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDatePeriodExtractorConfiguration : IOptionsConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegexes { get; }

        Regex YearRegex { get; }

        Regex TillRegex { get; }

        Regex DateUnitRegex { get; }

        Regex TimeUnitRegex { get; }

        Regex FollowedDateUnit { get; }

        Regex NumberCombinedWithDateUnit { get; }

        Regex PastRegex { get; }

        Regex FutureRegex { get; }

        Regex FutureSuffixRegex { get; }

        Regex WeekOfRegex { get; }

        Regex MonthOfRegex { get; }

        Regex RangeUnitRegex { get; }

        Regex InConnectorRegex { get; }

        Regex WithinNextPrefixRegex { get; }

        Regex YearPeriodRegex { get; }

        Regex RelativeDecadeRegex { get; }

        Regex ComplexDatePeriodRegex { get; }

        Regex ReferenceDatePeriodRegex { get; }

        Regex AgoRegex { get; }

        Regex LaterRegex { get; }

        Regex LessThanRegex { get; }

        Regex MoreThanRegex { get; }

        Regex CenturySuffixRegex { get; }

        IDateTimeExtractor DatePointExtractor { get; }

        IExtractor CardinalExtractor { get; }

        IExtractor OrdinalExtractor { get; }

        IDateTimeExtractor DurationExtractor { get; }

        IParser NumberParser { get; }

        bool GetFromTokenIndex(string text, out int index);

        bool HasConnectorToken(string text);

        bool GetBetweenTokenIndex(string text, out int index);

        string[] DurationDateRestrictions { get; }
    }
}