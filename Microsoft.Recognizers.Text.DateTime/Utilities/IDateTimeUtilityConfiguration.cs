using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public interface IDateTimeUtilityConfiguration
    {
        Regex AgoRegex { get; }

        Regex LaterRegex { get; }

        Regex InConnectorRegex { get; }

        Regex AmDescRegex { get; }

        Regex PmDescRegex { get; }

        Regex AmPmDescRegex { get; }
    }
}
