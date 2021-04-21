using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKTimeExtractorConfiguration : IDateTimeOptionsConfiguration
    {
        ImmutableDictionary<Regex, TimeType> Regexes { get; }
    }
}