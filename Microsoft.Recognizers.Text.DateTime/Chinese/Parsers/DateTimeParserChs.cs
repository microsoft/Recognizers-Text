using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class DateTimeParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIME;

        public static readonly Regex SimpleAmRegex = new Regex(@"(?<am>早|晨)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex SimplePmRegex = new Regex(@"(?<pm>晚)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly IExtractor _singleDateExtractor = new DateExtractorChs();
        private static readonly IExtractor _singleTimeExtractor = new TimeExtractorChs();

        private readonly IFullDateTimeParserConfiguration config;

        public DateTimeParserChs(IFullDateTimeParserConfiguration configuration)
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

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = MergeDateAndTime(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseBasicRegex(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseTimeOfToday(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, Util.FormatDateTime((DateObject) innerResult.FutureValue)}
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, Util.FormatDateTime((DateObject) innerResult.PastValue)}
                    };
                    innerResult.IsLunar = IsLunarCalendar(er.Text);

                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? "" : ((DTParseResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimedText = text.Trim();
            var match = DateExtractorChs.LunarRegex.Match(trimedText);
            if (match.Success)
            {
                return true;
            }
            return ChineseHolidayExtractorConfiguration.LunarHolidayRegex.IsMatch(trimedText);
        }

        private static DTParseResult ParseBasicRegex(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var trimedText = text.Trim().ToLower();

            // handle "现在"
            var match = DateTimeExtractorChs.NowRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                if (trimedText.EndsWith("现在"))
                {
                    ret.Timex = "PRESENT_REF";
                }
                else if (trimedText.Equals("刚刚才") || trimedText.Equals("刚刚") || trimedText.Equals("刚才"))
                {
                    ret.Timex = "PAST_REF";
                }
                else if (trimedText.Equals("立刻") || trimedText.Equals("马上"))
                {
                    ret.Timex = "FUTURE_REF";
                }
                ret.FutureValue = ret.PastValue = referenceTime;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // merge a Date entity and a Time entity
        private DTParseResult MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();

            var er1 = _singleDateExtractor.Extract(text);
            if (er1.Count == 0)
            {
                return ret;
            }

            var er2 = _singleTimeExtractor.Extract(text);
            if (er2.Count == 0)
            {
                return ret;
            }


            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime.Date);
            // TODO: Add reference time
            var pr2 = this.config.TimeParser.Parse(er2[0], referenceTime);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var futureDate = (DateObject) ((DTParseResult) pr1.Value).FutureValue;
            var pastDate = (DateObject) ((DTParseResult) pr1.Value).PastValue;
            var time = (DateObject) ((DTParseResult) pr2.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            // handle morning, afternoon
            if (SimplePmRegex.IsMatch(text) && hour < 12)
            {
                hour += 12;
            }
            else if (SimpleAmRegex.IsMatch(text) && hour >= 12)
            {
                hour -= 12;
            }

            var timeStr = pr2.TimexStr;
            if (timeStr.EndsWith("ampm"))
            {
                timeStr = timeStr.Substring(0, timeStr.Length - 4);
            }
            timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);
            ret.Timex = pr1.TimexStr + timeStr;
            var Val = (DTParseResult) pr2.Value;
            if (hour <= 12 && !SimplePmRegex.IsMatch(text) && !SimpleAmRegex.IsMatch(text) &&
                !string.IsNullOrEmpty(Val.comment))
            {
                //ret.Timex += "ampm";
                ret.comment = "ampm";
            }
            ret.FutureValue = new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, hour, min, sec);
            ret.PastValue = new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, hour, min, sec);
            ret.Success = true;

            return ret;
        }

        private DTParseResult ParseTimeOfToday(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var ers = _singleTimeExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }

            // TODO: Add reference time
            var pr = this.config.TimeParser.Parse(ers[0], referenceTime);
            if (pr.Value == null)
            {
                return ret;
            }

            var time = (DateObject) ((DTParseResult) pr.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            var match = DateTimeExtractorChs.TimeOfTodayRegex.Match(text);

            if (match.Success)
            {
                var matchStr = match.Value.ToLowerInvariant();


                var swift = 0;
                switch (matchStr)
                {
                    case "今晚":
                        if (hour < 12)
                        {
                            hour += 12;
                        }
                        break;
                    case "今早":
                    case "今晨":
                        if (hour >= 12)
                        {
                            hour -= 12;
                        }
                        break;
                    case "明晚":
                        swift = 1;
                        if (hour < 12)
                        {
                            hour += 12;
                        }
                        break;
                    case "明早":
                    case "明晨":
                        swift = 1;
                        if (hour >= 12)
                        {
                            hour -= 12;
                        }
                        break;
                    case "昨晚":
                        swift = -1;
                        if (hour < 12)
                        {
                            hour += 12;
                        }
                        break;
                    default:
                        break;
                }

                var date = referenceTime.AddDays(swift).Date;

                // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
                var timeStr = pr.TimexStr;
                if (timeStr.EndsWith("ampm"))
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 4);
                }
                timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);

                ret.Timex = Util.FormatDate(date) + timeStr;
                ret.FutureValue = ret.PastValue = new DateObject(date.Year, date.Month, date.Day, hour, min, sec);
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}