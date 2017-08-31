using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimeExtractorConfiguration : ITimeExtractorConfiguration
    {
        // part 1: smallest component
        // --------------------------------

        public static readonly Regex DescRegex = new Regex(@"(?<desc>pm\b|am\b|p\.m\.|a\.m\.)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex HourNumRegex =
            new Regex(@"\b(?<hournum>zero|un|deus|trois|quatre|cinq|six|sept|huit|nuef|dix|onze|douze)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MinuteNumRegex =
            new Regex(
                @"(?<minnum>un|deus|trois|quatre|cinq|six|sept|huit|neuf|dix|onze|douze|treize|quatorze|quinze|seize|dix-sept|dix-huit|dix-neuf|vingt|trente|quarante|cinquante)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 2: middle level component
        // --------------------------------------
        // handle "... heures (o'clock, en punto)"

        public static readonly Regex OclockRegex = new Regex(@"(?<oclock>heures)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... après midi (afternoon, tarde)"
        public static readonly Regex PmRegex =
            new Regex(@"(?<pm>((por|de|a|en)\s+la)\s+(tarde|noche))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "... dans la matinee (in the morning)"
        public static readonly Regex AmRegex = new Regex(@"(?<am>((du|dans)\s+l[ea])\s+(matin|matin[ée]e))",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle "half past ..." "a quarter to ..."
        // rename 'min' group to 'deltamin'
        public static readonly Regex LessThanOneHour =
            new Regex(
                $@"(?<lth>((\s+et\s+)?quart|(\s*)quart de|(\s+et\s+)demi|{
                    BaseTimeExtractor.MinuteRegex.ToString().Replace("min", "deltamin")}(\s+(minute|minutes|min|mins))|{
                    MinuteNumRegex.ToString().Replace("minnum", "deltaminnum")}(\s+(minute|minutes|min|mins))))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TensTimeRegex =
            new Regex("(?<tens>dix|vingt|trente|quarante|cinquante)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // TODO - will have change below

        // handle "six heures et demie" (six thirty), "six heures et vingt-et-un" (six twenty one) 
        public static readonly Regex EngTimeRegex =
            new Regex(
                $@"(?<engtime>{HourNumRegex}\s*((heures)\s+)?({MinuteNumRegex}|({TensTimeRegex}((\s*et\s+)?{MinuteNumRegex})?)))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimePrefix =
            new Regex(
                $@"(?<prefix>{LessThanOneHour}(\s+(pass[ée])\s+(de\s+las|las)?|\s+(para|antes\s+de)?\s+(las?))?)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeSuffix =
            new Regex($@"(?<suffix>({LessThanOneHour}\s+)?({AmRegex}|{PmRegex}|{OclockRegex}))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex BasicTime =
            new Regex(
                $@"(?<basictime>{EngTimeRegex}|{HourNumRegex}|{BaseTimeExtractor.HourRegex}:{BaseTimeExtractor.MinuteRegex}(:{BaseTimeExtractor.SecondRegex})?|{BaseTimeExtractor.HourRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // handle special time such as 'at midnight', 'midnight', 'midday'
        // midnight - le minuit, la zero heure
        // midday - midi 

        public static readonly Regex MidnightRegex =
            new Regex($@"minuit|(z[ée]ro-heure)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex MidafternoonRegex =
            new Regex($@"",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // part 3: regex for time
        // --------------------------------------
        // handle "at four" "at 3"
        public static readonly Regex AtRegex =
            new Regex(string.Format(@"\b(?<=\b([àa]?)\s+)({2}|{0}|{1})\b", HourNumRegex, BaseTimeExtractor.HourRegex, EngTimeRegex),
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex ConnectNumRegex =
            new Regex(
                $@"{BaseTimeExtractor.HourRegex
                    }(?<min>00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59)\s*{
                    DescRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex[] TimeRegexList =
        {
            // (three min past)? seven|7|(seven thirty) pm
            new Regex(
                $@"(\b{TimePrefix}\s+)?({EngTimeRegex}|{HourNumRegex}|{BaseTimeExtractor.HourRegex})\s*({DescRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3:00(:00)? (pm)?
            new Regex(
                $@"(\b{TimePrefix}\s+)?(T)?{BaseTimeExtractor.HourRegex}(\s*)?:(\s*)?{BaseTimeExtractor.MinuteRegex}((\s*)?:(\s*)?{BaseTimeExtractor.SecondRegex})?((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past)? 3.00 (pm)?
            new Regex($@"(\b{TimePrefix}\s+)?{BaseTimeExtractor.HourRegex}\.{BaseTimeExtractor.MinuteRegex}(\s*{DescRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex($@"\b(({DescRegex}?)|({BasicTime}?)({DescRegex}?))({TimePrefix}\s*)({HourNumRegex}|{BaseTimeExtractor.HourRegex})?(\s+{TensTimeRegex}(\s+y\s+)?{MinuteNumRegex}?)?({OclockRegex})?\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (three min past) (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex($@"\b({TimePrefix}|{BasicTime}{TimePrefix})\s+(\s*{DescRegex})?{BasicTime}?\s*{TimeSuffix}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (five thirty|seven|7|7:00(:00)?) (pm)? (in the night)
            new Regex($@"{BasicTime}(\s*{DescRegex})?\s+{TimeSuffix}\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // (in the night) at (five thirty|seven|7|7:00(:00)?) (pm)?
            new Regex($@"\b{TimeSuffix}\s+a\s+las\s+{BasicTime}((\s*{DescRegex})|\b)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            new Regex($@"(([àa])|(dans\s)+l['ae])\s+(apr[eè]s\s+midi|matin[eé]e|midi|tard|nuit)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline),

            // 340pm
            ConnectNumRegex
        };
        Regex ITimeExtractorConfiguration.IshRegex => null;
        IEnumerable<Regex> ITimeExtractorConfiguration.TimeRegexList => TimeRegexList;
        Regex ITimeExtractorConfiguration.AtRegex => AtRegex;

        public IExtractor DurationExtractor { get; }
        public FrenchTimeExtractorConfiguration()
        {
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        }
    }
}
