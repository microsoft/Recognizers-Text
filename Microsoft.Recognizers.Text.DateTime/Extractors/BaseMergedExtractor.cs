using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseMergedExtractor : IExtractor
    {
        private readonly IMergedExtractorConfiguration config;

        public BaseMergedExtractor(IMergedExtractorConfiguration config)
        {
            this.config = config;
        }

        public List<ExtractResult> Extract(string text)
        {
            var ret = new List<ExtractResult>();

            // the order is important, since there is a problem in merging
            ret = this.config.DateExtractor.Extract(text);
            AddTo(ret, this.config.TimeExtractor.Extract(text));
            AddTo(ret, this.config.DurationExtractor.Extract(text));
            AddTo(ret, this.config.DatePeriodExtractor.Extract(text));
            AddTo(ret, this.config.DateTimeExtractor.Extract(text));
            AddTo(ret, this.config.TimePeriodExtractor.Extract(text));
            AddTo(ret, this.config.DateTimePeriodExtractor.Extract(text));
            AddTo(ret, this.config.SetExtractor.Extract(text));
            AddTo(ret, this.config.HolidayExtractor.Extract(text));

            AddMod(ret, text);

            return ret;
        }

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src)
        {
            foreach (var result in src)
            {
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

        private void AddMod(List<ExtractResult> ers, string text)
        {
            var lastEnd = 0;
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(lastEnd, er.Start ?? 0).ToLowerInvariant();
                int tokenIndex;
                
                if (this.config.HasBeforeTokenIndex(beforeStr.Trim(), out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }
                
                if (this.config.HasAfterTokenIndex(beforeStr.Trim(), out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }
            }
        }
    }
}
