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

        //TODO: add the word prefix for "am", "pm", ampm is special
        public static readonly string AmPrefix = "a";
        public static readonly string PmPrefix = "p";
        public static readonly string AmPmPrefix = "ampm";

        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;

        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;

        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;

        string IDateTimeUtilityConfiguration.AmPrefix => AmPrefix;

        string IDateTimeUtilityConfiguration.PmPrefix => PmPrefix;

        string IDateTimeUtilityConfiguration.AmPmPrefix => AmPmPrefix;

    }
}
