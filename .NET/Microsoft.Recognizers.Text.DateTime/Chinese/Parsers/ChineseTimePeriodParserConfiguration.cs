using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseTimePeriodParserConfiguration : IDateTimeParser
    {
        private static TimeFunctions timeFunc = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = ChineseTimeExtractorConfiguration.DayDescRegex,
        };

        private readonly IFullDateTimeParserConfiguration config;

        public ChineseTimePeriodParserConfiguration(IFullDateTimeParserConfiguration configuration)
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
                var result = new ChineseTimeExtractorConfiguration().Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<PeriodType>;
            }

            if (extra != null)
            {
                // Handle special case like '上午', '下午'
                var parseResult = ParseChineseTimeOfDay(er.Text, referenceTime);

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

            var timeOfDay = string.Empty;

            if (DateTimeDefinitions.MorningTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Morning;
            }
            else if (DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (DateTimeDefinitions.EveningTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Evening;
            }
            else if (DateTimeDefinitions.DaytimeTermList.Any(o => trimmedText.Equals(o)))
            {
                timeOfDay = Constants.Daytime;
            }
            else if (DateTimeDefinitions.NightTermList.Any(o => trimmedText.EndsWith(o)))
            {
                timeOfDay = Constants.Night;
            }
            else
            {
                timex = null;
                return false;
            }

            var parseResult = TimexUtility.ParseTimeOfDay(timeOfDay);
            timex = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }

        private DateTimeResolutionResult ParseChineseTimeOfDay(string text, DateObject referenceTime)
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
