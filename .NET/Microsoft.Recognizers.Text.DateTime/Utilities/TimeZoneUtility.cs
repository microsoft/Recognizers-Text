using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class TimeZoneUtility
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex BracketRegex =
            new Regex(BaseDateTime.BracketRegex, RegexFlags);

        public static List<ExtractResult> MergeTimeZones(List<ExtractResult> originalErs, List<ExtractResult> timeZoneErs, string text)
        {
            foreach (var er in originalErs)
            {
                foreach (var timeZoneEr in timeZoneErs)
                {
                    // Extend timezone extraction to include brackets if any.
                    var tzEr = ExtendTimeZoneExtraction(timeZoneEr, text);

                    var begin = er.Start + er.Length;
                    var end = tzEr.Start;

                    if (begin < end)
                    {
                        var gapText = text.Substring((int)begin, (int)(end - begin));

                        if (string.IsNullOrWhiteSpace(gapText))
                        {
                            var newLength = (int)(tzEr.Start + tzEr.Length - er.Start);

                            er.Text = text.Substring((int)er.Start, newLength);
                            er.Length = newLength;
                            er.Data = new Dictionary<string, object>()
                            {
                                { Constants.SYS_DATETIME_TIMEZONE, timeZoneEr },
                            };
                        }
                    }

                    // Make sure timezone info propagates to longer span entity.
                    if (er.IsOverlap(timeZoneEr))
                    {
                        er.Data = new Dictionary<string, object>()
                            {
                                { Constants.SYS_DATETIME_TIMEZONE, timeZoneEr },
                            };
                    }
                }
            }

            return originalErs;
        }

        public static bool ShouldResolveTimeZone(ExtractResult er, DateTimeOptions options)
        {
            var enablePreview = (options & DateTimeOptions.EnablePreview) != 0;
            if (!enablePreview)
            {
                return false;
            }

            var hasTimeZoneData = false;

            if (er.Data is Dictionary<string, object>)
            {
                var metaData = er.Data as Dictionary<string, object>;

                if (metaData != null && metaData.ContainsKey(Constants.SYS_DATETIME_TIMEZONE))
                {
                    hasTimeZoneData = true;
                }
            }

            return hasTimeZoneData;
        }

        public static StringMatcher BuildMatcherFromLists(params List<string>[] collections)
        {
            StringMatcher matcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
            List<string> matcherList = new List<string>();

            foreach (List<string> collection in collections)
            {
                collection.ForEach(o => matcherList.Add(o.Trim().ToLowerInvariant()));
            }

            matcherList = matcherList.Distinct().ToList();

            matcher.Init(matcherList);

            return matcher;
        }

        private static ExtractResult ExtendTimeZoneExtraction(ExtractResult timeZoneEr, string text)
        {
            var beforeStr = text.Substring(0, (int)timeZoneEr.Start);
            var afterStr = text.Substring((int)timeZoneEr.Start + (int)timeZoneEr.Length);
            var matchLeft = BracketRegex.Match(beforeStr);
            var matchRight = BracketRegex.Match(afterStr);
            if (matchLeft.Success && matchRight.Success)
            {
                timeZoneEr.Start -= matchLeft.Length;
                timeZoneEr.Length += matchLeft.Length + matchRight.Length;
            }

            return timeZoneEr;
        }
    }
}
