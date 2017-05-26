using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Extractors;

namespace Microsoft.Recognizers.Text.DateTime.English.Extractors
{
    public class EnglishTimePeriodExtractorConfiguration : ITimePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex = new Regex(@"(?<till>to|till|until|thru|through|--|-|—|——)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourRegex =
            new Regex(
                @"(?<hour>00|01|02|03|04|05|06|07|08|09|0|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|1|2|3|4|5|6|7|8|9)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(
                @"(?<hour>twenty one|twenty two|twenty three|twenty four|zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nighteen|twenty)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex DescRegex = new Regex(@"(?<desc>pm|am|p\.m\.|a\.m\.|p|a)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(@"(?<pm>afternoon|evening|in the afternoon|in the evening|in the night)s?",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex AmRegex = new Regex(@"(?<am>morning|in the morning)s?",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumFromTo =
            new Regex(
                string.Format(@"(from\s+)?({0}|{1})(\s*{5})?\s*{2}\s*({0}|{1})\s*({3}|{4}|{5})?", HourRegex,
                    HourNumRegex, TillRegex, PmRegex, AmRegex, DescRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(
                string.Format(@"(between\s+)({0}|{1})(\s*{5})?\s*{2}\s*({0}|{1})\s*({3}|{4}|{5})?", HourRegex,
                    HourNumRegex, "and", PmRegex, AmRegex, DescRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>^(at|on|of)(\s+the)?$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex =
            new Regex(@"\b(?<night>daytime|morning|afternoon|(late\s+)?night|evening)s?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificNightRegex =
            new Regex($@"\b(((this|next|last)\s+{NightRegex})\b|\btonight)s?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex =
            new Regex(@"(?<unit>hours|hour|hrs|hr|h|minutes|minute|mins|min|seconds|second|secs|sec)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FollowedUnit = new Regex($@"^\s*{UnitRegex}\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?){UnitRegex}\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex PastRegex = new Regex(@"(?<past>\b(past|last|previous)\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex FutureRegex = new Regex(@"(?<past>\b(next|in)\b)");

        public EnglishTimePeriodExtractorConfiguration()
        {
            SingleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
        }

        public IExtractor SingleTimeExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[]{ PureNumFromTo, PureNumBetweenAnd };

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.NightRegex => NightRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from");
                return true;
            }
            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between");
                return true;
            }
            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }
    }
}