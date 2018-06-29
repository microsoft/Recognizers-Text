using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeZoneParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEZONE; //"TimeZone";

        public ParseResult Parse(ExtractResult result)
        {
            return Parse(result, DateObject.Now);
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        // Compute UTC offset in minutes from matched timezone offset in text. e.g. "-4:30" -> -270; "+8"-> 480.
        public int ComputeMinutes(string utcOffset)
        {
            if (utcOffset.Length == 0)
            {
                return Constants.InvalidOffsetValue;
            }

            int sign = Constants.PositiveSign; // later than utc, default value
            if (utcOffset.StartsWith("+") || utcOffset.StartsWith("-") || utcOffset.StartsWith("±"))
            {
                if (utcOffset.StartsWith("-"))
                {
                    sign = Constants.NegativeSign; // earlier than utc 0
                }

                utcOffset = utcOffset.Substring(1).Trim();
            }

            int hours = 0;
            int minutes = 0;
            if (utcOffset.Contains(":"))
            {
                var tokens = utcOffset.Split(':').ToList();
                hours = int.Parse(tokens[0]);
                minutes = int.Parse(tokens[1]);
            }
            else if (int.TryParse(utcOffset, out hours))
            {
                minutes = 0;
            }

            if (hours > Constants.HalfDayHourCount)
            {
                return Constants.InvalidOffsetValue;
            }

            if (minutes != 0 && minutes != 15 && minutes != 30 && minutes != 45 && minutes != 60)
            {
                return Constants.InvalidOffsetValue;
            }

            int offsetInMinutes = hours * 60 + minutes;
            offsetInMinutes *= sign;

            return offsetInMinutes;
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            DateTimeParseResult result;
            result = new DateTimeParseResult
            {
                Start = er.Start,
                Length = er.Length,
                Text = er.Text,
                Type = er.Type
            };
            
            string text = er.Text.ToLower();
            string matched = Regex.Match(text, TimeZoneDefinitions.DirectUtcRegex).Groups[2].Value;
            int offsetInMinutes = ComputeMinutes(matched);

            if (offsetInMinutes != Constants.InvalidOffsetValue)
            {
                result.Value = GetDateTimeResolutionResult(offsetInMinutes, text);
                result.ResolutionStr = Constants.UtcOffsetMinsKey + ": " + offsetInMinutes;
            }
            else if (TimeZoneDefinitions.AbbrToMinMapping.ContainsKey(text) &&
                     TimeZoneDefinitions.AbbrToMinMapping[text] != Constants.InvalidOffsetValue)
            {
                int utcMinuteShift = TimeZoneDefinitions.AbbrToMinMapping[text];

                result.Value = GetDateTimeResolutionResult(utcMinuteShift, text);
                result.ResolutionStr = Constants.UtcOffsetMinsKey + ": " + utcMinuteShift;
            }
            else if (TimeZoneDefinitions.FullToMinMapping.ContainsKey(text))
            {
                int utcMinuteShift = TimeZoneDefinitions.FullToMinMapping[text.ToLower()];
                result.Value = GetDateTimeResolutionResult(utcMinuteShift, text);
                result.ResolutionStr = Constants.UtcOffsetMinsKey + ": " + utcMinuteShift;
            }
            else
            {
                // TODO: Temporary solution for city timezone and ambiguous data
                result.Value = new DateTimeResolutionResult
                {
                    Success = true,
                    TimeZoneResolution = new TimeZoneResolutionResult
                    {
                        Value = "UTC+XX:XX",
                        UtcOffsetMins = Constants.InvalidOffsetValue,
                        TimeZoneText = text
                    }
                };
                result.ResolutionStr = Constants.UtcOffsetMinsKey + ": XX:XX";
            }

            return result;
        }

        public DateTimeResolutionResult GetDateTimeResolutionResult(int offsetMins, string text)
        {
            var val = new DateTimeResolutionResult
            {
                Success = true,
                TimeZoneResolution = new TimeZoneResolutionResult
                {
                    Value = ConvertOffsetInMinsToOffsetString(offsetMins),
                    UtcOffsetMins = offsetMins,
                    TimeZoneText = text
                }
            };

            return val;
        }

        public string ConvertOffsetInMinsToOffsetString(int offsetMins)
        {
            return $"UTC{(offsetMins >= 0 ? "+" : "-")}{ConvertMinsToRegularFormat(Math.Abs(offsetMins))}";
        }

        public string ConvertMinsToRegularFormat(int offsetMins)
        {
            return TimeSpan.FromMinutes(offsetMins).ToString(@"hh\:mm");
        }
    }
}