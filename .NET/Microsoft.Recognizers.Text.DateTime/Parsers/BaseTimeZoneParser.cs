using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Definitions.English;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimeZoneParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIMEZONE; //"TimeZone";

        public BaseTimeZoneParser() { }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        // GetMinutes compute minutes shift from UTC 0 from matched numbers in texts. e.g. "-4:30" -> -270; "8"-> 480; "+8"-> 480
        public int GetMinutes(string matchedNumber)
        {
            if (matchedNumber.Length == 0)
                return Constants.Illegal_minutes;

            int bs = 1; // earlier than utc, default value
            if (matchedNumber.StartsWith("+") || matchedNumber.StartsWith("-"))
            {
                if (matchedNumber.StartsWith("-"))
                    bs = -1; // later than utc 0

                matchedNumber = matchedNumber.Substring(1).Trim();
            }

            int hours = 0;
            int minutes = 0;
            if (matchedNumber.Contains(":"))
            {
                List<string> splitParts = matchedNumber.Split(':').ToList();
                string f1 = splitParts[0];
                string f2 = splitParts[1];
                hours = int.Parse(f1);
                minutes = int.Parse(f2);
            }
            else if (int.TryParse(matchedNumber, out hours))
            {
                minutes = 0;
            }

            if (hours > 12)
            {
                return Constants.Illegal_minutes;
            }

            if (minutes != 0 && minutes != 15 && minutes != 30 && minutes != 45 && minutes != 60)
            {
                return Constants.Illegal_minutes;
            }

            int totalm = hours * 60 + minutes;
            totalm *= bs;
            return totalm;
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            DateTimeParseResult result = null;
            result = new DateTimeParseResult
            {
                Start = er.Start,
                Length = er.Length,
                Text = er.Text,
                Type = er.Type
            };

            SortedDictionary<string, object> val = new SortedDictionary<string, object>();
            string text = er.Text.ToLower();
            string matched = Regex.Match(text, TimeZoneDefinitions.DirectUTCRegex).Groups[2].Value;
            int tmpMinutes = GetMinutes(matched);
            if (tmpMinutes != Constants.Illegal_minutes)
            {
                val[Constants.UTCSHIFT] = tmpMinutes;
                result.Value = val;
                result.ResolutionStr = Constants.UTCSHIFT + ": " + tmpMinutes.ToString();
            }
            else if (TimeZoneDefinitions.abbr2Minute.ContainsKey(text))
            {
                int utcMinuteShift = TimeZoneDefinitions.abbr2Minute[text];
                val[Constants.UTCSHIFT] = utcMinuteShift;
                result.Value = val;
                result.ResolutionStr = Constants.UTCSHIFT + ": " + utcMinuteShift.ToString();
            }
            else if(TimeZoneDefinitions.full2Minute.ContainsKey(text))
            {
                int utcMinuteShift = TimeZoneDefinitions.full2Minute[text.ToLower()];
                val[Constants.UTCSHIFT] = utcMinuteShift;
                result.Value = val;
                result.ResolutionStr = Constants.UTCSHIFT + ": " + utcMinuteShift.ToString();
            }
            else
            {
                result.Value = null;
                result.ResolutionStr = "";
            }

            return result;
        }
    }
}
