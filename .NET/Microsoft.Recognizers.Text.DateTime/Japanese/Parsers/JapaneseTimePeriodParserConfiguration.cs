using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimePeriodParserConfiguration : IDateTimeParser
    {
        private static TimeFunctions timeFunc = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = JapaneseTimeExtractorConfiguration.DayDescRegex,
        };

        private readonly IFullDateTimeParserConfiguration config;

        public JapaneseTimePeriodParserConfiguration(IFullDateTimeParserConfiguration configuration)
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
                var result = new JapaneseTimeExtractorConfiguration().Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<PeriodType>;
            }

            if (extra != null)
            {
                // Handle special case like '上午', '下午'
                var parseResult = ParseJapaneseTimeOfDay(er.Text, referenceTime);

                if (!parseResult.Success)
                {
                    parseResult = TimePeriodFunctions.Handle(this.config.TimeParser, extra, referenceTime, timeFunc);
                }

                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)parseResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)parseResult.FutureValue).Item2)
                        },
                    };

                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)parseResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)parseResult.PastValue).Item2)
                        },
                    };
                }

                var ret = new DateTimeParseResult
                {
                    Start = er.Start,
                    Text = er.Text,
                    Type = er.Type,
                    Length = er.Length,
                    Value = parseResult,
                    ResolutionStr = string.Empty,
                    TimexStr = parseResult.Timex,
                };

                return ret;
            }

            return null;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private static bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
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
               DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinSeg, 0));
            ret.Success = true;

            return ret;
        }
    }
}
