using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class TimeParserChs : IDateTimeParser
    {
        public static readonly IExtractor TimeExtractor = new TimeExtractorChs();

        private delegate TimeResult TimeFunction(DateTimeExtra<TimeType> extra);

        private static readonly Dictionary<TimeType, TimeFunction> FunctionMap =
            new Dictionary<TimeType, TimeFunction>
            {
                {TimeType.DigitTime, TimeFunctions.HandleDigit},
                {TimeType.ChineseTime, TimeFunctions.HandleChinese},
                {TimeType.LessTime, TimeFunctions.HandleLess}
            };


        private readonly IFullDateTimeParserConfiguration config;

        public TimeParserChs(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }
        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceTime = refDate;
            var extra = er.Data as DateTimeExtra<TimeType>;
            if (extra == null)
            {
                var result = TimeExtractor.Extract(er.Text);
                extra = result[0]?.Data as DateTimeExtra<TimeType>;
            }
            if (extra != null)
            {
                var timeResult = FunctionMap[extra.Type](extra);
                var parseResult = TimeFunctions.PackTimeResult(extra, timeResult, referenceTime);
                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, Util.FormatTime((DateObject) parseResult.FutureValue)}
                    };
                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, Util.FormatTime((DateObject) parseResult.PastValue)}
                    };
                }
                var ret = new DateTimeParseResult
                {
                    Start = er.Start,
                    Text = er.Text,
                    Type = er.Type,
                    Length = er.Length,
                    Value = parseResult,
                    Data = timeResult,
                    ResolutionStr = "",
                    TimexStr = parseResult.Timex
                };
                return ret;
            }
            return null;
        }
    }

    public static class TimeFunctions
    {
        public static readonly Dictionary<char, int> NumberDictionary = new Dictionary<char, int>
        {
            {'零', 0},
            {'一', 1},
            {'二', 2},
            {'三', 3},
            {'四', 4},
            {'五', 5},
            {'六', 6},
            {'七', 7},
            {'八', 8},
            {'九', 9},
            {'〇', 0},
            {'两', 2},
            {'十', 10}
        };

        public static readonly Dictionary<string, int> LowBoundDesc = new Dictionary<string, int>
        {
            {"中午", 11},
            {"下午", 12},
            {"午后", 12},
            {"晚上", 18},
            {"夜里", 18},
            {"夜晚", 18},
            {"夜间", 18},
            {"深夜", 18},
            {"傍晚", 18},
            {"晚", 18}
        };

        public static TimeResult HandleLess(DateTimeExtra<TimeType> extra)
        {
            var hour = MatchToValue(extra.NamedEntity["hour"].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter*15 : 0;
            var second = MatchToValue(extra.NamedEntity["sec"].Value);
            var less = MatchToValue(extra.NamedEntity["min"].Value);
            var all = hour*60 + minute - less;
            if (all < 0)
            {
                all += 1440;
            }
            return new TimeResult
            {
                Hour = all/60,
                Minute = all%60,
                Second = second
            };
        }

        public static TimeResult HandleChinese(DateTimeExtra<TimeType> extra)
        {
            var hour = MatchToValue(extra.NamedEntity["hour"].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = MatchToValue(extra.NamedEntity["min"].Value);
            var second = MatchToValue(extra.NamedEntity["sec"].Value);
            minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter*15 : minute;
            return new TimeResult
            {
                Hour = hour,
                Minute = minute,
                Second = second
            };
        }

        public static TimeResult HandleDigit(DateTimeExtra<TimeType> extra)
        {
            var timeResult = new TimeResult
            {
                Hour = MatchToValue(extra.NamedEntity["hour"].Value),
                Minute = MatchToValue(extra.NamedEntity["min"].Value),
                Second = MatchToValue(extra.NamedEntity["sec"].Value)
            };
            return timeResult;
        }

        public static DTParseResult PackTimeResult(DateTimeExtra<TimeType> extra, TimeResult timeResult,
            DateObject referenceTime)
        {
            //Find if there is a description
            var noDesc = true;
            var dayDesc = extra.NamedEntity["daydesc"]?.Value;
            if (!string.IsNullOrEmpty(dayDesc))
            {
                AddDesc(timeResult, dayDesc);
                noDesc = false;
            }

            int hour = timeResult.Hour > 0 ? timeResult.Hour : 0,
                min = timeResult.Minute > 0 ? timeResult.Minute : 0,
                second = timeResult.Second > 0 ? timeResult.Second : 0,
                day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;

            var dtResult = new DTParseResult();


            var build = new StringBuilder("T");
            if (timeResult.Hour >= 0)
            {
                build.Append(timeResult.Hour.ToString("D2"));
            }
            if (timeResult.Minute >= 0)
            {
                build.Append(":" + timeResult.Minute.ToString("D2"));
            }
            if (timeResult.Second >= 0)
            {
                build.Append(":" + timeResult.Second.ToString("D2"));
            }
            if (noDesc)
            {
                //build.Append("ampm");
                dtResult.comment = "ampm";
            }
            dtResult.Timex = build.ToString();
            if (hour == 24)
            {
                hour = 0;
            }
            dtResult.FutureValue = dtResult.PastValue = new DateObject(year, month, day, hour, min, second);
            dtResult.Success = true;
            return dtResult;
        }

        public static int MatchToValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }
            if (Regex.IsMatch(text, @"\d+"))
            {
                return Int32.Parse(text);
            }
            if (text.Length == 1)
            {
                return NumberDictionary[text[0]];
            }
            //五十九,十一,二
            var tempValue = 1;
            foreach (var c in text)
            {
                if (c.Equals('十'))
                {
                    tempValue *= 10;
                }
                else if (c.Equals(text.First()))
                {
                    tempValue *= NumberDictionary[c];
                }
                else
                {
                    tempValue += NumberDictionary[c];
                }
            }
            return tempValue;
        }

        public static void AddDesc(TimeResult result, string dayDesc)
        {
            if (string.IsNullOrEmpty(dayDesc))
            {
                return;
            }
            if (LowBoundDesc.ContainsKey(dayDesc) && result.Hour < LowBoundDesc[dayDesc])
            {
                result.Hour += 12;
                result.LowBound = LowBoundDesc[dayDesc];
            }
            else
            {
                result.LowBound = 0;
            }
        }

        public static TimeResult GetShortLeft(string text)
        {
            string des = null;
            if (Regex.IsMatch(text, TimeExtractorChs.DayDescRegex))
            {
                des = text.Substring(0, text.Length - 1);
            }
            var hour = MatchToValue(text.Substring(text.Length - 1, 1));
            var timeResult = new TimeResult
            {
                Hour = hour,
                Minute = -1,
                Second = -1
            };
            AddDesc(timeResult, des);
            return timeResult;
        }
    }

    public class TimeResult
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int LowBound { get; set; } = -1;
    }
}