using System;
using System.Collections.Generic;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public class BaseTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";
        
        private readonly ITimePeriodParserConfiguration config;

        public BaseTimePeriodParser(ITimePeriodParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseSimpleCases(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseNight(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            Util.FormatTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            Util.FormatTime(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                        }
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.START_TIME,
                            Util.FormatTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                        },
                        {
                            TimeTypeConstants.END_TIME,
                            Util.FormatTime(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
                        }
                    };
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

        private DTParseResult ParseSimpleCases(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            int year = referenceTime.Year, month = referenceTime.Month, day = referenceTime.Day;
            int beginHour = 0, endHour = 0;
            var trimedText = text.Trim().ToLower();
            var match = this.config.PureNumberFromToRegex.Match(trimedText);
            if (!match.Success)
            {
                match = this.config.PureNumberBetweenAndRegex.Match(trimedText);
            }
            if (match.Success && match.Index == 0)
            {
                // this "from .. to .." pattern is valid if followed by a Date OR "pm"
                var isValid = false;

                // get hours
                var hourGroup = match.Groups["hour"];
                var hourStr = hourGroup.Captures[0].Value;

                if (!this.config.Numbers.TryGetValue(hourStr, out beginHour))
                {
                    beginHour = int.Parse(hourStr);
                }
                hourStr = hourGroup.Captures[1].Value;

                if (!this.config.Numbers.TryGetValue(hourStr, out endHour))
                {
                    endHour = int.Parse(hourStr);
                }

                // parse "pm" 
                var pmStr = match.Groups["pm"].Value;
                var amStr = match.Groups["am"].Value;
                var descStr = match.Groups["desc"].Value;
                if (!string.IsNullOrEmpty(amStr) || !string.IsNullOrEmpty(descStr) && descStr.StartsWith("a"))
                {
                    if (beginHour >= 12)
                    {
                        beginHour -= 12;
                    }
                    if (endHour >= 12)
                    {
                        endHour -= 12;
                    }
                    isValid = true;
                }
                else if (!string.IsNullOrEmpty(pmStr) || !string.IsNullOrEmpty(descStr) && descStr.StartsWith("p"))
                {
                    if (beginHour < 12)
                    {
                        beginHour += 12;
                    }
                    if (endHour < 12)
                    {
                        endHour += 12;
                    }
                    isValid = true;
                }

                if (isValid)
                {
                    var beginStr = "T" + beginHour.ToString("D2");
                    var endStr = "T" + endHour.ToString("D2");
                    ret.Timex = $"({beginStr},{endStr},PT{endHour - beginHour}H)";
                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                        new DateObject(year, month, day, beginHour, 0, 0),
                        new DateObject(year, month, day, endHour, 0, 0));
                    ret.Success = true;
                    return ret;
                }
            }
            return ret;
        }

        private DTParseResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            var ers = this.config.TimeExtractor.Extract(text);
            if (ers.Count != 2)
            {
                return ret;
            }

            pr1 = this.config.TimeParser.Parse(ers[0], referenceTime);
            pr2 = this.config.TimeParser.Parse(ers[1], referenceTime);


            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var beginTime = (DateObject) ((DTParseResult) pr1.Value).FutureValue;
            var endTime = (DateObject) ((DTParseResult) pr2.Value).FutureValue;

            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((endTime - beginTime).TotalHours)}H)";
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
            ret.Success = true;
            var ampmStr1 = ((DTParseResult)pr1.Value).comment;
            var ampmStr2 = ((DTParseResult)pr2.Value).comment;
            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith("ampm") && !string.IsNullOrEmpty(ampmStr2) &&
                ampmStr2.EndsWith("ampm"))
            {
                ret.comment = "ampm";
            }
            return ret;
        }

        // parse "morning", "afternoon", "night"
        private DTParseResult ParseNight(string text, DateObject referenceTime)
        {
            int day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;
            string timex;
            int beginHour, endHour, endMinSeg;
            if (!this.config.GetMatchedTimexRange(text, out timex, out beginHour, out endHour, out endMinSeg))
            {
                return new DTParseResult();
            }
            var ret = new DTParseResult();
            ret.Timex = timex;
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(
                new DateObject(year, month, day, beginHour, 0, 0),
                new DateObject(year, month, day, endHour, endMinSeg, endMinSeg)
                );
            ret.Success = true;
            return ret;
        }
    }
}