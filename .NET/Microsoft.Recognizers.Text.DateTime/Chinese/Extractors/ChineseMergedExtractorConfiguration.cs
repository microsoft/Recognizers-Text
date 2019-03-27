using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseMergedExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.ParserConfigurationBefore, RegexOptions.Singleline);
        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfter, RegexOptions.Singleline);
        public static readonly Regex UntilRegex = new Regex(DateTimeDefinitions.ParserConfigurationUntil, RegexOptions.Singleline);
        public static readonly Regex SincePrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSincePrefix, RegexOptions.Singleline);
        public static readonly Regex SinceSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSinceSuffix, RegexOptions.Singleline);

        private static readonly ChineseDateExtractorConfiguration DateExtractor = new ChineseDateExtractorConfiguration();
        private static readonly ChineseTimeExtractorConfiguration TimeExtractor = new ChineseTimeExtractorConfiguration();
        private static readonly ChineseDateTimeExtractorConfiguration DateTimeExtractor = new ChineseDateTimeExtractorConfiguration();
        private static readonly ChineseDatePeriodExtractorConfiguration DatePeriodExtractor = new ChineseDatePeriodExtractorConfiguration();
        private static readonly ChineseTimePeriodExtractorChsConfiguration TimePeriodExtractor = new ChineseTimePeriodExtractorChsConfiguration();
        private static readonly ChineseDateTimePeriodExtractorConfiguration DateTimePeriodExtractor = new ChineseDateTimePeriodExtractorConfiguration();
        private static readonly ChineseDurationExtractorConfiguration DurationExtractor = new ChineseDurationExtractorConfiguration();
        private static readonly ChineseSetExtractorConfiguration SetExtractor = new ChineseSetExtractorConfiguration();
        private static readonly BaseHolidayExtractor HolidayExtractor = new BaseHolidayExtractor(new ChineseHolidayExtractorConfiguration());

        private readonly DateTimeOptions options;

        public ChineseMergedExtractorConfiguration(DateTimeOptions options)
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

        // add some negative case
        private static void CheckBlackList(ref List<ExtractResult> extractResults, string text)
        {
            var ret = new List<ExtractResult>();
            const string negativeCaseRegex = @"^\d{1,2}号";

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
                if (Regex.IsMatch(extractResult.Text, negativeCaseRegex))
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

                var match = BeforeRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = AfterRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = UntilRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var modLength = beforeStr.Length - match.Index;
                    er.Length += modLength;
                    er.Start -= modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = SincePrefixRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var modLength = beforeStr.Length - match.Index;
                    er.Length += modLength;
                    er.Start -= modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = SinceSuffixRegex.MatchBegin(afterStr, trim: true);
                if (match.Success)
                {
                    var modLength = match.Index + match.Length;
                    er.Length += modLength;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
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
    }
}