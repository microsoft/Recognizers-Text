using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Extractors
{
    public class DateTimeExtractorChs : IExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        public static readonly Regex PrepositionRegex = new Regex(@"(?<prep>^的|在$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NowRegex =
            new Regex(@"(?<now>现在|马上|立刻|刚刚才|刚刚|刚才)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex = new Regex(@"(?<night>早|晚)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayRegex = new Regex(@"(今晚|今早|今晨|明晚|明早|明晨|昨晚)(的|在)?",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly DateExtractorChs _datePointExtractor = new DateExtractorChs();
        private static readonly TimeExtractorChs _timePointExtractor = new TimeExtractorChs();

        public List<ExtractResult> Extract(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTime(text));
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(TimeOfToday(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        // match now
        public List<Token> BasicRegexMatch(string text)
        {
            var ret = new List<Token>();
            text = text.Trim().ToLower();

            // handle "now"
            var matches = NowRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }

        // merge a Date entity and a Time entity, like "明天早上七点"
        public List<Token> MergeDateAndTime(string text)
        {
            var ret = new List<Token>();
            var ers = _datePointExtractor.Extract(text);
            if (ers.Count == 0)
            {
                return ret;
            }

            ers.AddRange(_timePointExtractor.Extract(text));
            if (ers.Count < 2)
            {
                return ret;
            }


            ers.Sort(delegate(ExtractResult er1, ExtractResult er2)
            {
                var start1 = er1.Start ?? 0;
                var start2 = er2.Start ?? 0;
                if (start1 < start2)
                {
                    return -1;
                }
                if (start1 == start2)
                {
                    return 0;
                }
                return 1;
            });

            var i = 0;
            while (i < ers.Count - 1)
            {
                var j = i + 1;
                while (j < ers.Count && ers[i].IsOverlap(ers[j]))
                {
                    j++;
                }
                if (j >= ers.Count)
                {
                    break;
                }

                if (ers[i].Type.Equals(Constants.SYS_DATETIME_DATE) && ers[j].Type.Equals(Constants.SYS_DATETIME_TIME))
                {
                    var middleBegin = ers[i].Start + ers[i].Length ?? 0;
                    var middleEnd = ers[j].Start ?? 0;
                    if (middleBegin > middleEnd)
                    {
                        break;
                    }
                    var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLower();
                    if (string.IsNullOrEmpty(middleStr) || middleStr.Equals(",") ||
                        PrepositionRegex.IsMatch(middleStr))
                    {
                        var begin = ers[i].Start ?? 0;
                        var end = (ers[j].Start ?? 0) + (ers[j].Length ?? 0);
                        ret.Add(new Token(begin, end));
                    }
                    i = j + 1;
                    continue;
                }
                i = j;
            }

            return ret;
        }

        // parse a specific time of today, tonight, this afternoon, "今天下午七点"
        public List<Token> TimeOfToday(string text)
        {
            var ret = new List<Token>();
            var ers = _timePointExtractor.Extract(text);
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(0, er.Start ?? 0);

                // handle "今晚7点"
                var innerMatch = NightRegex.Match(er.Text);
                if (innerMatch.Success && innerMatch.Index == 0)
                {
                    beforeStr = text.Substring(0, (er.Start ?? 0) + innerMatch.Length);
                }


                if (string.IsNullOrEmpty(beforeStr))
                {
                    continue;
                }
                var match = TimeOfTodayRegex.Match(beforeStr);
                if (match.Success && string.IsNullOrWhiteSpace(beforeStr.Substring(match.Index + match.Length)))
                {
                    var begin = match.Index;
                    var end = er.Start + er.Length ?? 0;
                    ret.Add(new Token(begin, end));
                }
            }

            return ret;
        }
    }
}