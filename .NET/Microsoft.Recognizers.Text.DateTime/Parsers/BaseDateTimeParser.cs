using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIME; // "DateTime";
        
        private readonly IDateTimeParserConfiguration config;

        public BaseDateTimeParser(IDateTimeParserConfiguration configuration)
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
                var innerResult = MergeDateAndTime(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseBasicRegex(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseTimeOfToday(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParseSpecialTimeOfDate(er.Text, referenceTime);
                }

                if (!innerResult.Success)
                {
                    innerResult = ParserDurationWithAgoAndLater(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime((DateObject) innerResult.FutureValue)}
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime((DateObject) innerResult.PastValue)}
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
                TimexStr = value == null ? "" : ((DateTimeResolutionResult) value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        private DateTimeResolutionResult ParseBasicRegex(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimedText = text.Trim().ToLower();

            // Handle "now"
            var match = this.config.NowRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                this.config.GetMatchedNowTimex(trimedText, out string timex);
                ret.Timex = timex;
                ret.FutureValue = ret.PastValue = referenceTime;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // Merge a Date entity and a Time entity
        private DateTimeResolutionResult MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            var er1 = this.config.DateExtractor.Extract(text, referenceTime);
            if (er1.Count == 0)
            {
                er1 = this.config.DateExtractor.Extract(this.config.TokenBeforeDate + text, referenceTime);
                if (er1.Count == 1)
                {
                    er1[0].Start -= this.config.TokenBeforeDate.Length;
                }
                else
                {
                    return ret;
                }
            }
            else
            {
                // This is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish),
                // the same word could mean different things (e.g a time in the day or an specific day).
                if (this.config.ContainsAmbiguousToken(text, er1[0].Text))
                {
                    return ret;
                }
            }

            var er2 = this.config.TimeExtractor.Extract(text, referenceTime);
            if (er2.Count == 0)
            {
                // Here we filter out "morning, afternoon, night..." time entities
                er2 = this.config.TimeExtractor.Extract(this.config.TokenBeforeTime + text, referenceTime);
                if (er2.Count == 1)
                {
                    er2[0].Start -= this.config.TokenBeforeTime.Length;
                }
                else if (er2.Count == 0)
                {
                    // check whether there is a number being used as a time point
                    bool hasTimeNumber = false;
                    var numErs = this.config.IntegerExtractor.Extract(text);
                    if (numErs.Count > 0 && er1.Count == 1)
                    {
                        foreach (var num in numErs)
                        {
                            var middleBegin = er1[0].Start + er1[0].Length ?? 0;
                            var middleEnd = num.Start ?? 0;
                            if (middleBegin > middleEnd)
                            {
                                continue;
                            }

                            var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                            var match = this.config.DateNumberConnectorRegex.Match(middleStr);
                            if (string.IsNullOrEmpty(middleStr) || match.Success)
                            {
                                num.Type = Constants.SYS_DATETIME_TIME;
                                er2.Add(num);
                                hasTimeNumber = true;
                            }
                        }
                    }

                    if (!hasTimeNumber)
                    {
                        return ret;
                    }
                }
            }

            // Handle cases like "Oct. 5 in the afternoon at 7:00";
            // in this case "5 in the afternoon" will be extracted as a Time entity
            var correctTimeIdx = 0;
            while (correctTimeIdx < er2.Count && er2[correctTimeIdx].IsOverlap(er1[0]))
            {
                correctTimeIdx++;
            }

            if (correctTimeIdx >= er2.Count)
            {
                return ret;
            }

            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime.Date);
            var pr2 = this.config.TimeParser.Parse(er2[correctTimeIdx], referenceTime);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var futureDate = (DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue;
            var pastDate = (DateObject)((DateTimeResolutionResult)pr1.Value).PastValue;
            var time = (DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            // Handle morning, afternoon
            if (this.config.PMTimeRegex.IsMatch(text) && hour < 12)
            {
                hour += 12;
            }
            else if (this.config.AMTimeRegex.IsMatch(text) && hour >= 12)
            {
                hour -= 12;
            }

            var timeStr = pr2.TimexStr;
            if (timeStr.EndsWith(Constants.Comment_AmPm))
            {
                timeStr = timeStr.Substring(0, timeStr.Length - 4);
            }
            timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);
            ret.Timex = pr1.TimexStr + timeStr;

            var val = (DateTimeResolutionResult) pr2.Value;
            if (hour <= 12 && !this.config.PMTimeRegex.IsMatch(text) && !this.config.AMTimeRegex.IsMatch(text) &&
                !string.IsNullOrEmpty(val.Comment))
            {
                ret.Comment = Constants.Comment_AmPm;
            }

            ret.FutureValue = DateObject.MinValue.SafeCreateFromValue(futureDate.Year, futureDate.Month, futureDate.Day, hour, min, sec);
            ret.PastValue = DateObject.MinValue.SafeCreateFromValue(pastDate.Year, pastDate.Month, pastDate.Day, hour, min, sec);
            ret.Success = true;

            // Change the value of time object
            pr2.TimexStr = timeStr;
            if (!string.IsNullOrEmpty(ret.Comment))
            {
                ((DateTimeResolutionResult)pr2.Value).Comment = ret.Comment.Equals(Constants.Comment_AmPm) ? Constants.Comment_AmPm : "";
            }
            
            // Add the date and time object in case we want to split them
            ret.SubDateTimeEntities = new List<object> {pr1, pr2};

            // Add timezone
            ret.TimeZoneResolution = ((DateTimeResolutionResult)pr2.Value).TimeZoneResolution;

            return ret;
        }

        private DateTimeResolutionResult ParseTimeOfToday(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimedText = text.ToLowerInvariant().Trim();

            int hour = 0, min = 0, sec = 0;
            string timeStr;

            var wholeMatch = this.config.SimpleTimeOfTodayAfterRegex.Match(trimedText);
            if (!(wholeMatch.Success && wholeMatch.Length == trimedText.Length))
            {
                wholeMatch = this.config.SimpleTimeOfTodayBeforeRegex.Match(trimedText);
            }

            if (wholeMatch.Success && wholeMatch.Length == trimedText.Length)
            {
                var hourStr = wholeMatch.Groups["hour"].Value;
                if (string.IsNullOrEmpty(hourStr))
                {
                    hourStr = wholeMatch.Groups["hournum"].Value.ToLower();
                    hour = this.config.Numbers[hourStr];
                }
                else
                {
                    hour = int.Parse(hourStr);
                }
                timeStr = "T" + hour.ToString("D2");
            }
            else
            {
                var ers = this.config.TimeExtractor.Extract(trimedText, referenceTime);
                if (ers.Count != 1)
                {
                    ers = this.config.TimeExtractor.Extract(this.config.TokenBeforeTime + trimedText, referenceTime);
                    if (ers.Count == 1)
                    {
                        ers[0].Start -= this.config.TokenBeforeTime.Length;
                    }
                    else
                    {
                        return ret;
                    }
                }

                var pr = this.config.TimeParser.Parse(ers[0], referenceTime);
                if (pr.Value == null)
                {
                    return ret;
                }

                var time = (DateObject) ((DateTimeResolutionResult) pr.Value).FutureValue;

                hour = time.Hour;
                min = time.Minute;
                sec = time.Second;
                timeStr = pr.TimexStr;
            }
            
            var match = this.config.SpecificTimeOfDayRegex.Match(trimedText);

            if (match.Success)
            {
                var matchStr = match.Value.ToLowerInvariant();

                // Handle "last", "next"
                var swift = this.config.GetSwiftDay(matchStr);

                var date = referenceTime.AddDays(swift).Date;

                // Handle "morning", "afternoon"
                hour = this.config.GetHour(matchStr, hour);

                // In this situation, timeStr cannot end up with "ampm", because we always have a "morning" or "night"
                if (timeStr.EndsWith(Constants.Comment_AmPm))
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 4);
                }
                timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);

                ret.Timex = FormatUtil.FormatDate(date) + timeStr;
                ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(date.Year, date.Month, date.Day, hour, min, sec);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseSpecialTimeOfDate(string text, DateObject refDateTime)
        {
            var ret = new DateTimeResolutionResult();

            var ers = this.config.DateExtractor.Extract(text, refDateTime);
            if (ers.Count != 1)
            {
                return ret;
            }

            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            if (this.config.TheEndOfRegex.IsMatch(beforeStr))
            {
                var pr = this.config.DateParser.Parse(ers[0], refDateTime);
                var futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                var pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;
                ret.Timex = pr.TimexStr + "T23:59";
                ret.FutureValue = futureDate.AddDays(1).AddMinutes(-1);
                ret.PastValue = pastDate.AddDays(1).AddMinutes(-1);
                ret.Success = true;
            }

            return ret;
        }

        // Handle cases like "two hours ago" 
        private DateTimeResolutionResult ParserDurationWithAgoAndLater(string text, DateObject referenceTime)
        {

            return AgoLaterUtil.ParseDurationWithAgoAndLater(text, referenceTime,
                config.DurationExtractor, config.DurationParser, config.UnitMap, config.UnitRegex, 
                config.UtilityConfiguration, config.GetSwiftDay);
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

    }
}