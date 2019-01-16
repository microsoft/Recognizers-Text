using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class TimeFunctions
    {
        public static readonly Dictionary<char, int> NumberDictionary = DateTimeDefinitions.TimeNumberDictionary;

        public static readonly Dictionary<string, int> LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc;

        public static TimeResult HandleLess(DateTimeExtra<TimeType> extra)
        {
            var hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter * 15 : 0;
            var second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value);
            var less = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value);

            var all = hour * 60 + minute - less;
            if (all < 0)
            {
                all += 1440;
            }

            return new TimeResult
            {
                Hour = all / 60,
                Minute = all % 60,
                Second = second
            };
        }

        public static TimeResult HandleChinese(DateTimeExtra<TimeType> extra)
        {
            //Dictionary<string, int> LowBoundDesc = Microsoft.Recognizers.Definitions.Chinese.DateTimeDefinitions.TimeLowBoundDesc;
            var hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value);
            var second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value);
            minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter * 15 : minute;

            return new TimeResult
            {
                Hour = hour,
                Minute = minute,
                Second = second
            };
        }

        public static TimeResult HandleJapanese(DateTimeExtra<TimeType> extra)
        {
            var hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value);
            var second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value);
            minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter * 15 : minute;

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
                Hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value),
                Minute = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value),
                Second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value)
            };
            return timeResult;
        }

        public static DateTimeResolutionResult PackTimeResult(DateTimeExtra<TimeType> extra, TimeResult timeResult, DateObject referenceTime)
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

            var dtResult = new DateTimeResolutionResult();

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
                dtResult.Comment = Constants.Comment_AmPm;
            }

            dtResult.Timex = build.ToString();
            if (hour == 24)
            {
                hour = 0;
            }

            dtResult.FutureValue = dtResult.PastValue = DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);
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

            dayDesc = NormalizeDayDesc(dayDesc);

            if (LowBoundDesc.ContainsKey(dayDesc) && result.Hour < LowBoundDesc[dayDesc])
            {
                result.Hour += Constants.HalfDayHourCount;
                result.LowBound = LowBoundDesc[dayDesc];
            }
            else
            {
                result.LowBound = 0;
            }
        }

        // Normalize cases like "p.m.", "p m" to canonical form "pm"
        private static string NormalizeDayDesc(string dayDesc)
        {
            return dayDesc.Replace(" ", string.Empty).Replace(".", string.Empty);
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
}
