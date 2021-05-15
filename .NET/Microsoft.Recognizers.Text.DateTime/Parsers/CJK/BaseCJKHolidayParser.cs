using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKHolidayParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; // "Date"

        private readonly ICJKHolidayParserConfiguration config;

        public BaseCJKHolidayParser(ICJKHolidayParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;
            object value = null;

            if (er.Type.Equals(ParserName, StringComparison.Ordinal))
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private DateObject GetFutureValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value < referenceDate)
            {
                if (this.config.FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(1);
                }

                if (this.config.HolidayFuncDict.ContainsKey(holiday))
                {
                    value = this.config.HolidayFuncDict[holiday](referenceDate.Year + 1);
                }
            }

            return value;
        }

        private DateObject GetPastValue(DateObject value, DateObject referenceDate, string holiday)
        {
            if (value >= referenceDate)
            {
                if (this.config.FixedHolidaysDict.ContainsKey(holiday))
                {
                    return value.AddYears(-1);
                }

                if (this.config.HolidayFuncDict.ContainsKey(holiday))
                {
                    value = this.config.HolidayFuncDict[holiday](referenceDate.Year - 1);
                }
            }

            return value;
        }

        private DateTimeResolutionResult ParseHolidayRegexMatch(string text, DateObject referenceDate)
        {
            foreach (var regex in this.config.HolidayRegexList)
            {
                var match = regex.Match(text);

                if (match.Success)
                {
                    // Value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }

            return new DateTimeResolutionResult();
        }

        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();
            var holidayStr = match.Groups["holiday"].Value;

            var year = referenceDate.Year;
            var hasYear = false;
            var yearNum = match.Groups["year"].Value;
            var yearCJK = match.Groups[Constants.YearCJKGroupName].Value;
            var yearRel = match.Groups["yearrel"].Value;

            if (!string.IsNullOrEmpty(yearNum))
            {
                hasYear = true;
                yearNum = this.config.SanitizeYearToken(yearNum);

                year = int.Parse(yearNum, CultureInfo.InvariantCulture);
            }
            else if (!string.IsNullOrEmpty(yearCJK))
            {
                hasYear = true;
                yearCJK = this.config.SanitizeYearToken(yearCJK);

                year = ConvertToInteger(yearCJK);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                hasYear = true;
                int swift = this.config.GetSwiftYear(yearRel);
                if (swift >= -1)
                {
                    year += swift;
                }
            }

            if (year < 100 && year >= 90)
            {
                year += Constants.BASE_YEAR_PAST_CENTURY;
            }
            else if (year < 20)
            {
                year += Constants.BASE_YEAR_CURRENT_CENTURY;
            }

            if (!string.IsNullOrEmpty(holidayStr))
            {
                DateObject value;
                string timexStr;
                if (this.config.FixedHolidaysDict.ContainsKey(holidayStr))
                {
                    value = this.config.FixedHolidaysDict[holidayStr](year);
                    timexStr = $"-{value.Month:D2}-{value.Day:D2}";
                }
                else
                {
                    if (this.config.HolidayFuncDict.ContainsKey(holidayStr))
                    {
                        value = this.config.HolidayFuncDict[holidayStr](year);
                        timexStr = this.config.NoFixedTimex[holidayStr];
                    }
                    else
                    {
                        return ret;
                    }
                }

                if (hasYear)
                {
                    ret.Timex = year.ToString("D4", CultureInfo.InvariantCulture) + timexStr;
                    ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, value.Month, value.Day);
                    ret.Success = true;
                    return ret;
                }

                ret.Timex = "XXXX" + timexStr;
                ret.FutureValue = GetFutureValue(value, referenceDate, holidayStr);
                ret.PastValue = GetPastValue(value, referenceDate, holidayStr);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private int ConvertToInteger(string yearCJKStr)
        {
            var year = 0;
            var num = 0;

            var er = this.config.IntegerExtractor.Extract(yearCJKStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                {
                    num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                }
            }

            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearCJKStr)
                {
                    num *= 10;
                    er = this.config.IntegerExtractor.Extract(ch.ToString(CultureInfo.InvariantCulture));
                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER, StringComparison.Ordinal))
                        {
                            num += Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));
                        }
                    }
                }

                year = num;
            }
            else
            {
                year = num;
            }

            return year == 0 ? -1 : year;
        }

        // parse if lunar contains
        private bool IsLunarCalendar(string text)
        {
            var trimmedText = text.Trim();
            var match = this.config.LunarHolidayRegex.Match(trimmedText);
            return match.Success;
        }
    }
}