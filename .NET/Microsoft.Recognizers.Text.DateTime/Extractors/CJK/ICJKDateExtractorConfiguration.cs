using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        IEnumerable<Regex> DateRegexList { get; }

        IEnumerable<Regex> ImplicitDateList { get; }

        Regex DateTimePeriodUnitRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        IDateTimeExtractor DurationExtractor { get; }

        Dictionary<char, char> NormalizeCharMap { get; }
    }
}