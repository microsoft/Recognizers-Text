using System;
using System.Collections.Generic;
using System.Text;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class TimePeriodParser : IDateTimeParser
    {
        private readonly IFullDateTimeParserConfiguration config;

        public TimePeriodParser(IFullDateTimeParserConfiguration configuration)
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
            var extra = er.Data as DateTimeExtra<PeriodType>;
            if (extra == null)
            {
                var result = new TimeExtractor().Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<PeriodType>;
            }

            if (extra != null)
            {
                // Handle special case like '上午', '下午'
                var parseResult = ParseJapaneseTimeOfDay(er.Text, referenceTime);

                if (!parseResult.Success)
                {
                    parseResult = TimePeriodFunctions.Handle(this.config.TimeParser, extra, referenceTime);
                }

                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.FutureValue).Item2)
                        }
                    };

                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.PastValue).Item2)
                        }
                    };
                }

                var ret = new DateTimeParseResult
                {
                    Start = er.Start,
                    Text = er.Text,
                    Type = er.Type,
                    Length = er.Length,
                    Value = parseResult,
                    ResolutionStr = "",
                    TimexStr = parseResult.Timex
                };

                return ret;
            }

            return null;
        }

        private DateTimeResolutionResult ParseJapaneseTimeOfDay(string text, DateObject referenceTime)
        {
            int day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;
            var ret = new DateTimeResolutionResult();
            
            if (!GetMatchedTimexRange(text, out string timex, out int beginHour, out int endHour, out int endMinSeg))
            {
                return new DateTimeResolutionResult();
            }
            
            ret.Timex = timex;
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
               DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
               DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinSeg, 0)
               );
            ret.Success = true;
            
            return ret;
        }
        
        private bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();
            beginHour = 0;
            endHour = 0;
            endMin = 0;
            
            if (trimmedText.EndsWith("上午"))
            {
                timex = "TMO";
                beginHour = 8;
                endHour = Constants.HalfDayHourCount;
            }
            else if (trimmedText.EndsWith("下午"))
            {
                timex = "TAF";
                beginHour = Constants.HalfDayHourCount;
                endHour = 16;
            }
            else if (trimmedText.EndsWith("晚上"))
            {
                timex = "TEV";
                beginHour = 16;
                endHour = 20;
            }
            else if (trimmedText.Equals("白天"))
            {
                timex = "TDT";
                beginHour = 8;
                endHour = 18;
            }
            else if (trimmedText.EndsWith("深夜"))
            {
                timex = "TNI";
                beginHour = 20;
                endHour = 23;
                endMin = 59;
            }
            else
            {
                timex = null;
                return false;
            }
            
            return true;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

    }

    public static class TimePeriodFunctions
    {
        public static DateTimeResolutionResult Handle(IDateTimeParser timeParser, DateTimeExtra<PeriodType> extra, DateObject refTime)
        {
            //Left is a time
            var left = extra.NamedEntity["left"];
            TimeResult leftResult, rightResult = null;
            
            // 下午四点十分到五点十分
            if (extra.Type == PeriodType.FullTime)
            {
                var leftExtract = new ExtractResult
                {
                    Start = left.Index,
                    Length = left.Length,
                    Text = left.Value,
                    Type = Constants.SYS_DATETIME_TIME
                };
                leftResult = timeParser.Parse(leftExtract, refTime).Data as TimeResult;
            }
            else
            {
                // 下午四到五点
                leftResult = TimeFunctions.GetShortLeft(left.Value);
            }

            //Right is a time
            var right = extra.NamedEntity["right"];
            var rightExtract = new ExtractResult
            {
                Start = right.Index,
                Length = right.Length,
                Text = right.Value,
                Type = Constants.SYS_DATETIME_TIME
            };

            rightResult = timeParser.Parse(rightExtract, refTime).Data as TimeResult;

            var ret = new DateTimeResolutionResult()
            {
                Success = true
            };

            //the right side doesn't contain desc while the left side does
            if (rightResult.LowBound == -1 && leftResult.LowBound != -1 && rightResult.Hour <= leftResult.LowBound)
            {
                rightResult.Hour += Constants.HalfDayHourCount;
            }

            int day = refTime.Day,
                month = refTime.Month,
                year = refTime.Year;

            //determine if the right side time is smaller than the left side, if yes, add one day
            int hour = leftResult.Hour > 0 ? leftResult.Hour : 0,
                min = leftResult.Minute > 0 ? leftResult.Minute : 0,
                second = leftResult.Second > 0 ? leftResult.Second : 0;

            var leftTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);

            hour = rightResult.Hour > 0 ? rightResult.Hour : 0;
            min = rightResult.Minute > 0 ? rightResult.Minute : 0;
            second = rightResult.Second > 0 ? rightResult.Second : 0;

            var rightTime = DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);

            if (rightTime.Hour < leftTime.Hour)
            {
                rightTime = rightTime.AddDays(1);
            }

            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(leftTime, rightTime);

            var leftTimex = BuildTimex(leftResult);
            var rightTimex = BuildTimex(rightResult);
            ret.Timex = $"({leftTimex},{rightTimex},{BuildSpan(leftResult, rightResult)})";
            return ret;
        }

        public static string BuildTimex(TimeResult timeResult)
        {
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

            return build.ToString();
        }

        public static string BuildSpan(TimeResult left, TimeResult right)
        {
            if (right.Minute == -1)
            {
                right.Minute = 0;
            }

            if (right.Second == -1)
            {
                right.Second = 0;
            }

            if (left.Minute == -1)
            {
                left.Minute = 0;
            }

            if (left.Second == -1)
            {
                left.Second = 0;
            }

            var spanHour = right.Hour - left.Hour;
            var spanMinute = right.Minute - left.Minute;
            var spanSecond = right.Second - left.Second;
            if (spanSecond < 0)
            {
                spanSecond += 60;
                spanMinute -= 1;
            }

            if (spanMinute < 0)
            {
                spanMinute += 60;
                spanHour -= 1;
            }

            if (spanHour < 0)
            {
                spanHour += 24;
            }

            var spanTimex = new StringBuilder();
            spanTimex.Append($"PT{spanHour}H");

            if (spanMinute != 0 && spanSecond == 0)
            {
                spanTimex.Append($"{spanMinute}M");
            }
            else if (spanSecond != 0)
            {
                spanTimex.Append($"{spanMinute}M{spanSecond}S");
            }

            return spanTimex.ToString();
        }

    }
}
