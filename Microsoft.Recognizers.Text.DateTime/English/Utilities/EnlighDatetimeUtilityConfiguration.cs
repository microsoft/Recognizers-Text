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

        public static readonly string AmPrefix = "a";
        public static readonly string PmPrefix = "p";
        public static readonly string AmPmPrefix = "ampm";


        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;

        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;

        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;

        string IDateTimeUtilityConfiguration.AmPrefix=> AmPrefix;

        string IDateTimeUtilityConfiguration.PmPrefix => PmPrefix;

        string IDateTimeUtilityConfiguration.AmPmPrefix => AmPmPrefix;
    }
}
