using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimePeriodParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEPERIOD;

        public BaseDateTimePeriodParser(IDateTimePeriodParserConfiguration configuration)
        {
            Config = configuration;
        }

        protected IDateTimePeriodParserConfiguration Config { get;  private set; }

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
                var innerResult = MergeDateWithSingleTimePeriod(er.Text, referenceTime);

                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseSpecificTimeOfDay(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDuration(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseRelativeUnit(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseDateWithPeriodPrefix(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
                    innerResult = ParseDateWithTimePeriodSuffix(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    if (!IsBeforeOrAfterMod(innerResult.Mod))
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATETIME,
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATETIME,
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.FutureValue).Item2)
                            },
                        };

                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATETIME,
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATETIME,
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)innerResult.PastValue).Item2)
                            },
                        };
                    }
                    else
                    {
                        if (innerResult.Mod == Constants.AFTER_MOD)
                        {
                            // Cases like "1/1/2015 after 2:00" there is no EndTime
                            innerResult.FutureResolution = new Dictionary<string, string>
                            {
                                {
                                    TimeTypeConstants.START_DATETIME,
                                    DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.FutureValue)
                                },
                            };

                            innerResult.PastResolution = new Dictionary<string, string>
                            {
                                {
                                    TimeTypeConstants.START_DATETIME,
                                    DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.PastValue)
                                },
                            };
                        }
                        else
                        {
                            // Cases like "1/1/2015 before 5:00 in the afternoon" there is no StartTime
                            innerResult.FutureResolution = new Dictionary<string, string>
                            {
                                {
                                    TimeTypeConstants.END_DATETIME,
                                    DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.FutureValue)
                                },
                            };

                            innerResult.PastResolution = new Dictionary<string, string>
                            {
                                {
                                    TimeTypeConstants.END_DATETIME,
                                    DateTimeFormatUtil.FormatDateTime((DateObject)innerResult.PastValue)
                                },
                            };
                        }
                    }

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

        // Parse specific TimeOfDay like "this night", "early morning", "late evening"
        protected virtual DateTimeResolutionResult ParseSpecificTimeOfDay(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim().ToLowerInvariant();
            var timeText = trimmedText;

            var match = this.Config.PeriodTimeOfDayWithDateRegex.Match(trimmedText);

            // Extract early/late prefix from text if any
            bool hasEarly = false, hasLate = false;
            if (match.Success)
            {
                timeText = match.Groups[Constants.TimeOfDayGroupName].Value;

                if (!string.IsNullOrEmpty(match.Groups["early"].Value))
                {
                    hasEarly = true;
                    ret.Comment = Constants.Comment_Early;
                }

                if (!hasEarly && !string.IsNullOrEmpty(match.Groups["late"].Value))
                {
                    hasLate = true;
                    ret.Comment = Constants.Comment_Late;
                }
            }
            else
            {
                match = this.Config.AmDescRegex.Match(trimmedText);
                if (!match.Success)
                {
                    match = this.Config.PmDescRegex.Match(trimmedText);
                }

                if (match.Success)
                {
                    timeText = match.Value;
                }
            }

            // Handle time of day

            // Late/early only works with time of day
            // Only standard time of day (morning, afternoon, evening and night) will not directly return
            if (!this.Config.GetMatchedTimeRange(timeText, out string timeStr, out int beginHour, out int endHour, out int endMin))
            {
                return ret;
            }

            // Modify time period if "early" or "late" exists
            // Since 'time of day' is defined as four hour periods
            // the first 2 hours represent early, the later 2 hours represent late
            if (hasEarly)
            {
                endHour = beginHour + 2;

                // Handling speical case: night ends with 23:59
                if (endMin == 59)
                {
                    endMin = 0;
                }
            }
            else if (hasLate)
            {
                beginHour = beginHour + 2;
            }

            if (Config.SpecificTimeOfDayRegex.IsExactMatch(trimmedText, trim: true))
            {
                var swift = this.Config.GetSwiftPrefix(trimmedText);

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = DateTimeFormatUtil.FormatDate(date) + timeStr;

                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, beginHour, 0, 0),
                            DateObject.MinValue.SafeCreateFromValue(year, month, day, endHour, endMin, endMin));

                ret.Success = true;
                return ret;
            }

            // Handle Date followed by morning, afternoon and morning, afternoon followed by Date
            match = this.Config.PeriodTimeOfDayWithDateRegex.Match(trimmedText);

            if (!match.Success)
            {
                match = this.Config.AmDescRegex.Match(trimmedText);
                if (!match.Success)
                {
                    match = this.Config.PmDescRegex.Match(trimmedText);
                }
            }

            if (match.Success)
            {
                var beforeStr = trimmedText.Substring(0, match.Index).Trim();
                var afterStr = trimmedText.Substring(match.Index + match.Length).Trim();

                // Eliminate time period, if any
                var timePeriodErs = this.Config.TimePeriodExtractor.Extract(beforeStr);
                if (timePeriodErs.Count > 0)
                {
                    beforeStr = beforeStr.Remove(timePeriodErs[0].Start ?? 0, timePeriodErs[0].Length ?? 0).Trim();
                }
                else
                {
                    timePeriodErs = this.Config.TimePeriodExtractor.Extract(afterStr);
                    if (timePeriodErs.Count > 0)
                    {
                        afterStr = afterStr.Remove(timePeriodErs[0].Start ?? 0, timePeriodErs[0].Length ?? 0).Trim();
                    }
                }

                var ers = this.Config.DateExtractor.Extract(beforeStr + ' ' + afterStr, referenceTime);

                if (ers.Count == 0 || ers[0].Length < beforeStr.Length)
                {
                    var valid = false;

                    if (ers.Count > 0 && ers[0].Start == 0)
                    {
                        var midStr = beforeStr.Substring(ers[0].Start + ers[0].Length ?? 0);
                        if (string.IsNullOrWhiteSpace(midStr.Replace(',', ' ')))
                        {
                            valid = true;
                        }
                    }

                    if (!valid)
                    {
                        ers = this.Config.DateExtractor.Extract(afterStr, referenceTime);

                        if (ers.Count == 0 || ers[0].Length != afterStr.Length)
                        {
                            if (ers.Count > 0 && ers[0].Start + ers[0].Length == afterStr.Length)
                            {
                                var midStr = afterStr.Substring(0, ers[0].Start ?? 0);
                                if (string.IsNullOrWhiteSpace(midStr.Replace(',', ' ')))
                                {
                                    valid = true;
                                }
                            }
                        }
                        else
                        {
                            valid = true;
                        }
                    }

                    if (!valid)
                    {
                        return ret;
                    }
                }

                var hasSpecificTimePeriod = false;
                if (timePeriodErs.Count > 0)
                {
                    var timePr = this.Config.TimePeriodParser.Parse(timePeriodErs[0], referenceTime);
                    if (timePr != null)
                    {
                        var periodFuture = (Tuple<DateObject, DateObject>)((DateTimeResolutionResult)timePr.Value).FutureValue;
                        var periodPast = (Tuple<DateObject, DateObject>)((DateTimeResolutionResult)timePr.Value).PastValue;

                        if (periodFuture == periodPast)
                        {
                            beginHour = periodFuture.Item1.Hour;
                            endHour = periodFuture.Item2.Hour;
                        }
                        else
                        {
                            if (periodFuture.Item1.Hour >= beginHour || periodFuture.Item2.Hour <= endHour)
                            {
                                beginHour = periodFuture.Item1.Hour;
                                endHour = periodFuture.Item2.Hour;
                            }
                            else
                            {
                                beginHour = periodPast.Item1.Hour;
                                endHour = periodPast.Item2.Hour;
                            }
                        }

                        hasSpecificTimePeriod = true;
                    }
                }

                var pr = this.Config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                var pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

                if (!hasSpecificTimePeriod)
                {
                    ret.Timex = pr.TimexStr + timeStr;
                }
                else
                {
                    ret.Timex = string.Format("({0}T{1},{0}T{2},PT{3}H)", pr.TimexStr, beginHour, endHour, endHour - beginHour);
                }

                ret.FutureValue =
                    new Tuple<DateObject, DateObject>(
                        DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                        DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin));

                ret.PastValue =
                    new Tuple<DateObject, DateObject>(
                        DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                        DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin));

                ret.Success = true;

                return ret;
            }

            return ret;
        }

        private bool IsBeforeOrAfterMod(string mod)
        {
            return !string.IsNullOrEmpty(mod) && (mod == Constants.BEFORE_MOD || mod == Constants.AFTER_MOD);
        }

        // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
        private DateTimeResolutionResult ParseDateWithTimePeriodSuffix(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var dateEr = this.Config.DateExtractor.Extract(text).FirstOrDefault();
            var timeEr = this.Config.TimeExtractor.Extract(text).FirstOrDefault();

            if (dateEr != null && timeEr != null)
            {
                var dateStrEnd = (int)(dateEr.Start + dateEr.Length);

                if (dateStrEnd < timeEr.Start)
                {
                    var midStr = text.Substring(dateStrEnd, timeEr.Start.Value - dateStrEnd).Trim();

                    if (IsValidConnectorForDateAndTimePeriod(midStr))
                    {
                        var datePr = this.Config.DateParser.Parse(dateEr, referenceTime);
                        var timePr = this.Config.TimeParser.Parse(timeEr, referenceTime);

                        if (datePr != null && timePr != null)
                        {
                            var timeResolutionResult = (DateTimeResolutionResult)timePr.Value;
                            var dateResolutionResult = (DateTimeResolutionResult)datePr.Value;
                            var futureDateValue = (DateObject)dateResolutionResult.FutureValue;
                            var pastDateValue = (DateObject)dateResolutionResult.PastValue;
                            var futureTimeValue = (DateObject)timeResolutionResult.FutureValue;
                            var pastTimeValue = (DateObject)timeResolutionResult.PastValue;

                            ret.Comment = timeResolutionResult.Comment;
                            ret.Timex = $"{datePr.TimexStr}{timePr.TimexStr}";

                            ret.FutureValue = DateObject.MinValue.SafeCreateFromValue(futureDateValue.Year, futureDateValue.Month, futureDateValue.Day, futureTimeValue.Hour, futureTimeValue.Minute, futureTimeValue.Second);

                            ret.PastValue = DateObject.MinValue.SafeCreateFromValue(pastDateValue.Year, pastDateValue.Month, pastDateValue.Day, pastTimeValue.Hour, pastTimeValue.Minute, pastTimeValue.Second);

                            ret.Mod = this.Config.BeforeRegex.Match(midStr).Success ? Constants.BEFORE_MOD : Constants.AFTER_MOD;
                            ret.SubDateTimeEntities = new List<object>()
                            {
                                datePr,
                                timePr,
                            };

                            if (((DateTimeResolutionResult)timePr.Value).TimeZoneResolution != null)
                            {
                                ret.TimeZoneResolution = ((DateTimeResolutionResult)timePr.Value).TimeZoneResolution;
                            }

                            ret.Success = true;
                        }
                    }
                }
            }

            return ret;
        }

        // Cases like "today after 2:00pm", "1/1/2015 before 2:00 in the afternoon"
        // Valid connector in English for Before include: "before", "no later than", "in advance of", "prior to", "earlier than", "sooner than", "by", "till", "until"...
        // Valid connector in English for After include: "after", "later than"
        private bool IsValidConnectorForDateAndTimePeriod(string text)
        {
            var beforeAfterRegexes = new List<Regex>()
            {
                this.Config.BeforeRegex,
                this.Config.AfterRegex,
            };

            foreach (var regex in beforeAfterRegexes)
            {
                var match = regex.MatchExact(text, trim: true);

                if (match.Success)
                {
                    return true;
                }
            }

            return false;
        }

        private DateTimeResolutionResult ParseDateWithPeriodPrefix(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var dateResult = this.Config.DateExtractor.Extract(text);
            if (dateResult.Count > 0)
            {
                var beforeString = text.Substring(0, (int)dateResult.Last().Start).TrimEnd();
                var match = Config.PrefixDayRegex.Match(beforeString);
                if (match.Success)
                {
                    var pr = this.Config.DateParser.Parse(dateResult.Last(), referenceTime);
                    if (pr.Value != null)
                    {
                        var startTime = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                        startTime = new DateObject(startTime.Year, startTime.Month, startTime.Day);
                        var endTime = startTime;

                        if (match.Groups["EarlyPrefix"].Success)
                        {
                            endTime = endTime.AddHours(Constants.HalfDayHourCount);
                            ret.Mod = Constants.EARLY_MOD;
                        }
                        else if (match.Groups["MidPrefix"].Success)
                        {
                            startTime = startTime.AddHours(Constants.HalfDayHourCount - Constants.HalfMidDayDurationHourCount);
                            endTime = endTime.AddHours(Constants.HalfDayHourCount + Constants.HalfMidDayDurationHourCount);
                            ret.Mod = Constants.MID_MOD;
                        }
                        else if (match.Groups["LatePrefix"].Success)
                        {
                            startTime = startTime.AddHours(Constants.HalfDayHourCount);
                            endTime = startTime.AddHours(Constants.HalfDayHourCount);
                            ret.Mod = Constants.LATE_MOD;
                        }
                        else
                        {
                            return ret;
                        }

                        ret.Timex = pr.TimexStr;

                        ret.PastValue = ret.FutureValue = new Tuple<DateObject, DateObject>(startTime, endTime);

                        ret.Success = true;
                    }
                }
            }

            return ret;
        }

        private DateTimeResolutionResult MergeDateWithSingleTimePeriod(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim().ToLower();

            var ers = Config.TimePeriodExtractor.Extract(trimmedText, referenceTime);

            if (ers.Count == 0)
            {
                return ParsePureNumberCases(text, referenceTime);
            }
            else if (ers.Count == 1)
            {
                var timePeriodParseResult = Config.TimePeriodParser.Parse(ers[0]);
                var timePeriodResolutionResult = (DateTimeResolutionResult)timePeriodParseResult.Value;

                if (timePeriodResolutionResult == null)
                {
                    return ParsePureNumberCases(text, referenceTime);
                }

                if (timePeriodResolutionResult.TimeZoneResolution != null)
                {
                    ret.TimeZoneResolution = timePeriodResolutionResult.TimeZoneResolution;
                }

                var periodTimex = timePeriodResolutionResult.Timex;

                // If it is a range type timex
                if (TimexUtility.IsRangeTimex(periodTimex))
                {
                    var dateResult = this.Config.DateExtractor.Extract(trimmedText.Replace(ers[0].Text, string.Empty), referenceTime);

                    var dateText = trimmedText.Replace(ers[0].Text, string.Empty).Replace(Config.TokenBeforeDate, string.Empty).Trim();

                    // If only one Date is extracted and the Date text equals to the rest part of source text
                    if (dateResult.Count == 1 && dateText.Equals(dateResult[0].Text))
                    {
                        string dateTimex;
                        DateObject futureTime;
                        DateObject pastTime;

                        var pr = this.Config.DateParser.Parse(dateResult[0], referenceTime);

                        if (pr.Value != null)
                        {
                            futureTime = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                            pastTime = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

                            dateTimex = pr.TimexStr;
                        }
                        else
                        {
                            return ParsePureNumberCases(text, referenceTime);
                        }

                        var rangeTimexComponents = TimexUtility.GetRangeTimexComponents(periodTimex);

                        if (rangeTimexComponents.IsValid)
                        {
                            var beginTimex = TimexUtility.CombineDateAndTimeTimex(dateTimex, rangeTimexComponents.BeginTimex);
                            var endTimex = TimexUtility.CombineDateAndTimeTimex(dateTimex, rangeTimexComponents.EndTimex);
                            ret.Timex = TimexUtility.GenerateDateTimePeriodTimex(beginTimex, endTimex, rangeTimexComponents.DurationTimex);

                            var timePeriodFutureValue = (Tuple<DateObject, DateObject>)timePeriodResolutionResult.FutureValue;
                            var beginTime = timePeriodFutureValue.Item1;
                            var endTime = timePeriodFutureValue.Item2;

                            ret.FutureValue = new Tuple<DateObject, DateObject>(
                                DateObject.MinValue.SafeCreateFromValue(
                                    futureTime.Year, futureTime.Month, futureTime.Day, beginTime.Hour, beginTime.Minute, beginTime.Second),
                                DateObject.MinValue.SafeCreateFromValue(
                                    futureTime.Year, futureTime.Month, futureTime.Day, endTime.Hour, endTime.Minute, endTime.Second));

                            ret.PastValue = new Tuple<DateObject, DateObject>(
                                DateObject.MinValue.SafeCreateFromValue(
                                    pastTime.Year, pastTime.Month, pastTime.Day, beginTime.Hour, beginTime.Minute, beginTime.Second),
                                DateObject.MinValue.SafeCreateFromValue(
                                    pastTime.Year, pastTime.Month, pastTime.Day, endTime.Hour, endTime.Minute, endTime.Second));

                            if (!string.IsNullOrEmpty(timePeriodResolutionResult.Comment) &&
                                timePeriodResolutionResult.Comment.Equals(Constants.Comment_AmPm))
                            {
                                // AmPm comment is used for later SetParserResult to judge whether this parse result should have two parsing results
                                // Cases like "from 10:30 to 11 on 1/1/2015" should have AmPm comment, as it can be parsed to "10:30am to 11am" and also be parsed to "10:30pm to 11pm"
                                // Cases like "from 10:30 to 3 on 1/1/2015" should not have AmPm comment
                                if (beginTime.Hour < Constants.HalfDayHourCount && endTime.Hour < Constants.HalfDayHourCount)
                                {
                                    ret.Comment = Constants.Comment_AmPm;
                                }
                            }

                            ret.Success = true;
                            ret.SubDateTimeEntities = new List<object> { pr, timePeriodParseResult };

                            return ret;
                        }
                    }

                    return ParsePureNumberCases(text, referenceTime);
                }
            }

            return ret;
        }

        // Handle cases like "Monday 7-9", where "7-9" can't be extracted by the TimePeriodExtractor
        private DateTimeResolutionResult ParsePureNumberCases(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim().ToLower();

            var match = this.Config.PureNumberFromToRegex.Match(trimmedText);

            if (!match.Success)
            {
                match = this.Config.PureNumberBetweenAndRegex.Match(trimmedText);
            }

            if (match.Success && (match.Index == 0 || match.Index + match.Length == trimmedText.Length))
            {
                int beginHour, endHour;
                ret.Comment = ParseTimePeriod(match, out beginHour, out endHour);

                var dateStr = string.Empty;

                // Parse following date
                var dateExtractResult = this.Config.DateExtractor.Extract(trimmedText.Replace(match.Value, string.Empty), referenceTime);

                DateObject futureDate, pastDate;
                if (dateExtractResult.Count > 0)
                {
                    var pr = this.Config.DateParser.Parse(dateExtractResult[0], referenceTime);
                    if (pr.Value != null)
                    {
                        futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                        pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

                        dateStr = pr.TimexStr;

                        if (((DateTimeResolutionResult)pr.Value).TimeZoneResolution != null)
                        {
                            ret.TimeZoneResolution = ((DateTimeResolutionResult)pr.Value).TimeZoneResolution;
                        }
                    }
                    else
                    {
                        return ret;
                    }
                }
                else
                {
                    return ret;
                }

                var pastHours = endHour - beginHour;
                var beginTimex = TimexUtility.CombineDateAndTimeTimex(dateStr, DateTimeFormatUtil.ShortTime(beginHour));
                var endTimex = TimexUtility.CombineDateAndTimeTimex(dateStr, DateTimeFormatUtil.ShortTime(endHour));
                var durationTimex = TimexUtility.GenerateDurationTimex(endHour - beginHour, Constants.TimexHour, isLessThanDay: true);

                ret.Timex = TimexUtility.GenerateDateTimePeriodTimex(beginTimex, endTimex, durationTimex);

                ret.FutureValue = new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                    DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, endHour, 0, 0));

                ret.PastValue = new Tuple<DateObject, DateObject>(
                    DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                    DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, endHour, 0, 0));

                ret.Success = true;
            }

            return ret;
        }

        private string ParseTimePeriod(Match match, out int beginHour, out int endHour)
        {
            // This "from .. to .." pattern is valid if followed by a Date OR "pm"
            var hasAm = false;
            var hasPm = false;
            var comments = string.Empty;

            // Get hours
            var hourGroup = match.Groups[Constants.HourGroupName];
            var hourStr = hourGroup.Captures[0].Value;

            if (this.Config.Numbers.ContainsKey(hourStr))
            {
                beginHour = this.Config.Numbers[hourStr];
            }
            else
            {
                beginHour = int.Parse(hourStr);
            }

            hourStr = hourGroup.Captures[1].Value;

            if (this.Config.Numbers.ContainsKey(hourStr))
            {
                endHour = this.Config.Numbers[hourStr];
            }
            else
            {
                endHour = int.Parse(hourStr);
            }

            // Parse "pm"
            var matchPmStr = match.Groups[Constants.PmGroupName].Value;
            var matchAmStr = match.Groups[Constants.AmGroupName].Value;
            var descStr = match.Groups[Constants.DescGroupName].Value;
            if (!string.IsNullOrEmpty(matchAmStr) || (!string.IsNullOrEmpty(descStr) && descStr.StartsWith("a")))
            {
                if (beginHour >= Constants.HalfDayHourCount)
                {
                    beginHour -= Constants.HalfDayHourCount;
                }

                if (endHour >= Constants.HalfDayHourCount)
                {
                    endHour -= Constants.HalfDayHourCount;
                }

                hasAm = true;
            }
            else if (!string.IsNullOrEmpty(matchPmStr) || (!string.IsNullOrEmpty(descStr) && descStr.StartsWith("p")))
            {
                if (beginHour < Constants.HalfDayHourCount)
                {
                    beginHour += Constants.HalfDayHourCount;
                }

                if (endHour < Constants.HalfDayHourCount)
                {
                    endHour += Constants.HalfDayHourCount;
                }

                hasPm = true;
            }

            if (!hasAm && !hasPm && beginHour <= Constants.HalfDayHourCount && endHour <= Constants.HalfDayHourCount)
            {
                if (beginHour > endHour)
                {
                    if (beginHour == Constants.HalfDayHourCount)
                    {
                        beginHour = 0;
                    }
                    else
                    {
                        endHour += Constants.HalfDayHourCount;
                    }
                }

                comments = Constants.Comment_AmPm;
            }

            return comments;
        }

        private DateTimeResolutionResult MergeTwoTimePoints(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            DateTimeParseResult pr1 = null, pr2 = null;
            bool bothHaveDates = false, beginHasDate = false, endHasDate = false;

            var timeExtractResults = this.Config.TimeExtractor.Extract(text, referenceTime);
            var dateTimeExtractResults = this.Config.DateTimeExtractor.Extract(text, referenceTime);

            if (dateTimeExtractResults.Count == 2)
            {
                pr1 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[0], referenceTime);
                pr2 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[1], referenceTime);
                bothHaveDates = true;
            }
            else if (dateTimeExtractResults.Count == 1 && timeExtractResults.Count == 2)
            {
                if (!dateTimeExtractResults[0].IsOverlap(timeExtractResults[0]))
                {
                    pr1 = this.Config.TimeParser.Parse(timeExtractResults[0], referenceTime);
                    pr2 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[0], referenceTime);
                    endHasDate = true;
                }
                else
                {
                    pr1 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[0], referenceTime);
                    pr2 = this.Config.TimeParser.Parse(timeExtractResults[1], referenceTime);
                    beginHasDate = true;
                }
            }
            else if (dateTimeExtractResults.Count == 1 && timeExtractResults.Count == 1)
            {
                if (timeExtractResults[0].Start < dateTimeExtractResults[0].Start)
                {
                    pr1 = this.Config.TimeParser.Parse(timeExtractResults[0], referenceTime);
                    pr2 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[0], referenceTime);
                    endHasDate = true;
                }
                else if (timeExtractResults[0].Start >= dateTimeExtractResults[0].Start + dateTimeExtractResults[0].Length)
                {
                    pr1 = this.Config.DateTimeParser.Parse(dateTimeExtractResults[0], referenceTime);
                    pr2 = this.Config.TimeParser.Parse(timeExtractResults[0], referenceTime);
                    beginHasDate = true;
                }
                else
                {
                    // If the only TimeExtractResult is part of DateTimeExtractResult, then it should not be handled in this method
                    return ret;
                }
            }
            else if (timeExtractResults.Count == 2)
            {
                // If both ends are Time. then this is a TimePeriod, not a DateTimePeriod
                return ret;
            }
            else
            {
                return ret;
            }

            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            DateObject futureBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue,
                       futureEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            DateObject pastBegin = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue,
                       pastEnd = (DateObject)((DateTimeResolutionResult)pr2.Value).PastValue;

            if (bothHaveDates)
            {
                if (futureBegin > futureEnd)
                {
                    futureBegin = pastBegin;
                }

                if (pastEnd < pastBegin)
                {
                    pastEnd = futureEnd;
                }
            }

            if (bothHaveDates)
            {
                ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((futureEnd - futureBegin).TotalHours)}H)";

                // Do nothing
            }
            else if (beginHasDate)
            {
                futureEnd = DateObject.MinValue.SafeCreateFromValue(
                    futureBegin.Year, futureBegin.Month, futureBegin.Day, futureEnd.Hour, futureEnd.Minute, futureEnd.Second);

                pastEnd = DateObject.MinValue.SafeCreateFromValue(
                    pastBegin.Year, pastBegin.Month, pastBegin.Day, pastEnd.Hour, pastEnd.Minute, pastEnd.Second);

                var dateStr = pr1.TimexStr.Split('T')[0];
                var durationStr = DateTimeFormatUtil.LuisTimeSpan(futureEnd - futureBegin);
                ret.Timex = $"({pr1.TimexStr},{dateStr + pr2.TimexStr},{durationStr}";
            }
            else if (endHasDate)
            {
                futureBegin = DateObject.MinValue.SafeCreateFromValue(
                    futureEnd.Year, futureEnd.Month, futureEnd.Day, futureBegin.Hour, futureBegin.Minute, futureBegin.Second);

                pastBegin = DateObject.MinValue.SafeCreateFromValue(
                    pastEnd.Year, pastEnd.Month, pastEnd.Day, pastBegin.Hour, pastBegin.Minute, pastBegin.Second);

                var dateStr = pr2.TimexStr.Split('T')[0];
                var durationStr = DateTimeFormatUtil.LuisTimeSpan(pastEnd - pastBegin);
                ret.Timex = $"({dateStr + pr1.TimexStr},{pr2.TimexStr},PT{Convert.ToInt32((pastEnd - pastBegin).TotalHours)}H)";
            }

            var ampmStr1 = ((DateTimeResolutionResult)pr1.Value).Comment;
            var ampmStr2 = ((DateTimeResolutionResult)pr2.Value).Comment;
            if (!string.IsNullOrEmpty(ampmStr1) && ampmStr1.EndsWith(Constants.Comment_AmPm) &&
                !string.IsNullOrEmpty(ampmStr2) && ampmStr2.EndsWith(Constants.Comment_AmPm))
            {
                ret.Comment = Constants.Comment_AmPm;
            }

            if (((DateTimeResolutionResult)pr1.Value).TimeZoneResolution != null)
            {
                ret.TimeZoneResolution = ((DateTimeResolutionResult)pr1.Value).TimeZoneResolution;
            }
            else if (((DateTimeResolutionResult)pr2.Value).TimeZoneResolution != null)
            {
                ret.TimeZoneResolution = ((DateTimeResolutionResult)pr2.Value).TimeZoneResolution;
            }

            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;

            ret.SubDateTimeEntities = new List<object> { pr1, pr2 };

            return ret;
        }

        // TODO: this can be abstracted with the similar method in BaseDatePeriodParser
        // Parse "in 20 minutes"
        private DateTimeResolutionResult ParseDuration(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            // For the rest of datetime, it will be handled in next function
            if (Config.RestOfDateTimeRegex.IsMatch(text))
            {
                return ret;
            }

            var ers = Config.DurationExtractor.Extract(text, referenceTime);
            if (ers.Count == 1)
            {
                var pr = Config.DurationParser.Parse(ers[0]);

                var beforeStr = text.Substring(0, pr.Start ?? 0).Trim().ToLowerInvariant();
                var afterStr = text.Substring((pr.Start ?? 0) + (pr.Length ?? 0)).Trim().ToLowerInvariant();

                var numbersInSuffix = Config.CardinalExtractor.Extract(beforeStr);
                var numbersInDuration = Config.CardinalExtractor.Extract(ers[0].Text);

                // Handle cases like "2 upcoming days", "5 previous years"
                if (numbersInSuffix.Any() && !numbersInDuration.Any())
                {
                    var numberEr = numbersInSuffix.First();
                    var numberText = numberEr.Text;
                    var durationText = ers[0].Text;
                    var combinedText = $"{numberText} {durationText}";
                    var combinedDurationEr = Config.DurationExtractor.Extract(combinedText, referenceTime);

                    if (combinedDurationEr.Any())
                    {
                        pr = Config.DurationParser.Parse(combinedDurationEr.First());
                        var startIndex = numberEr.Start.Value + numberEr.Length.Value;
                        beforeStr = beforeStr.Substring(startIndex).Trim();
                    }
                }

                if (pr.Value != null)
                {
                    var swiftSeconds = 0;
                    var mod = string.Empty;
                    var durationResult = (DateTimeResolutionResult)pr.Value;
                    if (durationResult.PastValue is double && durationResult.FutureValue is double)
                    {
                        swiftSeconds = (int)((double)durationResult.FutureValue);
                    }

                    DateObject beginTime;
                    var endTime = beginTime = referenceTime;

                    if (Config.PreviousPrefixRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        mod = Constants.BEFORE_MOD;
                        beginTime = referenceTime.AddSeconds(-swiftSeconds);
                    }

                    // Handle the "within (the) (next) xx seconds/minutes/hours" case
                    // Should also handle the multiple duration case like P1DT8H
                    // Set the beginTime equal to reference time for now
                    if (Config.WithinNextPrefixRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        endTime = beginTime.AddSeconds(swiftSeconds);
                    }

                    if (Config.FutureRegex.IsExactMatch(beforeStr, trim: true))
                    {
                        mod = Constants.AFTER_MOD;
                        endTime = beginTime.AddSeconds(swiftSeconds);
                    }

                    if (Config.PreviousPrefixRegex.IsExactMatch(afterStr, trim: true))
                    {
                        mod = Constants.BEFORE_MOD;
                        beginTime = referenceTime.AddSeconds(-swiftSeconds);
                    }

                    if (Config.FutureRegex.IsExactMatch(afterStr, trim: true))
                    {
                        mod = Constants.AFTER_MOD;
                        endTime = beginTime.AddSeconds(swiftSeconds);
                    }

                    if (Config.FutureSuffixRegex.IsExactMatch(afterStr, trim: true))
                    {
                        mod = Constants.AFTER_MOD;
                        endTime = beginTime.AddSeconds(swiftSeconds);
                    }

                    ret.Timex =
                        $"({DateTimeFormatUtil.LuisDate(beginTime)}T{DateTimeFormatUtil.LuisTime(beginTime)}," +
                        $"{DateTimeFormatUtil.LuisDate(endTime)}T{DateTimeFormatUtil.LuisTime(endTime)}," +
                        $"{durationResult.Timex})";

                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
                    ret.Success = true;

                    if (!string.IsNullOrEmpty(mod))
                    {
                        ((DateTimeResolutionResult)pr.Value).Mod = mod;
                    }

                    ret.SubDateTimeEntities = new List<object> { pr };

                    return ret;
                }
            }

            return ret;
        }

        // Parse "last minute", "next hour"
        private DateTimeResolutionResult ParseRelativeUnit(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var match = Config.RelativeTimeUnitRegex.Match(text);

            if (!match.Success)
            {
                match = this.Config.RestOfDateTimeRegex.Match(text);
            }

            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLower();

                var unitStr = Config.UnitMap[srcUnit];

                int swiftValue = 1;
                var prefixMatch = Config.PreviousPrefixRegex.Match(text);
                if (prefixMatch.Success)
                {
                    swiftValue = -1;
                }

                DateObject beginTime;
                var endTime = beginTime = referenceTime;
                var sufixPtTimex = string.Empty;

                if (Config.UnitMap.ContainsKey(srcUnit))
                {
                    switch (unitStr)
                    {
                        case "D":
                            endTime = DateObject.MinValue.SafeCreateFromValue(beginTime.Year, beginTime.Month, beginTime.Day);
                            endTime = endTime.AddDays(1).AddSeconds(-1);
                            sufixPtTimex = "PT" + (endTime - beginTime).TotalSeconds + "S";
                            break;
                        case "H":
                            beginTime = swiftValue > 0 ? beginTime : referenceTime.AddHours(swiftValue);
                            endTime = swiftValue > 0 ? referenceTime.AddHours(swiftValue) : endTime;
                            sufixPtTimex = "PT1H";
                            break;
                        case "M":
                            beginTime = swiftValue > 0 ? beginTime : referenceTime.AddMinutes(swiftValue);
                            endTime = swiftValue > 0 ? referenceTime.AddMinutes(swiftValue) : endTime;
                            sufixPtTimex = "PT1M";
                            break;
                        case "S":
                            beginTime = swiftValue > 0 ? beginTime : referenceTime.AddSeconds(swiftValue);
                            endTime = swiftValue > 0 ? referenceTime.AddSeconds(swiftValue) : endTime;
                            sufixPtTimex = "PT1S";
                            break;
                        default:
                            return ret;
                    }

                    ret.Timex =
                            $"({DateTimeFormatUtil.LuisDate(beginTime)}T{DateTimeFormatUtil.LuisTime(beginTime)}," +
                            $"{DateTimeFormatUtil.LuisDate(endTime)}T{DateTimeFormatUtil.LuisTime(endTime)},{sufixPtTimex})";

                    ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginTime, endTime);
                    ret.Success = true;

                    return ret;
                }
            }

            return ret;
        }
    }
}