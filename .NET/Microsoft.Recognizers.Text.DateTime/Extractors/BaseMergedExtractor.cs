using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedExtractor : IExtractor
    {
        private readonly IMergedExtractorConfiguration config;
        private readonly DateTimeOptions options;

        public BaseMergedExtractor(IMergedExtractorConfiguration config, DateTimeOptions options)
        {
            this.config = config;
            this.options = options;
        }

        public List<ExtractResult> Extract(string text)
        {
            var ret = new List<ExtractResult>();
            // the order is important, since there is a problem in merging
            AddTo(ret, this.config.DateExtractor.Extract(text), text);
            AddTo(ret, this.config.TimeExtractor.Extract(text), text);
            AddTo(ret, this.config.DurationExtractor.Extract(text), text);
            AddTo(ret, this.config.DatePeriodExtractor.Extract(text), text);
            AddTo(ret, this.config.DateTimeExtractor.Extract(text), text);
            AddTo(ret, this.config.TimePeriodExtractor.Extract(text), text);
            AddTo(ret, this.config.DateTimePeriodExtractor.Extract(text), text);
            AddTo(ret, this.config.GetExtractor.Extract(text), text);
            AddTo(ret, this.config.HolidayExtractor.Extract(text), text);

            AddMod(ret, text);

            ret = ret.OrderBy(p => p.Start).ToList();

            return ret;
        }

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src, string text)
        {
            foreach (var result in src)
            {
                if ((options & DateTimeOptions.SkipFromToMerge) != 0)
                {
                    if (ShouldSkipFromToMerge(result))
                    {
                        continue;
                    }
                }

                // comment this code for now
                //if (FilterAmbiguousSingleWord(result, text))
                //{
                //    continue;
                //}

                var isFound = false;
                List<int> overlapIndexes=new List<int>();
                int firstIndex = -1;
                for (var i = 0; i < dst.Count; i++)
                {
                    //if (dst[i].IsOverlap(result))
                    //{
                    //    if (firstIndex == -1)
                    //    {
                    //        firstIndex = i;
                    //    }
                    //    isFound = true;
                    //    if (result.Length > dst[i].Length)
                    //    {
                    //        overlapIndexes.Add(i);
                    //    }
                    //}
                    if (dst[i].IsOverlap(result))
                    {
                        isFound = true;
                        if (dst[i].IsCover(result))
                        {
                            if (firstIndex == -1)
                            {
                                firstIndex = i;
                            }
                            overlapIndexes.Add(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (!isFound)
                {
                    dst.Add(result);
                }
                else if (overlapIndexes.Count>0)
                {
                    var tempDst = new List<ExtractResult>();
                    for (var i = 0; i < dst.Count; i++)
                    {
                        if (!overlapIndexes.Contains(i))
                        {
                            tempDst.Add(dst[i]);
                        }
                    }
                    //insert at the first overlap occurence to keep the order
                    tempDst.Insert(firstIndex, result);
                    dst.Clear();
                    dst.AddRange(tempDst);
                }
            }
        }

        private bool ShouldSkipFromToMerge(ExtractResult er) {
            return config.FromToRegex.IsMatch(er.Text);
        }

        private bool FilterAmbiguousSingleWord(ExtractResult er, string text)
        {
            if (config.SingleAmbiguousMonthRegex.IsMatch(er.Text.ToLowerInvariant()))
            {
                var stringBefore = text.Substring(0, (int) er.Start).TrimEnd();
                if (!config.PrepositionSuffixRegex.IsMatch(stringBefore))
                {
                    return true;
                }
            }

            return false;
        }

        private void AddMod(List<ExtractResult> ers, string text)
        {
            var lastEnd = 0;
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(lastEnd, er.Start ?? 0).ToLowerInvariant();
                int tokenIndex;
                
                if (HasTokenIndex(beforeStr.TrimEnd(), config.BeforeRegex, out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }
                
                if (HasTokenIndex(beforeStr.TrimEnd(), config.AfterRegex, out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                if (HasTokenIndex(beforeStr.TrimEnd(), config.SinceRegex, out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }
            }
        }

        public bool HasTokenIndex(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text);

            if (match.Success && string.IsNullOrWhiteSpace(text.Substring(match.Index+match.Length)))
            {
                index = match.Index;
                return true;
            }

            return false;
        }
    }
}
