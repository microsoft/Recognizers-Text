using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ISimpleDatePeriodParserConfiguration : IOptionsConfiguration
    {
        Regex YearRegex { get; }

        Regex RelativeRegex { get; }

        IDateExtractor DateExtractor { get; }
    }
}
