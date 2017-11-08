using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class DateTimeExtractorChs : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.PrepositionRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NowRegex = new Regex(DateTimeDefinitions.NowRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex NightRegex = new Regex(DateTimeDefinitions.NightRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeOfTodayRegex = new Regex(DateTimeDefinitions.TimeOfTodayRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly DateExtractorChs DatePointExtractor = new DateExtractorChs();
        private static readonly TimeExtractorChs TimePointExtractor = new TimeExtractorChs();

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MergeDateAndTime(text, referenceTime));
            tokens.AddRange(BasicRegexMatch(text));
            tokens.AddRange(TimeOfToday(text, referenceTime));

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
        public List<Token> MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = DatePointExtractor.Extract(text, referenceTime);
            if (ers.Count == 0)
            {
                return ret;
            }

            ers.AddRange(TimePointExtractor.Extract(text, referenceTime));
            if (ers.Count < 2)
            {
                return ret;
            }

            ers = ers.OrderBy(o => o.Start).ToList();

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
                    if (string.IsNullOrEmpty(middleStr) || middleStr.Equals(",") || PrepositionRegex.IsMatch(middleStr))
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
        public List<Token> TimeOfToday(string text, DateObject referenceTime)
        {
            var ret = new List<Token>();
            var ers = TimePointExtractor.Extract(text, referenceTime);
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