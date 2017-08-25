using System.Collections.Generic;
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

            // the order is important, since there is a problem in merging
            var ret = this.config.DateExtractor.Extract(text);
            AddTo(ret, this.config.TimeExtractor.Extract(text));
            AddTo(ret, this.config.DurationExtractor.Extract(text));
            AddTo(ret, this.config.DatePeriodExtractor.Extract(text));
            AddTo(ret, this.config.DateTimeExtractor.Extract(text));
            AddTo(ret, this.config.TimePeriodExtractor.Extract(text));
            AddTo(ret, this.config.DateTimePeriodExtractor.Extract(text));
            AddTo(ret, this.config.GetExtractor.Extract(text));
            AddTo(ret, this.config.HolidayExtractor.Extract(text));

            AddMod(ret, text);

            return ret;
        }

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src)
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

                var isFound = false;
                int rmIndex = -1, rmLength = 1;
                for (var i = 0; i < dst.Count; i++)
                {
                    if (dst[i].IsOverlap(result))
                    {
                        isFound = true;
                        if (result.Length > dst[i].Length)
                        {
                            rmIndex = i;
                            var j = i + 1;
                            while (j < dst.Count && dst[j].IsOverlap(result))
                            {
                                rmLength++;
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
                else if (rmIndex >= 0)
                {
                    dst.RemoveRange(rmIndex, rmLength);
                    dst.Insert(rmIndex, result);
                }
            }
        }

        private bool ShouldSkipFromToMerge(ExtractResult er) {
            return config.FromToRegex.IsMatch(er.Text);
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
