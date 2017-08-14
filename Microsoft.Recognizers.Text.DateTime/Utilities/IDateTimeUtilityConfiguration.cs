using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public interface IDateTimeUtilityConfiguration
    {
        List<string> AgoStringList { get; }

        List<string> LaterStringList { get; }

        List<string> InStringList { get; }

        List<string> PmStringList { get; }

        List<string> AmStringList { get; }
    }
}
