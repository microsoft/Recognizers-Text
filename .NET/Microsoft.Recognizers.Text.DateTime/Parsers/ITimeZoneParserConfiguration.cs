using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ITimeZoneParserConfiguration : IDateTimeOptionsConfiguration
    {
        Dictionary<string, int> AbbrToMinMapping { get; }

        Dictionary<string, int> FullToMinMapping { get; }

        Regex DirectUtcRegex { get; }

        string TimeZoneEndRegex { get; }
    }
}
