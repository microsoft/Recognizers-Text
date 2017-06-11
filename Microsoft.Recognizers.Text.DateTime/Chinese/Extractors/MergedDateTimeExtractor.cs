using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class MergedExtractorChs : IExtractor
    {
        private static readonly DateExtractorChs _dateExtractor = new DateExtractorChs();
        private static readonly TimeExtractorChs _timeExtractor = new TimeExtractorChs();
        private static readonly DateTimeExtractorChs _dateTimeExtractor = new DateTimeExtractorChs();
        private static readonly DatePeriodExtractorChs _datePeriodExtractor = new DatePeriodExtractorChs();
        private static readonly TimePeriodExtractorChs _timePeriodExtractor = new TimePeriodExtractorChs();
        private static readonly DateTimePeriodExtractorChs _dateTimePeriodExtractor = new DateTimePeriodExtractorChs();
        private static readonly DurationExtractorChs _durationExtractor = new DurationExtractorChs();
        private static readonly SetExtractorChs _setExtractor = new SetExtractorChs();
        private static readonly BaseHolidayExtractor _holidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());


        public List<ExtractResult> Extract(string text)
        {
            var ret = new List<ExtractResult>();
            // the order is important, since there is a problem in merging
            ret = _dateExtractor.Extract(text);
            AddTo(ret, _timeExtractor.Extract(text));
            AddTo(ret, _durationExtractor.Extract(text));
            AddTo(ret, _datePeriodExtractor.Extract(text));
            AddTo(ret, _dateTimeExtractor.Extract(text));
            AddTo(ret, _timePeriodExtractor.Extract(text));
            AddTo(ret, _dateTimePeriodExtractor.Extract(text));
            AddTo(ret, _setExtractor.Extract(text));
            AddTo(ret, _holidayExtractor.Extract(text));

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
                var tmp = (int) d.Start + (int) d.Length;
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