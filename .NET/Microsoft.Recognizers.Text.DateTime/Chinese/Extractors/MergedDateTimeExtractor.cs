using System.Collections.Generic;
using System.Text.RegularExpressions;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Definitions.Chinese;
using System.Linq;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class MergedExtractorChs : IDateTimeExtractor
    {
        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.ParserConfigurationBefore, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfter, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex UntilRegex = new Regex(DateTimeDefinitions.ParserConfigurationUntil, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SincePrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSincePrefix, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static readonly Regex SinceSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSinceSuffix, RegexOptions.IgnoreCase | RegexOptions.Singleline);

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

            CheckBlackList(ref ret, text);

            AddMod(ret, text);

            ret = ret.OrderBy(p => p.Start).ToList();

            return ret;
        }

        // add some negative case
        private static void CheckBlackList(ref List<ExtractResult> extractResults, string text)
        {
            var ret = new List<ExtractResult>();
            var regex = new Regex(@"^\d{1,2}号", RegexOptions.IgnoreCase);

            foreach (var extractResult in extractResults)
            {
                var endIndex = (int)extractResult.Start + (int)extractResult.Length;
                if (endIndex != text.Length)
                {
                    var tmpChar = text.Substring(endIndex, 1);

                    // for cases like "12周岁"
                    if (extractResult.Text.EndsWith("周") && endIndex < text.Length && tmpChar.Equals("岁"))
                    {
                        continue;
                    }
                }

                // for cases like "12号"
                if (regex.Match(extractResult.Text).Success)
                {
                    continue;
                }

                ret.Add(extractResult);
            }

            extractResults = ret;
        }

        private void AddMod(List<ExtractResult> ers, string text)
        {
            var lastEnd = 0;
            foreach (var er in ers)
            {
                var beforeStr = text.Substring(lastEnd, er.Start ?? 0).ToLowerInvariant();
                var afterStr = text.Substring((er.Start ?? 0) + (er.Length ?? 0)).ToLowerInvariant();

                if (HasTokenValueAfterStr(afterStr.TrimStart(), BeforeRegex, out string tokenValue))
                {
                    var modLengh = tokenValue.Length + afterStr.IndexOf(tokenValue);
                    er.Length += modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                if (HasTokenValueAfterStr(afterStr.TrimStart(), AfterRegex, out tokenValue))
                {
                    var modLengh = tokenValue.Length + afterStr.IndexOf(tokenValue);
                    er.Length += modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                if (HasTokenIndexBeforeStr(beforeStr.TrimEnd(), UntilRegex, out int tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                if (HasTokenIndexBeforeStr(beforeStr.TrimEnd(), SincePrefixRegex, out tokenIndex))
                {
                    var modLengh = beforeStr.Length - tokenIndex;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                if (HasTokenValueAfterStr(afterStr.TrimStart(), SinceSuffixRegex, out tokenValue))
                {
                    var modLengh = tokenValue.Length + afterStr.IndexOf(tokenValue);
                    er.Length += modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

            }
        }

        private bool HasTokenIndexBeforeStr(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text);

            if (match.Success && string.IsNullOrWhiteSpace(text.Substring(match.Index + match.Length)))
            {
                index = match.Index;
                return true;
            }

            return false;
        }

        private bool HasTokenValueAfterStr(string text, Regex regex, out string value)
        {
            value = string.Empty;
            var match = regex.Match(text);

            if (match.Success && match.Index == 0)
            {
                value = match.Value;
                return true;
            }

            return false;
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