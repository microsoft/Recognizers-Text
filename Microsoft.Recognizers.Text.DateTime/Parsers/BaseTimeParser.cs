using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public class BaseTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIME; //"Time";

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
                var innerResult = InternalParse(er.Text, referenceTime);
                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, Util.FormatTime((DateObject) innerResult.FutureValue)}
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, Util.FormatTime((DateObject) innerResult.PastValue)}
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

        protected virtual DTParseResult InternalParse(string text, DateObject referenceTime)
        {
            var innerResult = ParseBasicRegexMatch(text, referenceTime);
            return innerResult;
        }

        // parse basic patterns in TimeRegexList
        private DTParseResult ParseBasicRegexMatch(string text, DateObject referenceTime)
        {
            var trimedText = text.Trim().ToLowerInvariant();
            var offset = 0;

            var match = this.config.AtRegex.Match(trimedText);
            if (!match.Success)
            {
                match = this.config.AtRegex.Match(this.config.TimeTokenPrefix + trimedText);
                offset = this.config.TimeTokenPrefix.Length;
            }
            if (match.Success && match.Index == offset && match.Length == trimedText.Length)
            {
                return Match2Time(match, referenceTime);
            }

            var regexes = this.config.TimeRegexes;

            foreach (var regex in regexes)
            {
                offset = 0;
                match = regex.Match(trimedText);

                if (match.Success && match.Index == offset && match.Length == trimedText.Length)
                {
                    return Match2Time(match, referenceTime);
                }
            }
            return new DTParseResult();
        }

        private DTParseResult Match2Time(Match match, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            int hour = 0,
                min = 0,
                second = 0,
                day = referenceTime.Day,
                month = referenceTime.Month,
                year = referenceTime.Year;
            bool hasMin = false, hasSec = false, hasAm = false, hasPm = false;

            var engTimeStr = match.Groups["engtime"].Value;
            if (!string.IsNullOrEmpty(engTimeStr))
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
            else
            {
                // get hour
                var hourStr = match.Groups["hour"].Value;
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
                    hour = int.Parse(hourStr);
                }

                // get minute
                var minStr = match.Groups["min"].Value.ToLower();
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
                var secStr = match.Groups["sec"].Value.ToLower();
                if (!string.IsNullOrEmpty(secStr))
                {
                    second = int.Parse(secStr);
                    hasSec = true;
                }
            }
            
            //adjust by desc string
            var descStr = match.Groups["desc"].Value.ToLower();
            if (!string.IsNullOrEmpty(descStr))
            {
                if (descStr.ToLower().StartsWith("a"))
                {
                    if (hour >= 12)
                    {
                        hour -= 12;
                    }
                    hasAm = true;
                }
                else if (descStr.ToLower().StartsWith("p"))
                {
                    if (hour < 12)
                    {
                        hour += 12;
                    }
                    hasPm = true;
                }
            }

            // adjust min by prefix
            var timePrefix = match.Groups["prefix"].Value.ToLower();
            if (!string.IsNullOrEmpty(timePrefix))
            {
                this.config.AdjustByPrefix(timePrefix, ref hour, ref min, ref hasMin);
            }

            // adjust hour by suffix
            var timeSuffix = match.Groups["suffix"].Value.ToLower();
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
            if (hour <= 12 && !hasPm && !hasAm)
            {
                ret.comment = "ampm";
            }
            
            ret.FutureValue = ret.PastValue = new DateObject(year, month, day, hour, min, second);
            ret.Success = true;
            return ret;
        }
    }
}