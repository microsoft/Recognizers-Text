using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.English.Extractors
{
    public class EnglishDateTimeExtractorConfiguration : IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>^(at|on|of)(\s+the)?$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NowRegex =
            new Regex(@"\b(?<now>(right\s+)?now|as soon as possible|asap|recently|previously)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SuffixRegex = new Regex(@"^\s*(in the\s+)?(morning|afternoon|evening|night)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex = new Regex(@"\b(?<night>morning|afternoon|night|evening)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificNightRegex =
            new Regex($@"\b(((this|next|last)\s+{NightRegex})\b|\btonight)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayAfterRegex =
             new Regex($@"^\s*(,\s*)?(in\s+)?{SpecificNightRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayBeforeRegex =
            new Regex($@"{SpecificNightRegex}(\s*,)?(\s+(at|for))?\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimpleTimeOfTodayAfterRegex =
            new Regex($@"({EnglishTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})\s*(,\s*)?(in\s+)?{SpecificNightRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimpleTimeOfTodayBeforeRegex =
            new Regex($@"{SpecificNightRegex}(\s*,)?(\s+(at|for))?\s*({EnglishTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TheEndOfRegex = new Regex(@"(the\s+)?end of(\s+the)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public EnglishDateTimeExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            TimePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor TimePointExtractor { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.NightRegex => NightRegex;

        Regex IDateTimeExtractorConfiguration.TheEndOfRegex => TheEndOfRegex;

        public bool IsConnector(string text)
        {
            return (string.IsNullOrEmpty(text) || text.Equals(",") ||
                        PrepositionRegex.IsMatch(text) || text.Equals("t") || text.Equals("for") ||
                        text.Equals("around"));
        }
    }
}