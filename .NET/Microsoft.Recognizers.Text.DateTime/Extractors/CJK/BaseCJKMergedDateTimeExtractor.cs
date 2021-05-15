using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKMergedDateTimeExtractor : IDateTimeExtractor
    {
        private readonly ICJKMergedExtractorConfiguration config;

        public BaseCJKMergedDateTimeExtractor(ICJKMergedExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

        public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var ret = this.config.DateExtractor.Extract(text, referenceTime);

            // the order is important, since there is a problem in merging
            AddTo(ret, this.config.TimeExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.DurationExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.DatePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.DateTimeExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.TimePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.DateTimePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.SetExtractor.Extract(text, referenceTime));
            AddTo(ret, this.config.HolidayExtractor.Extract(text, referenceTime));

            ret = FilterAmbiguity(ret, text);

            AddMod(ret, text);

            ret = ret.OrderBy(p => p.Start).ToList();

            return ret;
        }

        private static List<ExtractResult> MoveOverlap(List<ExtractResult> dst, ExtractResult result)
        {
            var duplicate = new List<int>();
            for (var i = 0; i < dst.Count; ++i)
            {
                if (result.Text.Contains(dst[i].Text) &&
                    (result.Start == dst[i].Start || result.Start + result.Length == dst[i].Start + dst[i].Length))
                {
                    duplicate.Add(i);
                }
            }

            var tempDst = dst.Where((_, i) => !duplicate.Contains(i)).ToList();

            return tempDst;
        }

        // Filter some bad cases like "十二周岁" and "12号", etc.
        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in this.config.AmbiguityFiltersDict)
                {
                    foreach (var extractResult in extractResults)
                    {
                        if (regex.Key.IsMatch(extractResult.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            extractResults = extractResults.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                .ToList();
                        }
                    }
                }
            }

            return extractResults;
        }

        private void AddMod(List<ExtractResult> ers, string text)
        {
            var lastEnd = 0;
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(lastEnd, er.Start ?? 0);
                var afterStr = text.Substring((er.Start ?? 0) + (er.Length ?? 0));

                var match = this.config.BeforeRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }

                match = this.config.AfterRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }

                match = this.config.UntilRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var modLength = beforeStr.Length - match.Index;
                    er.Length += modLength;
                    er.Start -= modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }

                match = this.config.SincePrefixRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success && AmbiguousRangeChecker(beforeStr, text, er))
                {
                    var modLength = beforeStr.Length - match.Index;
                    er.Length += modLength;
                    er.Start -= modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }

                match = this.config.SinceSuffixRegex.MatchBegin(afterStr, trim: true);
                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }

                match = this.config.EqualRegex.MatchBegin(beforeStr, trim: true);
                if (match.Success)
                {
                    var modLength = beforeStr.Length - match.Index;
                    er.Length += modLength;
                    er.Start -= modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);

                    er.Metadata = AssignModMetadata(er.Metadata);
                }
            }
        }

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src)
        {
            foreach (var result in src)
            {
                var isFound = false;
                int indexRm = -1, lengthRm = 1;
                for (var i = 0; i < dst.Count; i++)
                {
                    if (dst[i].IsOverlap(result))
                    {
                        isFound = true;
                        if (result.Length > dst[i].Length)
                        {
                            indexRm = i;
                            var j = i + 1;
                            while (j < dst.Count && dst[j].IsOverlap(result))
                            {
                                lengthRm++;
                                j++;
                            }
                        }

                        break;
                    }
                }

                if (!isFound)
                {
                    dst.Add(result);
                }
                else if (indexRm >= 0)
                {
                    dst.RemoveRange(indexRm, lengthRm);
                    var tmpDst = MoveOverlap(dst, result);
                    dst.Clear();
                    dst.AddRange(tmpDst);
                    dst.Insert(indexRm, result);
                }
            }
        }

        // Avoid adding mod for ambiguity cases, such as "从" in "从 ... 到 ..." should not add mod
        // TODO: Revise PotentialAmbiguousRangeRegex to support cases like "从2015年起，哪所大学需要的分数在80到90之间"
        private bool AmbiguousRangeChecker(string beforeStr, string text, ExtractResult er)
        {
            if (this.config.AmbiguousRangeModifierPrefix.MatchEnd(beforeStr, true).Success)
            {
                var matches = this.config.PotentialAmbiguousRangeRegex.Matches(text).Cast<Match>();
                if (matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                {
                    return false;
                }
            }

            return true;
        }

        private Metadata AssignModMetadata(Metadata metadata)
        {
            if (metadata == null)
            {
                metadata = new Metadata { HasMod = true };
            }
            else
            {
                metadata.HasMod = true;
            }

            return metadata;
        }
    }
}
