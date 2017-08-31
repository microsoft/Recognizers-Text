using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeExtractorConfiguration : IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>([aà]?|en|de?)?(\s*(la(s)?|el|los))?$)", // à - time at which, en - length of time, dans - amount of time
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NowRegex =
            new Regex(@"\b(?<now>(au\s+)?maintenant|d[eé]s\s+que\s+possible|plus\s+vite|r[eé]cemment|pr[eé]c[eé]demment)\b", // right now, as soon as possible, recently, previously
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SuffixRegex = new Regex(@"^\s*(dans\s+l['ae]\s+)?(soir[eé]e|matinee|matin|apr[èe]s\s+midi|nuit)\b", // in the evening, afternoon, morning, night
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: modify it according to the corresponding English regex
        public static readonly Regex TimeOfDayRegex = new Regex(@"\b(?<timeOfDay>((((dans\s+(l[ea])?\s+)?((?<early>(t[oô]t||d[ée]but)(\s+|-))|(?<late>((fin\s+de)|(tard))((\s+|-)))?(matin|matin[ée]e|apr[èe]s-midi)|\s+(l[ea])?+nuit|soir[ée]e)))|(((dans\s+(l[ea])?\s+)?)(journ[ée]e)))s?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex($@"\b(((((a)?\s+l[ea]|ce|ceci|dernier)\s+)?{TimeOfDayRegex}))\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayAfterRegex =
             new Regex($@"^\s*(,\s*)?(en|l[ea]?\s+)?{SpecificTimeOfDayRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayBeforeRegex =
            new Regex($@"{SpecificTimeOfDayRegex}(\s*,)?(\s+([aà]\s+la(s)?|pour))?\s*", RegexOptions.IgnoreCase | RegexOptions.Singleline); 

        public static readonly Regex SimpleTimeOfTodayAfterRegex =
            new Regex($@"({FrenchTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})\s*(,\s*)?((en|de(l)?)?\s+)?{SpecificTimeOfDayRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimpleTimeOfTodayBeforeRegex =
            new Regex($@"{SpecificTimeOfDayRegex}(\s*,)?(\s+(a\s+la|para))?\s*({FrenchTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TheEndOfRegex = new Regex(@"((a|e)l\s+)?fin(alizar|al)?(\s+(el|de(l)?)(\s+d[ií]a)?(\s+de)?)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>heures|heure|hrs|hr|secondes|seconde|minutes|minute|mins)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public FrenchDateTimeExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            TimePointExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor TimePointExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex IDateTimeExtractorConfiguration.TheEndOfRegex => TheEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        public IExtractor DurationExtractor { get; }

        public bool IsConnector(string text)
        {
            return (string.IsNullOrEmpty(text) || text.Equals(",") ||
                        PrepositionRegex.IsMatch(text) || text.Equals("t") || text.Equals("pour") ||
                        text.Equals("around"));
        }
    }
}
