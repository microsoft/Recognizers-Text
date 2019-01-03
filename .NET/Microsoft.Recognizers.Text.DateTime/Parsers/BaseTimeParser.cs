using System.Collections.Generic;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIME; // "Time";

        private readonly ITimeParserConfiguration config;

        public BaseTimeParser(ITimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject referenceTime)
        {
            object value = null;
            if (er.Type.Equals(ParserName))
            {
                DateTimeResolutionResult innerResult;

                // Resolve timezome
                if (TimeZoneUtility.ShouldResolveTimeZone(er, config.Options))
                {
                    var metadata = er.Data as Dictionary<string, object>;
                    var timezoneEr = metadata[Constants.SYS_DATETIME_TIMEZONE] as ExtractResult;
                    var timezonePr = config.TimeZoneParser.Parse(timezoneEr);

                    innerResult = InternalParse(er.Text.Substring(0, (int)(er.Text.Length - timezoneEr.Length)), referenceTime);

                    if (timezonePr != null && timezonePr.Value != null)
                    {
                        innerResult.TimeZoneResolution = ((DateTimeResolutionResult)timezonePr.Value).TimeZoneResolution;
                    }
                }
                else
                {
                    innerResult = InternalParse(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject)innerResult.FutureValue) },
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject)innerResult.PastValue) },
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

        protected virtual DateTimeResolutionResult InternalParse(string text, DateObject referenceTime)
        {
            var innerResult = ParseBasicRegexMatch(text, referenceTime);
            return innerResult;
        }

        // parse basic patterns in TimeRegexList
        private DateTimeResolutionResult ParseBasicRegexMatch(string text, DateObject referenceTime)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var offset = 0;

            var match = this.config.AtRegex.Match(trimmedText);

            if (!match.Success)
            {
                match = this.config.AtRegex.Match(this.config.TimeTokenPrefix + trimmedText);
                offset = this.config.TimeTokenPrefix.Length;
            }

            if (match.Success && match.Index == offset && match.Length == trimmedText.Length)
            {
                return Match2Time(match, referenceTime);
            }

            // parse hour pattern, like "twenty one", "16"
            // create a extract result which content the pass-in text
            int hour;
            if (this.config.Numbers.TryGetValue(text, out hour) || int.TryParse(text, out hour))
            {
                if (hour >= 0 && hour <= 24)
                {
                    var ret = new DateTimeResolutionResult();

                    if (hour == 24)
                    {
                        hour = 0;
                    }

                    if (hour <= Constants.HalfDayHourCount && hour != 0)
                    {
                        ret.Comment = Constants.Comment_AmPm;
                    }

                    ret.Timex = "T" + hour.ToString("D2");
                    ret.FutureValue = ret.PastValue =
                        DateObject.MinValue.SafeCreateFromValue(referenceTime.Year, referenceTime.Month, referenceTime.Day, hour, 0, 0);
                    ret.Success = true;
                    return ret;
                }
            }

            var regexes = this.config.TimeRegexes;

            foreach (var regex in regexes)
            {
                var exactMatch = regex.MatchExact(trimmedText, trim: true);

                if (exactMatch.Success)
                {
                    return Match2Time(exactMatch.Match, referenceTime);
                }
            }

            return new DateTimeResolutionResult();
        }

        private DateTimeResolutionResult Match2Time(Match match, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            bool hasMin = false, hasSec = false, hasAm = false, hasPm = false, hasMid = false;
            int hour = 0,
                min = 0,
                second = 0,
                day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;

            var writtenTimeStr = match.Groups["writtentime"].Value;

            if (!string.IsNullOrEmpty(writtenTimeStr))
            {
                // get hour
                var hourStr = match.Groups["hournum"].Value.ToLower();
                hour = this.config.Numbers[hourStr];

                // get minute
                var minStr = match.Groups["minnum"].Value;
                var tensStr = match.Groups["tens"].Value;

                if (!string.IsNullOrEmpty(minStr))
                {
                    min = this.config.Numbers[minStr];
                    if (!string.IsNullOrEmpty(tensStr))
                    {
                        min += this.config.Numbers[tensStr];
                    }

                    hasMin = true;
                }
            }
            else if (!string.IsNullOrEmpty(match.Groups["mid"].Value))
            {
                hasMid = true;
                if (!string.IsNullOrEmpty(match.Groups["midnight"].Value))
                {
                    hour = 0;
                    min = 0;
                    second = 0;
                }
                else if (!string.IsNullOrEmpty(match.Groups["midmorning"].Value))
                {
                    hour = 10;
                    min = 0;
                    second = 0;
                }
                else if (!string.IsNullOrEmpty(match.Groups["midafternoon"].Value))
                {
                    hour = 14;
                    min = 0;
                    second = 0;
                }
                else if (!string.IsNullOrEmpty(match.Groups["midday"].Value))
                {
                    hour = Constants.HalfDayHourCount;
                    min = 0;
                    second = 0;
                }
            }
            else
            {
                // get hour
                var hourStr = match.Groups[Constants.HourGroupName].Value;
                if (string.IsNullOrEmpty(hourStr))
                {
                    hourStr = match.Groups["hournum"].Value.ToLower();
                    if (!this.config.Numbers.TryGetValue(hourStr, out hour))
                    {
                        return ret;
                    }
                }
                else
                {
                    if (!int.TryParse(hourStr, out hour))
                    {
                        if (!this.config.Numbers.TryGetValue(hourStr.ToLower(), out hour))
                        {
                            return ret;
                        }
                    }
                }

                // get minute
                var minStr = match.Groups[Constants.MinuteGroupName].Value.ToLower();
                if (string.IsNullOrEmpty(minStr))
                {
                    minStr = match.Groups["minnum"].Value;
                    if (!string.IsNullOrEmpty(minStr))
                    {
                        min = this.config.Numbers[minStr];
                        hasMin = true;
                    }

                    var tensStr = match.Groups["tens"].Value;
                    if (!string.IsNullOrEmpty(tensStr))
                    {
                        min += this.config.Numbers[tensStr];
                        hasMin = true;
                    }
                }
                else
                {
                    min = int.Parse(minStr);
                    hasMin = true;
                }

                // get second
                var secStr = match.Groups[Constants.SecondGroupName].Value.ToLower();
                if (!string.IsNullOrEmpty(secStr))
                {
                    second = int.Parse(secStr);
                    hasSec = true;
                }
            }

            // Adjust by desc string
            var descStr = match.Groups[Constants.DescGroupName].Value.ToLower();

            // ampm is a special case in which at 6ampm = at 6
            if (config.UtilityConfiguration.AmDescRegex.Match(descStr).Success
                || config.UtilityConfiguration.AmPmDescRegex.Match(descStr).Success
                || match.Groups[Constants.ImplicitAmGroupName].Success)
            {
                if (hour >= Constants.HalfDayHourCount)
                {
                    hour -= Constants.HalfDayHourCount;
                }

                if (!config.UtilityConfiguration.AmPmDescRegex.Match(descStr).Success)
                {
                    hasAm = true;
                }
            }
            else if (config.UtilityConfiguration.PmDescRegex.Match(descStr).Success
                || match.Groups[Constants.ImplicitPmGroupName].Success)
            {
                if (hour < Constants.HalfDayHourCount)
                {
                    hour += Constants.HalfDayHourCount;
                }

                hasPm = true;
            }

            // adjust min by prefix
            var timePrefix = match.Groups[Constants.PrefixGroupName].Value.ToLower();
            if (!string.IsNullOrEmpty(timePrefix))
            {
                this.config.AdjustByPrefix(timePrefix, ref hour, ref min, ref hasMin);
            }

            // adjust hour by suffix
            var timeSuffix = match.Groups[Constants.SuffixGroupName].Value.ToLower();
            if (!string.IsNullOrEmpty(timeSuffix))
            {
                this.config.AdjustBySuffix(timeSuffix, ref hour, ref min, ref hasMin, ref hasAm, ref hasPm);
            }

            if (hour == 24)
            {
                hour = 0;
            }

            ret.Timex = "T" + hour.ToString("D2");
            if (hasMin)
            {
                ret.Timex += ":" + min.ToString("D2");
            }

            if (hasSec)
            {
                ret.Timex += ":" + second.ToString("D2");
            }

            if (hour <= Constants.HalfDayHourCount && !hasPm && !hasAm && !hasMid)
            {
                ret.Comment = Constants.Comment_AmPm;
            }

            ret.FutureValue = ret.PastValue = DateObject.MinValue.SafeCreateFromValue(year, month, day, hour, min, second);
            ret.Success = true;

            return ret;
        }
    }
}