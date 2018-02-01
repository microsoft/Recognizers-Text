using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.English;

namespace Microsoft.Recognizers.Text.TimeZone.English
{
    class TimeZoneParser : IParser
    {
        public TimeZoneParser()
        {

        }
        public int getMinutes(string matched)
        {
            int bs = 1;
            if (matched.StartsWith("+")
                || matched.StartsWith("-"))
            {
                if (matched.StartsWith("-"))
                    bs = -1;

                matched = matched.Substring(1).Trim();
            }
            int h = 0;
            int m = 0;
            if (matched.Contains(":"))
            {
                string f1 = matched.Split(':')[0];
                string f2 = matched.Split(':')[1];
                h = int.Parse(f1);
                m = int.Parse(f2);
            }
            else if (int.TryParse(matched, out h))
            {
                m = 0;
            }

            if (h > 12)
            {
                return -10000;
            }

            if (m != 0 && m != 15 && m != 30 && m != 45 && m != 60)
            {
                return -10000;
            }
            int totalm = h * 60 + m;
            totalm *= bs;
            return totalm;
        }
        public ParseResult Parse(ExtractResult extResult)
        {
            ParseResult ret = null;
            string extra;
            ret = new ParseResult
            {
                Start = extResult.Start,
                Length = extResult.Length,
                Text = extResult.Text,
                Type = extResult.Type
            };
            if ((extra = extResult.Data as string).Equals("DirectUTC"))
            {
                string text = extResult.Text.ToLower();
                string matched = Regex.Match(text, TimeZoneDefinitions.DirectUTCRegex).Groups[2].Value;
                int tmpMinutes = getMinutes(matched);
                if(tmpMinutes!=-10000)
                {
                    ret.Value = tmpMinutes;
                    ret.ResolutionStr = "UTC MINUTES SHIFT: " + tmpMinutes.ToString();
                    return ret;
                }
            }
            if ((extra = extResult.Data as string).Equals("Abbr"))
            {
                string text = extResult.Text.ToLower();
                if(TimeZoneDefinitions.abbr2Minute.ContainsKey(text))
                {
                    int utcMinuteShift = TimeZoneDefinitions.abbr2Minute[text];
                    ret.Value = utcMinuteShift;
                    ret.ResolutionStr = "UTC MINUTES SHIFT: " + utcMinuteShift.ToString();
                    return ret;
                }
            }
            if ((extra = extResult.Data as string).Equals("Full"))
            {
                string text = extResult.Text.ToLower();
                if(TimeZoneDefinitions.full2Minute.ContainsKey(text))
                {
                    int utcMinuteShift = TimeZoneDefinitions.full2Minute[text.ToLower()];
                    ret.Value = utcMinuteShift;
                    ret.ResolutionStr = "UTC MINUTES SHIFT: " + utcMinuteShift.ToString();
                    return ret;
                }
            }
            return ret;
        }
    }
}
