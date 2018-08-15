using System;
using System.Collections.Generic;
using System.Text;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimePeriodParserChs : IDateTimeParser
    {
        private readonly IFullDateTimeParserConfiguration config;

        public TimePeriodParserChs(IFullDateTimeParserConfiguration configuration)
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
                var result = new TimeExtractorChs().Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<PeriodType>;
            }

            if (extra != null)
            {
                var parseResult = TimePeriodFunctions.Handle(this.config.TimeParser, extra, referenceTime);
                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.FutureValue).Item2)
                        }
                    };

                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            FormatUtil.FormatTime(((Tuple<DateObject, DateObject>) parseResult.PastValue).Item2)
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