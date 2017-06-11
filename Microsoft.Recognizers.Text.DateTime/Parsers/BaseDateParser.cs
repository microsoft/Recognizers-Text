using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; //"Date";

        private readonly IDateParserConfiguration config;

        public BaseDateParser(IDateParserConfiguration config)
        {
            this.config = config;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject reference)
        {
            var referenceDate = reference;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseBasicRegexMatch(er.Text, referenceDate);
                if (!innerResult.Success)
                {
                    innerResult = ParseImplicitDate(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseWeekdayOfMonth(er.Text, referenceDate);
                }
                // NumberWithMonth must be the last one, because it only need to find a number and a month to get a "success"
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithMonth(er.Text, referenceDate);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, Util.FormatDate((DateObject) innerResult.FutureValue)}
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, Util.FormatDate((DateObject) innerResult.PastValue)}
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
                TimexStr = value == null ? "" : ((DTParseResult)value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        // parse basic patterns in DateRegexList
        private DTParseResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();
            foreach (var regex in this.config.DateRegexes)
            {
                var offset = 0;
                var match = regex.Match(trimedText);
                if (!match.Success)
                {
                    match = regex.Match(this.config.DateTokenPrefix + trimedText);
                    offset = this.config.DateTokenPrefix.Length;
                }

                if (match.Success && match.Index == offset && match.Length == trimedText.Length)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }
            return new DTParseResult();
        }

        // match several other cases
        // including 'today', 'the day after tomorrow', 'on 13'
        private DTParseResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var trimedText = text.Trim();

            var ret = new DTParseResult();

            // handle "on 12"
            var match = this.config.OnRegex.Match(this.config.DateTokenPrefix + trimedText);
            if (match.Success && match.Index == 3 && match.Length == trimedText.Length)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["day"].Value.ToLower();
                day =  this.config.DayOfMonth[dayStr];

                ret.Timex = Util.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate, temp;
                var tryStr = Util.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out temp))
                {
                    futureDate = new DateObject(year, month, day);
                    pastDate = new DateObject(year, month, day);
                    if (futureDate < referenceDate)
                    {
                        futureDate = futureDate.AddMonths(+1);
                    }
                    if (pastDate >= referenceDate)
                    {
                        pastDate = pastDate.AddMonths(-1);
                    }
                }
                else
                {
                    futureDate = new DateObject(year, month + 1, day);
                    pastDate = new DateObject(year, month - 1, day);
                }


                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;


                return ret;
            }

            // handle "today", "the day before yesterday"
            match = this.config.SpecialDayRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = this.config.GetSwiftDay(match.Value);

                var value = referenceDate.AddDays(swift);

                ret.Timex = Util.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "next Sunday"
            match = this.config.NextRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = Util.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "this Friday"
            match = this.config.ThisRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = Util.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "last Friday", "last mon"
            match = this.config.LastRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Last((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = Util.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "Friday"
            match = this.config.StrictWeekDay.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var weekdayStr = match.Groups["weekday"].Value.ToLower();
                var weekDay = this.config.DayOfWeek[weekdayStr];
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                if (weekDay == 0)
                {
                    weekDay = 7;
                }
                if (weekDay < (int)referenceDate.DayOfWeek)
                {
                    value = referenceDate.Next((DayOfWeek)weekDay);
                }

                ret.Timex = "XXXX-WXX-" + weekDay;
                var futureDate = value;
                var pastDate = value;
                if (futureDate < referenceDate)
                {
                    futureDate = futureDate.AddDays(7);
                }
                if (pastDate >= referenceDate)
                {
                    pastDate = pastDate.AddDays(-7);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // handle cases like "January first", "twenty-two of August"
        private DTParseResult ParseNumberWithMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();

            var trimedText = text.Trim().ToLower();
            int month = 0, day = 0, year = referenceDate.Year;
            var match = this.config.MonthRegex.Match(trimedText);
            if (!match.Success)
            {
                return ret;
            }
            month = this.config.MonthOfYear[match.Value.Trim()];

            var er = this.config.OrdinalExtractor.Extract(trimedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimedText);
            }
            if (er.Count == 0)
            {
                return ret;
            }

            day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            // for LUIS format value string
            ret.Timex = Util.LuisDate(-1, month, day);
            var futureDate = new DateObject(year, month, day);
            var pastDate = new DateObject(year, month, day);
            if (futureDate < referenceDate)
            {
                futureDate = futureDate.AddYears(+1);
            }
            if (pastDate >= referenceDate)
            {
                pastDate = pastDate.AddYears(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // parse a regex match which includes 'day', 'month' and 'year' (optional) group
        private DTParseResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DTParseResult();

            var monthStr = match.Groups["month"].Value.ToLower();
            var dayStr = match.Groups["day"].Value.ToLower();
            var yearStr = match.Groups["year"].Value.ToLower();
            int month = 0, day = 0, year = 0;

            if (this.config.MonthOfYear.ContainsKey(monthStr) && this.config.DayOfMonth.ContainsKey(dayStr))
            {
                month = this.config.MonthOfYear[monthStr];
                day = this.config.DayOfMonth[dayStr];
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr);
                    if (year < 100 && year >= 90)
                    {
                        year += 1900;
                    }
                    else if (year < 100 && year < 20)
                    {
                        year += 2000;
                    }
                }
            }

            var noYear = false;
            if (year == 0)
            {
                year = referenceDate.Year;
                ret.Timex = Util.LuisDate(-1, month, day);
                noYear = true;
            }
            else
            {
                ret.Timex = Util.LuisDate(year, month, day);
            }

            var futureDate = new DateObject(year, month, day);
            var pastDate = new DateObject(year, month, day);
            if (noYear && futureDate < referenceDate)
            {
                futureDate = futureDate.AddYears(+1);
            }
            if (noYear && pastDate >= referenceDate)
            {
                pastDate = pastDate.AddYears(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private DTParseResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();

            var trimedText = text.Trim().ToLowerInvariant();
            var match = this.config.WeekDayOfMonthRegex.Match(trimedText);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var weekdayStr = match.Groups["weekday"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (this.config.IsCardinalLast(cardinalStr))
            {
                cardinal = 5;
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
            }
            var weekday = this.config.DayOfWeek[weekdayStr];
            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = this.config.GetSwiftMonth(trimedText);

                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
            }
            else
            {
                month = this.config.MonthOfYear[monthStr];
                year = referenceDate.Year;
                noYear = true;
            }

            var value = ComputeDate(cardinal, weekday, month, year);
            if (value.Month != month)
            {
                cardinal -= 1;
                value = value.AddDays(-7);
            }

            var futureDate = value;
            var pastDate = value;
            if (noYear && futureDate < referenceDate)
            {
                futureDate = ComputeDate(cardinal, weekday, month, year + 1);
                if (futureDate.Month != month)
                {
                    futureDate = futureDate.AddDays(-7);
                }
            }
            if (noYear && pastDate >= referenceDate)
            {
                pastDate = ComputeDate(cardinal, weekday, month, year - 1);
                if (pastDate.Month != month)
                {
                    pastDate = pastDate.AddDays(-7);
                }
            }

            // here is a very special case, timeX followe future date
            ret.Timex = $@"XXXX-{month.ToString("D2")}-WXX-{weekday}-#{cardinal}";
            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        private static DateObject ComputeDate(int cardinal, int weekday, int month, int year)
        {
            var firstDay = new DateObject(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek)weekday);
            if (weekday == 0)
            {
                weekday = 7;
            }
            if (weekday < (int)firstDay.DayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }
            return firstWeekday.AddDays(7 * (cardinal - 1));
        }
    }
}