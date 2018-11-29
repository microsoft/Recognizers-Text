using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimeZoneUtility
    {
        public static List<ExtractResult> MergeTimeZones(List<ExtractResult> originalErs, List<ExtractResult> timeZoneErs, string text)
        {
            foreach (var er in originalErs)
            {
                foreach (var timeZoneEr in timeZoneErs)
                {
                    var begin = er.Start + er.Length;
                    var end = timeZoneEr.Start;

                    if (begin < end)
                    {
                        var gapText = text.Substring((int)begin, (int)(end - begin));

                        if (string.IsNullOrWhiteSpace(gapText))
                        {
                            var newLength = (int)(timeZoneEr.Start + timeZoneEr.Length - er.Start);

                            er.Text = text.Substring((int)er.Start, newLength);
                            er.Length = newLength;
                            er.Data = new KeyValuePair<string, ExtractResult>(Constants.SYS_DATETIME_TIMEZONE, timeZoneEr);
                        }
                    }
                }
            }

            return originalErs;
        }
    }
}
