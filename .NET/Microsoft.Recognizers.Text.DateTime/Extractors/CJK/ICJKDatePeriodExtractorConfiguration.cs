using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDatePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IEnumerable<Regex> SimpleCasesRegexes { get; }

        Regex TillRegex { get; }

        Regex RangePrefixRegex { get; }

        Regex RangeSuffixRegex { get; }

        Regex FutureRegex { get; }

        Regex PastRegex { get; }

        Regex NumberCombinedWithUnit { get; }

        Regex FollowedUnit { get; }

        IDateTimeExtractor DatePointExtractor { get; }

        IExtractor IntegerExtractor { get; }
    }
}