using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Utilities
{
    public class SpanishDatetimeUtilityConfiguration : IDateTimeUtilityConfiguration
    {
        //TODO: change the regexes to Spanish
        public static readonly Regex AgoRegex = new Regex(@"\b(ago)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex LaterRegex = new Regex(@"\b(later|from now)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex InConnectorRegex = new Regex(@"\b(in)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmDescRegex = new Regex(@"(am\b|a\.m\.|a m\b|a\. m\.\b|a\.m\b|a\. m\b)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmDescRegex = new Regex(@"(pm\b|p\.m\.|p\b|p m\b|p\. m\.\b|p\.m\b|p\. m\b)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmPmDescRegex = new Regex(@"(ampm)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimeUtilityConfiguration.LaterRegex => LaterRegex;

        Regex IDateTimeUtilityConfiguration.AgoRegex => AgoRegex;

        Regex IDateTimeUtilityConfiguration.InConnectorRegex => InConnectorRegex;

        Regex IDateTimeUtilityConfiguration.AmDescRegex => AmDescRegex;

        Regex IDateTimeUtilityConfiguration.PmDescRegex => PmDescRegex;

        Regex IDateTimeUtilityConfiguration.AmPmDescRegex => AmPmDescRegex;

    }
}
