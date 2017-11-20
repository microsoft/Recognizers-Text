using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseTimePeriodExtractor : IDateTimeExtractor
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; //"TimePeriod";

        private readonly ITimePeriodExtractorConfiguration config;

        public BaseTimePeriodExtractor(ITimePeriodExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject reference)
        {
            var tokens = new List<Token>();
            tokens.AddRange(MatchSimpleCases(text));
            tokens.AddRange(MergeTwoTimePoints(text, reference));
            tokens.AddRange(MatchNight(text));

            return Token.MergeAllTokens(tokens, text, ExtractorName);
        }

        private List<Token> MatchSimpleCases(string text)
        {
            var ret = new List<Token>();
            foreach (var regex in this.config.SimpleCasesRegex)
            {
                var matches = regex.Matches(text);
                foreach (Match match in matches)
                {
                    // is there "pm" or "am" ?
                    var pmStr = match.Groups["pm"].Value;
                    var amStr = match.Groups["am"].Value;
                    var descStr = match.Groups["desc"].Value;
                    // check "pm", "am"
                    if (!string.IsNullOrEmpty(pmStr) || !string.IsNullOrEmpty(amStr) || !string.IsNullOrEmpty(descStr))
                    {
                        ret.Add(new Token(match.Index, match.Index + match.Length));
                    }
                }
            }

            return ret;
        }

        private List<Token> MergeTwoTimePoints(string text, DateObject reference)
        {
            var ret = new List<Token>();
            var ers = this.config.SingleTimeExtractor.Extract(text, reference);

            // merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"

            // handling ending number as a time point.
            var numErs = this.config.IntegerExtractor.Extract(text);
            // check if it is a ending number
            if (numErs.Count > 0)
            {
                var endingNumber = false;
                var num = numErs[numErs.Count - 1];
                if (num.Start + num.Length == text.Length)
                {
                    endingNumber = true;
                }
                else
                {
                    var afterStr = text.Substring(num.Start + num.Length ?? 0);
                    var endingMatch = this.config.GeneralEndingRegex.Match(afterStr);
                    if (endingMatch.Success)
                    {
                        endingNumber = true;
                    }
                }
                if (endingNumber)
                {
                    ers.Add(num);
                }
            }

            var idx = 0;
            while (idx < ers.Count - 1)
            {
                var middleBegin = ers[idx].Start + ers[idx].Length ?? 0;
                var middleEnd = ers[idx + 1].Start ?? 0;

                if (middleEnd - middleBegin <= 0)
                {
                    idx++;
                    continue;
                }

                var middleStr = text.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLowerInvariant();
                var match = this.config.TillRegex.Match(middleStr);
                // handle "{TimePoint} to {TimePoint}"
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // handle "from"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex))
                    {
                        periodBegin = fromIndex;
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }
                // handle "between {TimePoint} and {TimePoint}"
                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // handle "between"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetBetweenTokenIndex(beforeStr, out int betweenIndex))
                    {
                        periodBegin = betweenIndex;
                        ret.Add(new Token(periodBegin, periodEnd));
                        idx += 2;
                        continue;
                    }
                }

                idx++;
            }

            return ret;
        }

        private List<Token> MatchNight(string text)
        {
            var ret = new List<Token>();
            var matches = this.config.TimeOfDayRegex.Matches(text);
            foreach (Match match in matches)
            {
                ret.Add(new Token(match.Index, match.Index + match.Length));
            }

            return ret;
        }
    }
}
