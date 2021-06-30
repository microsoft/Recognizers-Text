using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class TimeFunctions
    {
        public Dictionary<char, int> NumberDictionary { get; set; }

        public Dictionary<string, int> LowBoundDesc { get; set; }

        public string DayDescRegex { get; set; }

        public TimeResult HandleLess(DateTimeExtra<TimeType> extra)
        {
            var hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value);
            var quarter = MatchToValue(extra.NamedEntity["quarter"].Value);
            var minute = !string.IsNullOrEmpty(extra.NamedEntity["half"].Value) ? 30 : quarter != -1 ? quarter * 15 : 0;
            var second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value);
            var less = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value);

            var all = (hour * 60) + minute - less;
            if (all < 0)
            {
                all += 1440;
            }

            return new TimeResult
            {
                Hour = all / 60,
                Minute = all % 60,
                Second = second,
            };
        }

        public TimeResult HandleKanji(DateTimeExtra<TimeType> extra)
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
                Second = second,
            };
        }

        public TimeResult HandleDigit(DateTimeExtra<TimeType> extra)
        {
            var timeResult = new TimeResult
            {
                Hour = MatchToValue(extra.NamedEntity[Constants.HourGroupName].Value),
                Minute = MatchToValue(extra.NamedEntity[Constants.MinuteGroupName].Value),
                Second = MatchToValue(extra.NamedEntity[Constants.SecondGroupName].Value),
            };
            return timeResult;
        }

        public DateTimeResolutionResult PackTimeResult(DateTimeExtra<TimeType> extra, TimeResult timeResult, DateObject referenceTime)
        {
            // Find if there is a description
            var noDesc = true;
            var dayDesc = extra.NamedEntity["daydesc"]?.Value;
            if (!string.IsNullOrEmpty(dayDesc))
            {
                AddDesc(timeResult, dayDesc);
                noDesc = false;
            }

            int hour = timeResult.Hour > 0 ? timeResult.Hour % Constants.DayHourCount : 0,
                min = timeResult.Minute > 0 ? timeResult.Minute : 0,
                second = timeResult.Second > 0 ? timeResult.Second : 0,
                day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;

            var dateTimeResult = new DateTimeResolutionResult();

            var build = new StringBuilder("T");
            if (timeResult.Hour >= 0)
            {
                build.Append(hour.ToString("D2", CultureInfo.InvariantCulture));
            }

            if (timeResult.Minute >= 0)
            {
                build.Append(":" + min.ToString("D2", CultureInfo.InvariantCulture));
            }

            if (timeResult.Second >= 0)
            {
                if (timeResult.Minute < 0)
                {
                    build.Append(":" + min.ToString("D2", CultureInfo.InvariantCulture));
                }

                build.Append(":" + second.ToString("D2", CultureInfo.InvariantCulture));
            }

            if (noDesc && hour <= Constants.HalfDayHourCount)
            {
                // build.Append("ampm");
                dateTimeResult.Comment = Constants.Comment_AmPm;
            }

            dateTimeResult.Timex = build.ToString();

            dateTimeResult.FutureValue = dateTimeResult.PastValue =
                DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);
            dateTimeResult.Success = true;

            return dateTimeResult;
        }

        public int MatchToValue(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }

            if (Regex.IsMatch(text, @"\d+"))
            {
                return int.Parse(text, CultureInfo.InvariantCulture);
            }

            if (text.Length == 1)
            {
                return NumberDictionary[text[0]];
            }

            // 五十九,十一,二
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

        // Handle am/pm modifiers (e.g. "1 in the afternoon") and time of day (e.g. "mid-morning")
        public void AddDesc(TimeResult result, string dayDesc)
        {
            if (string.IsNullOrEmpty(dayDesc))
            {
                return;
            }

            dayDesc = NormalizeDayDesc(dayDesc);

            if (result.Hour >= 0 && LowBoundDesc.ContainsKey(dayDesc) && (result.Hour < LowBoundDesc[dayDesc] ||
                (result.Hour == Constants.HalfDayHourCount && LowBoundDesc[dayDesc] == Constants.DayHourStart)))
            {
                // cases like "1 in the afternoon", "12 midnight"
                result.Hour += Constants.HalfDayHourCount;
                result.LowBound = LowBoundDesc[dayDesc];
            }
            else if (result.Hour < 0 && LowBoundDesc.ContainsKey(dayDesc))
            {
                // cases like "mid-morning", "mid-afternoon"
                result.LowBound = LowBoundDesc[dayDesc];
                result.Hour = result.LowBound;
            }
            else
            {
                result.LowBound = 0;
            }
        }

        public TimeResult GetShortLeft(string text)
        {
            string des = null;
            if (Regex.IsMatch(text, DayDescRegex))
            {
                des = text.Substring(0, text.Length - 1);
            }

            var hour = MatchToValue(text.Substring(text.Length - 1, 1));
            var timeResult = new TimeResult
            {
                Hour = hour,
                Minute = -1,
                Second = -1,
            };

            AddDesc(timeResult, des);

            return timeResult;
        }

        // Normalize cases like "p.m.", "p m" to canonical form "pm"
        private static string NormalizeDayDesc(string dayDesc)
        {
            return dayDesc.Replace(" ", string.Empty).Replace(".", string.Empty);
        }
    }
}
