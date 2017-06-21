using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
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
                if (!innerResult.Success)
                {
                    innerResult = ParserDurationWithAgoAndLater(er.Text, referenceTime);
                }
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
                TimexStr = value == null ? "" : ((DTParseResult)value).Timex,
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

        // handle like "two hours ago" 
        private DTParseResult ParserDurationWithAgoAndLater(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var duration_res = config.DurationExtractor.Extract(text);
            var numStr = string.Empty;
            var unitStr = string.Empty;
            if (duration_res.Count > 0)
            {
                var match = this.config.UnitRegex.Match(text);
                if (match.Success)
                {
                    var AfterStr =
                        text.Substring((int)duration_res[0].Start + (int)duration_res[0].Length)
                            .Trim()
                            .ToLowerInvariant();
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr =
                        text.Substring((int)duration_res[0].Start, match.Index - (int)duration_res[0].Start)
                            .Trim()
                            .ToLowerInvariant();
                    var er = config.CardinalExtractor.Extract(numberStr);
                    if (er.Count != 0)
                    {
                        var pr = config.NumberParser.Parse(er[0]);

                        var number = int.Parse(pr.ResolutionStr);
                        if (config.UnitMap.ContainsKey(srcUnit))
                        {
                            unitStr = config.UnitMap[srcUnit];
                            numStr = number.ToString();
                            if (config.ContainsAgoString(AfterStr))
                            {
                                DateObject Time;
                                switch (unitStr)
                                {
                                    case "H":
                                        Time = referenceTime.AddHours(-double.Parse(numStr));
                                        break;
                                    case "M":
                                        Time = referenceTime.AddMinutes(-double.Parse(numStr));
                                        break;
                                    case "S":
                                        Time = referenceTime.AddSeconds(-double.Parse(numStr));
                                        break;
                                    default:
                                        return ret;
                                }
                                ret.Timex = $"{Util.LuisTime(Time)}";
                                ret.FutureValue = ret.PastValue = Time;
                                ret.Success = true;
                                return ret;
                            }
                            if (config.ContainsLaterString(AfterStr))
                            {
                                DateObject Time;
                                switch (unitStr)
                                {
                                    case "H":
                                        Time = referenceTime.AddHours(double.Parse(numStr));
                                        break;
                                    case "M":
                                        Time = referenceTime.AddMinutes(double.Parse(numStr));
                                        break;
                                    case "S":
                                        Time = referenceTime.AddSeconds(double.Parse(numStr));
                                        break;
                                    default:
                                        return ret;
                                }
                                ret.Timex =
                                    $"{Util.LuisTime(Time)}";
                                ret.FutureValue =
                                    ret.PastValue = Time;
                                ret.Success = true;
                                return ret;
                            }
                        }
                    }
                }
            }
            return ret;
        }

    }
}