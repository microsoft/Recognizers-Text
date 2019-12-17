using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseMergedUnitExtractor : IExtractor
    {
        private readonly INumberWithUnitExtractorConfiguration config;

        private readonly NumberWithUnitExtractor numberWithUnitExtractor;

        public BaseMergedUnitExtractor(INumberWithUnitExtractorConfiguration config)
        {
            this.config = config;
            this.numberWithUnitExtractor = new NumberWithUnitExtractor(config);
        }

        public List<ExtractResult> Extract(string source)
        {
            List<ExtractResult> ers;

            // Only merge currency's compound units for now.
            if (config.ExtractType.Equals(Constants.SYS_UNIT_CURRENCY, StringComparison.Ordinal))
            {
                ers = MergeCompoundUnits(source);
            }
            else
            {
                ers = numberWithUnitExtractor.Extract(source);
            }

            return ers;
        }

        private List<ExtractResult> MergeCompoundUnits(string source)
        {
            var result = new List<ExtractResult>();

            var ers = numberWithUnitExtractor.Extract(source);

            MergePureNumber(source, ers);

            if (ers.Count == 0)
            {
                return result;
            }

            var groups = new int[ers.Count];
            groups[0] = 0;

            for (var idx = 0; idx < ers.Count - 1; idx++)
            {
                if (ers[idx].Type != ers[idx + 1].Type &&
                    !ers[idx].Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal) &&
                    !ers[idx + 1].Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal))
                {
                    continue;
                }

                if (ers[idx].Data is ExtractResult er &&
                    !er.Data.ToString().StartsWith(Number.Constants.INTEGER_PREFIX, StringComparison.Ordinal))
                {
                    groups[idx + 1] = groups[idx] + 1;
                    continue;
                }

                var middleBegin = ers[idx].Start + ers[idx].Length ?? 0;
                var middleEnd = ers[idx + 1].Start ?? 0;
                var length = middleEnd - middleBegin;

                if (length < 0)
                {
                    continue; // @HERE
                }

                var middleStr = source.Substring(middleBegin, length).Trim();

                // Separated by whitespace
                if (string.IsNullOrEmpty(middleStr))
                {
                    groups[idx + 1] = groups[idx];
                    continue;
                }

                // Separated by connectors
                var match = config.CompoundUnitConnectorRegex.Match(middleStr);
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
                    var tmpExtractResult = ers[idx].Clone();

                    tmpExtractResult.Data = new List<ExtractResult>
                    {
                        new ExtractResult
                        {
                            Data = ers[idx].Data,
                            Length = ers[idx].Length,
                            Start = ers[idx].Start,
                            Text = ers[idx].Text,
                            Type = ers[idx].Type,
                        },
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
                    result[group].Type = Constants.SYS_UNIT_CURRENCY;
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

            result.RemoveAll(o => o.Type == Constants.SYS_NUM);

            return result;
        }

        private void MergePureNumber(string source, List<ExtractResult> ers)
        {
            var numErs = config.UnitNumExtractor.Extract(source);

            var unitNumbers = new List<ExtractResult>();
            for (int i = 0, j = 0; i < numErs.Count; i++)
            {
                bool hasBehindExtraction = false;
                while (j < ers.Count && ers[j].Start + ers[j].Length < numErs[i].Start)
                {
                    hasBehindExtraction = true;
                    j++;
                }

                if (!hasBehindExtraction)
                {
                    continue;
                }

                // Filter cases like "1 dollars 11a", "11" is not the fraction here.
                if (source.Length > numErs[i].Start + numErs[i].Length)
                {
                    var endChar = source.Substring(numErs[i].Length + numErs[i].Start ?? 0, 1);
                    if (char.IsLetter(endChar[0]) && !SimpleTokenizer.IsCjk(endChar[0]))
                    {
                        continue;
                    }
                }

                var middleBegin = ers[j - 1].Start + ers[j - 1].Length ?? 0;
                var middleEnd = numErs[i].Start ?? 0;

                var middleStr = source.Substring(middleBegin, middleEnd - middleBegin).Trim().ToLowerInvariant();

                // Separated by whitespace
                if (string.IsNullOrEmpty(middleStr))
                {
                    unitNumbers.Add(numErs[i]);
                    continue;
                }

                // Separated by connectors
                var match = config.CompoundUnitConnectorRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    unitNumbers.Add(numErs[i]);
                }
            }

            foreach (var extractResult in unitNumbers)
            {
                var overlap = false;
                foreach (var er in ers)
                {
                    if (er.Start <= extractResult.Start && er.Start + er.Length >= extractResult.Start)
                    {
                        overlap = true;
                    }
                }

                if (!overlap)
                {
                    ers.Add(extractResult);
                }
            }

            ers.Sort((x, y) => x.Start - y.Start ?? 0);
        }
    }
}