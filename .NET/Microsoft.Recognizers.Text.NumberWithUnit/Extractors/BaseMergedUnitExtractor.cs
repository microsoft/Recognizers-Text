using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class BaseMergedUnitExtractor : IExtractor
    {
        private readonly INumberWithUnitExtractorConfiguration config;

        public BaseMergedUnitExtractor(INumberWithUnitExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string source)
        {
            List<ExtractResult> ers;

            // Only merge currency's compound units for now.
            if (config.ExtractType.Equals(Constants.SYS_UNIT_CURRENCY))
            {
                ers = MergeCompoundUnits(source);
            }
            else
            {
                ers = new NumberWithUnitExtractor(config).Extract(source);
            }

            return ers;
        }

        private List<ExtractResult> MergeCompoundUnits(string source)
        {
            var result = new List<ExtractResult>();

            var ers = new NumberWithUnitExtractor(config).Extract(source);
            MergePureNumber(source, ers);

            if (ers.Count == 0)
            {
                return result;
            }

            var groups = new int[ers.Count];
            groups[0] = 0;

            for (var idx = 0; idx < ers.Count - 1; idx++)
            {
                if (ers[idx].Type != ers[idx + 1].Type && !ers[idx].Type.Equals(Constants.SYS_NUM) &&
                    !ers[idx + 1].Type.Equals(Constants.SYS_NUM))
                {
                    continue;
                }

                if (ers[idx].Data is ExtractResult er && !er.Data.ToString().StartsWith("Integer"))
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