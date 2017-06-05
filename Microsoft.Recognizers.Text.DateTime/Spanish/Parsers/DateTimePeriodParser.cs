using System;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class DateTimePeriodParser : BaseDateTimePeriodParser
    {
        public DateTimePeriodParser(IDateTimePeriodParserConfiguration configuration) : base(configuration)
        {
        }

        protected override DTParseResult ParseSpecificNight(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var trimedText = text.Trim().ToLowerInvariant();
            // handle morning, afternoon..
            int beginHour, endHour, endMin = 0;
            var timeStr = string.Empty;
            if (!this.config.GetMatchedTimeRange(trimedText, out timeStr, out beginHour, out endHour, out endMin))
            {
                return ret;
            }

            var match = this.config.SpecificNightRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var swift = this.config.GetSwiftPrefix(trimedText);

                var date = referenceTime.AddDays(swift).Date;
                int day = date.Day, month = date.Month, year = date.Year;

                ret.Timex = Util.FormatDate(date) + timeStr;
                ret.FutureValue =
                    ret.PastValue =
                        new Tuple<DateObject, DateObject>(new DateObject(year, month, day, beginHour, 0, 0),
                            new DateObject(year, month, day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            var startIndex = trimedText.IndexOf("mañana") == 0 ? 6 : 0;

            // handle Date followed by morning, afternoon
            match = this.config.NightRegex.Match(trimedText.Substring(startIndex));
            if (match.Success)
            {
                var beforeStr = trimedText.Substring(0, match.Index + startIndex).Trim();
                var ers = this.config.DateExtractor.Extract(beforeStr);
                if (ers.Count == 0)
                {
                    return ret;
                }
                var pr = this.config.DateParser.Parse(ers[0], referenceTime);
                var futureDate = (DateObject)((DTParseResult)pr.Value).FutureValue;
                var pastDate = (DateObject)((DTParseResult)pr.Value).PastValue;
                ret.Timex = pr.TimexStr + timeStr;
                ret.FutureValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, beginHour, 0, 0),
                        new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, endHour, endMin, endMin));
                ret.PastValue =
                    new Tuple<DateObject, DateObject>(
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, beginHour, 0, 0),
                        new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, endHour, endMin, endMin));
                ret.Success = true;
                return ret;
            }

            return ret;
        }
    }
}
