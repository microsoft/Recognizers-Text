using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATE; //"Date";

        public static readonly DateObject NoDate = DateObject.MinValue.SafeCreateFromValue(0, 0, 0);

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

                if (!innerResult.Success)
                {
                    innerResult = ParseDurationWithAgoAndLater(er.Text, referenceDate);
                }

                // NumberWithMonth must be the second last one, because it only need to find a number and a month to get a "success"
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithMonth(er.Text, referenceDate);
                }

                // SingleNumber last one
                if (!innerResult.Success)
                {
                    innerResult = ParseSingleNumber(er.Text, referenceDate);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject) innerResult.FutureValue)}
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject) innerResult.PastValue)}
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        // parse basic patterns in DateRegexList
        private DateTimeResolutionResult ParseBasicRegexMatch(string text, DateObject referenceDate)
        {
            var trimmedText = text.Trim();
            foreach (var regex in this.config.DateRegexes)
            {
                var offset = 0;
                var match = regex.Match(trimmedText);
                if (!match.Success)
                {
                    match = regex.Match(this.config.DateTokenPrefix + trimmedText);
                    offset = this.config.DateTokenPrefix.Length;
                }

                if (match.Success && match.Index == offset && match.Length == trimmedText.Length)
                {
                    // LUIS value string will be set in Match2Date method
                    var ret = Match2Date(match, referenceDate);
                    return ret;
                }
            }
            return new DateTimeResolutionResult();
        }

        // match several other cases
        // including 'today', 'the day after tomorrow', 'on 13'
        private DateTimeResolutionResult ParseImplicitDate(string text, DateObject referenceDate)
        {
            var trimmedText = text.Trim();

            var ret = new DateTimeResolutionResult();

            // handle "on 12"
            var match = this.config.OnRegex.Match(this.config.DateTokenPrefix + trimmedText);
            if (match.Success && match.Index == 3 && match.Length == trimmedText.Length)
            {
                int month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["day"].Value.ToLower();
                var day = this.config.DayOfMonth[dayStr];

                ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate, pastDate;
                var tryStr = DateTimeFormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out DateObject _))
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

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
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month + 1, day);
                    pastDate = DateObject.MinValue.SafeCreateFromValue(year, month - 1, day);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = pastDate;
                ret.Success = true;

                return ret;
            }

            // handle "today", "the day before yesterday"
            var exactMatch = this.config.SpecialDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var swift = GetSwiftDay(exactMatch.Value);

                var value = referenceDate.AddDays(swift);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "two days from tomorrow"
            exactMatch = this.config.SpecialDayWithNumRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var swift = GetSwiftDay(exactMatch.Groups["day"].Value);
                var numErs = this.config.IntegerExtractor.Extract(trimmedText);
                var numOfDays = Convert.ToInt32((double)(this.config.NumberParser.Parse(numErs[0]).Value ?? 0));

                var value = referenceDate.AddDays(numOfDays + swift);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }
            
            // handle "two sundays from now"
            exactMatch = this.config.RelativeWeekDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var numErs = this.config.IntegerExtractor.Extract(trimmedText);
                var num = Convert.ToInt32((double)(this.config.NumberParser.Parse(numErs[0]).Value ?? 0));
                var weekdayStr = exactMatch.Groups["weekday"].Value.ToLower();
                var value = referenceDate;

                // Check whether the determined day of this week has passed.
                if (value.DayOfWeek > (DayOfWeek)this.config.DayOfWeek[weekdayStr])
                {
                    num--;
                }

                while (num-- > 0)
                {
                    value = value.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);
                }

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "next Sunday"
            exactMatch = this.config.NextRegex.MatchExact(trimmedText, trim: true);
            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Next((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "this Friday"
            exactMatch = this.config.ThisRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value.ToLower();
                var value = referenceDate.This((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "last Friday", "last mon"
            exactMatch = this.config.LastRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value.ToLower();
                var value = referenceDate.Last((DayOfWeek)this.config.DayOfWeek[weekdayStr]);

                ret.Timex = DateTimeFormatUtil.LuisDate(value);
                ret.FutureValue = ret.PastValue = value;
                ret.Success = true;

                return ret;
            }

            // handle "Friday"
            exactMatch = this.config.WeekDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
            {
                var weekdayStr = exactMatch.Groups["weekday"].Value.ToLower();
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

            // handle "for the 27th."
            match = this.config.ForTheRegex.Match(text);

            if (match.Success)
            {
                int day = 0, month = referenceDate.Month, year = referenceDate.Year;
                var dayStr = match.Groups["DayOfMonth"].Value.ToLower();

                // create a extract result which content ordinal string of text
                ExtractResult er = new ExtractResult
                {
                    Text = dayStr,
                    Start = match.Groups["DayOfMonth"].Index,
                    Length = match.Groups["DayOfMonth"].Length
                };

                day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er).Value ?? 0));

                ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);

                DateObject futureDate;
                var tryStr = DateTimeFormatUtil.LuisDate(year, month, day);
                if (DateObject.TryParse(tryStr, out DateObject _))
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
                }
                else
                {
                    futureDate = DateObject.MinValue.SafeCreateFromValue(year, month + 1, day);
                }

                ret.FutureValue = futureDate;
                ret.PastValue = ret.FutureValue;
                ret.Success = true;

                return ret;
            }

            // handling cases like 'Thursday the 21st', which both 'Thursday' and '21st' refer to a same date
            match = this.config.WeekDayAndDayOfMothRegex.Match(text);
            if (match.Success)
            {
                int month = referenceDate.Month, year = referenceDate.Year;
                // create a extract result which content ordinal string of text
                ExtractResult erTmp = new ExtractResult
                {
                    Text = match.Groups["DayOfMonth"].Value,
                    Start = match.Groups["DayOfMonth"].Index,
                    Length = match.Groups["DayOfMonth"].Length
                };

                // parse the day in text into number
                var day = Convert.ToInt32((double)(this.config.NumberParser.Parse(erTmp).Value ?? 0));
                
                // the validity of the phrase is guaranteed in the Date Extractor
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
                ret.FutureValue = new DateObject(year, month, day); ;
                ret.PastValue = new DateObject(year, month, day); ;
                ret.Success = true;

                return ret;
            }

            return ret;
        }

        // handle cases like "January first", "twenty-two of August"
        // handle cases like "20th of next month"
        private DateTimeResolutionResult ParseNumberWithMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim().ToLower();
            int month = 0, day = 0, year = referenceDate.Year;
            bool ambiguous = true;

            var er = this.config.OrdinalExtractor.Extract(trimmedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimmedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            var num = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            var match = this.config.MonthRegex.Match(trimmedText);
            if (match.Success)
            {
                month = this.config.MonthOfYear[match.Value.Trim()];
                day = num;

                var suffix = trimmedText.Substring(er[0].Start + er[0].Length ?? 0);
                var matchYear = this.config.YearSuffix.Match(suffix);
                if (matchYear.Success)
                {
                    year = ((BaseDateExtractor)this.config.DateExtractor).GetYearFromText(matchYear);
                    if (year != Constants.InvalidYear)
                    {
                        ambiguous = false;
                    }
                } 
            }

            // handling relatived month
            if (!match.Success)
            {
                match = this.config.RelativeMonthRegex.Match(trimmedText);
                if (match.Success)
                {
                    var monthStr = match.Groups["order"].Value;
                    var swift = this.config.GetSwiftMonth(monthStr);
                    month = referenceDate.AddMonths(swift).Month;
                    year = referenceDate.AddMonths(swift).Year;
                    day = num;
                    ambiguous = false;
                }
            }

            // handling casesd like 'second Sunday'
            if (!match.Success)
            {
                match = this.config.WeekDayRegex.Match(trimmedText);
                if (match.Success)
                {
                    month = referenceDate.Month;
                    // resolve the date of wanted week day
                    var wantedWeekDay = this.config.DayOfWeek[match.Groups["weekday"].Value];
                    var firstDate = DateObject.MinValue.SafeCreateFromValue(referenceDate.Year, referenceDate.Month, 1);
                    var firstWeekDay = (int)firstDate.DayOfWeek;
                    var firstWantedWeekDay = firstDate.AddDays(wantedWeekDay > firstWeekDay ? wantedWeekDay - firstWeekDay : wantedWeekDay - firstWeekDay + 7);
                    var answerDay = firstWantedWeekDay.Day + (num - 1) * 7;
                    day = answerDay;
                    ambiguous = false;
                }
            }

            if (!match.Success)
            {
                return ret;
            }

            // for LUIS format value string
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (ambiguous)
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(-1, month, day);
                if (futureDate < referenceDate)
                {
                    futureDate = futureDate.AddYears(+1);
                }

                if (pastDate >= referenceDate)
                {
                    pastDate = pastDate.AddYears(-1);
                }
            }
            else
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // handle cases like "the 27th". In the extractor, only the unmatched weekday and date will output this date.
        private DateTimeResolutionResult ParseSingleNumber(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim().ToLower();
            int month = referenceDate.Month, day = 0, year = referenceDate.Year;

            var er = this.config.OrdinalExtractor.Extract(trimmedText);
            if (er.Count == 0)
            {
                er = this.config.IntegerExtractor.Extract(trimmedText);
            }

            if (er.Count == 0)
            {
                return ret;
            }

            day = Convert.ToInt32((double)(this.config.NumberParser.Parse(er[0]).Value ?? 0));

            // for LUIS format value string
            ret.Timex = DateTimeFormatUtil.LuisDate(-1, -1, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

            if (!futureDate.Equals(DateObject.MinValue) && futureDate < referenceDate)
            {
                futureDate = futureDate.AddMonths(1);
            }

            if (!pastDate.Equals(DateObject.MinValue) && pastDate >= referenceDate)
            {
                pastDate = pastDate.AddMonths(-1);
            }

            ret.FutureValue = futureDate;
            ret.PastValue = pastDate;
            ret.Success = true;

            return ret;
        }

        // Handle cases like "two days ago" 
        private DateTimeResolutionResult ParseDurationWithAgoAndLater(string text, DateObject referenceDate)
        {

            return AgoLaterUtil.ParseDurationWithAgoAndLater(text, referenceDate,
                config.DurationExtractor, config.DurationParser, config.UnitMap, config.UnitRegex,
                config.UtilityConfiguration, GetSwiftDay);
        }

        // parse a regex match which includes 'day', 'month' and 'year' (optional) group
        private DateTimeResolutionResult Match2Date(Match match, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

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
                    if (year < 100 && year >= Constants.MinTwoDigitYearPastNum)
                    {
                        year += 1900;
                    }
                    else if (year >= 0 && year < Constants.MaxTwoDigitYearFutureNum)
                    {
                        year += 2000;
                    }
                }
            }

            var noYear = false;
            if (year == 0)
            {
                year = referenceDate.Year;
                ret.Timex = DateTimeFormatUtil.LuisDate(-1, month, day);
                noYear = true;
            }
            else
            {
                ret.Timex = DateTimeFormatUtil.LuisDate(year, month, day);
            }

            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);

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

        private DateTimeResolutionResult ParseWeekdayOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DateTimeResolutionResult();

            var trimmedText = text.Trim().ToLowerInvariant();
            var match = this.config.WeekDayOfMonthRegex.Match(trimmedText);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var weekdayStr = match.Groups["weekday"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            var cardinal = this.config.IsCardinalLast(cardinalStr) ? 5 : this.config.CardinalMap[cardinalStr];

            var weekday = this.config.DayOfWeek[weekdayStr];
            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = this.config.GetSwiftMonth(trimmedText);

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
            var firstDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek)weekday);
            int dayOfWeekOfFirstDay = (int)firstDay.DayOfWeek;

            if (weekday == 0)
            {
                weekday = 7;
            }

            if (dayOfWeekOfFirstDay == 0)
            {
                dayOfWeekOfFirstDay = 7;
            }

            if (weekday < dayOfWeekOfFirstDay)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }

            return firstWeekday.AddDays(7 * (cardinal - 1));
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        private int GetSwiftDay(string text)
        {
            var trimmedText = this.config.Normalize(text.Trim().ToLowerInvariant());
            var swift = 0;

            var match = this.config.RelativeDayRegex.Match(text);

            // The sequence here is important
            // As suffix "day before yesterday" should be matched before suffix "day before" or "yesterday"
            if (config.SameDayTerms.Contains(trimmedText))
            {
                swift = 0;
            }
            else if (EndsWithTerms(trimmedText, config.PlusTwoDayTerms))
            {
                swift = 2;
            }
            else if (EndsWithTerms(trimmedText, config.MinusTwoDayTerms))
            {
                swift = -2;
            }
            else if (EndsWithTerms(trimmedText, config.PlusOneDayTerms))
            {
                swift = 1;
            }
            else if (EndsWithTerms(trimmedText, config.MinusOneDayTerms))
            {
                swift = -1;
            }
            else if (match.Success)
            {
                swift = GetSwift(text);
            }

            return swift;
        }

        private int GetSwift(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();

            var swift = 0;
            if (this.config.NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }
            else if (this.config.PastPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        private bool EndsWithTerms(string text, IImmutableList<string> terms)
        {
            var result = false;

            foreach (var term in terms)
            {
                if (text.EndsWith(term))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}