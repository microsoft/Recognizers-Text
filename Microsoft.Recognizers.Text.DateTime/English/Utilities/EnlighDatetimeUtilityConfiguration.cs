using System.Collections.Generic;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.English.Utilities
{
    public class EnlighDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        public static readonly List<string> AgoStringList = new List<string>
        {
            "ago",
        };

        public static readonly List<string> LaterStringList = new List<string>
        {
            "later",
            "from now"
        };

        public static readonly List<string> InStringList = new List<string>
        {
            "in",
        };

        public static readonly List<string> AmStringList = new List<string>
        {
            "am",
            "a.m.",
            "a",
        };

        public static readonly List<string> PmStringList = new List<string>
        {
            "pm",
            "p.m.",
            "p",
        };

        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;

        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;

        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;

        List<string> IDateTimeUtilityConfiguration.AmStringList => AmStringList;

        List<string> IDateTimeUtilityConfiguration.PmStringList => PmStringList;
    }
}
