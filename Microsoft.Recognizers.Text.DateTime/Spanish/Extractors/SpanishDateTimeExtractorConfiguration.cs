using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeExtractorConfiguration : IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>(a(l)?|en|de(l)?)?(\s*(la(s)?|el|los))?$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NowRegex =
            new Regex(@"\b(?<now>(justo\s+)?ahora(\s+mismo)?|en\s+este\s+momento|tan\s+pronto\s+como\s+sea\s+posible|tan\s+pronto\s+como\s+(pueda|puedas|podamos|puedan)|lo\s+m[aá]s\s+pronto\s+posible|recientemente|previamente)\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SuffixRegex = new Regex(@"^\s*(((y|a|en|por)\s+la|al)\s+)?(mañana|madrugada|medio\s*d[ií]a|tarde|noche)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex = new Regex(@"\b(?<night>mañana|madrugada|(pasado\s+(el\s+)?)?medio\s?d[ií]a|tarde|noche|anoche)\b",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SpecificNightRegex =
            new Regex($@"\b(((((a)?\s+la|esta|siguiente|pr[oó]xim[oa]|[uú]ltim[oa])\s+)?{NightRegex}))\b",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayAfterRegex =
             new Regex($@"^\s*(,\s*)?(en|de(l)?\s+)?{SpecificNightRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayBeforeRegex =
            new Regex($@"{SpecificNightRegex}(\s*,)?(\s+(a\s+la(s)?|para))?\s*", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimpleTimeOfTodayAfterRegex =
            new Regex($@"({SpanishTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})\s*(,\s*)?((en|de(l)?)?\s+)?{SpecificNightRegex}",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimpleTimeOfTodayBeforeRegex =
            new Regex($@"{SpecificNightRegex}(\s*,)?(\s+(a\s+la|para))?\s*({SpanishTimeExtractorConfiguration.HourNumRegex}|{BaseTimeExtractor.HourRegex})",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TheEndOfRegex = new Regex(@"((a|e)l\s+)?fin(alizar|al)?(\s+(el|de(l)?)(\s+d[ií]a)?(\s+de)?)?\s*$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex UnitRegex = new Regex(@"",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDateTimeExtractorConfiguration()
        {
            DatePointExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            TimePointExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        }

        public IExtractor DatePointExtractor { get; }

        public IExtractor TimePointExtractor { get; }
        public IExtractor DurationExtractor { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.NightRegex => NightRegex;

        Regex IDateTimeExtractorConfiguration.TheEndOfRegex => TheEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        public bool IsConnector(string text)
        {
            return (string.IsNullOrEmpty(text) || text.Equals(",") ||
                        PrepositionRegex.IsMatch(text) || text.Equals("t") ||
                        text.Equals("para la") || text.Equals("para las") ||
                        text.Equals("cerca de la") || text.Equals("cerca de las"));
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
                if (text.TrimStart().ToLower().StartsWith(laterString))
                {
                    index = text.LastIndexOf(laterString) + laterString.Length - 1;
                    return true;
                }
            }
            return false;
        }
    }
}