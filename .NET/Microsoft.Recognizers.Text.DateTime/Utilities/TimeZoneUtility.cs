using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class TimeZoneUtility
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
                            er.Data = new Dictionary<string, object>()
                            {
                                { Constants.SYS_DATETIME_TIMEZONE, timeZoneEr },
                            };
                        }
                    }
                }
            }

            return originalErs;
        }

        public static bool ShouldResolveTimeZone(ExtractResult er, DateTimeOptions options)
        {
            var enablePreview = (options & DateTimeOptions.EnablePreview) != 0;
            var hasTimeZoneData = false;

            if (er.Data is Dictionary<string, object>)
            {
                var metaData = er.Data as Dictionary<string, object>;

                if (metaData != null && metaData.ContainsKey(Constants.SYS_DATETIME_TIMEZONE))
                {
                    hasTimeZoneData = true;
                }
            }

            return enablePreview && hasTimeZoneData;
        }

        public static StringMatcher BuildMatcherFromLists(params List<string>[] collections)
        {
            StringMatcher matcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
            List<string> matcherList = new List<string>();

            foreach (List<string> collection in collections)
            {
                collection.ForEach(o => matcherList.Add(o.Trim().ToLower()));
            }

            matcherList = matcherList.Distinct().ToList();

            matcher.Init(matcherList);

            return matcher;
        }
    }
}
