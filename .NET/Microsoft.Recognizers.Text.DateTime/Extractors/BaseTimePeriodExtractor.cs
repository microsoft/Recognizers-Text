using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

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
            tokens.AddRange(MatchTimeOfDay(text));

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
                    // Is there "pm" or "am" ?
                    var pmStr = match.Groups["pm"].Value;
                    var amStr = match.Groups["am"].Value;
                    var descStr = match.Groups["desc"].Value;

                    // Check "pm", "am"
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

            // Merge "{TimePoint} to {TimePoint}", "between {TimePoint} and {TimePoint}"

            // Handling ending number as a time point.
            var numErs = this.config.IntegerExtractor.Extract(text);

            // Check if it is an ending number
            if (numErs.Count > 0)
            {
                var timeNumbers = new List<ExtractResult>();

                // check if it is a ending number
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
                    timeNumbers.Add(num);
                }

                var i = 0;
                var j = 0;
                while (i < numErs.Count)
                {
                    // find subsequent time point
                    var numEndPoint = numErs[i].Start + numErs[i].Length;
                    while (j < ers.Count && ers[j].Start <= numEndPoint)
                    {
                        j++;
                    }

                    if (j >= ers.Count)
                    {
                        break;
                    }

                    // check connector string
                    var midStr = text.Substring(numEndPoint?? 0, ers[j].Start-numEndPoint?? 0);
                    var match = this.config.TillRegex.Match(midStr);
                    if (match.Success && match.Length == midStr.Trim().Length)
                    {
                        timeNumbers.Add(numErs[i]);
                    }

                    i++;
                }

                // check overlap
                foreach (var timeNum in timeNumbers)
                {
                    var overlap = false;
                    foreach (var er in ers)
                    {
                        if (er.Start <= timeNum.Start && er.Start + er.Length >= timeNum.Start)
                        {
                            overlap = true;
                        }
                    }

                    if (!overlap)
                    {
                        ers.Add(timeNum);
                    }
                }

                ers.Sort((x, y) => (x.Start - y.Start ?? 0));
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
                
                // Handle "{TimePoint} to {TimePoint}"
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // Handle "from"
                    var beforeStr = text.Substring(0, periodBegin).Trim().ToLowerInvariant();
                    if (this.config.GetFromTokenIndex(beforeStr, out int fromIndex))
                    {
                        periodBegin = fromIndex;
                    }

                    ret.Add(new Token(periodBegin, periodEnd));
                    idx += 2;
                    continue;
                }

                // Handle "between {TimePoint} and {TimePoint}"
                if (this.config.HasConnectorToken(middleStr))
                {
                    var periodBegin = ers[idx].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    // Handle "between"
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

        private List<Token> MatchTimeOfDay(string text)
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
