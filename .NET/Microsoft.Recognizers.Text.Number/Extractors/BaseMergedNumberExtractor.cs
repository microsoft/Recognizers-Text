using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    internal abstract class BaseMergedNumberExtractor : IExtractor
    {
        public abstract BaseNumberExtractor NumberExtractor { get; set; }

        public abstract Regex RoundNumberIntegerRegexWithLocks { get; set; }

        public abstract Regex ConnectorRegex { get; set; }

        // Currently, this extractor is only for English number extracting.
        public List<ExtractResult> Extract(string source)
        {
            var result = new List<ExtractResult>();

            var ers = NumberExtractor.Extract(source);

            if (ers.Count == 0)
            {
                return result;
            }

            var groups = new int[ers.Count];
            groups[0] = 0;

            for (var idx = 0; idx < ers.Count - 1; idx++)
            {
                if (!((string)ers[idx].Data).StartsWith(Constants.INTEGER_PREFIX) ||
                    !((string)ers[idx + 1].Data).StartsWith(Constants.INTEGER_PREFIX))
                {
                    groups[idx + 1] = groups[idx] + 1;
                    continue;
                }

                var match = RoundNumberIntegerRegexWithLocks.Match(ers[idx].Text);

                if (!match.Success || match.Length != ers[idx].Length)
                {
                    groups[idx + 1] = groups[idx] + 1;
                    continue;
                }

                var middleBegin = ers[idx].Start + ers[idx].Length ?? 0;
                var middleEnd = ers[idx + 1].Start ?? 0;
                var middleStr = source.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLowerInvariant();

                // Separated by whitespace
                if (string.IsNullOrEmpty(middleStr))
                {
                    groups[idx + 1] = groups[idx];
                    continue;
                }

                // Separated by connectors
                match = ConnectorRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    groups[idx + 1] = groups[idx];
                }
                else
                {
                    groups[idx + 1] = groups[idx] + 1;
                }
            }

            for (var idx = 0; idx < ers.Count; idx++)
            {
                if (idx == 0 || groups[idx] != groups[idx - 1])
                {
                    var tmpExtractResult = ers[idx];
                    tmpExtractResult.Data = new List<ExtractResult>
                    {
                        new ExtractResult
                        {
                            Data = ers[idx].Data,
                            Length = ers[idx].Length,
                            Start = ers[idx].Start,
                            Text = ers[idx].Text,
                            Type = ers[idx].Type
                        }
                    };
                    result.Add(tmpExtractResult);
                }

                // Reduce extract results in same group
                if (idx + 1 < ers.Count && groups[idx + 1] == groups[idx])
                {
                    var group = groups[idx];

                    var periodBegin = result[group].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    result[group].Length = periodEnd - periodBegin;
                    result[group].Text = source.Substring(periodBegin, periodEnd - periodBegin);
                    result[group].Type = Constants.SYS_NUM;
                    (result[group].Data as List<ExtractResult>)?.Add(ers[idx + 1]);
                }
            }

            for (var idx = 0; idx < result.Count; idx++)
            {
                var innerData = result[idx].Data as List<ExtractResult>;
                if (innerData?.Count == 1)
                {
                    result[idx] = innerData[0];
                }
            }

            return result;
        }
    }
}
