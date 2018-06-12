using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeAltParserConfiguration
    {
        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimePeriodParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DatePeriodParser { get; }
    }
}
