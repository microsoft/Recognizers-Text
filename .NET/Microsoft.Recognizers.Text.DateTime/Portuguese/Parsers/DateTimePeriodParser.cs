using System;
using Microsoft.Recognizers.Definitions.Portuguese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class DateTimePeriodParser : BaseDateTimePeriodParser
    {
        public DateTimePeriodParser(IDateTimePeriodParserConfiguration configuration)
            : base(configuration)
        {
        }

        protected override DateTimeResolutionResult ParseSpecificTimeOfDay(string text, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var trimmedText = text.Trim().ToLowerInvariant();

            // Handle morning, afternoon..
            if (!this.Config.GetMatchedTimeRange(trimmedText, out string timeStr, out int beginHour, out int endHour, out int endMin))
            {
                return ret;
            }

            var exactMatch = this.Config.SpecificTimeOfDayRegex.MatchExact(trimmedText, trim: true);

            if (exactMatch.Success)
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

            var startIndex = trimmedText.IndexOf(DateTimeDefinitions.Tomorrow, StringComparison.Ordinal) == 0 ? DateTimeDefinitions.Tomorrow.Length : 0;

            // Handle Date followed by morning, afternoon, ...
            // Add handling code to handle morning, afternoon followed by Date
            // Add handling code to handle early/late morning, afternoon
            var match = this.Config.TimeOfDayRegex.Match(trimmedText.Substring(startIndex));
            if (match.Success)
            {
                var beforeStr = trimmedText.Substring(0, match.Index + startIndex).Trim();
                var ers = this.Config.DateExtractor.Extract(beforeStr, referenceTime);

                if (ers.Count == 0)
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
