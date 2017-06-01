using Microsoft.Recognizers.Text.DateTime.Extractors;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.English.Extractors
{
    public class EnglishTimeExtractorConfiguration : ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------------

        public static readonly Regex DescRegex = new Regex(@"(?<desc>pm\b|am\b|p\.m\.|a\.m\.|p\b|a\b)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(@"(?<hournum>zero|one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(
                @"(?<minnum>one|two|three|four|five|six|seven|eight|nine|ten|eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|nineteen|twenty|thirty|forty|fifty)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... o'clock"
        public static readonly Regex OclockRegex = new Regex(@"(?<oclock>o’clock|o'clock|oclock)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... afternoon"
        public static readonly Regex PmRegex =
            new Regex(@"(?<pm>(in the\s+)?afternoon|(in the\s+)?evening|(in the\s+)?night)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... in the morning"
        public static readonly Regex AmRegex = new Regex(@"(?<am>(in the\s+)?morning)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(
                $@"(?<lth>(a\s+)?quarter|three quarter(s)?|half( an hour)?|{
                    BaseTimeExtractor.MinuteRegex.ToString().Replace("min", "deltamin")
                    }(\s+(minute|minutes|min|mins))|{MinuteNumRegex.ToString().Replace("minnum", "deltaminnum")
                    }(\s+(minute|minutes|min|mins)))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "six thirty", "six twenty one" 
        public static readonly Regex EngTimeRegex =
            new Regex(
                string.Format(@"(?<engtime>{0}\s+({1}|(?<tens>twenty|thirty|forty|fourty|fifty)\s+{1}))", HourNumRegex,
                    MinuteNumRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public static readonly Regex TimePrefix =
            new Regex(string.Format(@"(?<prefix>({0} past|{0} to))", LessThanOneHour),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex($@"(?<suffix>{AmRegex}|{PmRegex}|{OclockRegex})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(
                string.Format(@"(?<basictime>{4}|{0}|{1}:{2}(:{3})?|{1})", HourNumRegex, BaseTimeExtractor.HourRegex, BaseTimeExtractor.MinuteRegex,
                    BaseTimeExtractor.SecondRegex, EngTimeRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline);


        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(string.Format(@"\b(?<=\bat\s+)({2}|{0}|{1})\b", HourNumRegex, BaseTimeExtractor.HourRegex, EngTimeRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex IshRegex = new Regex($@"{BaseTimeExtractor.HourRegex}(-|——)?ish|noonish|noon",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(
                $@"{BaseTimeExtractor.HourRegex
                    }(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{
                    DescRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(senven thirty) pm
            new Regex(
                string.Format(@"(\b{2}\s+)?({4}|{3}|{0})\s*{1}", BaseTimeExtractor.HourRegex, DescRegex, TimePrefix, HourNumRegex,
                    EngTimeRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(
                string.Format(@"(\b{3}\s+)?(T)?{0}(\s*)?:(\s*)?{1}((\s*)?:(\s*)?{4})?((\s*{2})|\b)", BaseTimeExtractor.HourRegex,
                    BaseTimeExtractor.MinuteRegex,
                    DescRegex, TimePrefix, BaseTimeExtractor.SecondRegex), RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)?
            new Regex(string.Format(@"(\b{3}\s+)?{0}\.{1}(\s*{2})", BaseTimeExtractor.HourRegex, BaseTimeExtractor.MinuteRegex, DescRegex, TimePrefix),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(string.Format(@"\b{0}\s+{1}(\s*{3})?\s+{2}\b", TimePrefix, BasicTime, TimeSuffix, DescRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimePrefix}\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex(string.Format(@"{0}(\s*{1})?\s+{2}\b", BasicTime, DescRegex, TimeSuffix),
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimeSuffix}\s+at\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimeSuffix}\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };

        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;

        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        Regex ITimeExtractorConfiguration.IshRegex => IshRegex;
    }
}