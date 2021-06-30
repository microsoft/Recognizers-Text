using System;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Spanish;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class DateTimePeriodParser : BaseDateTimePeriodParser
    {
        public static readonly Regex ConnectorRegex =
            new Regex(DateTimeDefinitions.ConnectorRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public DateTimePeriodParser(IDateTimePeriodParserConfiguration configuration)
            : base(configuration)
        {
        }

        protected override DateTimeResolutionResult ParseSpecificTimeOfDay(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim();

            // handle morning, afternoon..
            if (!this.Config.GetMatchedTimeRange(trimmedText, out string timeStr, out int beginHour, out int endHour, out int endMin))
            {
                return ret;
            }

            var exactMatch = this.Config.SpecificTimeOfDayRegex.MatchExact(trimmedText, trim: true);

            if (!exactMatch.Success)
            {
                exactMatch = this.Config.PeriodTimeOfDayWithDateRegex.MatchExact(trimmedText, trim: true);
            }

            if (exactMatch.Success)
            {
                // Extract early/late prefix from text if any
                bool hasEarly = false;
                if (!string.IsNullOrEmpty(exactMatch.Groups["early"].Value))
                {
                    hasEarly = true;
                    ret.Comment = Constants.Comment_Early;
                    ret.Mod = Constants.EARLY_MOD;
                    endHour = beginHour + 2;

                    // Handling special case: night ends with 23:59 due to C# issues.
                    if (endMin == 59)
                    {
                        endMin = 0;
                    }
                }

                if (!hasEarly && !string.IsNullOrEmpty(exactMatch.Groups["late"].Value))
                {
                    ret.Comment = Constants.Comment_Late;
                    ret.Mod = Constants.LATE_MOD;
                    beginHour = beginHour + 2;
                }

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

            var startIndex = trimmedText.IndexOf(DateTimeDefinitions.Tomorrow, StringComparison.Ordinal) == 0 ? DateTimeDefinitions.Tomorrow.Length : 0;

            // handle Date preceded/followed by morning, afternoon
            // @TODO Add handling code to handle early/late morning, afternoon
            var match = this.Config.TimeOfDayRegex.Match(trimmedText.Substring(startIndex));
            if (match.Success)
            {
                var subStr = match.Index > 0 ? trimmedText.Substring(0, match.Index + startIndex).Trim() : trimmedText.Substring(match.Index + match.Length).Trim();
                var ers = this.Config.DateExtractor.Extract(subStr, referenceTime);

                if (ers.Count == 0)
                {
                    return ret;
                }

                // Check if Date and TimeOfDay are contiguous
                var middleStr = match.Index > 0 ? subStr.Substring((int)ers[0].Start + (int)ers[0].Length).Trim() : subStr.Substring(0, (int)ers[0].Start).Trim();
                if (!(string.IsNullOrWhiteSpace(middleStr) || ConnectorRegex.IsMatch(middleStr)))
                {
                    return ret;
                }

                var pr = this.Config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject)((DateTimeResolutionResult)pr.Value).FutureValue;
                var pastDate = (DateObject)((DateTimeResolutionResult)pr.Value).PastValue;

                ret.Timex = pr.TimexStr + timeStr;

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
    }
}
