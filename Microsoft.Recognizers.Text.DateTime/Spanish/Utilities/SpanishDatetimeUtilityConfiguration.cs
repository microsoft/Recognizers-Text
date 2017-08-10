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

        List<string> IDateTimeUtilityConfiguration.AgoStringList => AgoStringList;

        List<string> IDateTimeUtilityConfiguration.LaterStringList => LaterStringList;

        List<string> IDateTimeUtilityConfiguration.InStringList => InStringList;
    }
}
