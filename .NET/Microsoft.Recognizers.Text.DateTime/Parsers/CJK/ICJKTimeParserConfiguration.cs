using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public delegate TimeResult TimeFunction(DateTimeExtra<TimeType> extra);

    public interface ICJKTimeParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor TimeExtractor { get; }

        TimeFunctions TimeFunc { get; }

        Dictionary<TimeType, TimeFunction> FunctionMap { get; }
    }
}
