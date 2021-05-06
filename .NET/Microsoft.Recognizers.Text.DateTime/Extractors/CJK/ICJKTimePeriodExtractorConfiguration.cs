using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKTimePeriodExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        ImmutableDictionary<Regex, PeriodType> Regexes { get; }
    }
}