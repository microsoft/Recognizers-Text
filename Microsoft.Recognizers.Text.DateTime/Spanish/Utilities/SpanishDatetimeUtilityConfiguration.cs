using System.Collections.Generic;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Utilities
{
    public class SpanishDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        //TODO: add the word list for ago later and in
        public static readonly List<string> AgoStringList = new List<string>
        {
        };

        public static readonly List<string> LaterStringList = new List<string>
        {
        };

        public static readonly List<string> InStringList = new List<string>
        {
        };

        //TODO: add the word list for "am", "pm"
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
