using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English
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

        public static readonly Regex UnitRegex = new Regex(@"(?<unit>hours|hour|hrs|seconds|second|minutes|minute|mins)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public EnglishDateTimeExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            TimePointExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
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

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        public IExtractor DurationExtractor { get; }

        public bool IsConnector(string text)
        {
            return (string.IsNullOrEmpty(text) || text.Equals(",") ||
                        PrepositionRegex.IsMatch(text) || text.Equals("t") || text.Equals("for") ||
                        text.Equals("around"));
        }

        public bool GetAgoIndex(string text, out int index)
        {
            index = -1;
            List<string> agoStringList = new List<string>
            {
                "ago",
            };
            foreach (var agoString in agoStringList)
            {
                if (text.TrimStart().StartsWith(agoString))
                {
                    index = text.LastIndexOf(agoString) + agoString.Length;
                    return true;
                }
            }
            return false;
        }

        public bool GetLaterIndex(string text, out int index)
        {
            index = -1;
            List<string> laterStringList = new List<string>
            {
                "later",
                "from now"
            };
            foreach (var laterString in laterStringList)
            {
                if (text.TrimStart().ToLower().StartsWith(laterString))
                {
                    index = text.LastIndexOf(laterString) + laterString.Length;
                    return true;
                }
            }
            return false;
        }

        public bool GetInIndex(string text, out int index)
        {
            index = -1;
            //add space to make sure it is a token
            List<string> laterStringList = new List<string>
            {
                " in",
            };
            foreach (var laterString in laterStringList)
            {
                if (text.TrimEnd().ToLower().EndsWith(laterString))
                {
                    index = text.Length - text.LastIndexOf(laterString) - 1;
                    return true;
                }
            }
            return false;
        }
    }
}