using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class MergedExtractorChs : IDateTimeExtractor
    {
        private static readonly DateExtractorChs DateExtractor = new DateExtractorChs();
        private static readonly TimeExtractorChs TimeExtractor = new TimeExtractorChs();
        private static readonly DateTimeExtractorChs DateTimeExtractor = new DateTimeExtractorChs();
        private static readonly DatePeriodExtractorChs DatePeriodExtractor = new DatePeriodExtractorChs();
        private static readonly TimePeriodExtractorChs TimePeriodExtractor = new TimePeriodExtractorChs();
        private static readonly DateTimePeriodExtractorChs DateTimePeriodExtractor = new DateTimePeriodExtractorChs();
        private static readonly DurationExtractorChs DurationExtractor = new DurationExtractorChs();
        private static readonly SetExtractorChs SetExtractor = new SetExtractorChs();
        private static readonly BaseHolidayExtractor HolidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());

        private readonly DateTimeOptions options;

        public MergedExtractorChs(DateTimeOptions options)
        {
            this.options = options;
        }

        public List<ExtractResult> Extract(string text)
        {
            return Extract(text, DateObject.Now);
        }

            public List<ExtractResult> Extract(string text, DateObject referenceTime)
        {
            var ret = DateExtractor.Extract(text, referenceTime);

            // the order is important, since there is a problem in merging
            AddTo(ret, TimeExtractor.Extract(text, referenceTime));
            AddTo(ret, DurationExtractor.Extract(text, referenceTime));
            AddTo(ret, DatePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, DateTimeExtractor.Extract(text, referenceTime));
            AddTo(ret, TimePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, DateTimePeriodExtractor.Extract(text, referenceTime));
            AddTo(ret, SetExtractor.Extract(text, referenceTime));
            AddTo(ret, HolidayExtractor.Extract(text, referenceTime));

            CheckBlackList(ret, text);
            return ret;
        }

        // add some negative case
        private void CheckBlackList(List<ExtractResult> dst, string text)
        {
            var ret = new List<ExtractResult>();
            var regex = new Regex(@"^\d{1,2}号", RegexOptions.IgnoreCase);
            foreach (var d in dst)
            {
                var tmp = (int)d.Start + (int)d.Length;
                if (tmp != text.Length)
                {
                    var tmpchar = text.Substring(tmp, 1);
                    if (d.Text.EndsWith("周") && tmp < text.Length && tmpchar.Equals("岁"))
                    {
                        continue;
                    }
                }

                if (regex.Match(d.Text).Success)
                {
                    continue;
                }
                ret.Add(d);
            }
            dst = ret;
        }

        private void MoveOverlap(List<ExtractResult> dst, ExtractResult result)
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

            foreach (var dup in duplicate)
            {
                dst.RemoveAt(dup);
            }
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
                    MoveOverlap(dst, result);
                    dst.Insert(rmIndex, result);
                }
            }
        }
    }
}