using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseHolidayParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date"

        private readonly IHolidayParserConfiguration config;

        public BaseHolidayParser(IHolidayParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;
            object value = null;

            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseHolidayRegexMatch(er.Text, referenceDate);

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.FutureValue) },
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)innerResult.PastValue) },
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            foreach (var regex in this.config.HolidayRegexList)
            {
                var match = regex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match.Match, referenceDate);
                    return ret;
                }
            }

            return new DateTimeResolutionResult();
        }

        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = this.config.SanitizeHolidayToken(match.Groups["holiday"].Value.ToLowerInvariant());

            // get year (if exist)
            var yearStr = match.Groups["year"].Value.ToLower();
            var orderStr = match.Groups["order"].Value.ToLower();
            int year;
            var hasYear = false;

            if (!string.IsNullOrEmpty(yearStr))
            {
                year = int.Parse(yearStr);
                hasYear = true;
            }
            else if (!string.IsNullOrEmpty(orderStr))
            {
                var swift = this.config.GetSwiftYear(orderStr);
                if (swift < -1)
                {
                    return ret;
                }

                year = referenceDate.Year + swift;
                hasYear = true;
            }
            else
            {
                year = referenceDate.Year;
            }

            string holidayKey = string.Empty;
            foreach (var holidayPair in this.config.HolidayNames)
            {
                if (holidayPair.Value.Contains(holidayStr))
                {
                    holidayKey = holidayPair.Key;
                    break;
                }
            }

            var timexStr = string.Empty;
            if (!string.IsNullOrEmpty(holidayKey))
            {
                var value = referenceDate;
                if (this.config.HolidayFuncDictionary.TryGetValue(holidayKey, out Func<int, DateObject> function))
                {
                    value = function(year);
                    this.config.VariableHolidaysTimexDictionary.TryGetValue(holidayKey, out timexStr);
                    if (string.IsNullOrEmpty(timexStr))
                    {
                        timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                    }
                }

                if (function == null)
                {
                    return ret;
                }

                if (value.Equals(DateObject.MinValue))
                {
                    ret.Timex = string.Empty;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue;
                    ret.Success = true;
                    return ret;
                }

                if (hasYear)
                {
                    ret.Timex = year.ToString("D4") + timexStr;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, value.Month, value.Day);
                    ret.Success = true;
                    return ret;
                }

                ret.Timex = "XXXX" + timexStr;
                ret.FutureValue = GetFutureValue(value, referenceDate, holidayKey);
                ret.PastValue = GetPastValue(value, referenceDate, holidayKey);
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        private DateObject GetFutureValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value < referenceDate)
            {
                if (this.config.HolidayFuncDictionary.TryGetValue(holiday, out Func<int, DateObject> function))
                {
                    return function(value.Year + 1);
                }
            }

            return value;
        }

        private DateObject GetPastValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value >= referenceDate)
            {
                if (this.config.HolidayFuncDictionary.TryGetValue(holiday, out Func<int, DateObject> function))
                {
                    return function(value.Year - 1);
                }
            }

            return value;
        }
    }
}