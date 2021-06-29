using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        private readonly ICJKTimePeriodParserConfiguration config;

        public BaseCJKTimePeriodParser(ICJKTimePeriodParserConfiguration configuration)
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
                var result = this.config.TimeExtractor.Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<PeriodType>;
            }

            if (extra != null)
            {
                // Handle special case like '上午' (morning), '下午' (afternoon)
                var parseResult = ParseTimeOfDay(er.Text, referenceTime);

                if (!parseResult.Success)
                {
                    parseResult = TimePeriodFunctions.Handle(this.config.TimeParser, extra, referenceTime, this.config.TimeFunc);
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

        private DateTimeResolutionResult ParseTimeOfDay(string text, DateObject referenceTime)
        {
            int day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;

            var ret = new DateTimeResolutionResult();

            if (!this.config.GetMatchedTimexRange(text, out string timex, out int beginHour, out int endHour, out int endMinSeg))
            {
                return new DateTimeResolutionResult();
            }

            // Add "early"/"late" Mod
            if (endHour == beginHour + Constants.HalfMidDayDurationHourCount && (beginHour == Constants.MorningBeginHour || beginHour == Constants.AfternoonBeginHour))
            {
                ret.Comment = Constants.Comment_Early;
                ret.Mod = Constants.EARLY_MOD;
            }
            else if (beginHour == endHour - Constants.HalfMidDayDurationHourCount && (endHour == Constants.MorningEndHour || endHour == Constants.AfternoonEndHour))
            {
                ret.Comment = Constants.Comment_Late;
                ret.Mod = Constants.LATE_MOD;
            }

            ret.Timex = timex;
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
               DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
               DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMinSeg, endMinSeg));
            ret.Success = true;

            return ret;
        }
    }
}