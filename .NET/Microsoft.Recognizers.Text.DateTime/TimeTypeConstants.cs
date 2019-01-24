using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class TimeTypeConstants
    {
        public const string DATE = "date";
        public const string DATETIME = "dateTime";
        public const string DATETIMEALT = "dateTimeAlt";
        public const string DURATION = "duration";
        public const string SET = "set";
        public const string TIME = "time";

        // Internal SubType for Future/Past in DateTimeResolutionResult
        public const string START_DATE = "startDate";
        public const string END_DATE = "endDate";
        public const string START_DATETIME = "startDateTime";
        public const string END_DATETIME = "endDateTime";
        public const string START_TIME = "startTime";
        public const string END_TIME = "endTime";
    }
}
