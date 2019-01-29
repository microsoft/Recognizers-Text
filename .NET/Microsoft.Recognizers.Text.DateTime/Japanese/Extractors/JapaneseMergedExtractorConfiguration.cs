using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseMergedExtractorConfiguration : IDateTimeExtractor
    {
        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.ParserConfigurationBefore, RegexOptions.Singleline);
        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.ParserConfigurationAfter, RegexOptions.Singleline);
        public static readonly Regex UntilRegex = new Regex(DateTimeDefinitions.ParserConfigurationUntil, RegexOptions.Singleline);
        public static readonly Regex SincePrefixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSincePrefix, RegexOptions.Singleline);
        public static readonly Regex SinceSuffixRegex = new Regex(DateTimeDefinitions.ParserConfigurationSinceSuffix, RegexOptions.Singleline);

        private static readonly JapaneseDateExtractorConfiguration DateExtractor = new JapaneseDateExtractorConfiguration();
        private static readonly JapaneseTimeExtractorConfiguration TimeExtractor = new JapaneseTimeExtractorConfiguration();
        private static readonly JapaneseDateTimeExtractorConfiguration DateTimeExtractor = new JapaneseDateTimeExtractorConfiguration();
        private static readonly JapaneseDatePeriodExtractorConfiguration DatePeriodExtractor = new JapaneseDatePeriodExtractorConfiguration();
        private static readonly JapaneseTimePeriodExtractorConfiguration TimePeriodExtractor = new JapaneseTimePeriodExtractorConfiguration();
        private static readonly JapaneseDateTimePeriodExtractorConfiguration DateTimePeriodExtractor = new JapaneseDateTimePeriodExtractorConfiguration();
        private static readonly JapaneseDurationExtractorConfiguration DurationExtractor = new JapaneseDurationExtractorConfiguration();
        private static readonly JapaneseSetExtractorConfiguration SetExtractor = new JapaneseSetExtractorConfiguration();
        private static readonly BaseHolidayExtractor HolidayExtractor = new BaseHolidayExtractor(new JapaneseHolidayExtractorConfiguration());

        private readonly DateTimeOptions options;

        public JapaneseMergedExtractorConfiguration(DateTimeOptions options)
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
            var regex = new Regex(@"^\d{1,2}号");

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
                    var modLengh = match.Index + match.Length;
                    er.Length += modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = AfterRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLengh = match.Index + match.Length;
                    er.Length += modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = UntilRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var modLengh = beforeStr.Length - match.Index;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = SincePrefixRegex.MatchEnd(beforeStr, trim: true);

                if (match.Success)
                {
                    var modLengh = beforeStr.Length - match.Index;
                    er.Length += modLengh;
                    er.Start -= modLengh;
                    er.Text = text.Substring(er.Start ?? 0, er.Length ?? 0);
                }

                match = SinceSuffixRegex.MatchBegin(afterStr, trim: true);

                if (match.Success)
                {
                    var modLengh = match.Index + match.Length;
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

        private void AddTo(List<ExtractResult> dst, List<ExtractResult> src)
        {
            foreach (var result in src)
            {
                var isFound = false;
                int resultMatchtIndex = -1, resultMatchLength = 1;
                for (var i = 0; i < dst.Count; i++)
                {
                    if (dst[i].IsOverlap(result))
                    {
                        isFound = true;
                        if (result.Length > dst[i].Length)
                        {
                            resultMatchtIndex = i;
                            var j = i + 1;
                            while (j < dst.Count && dst[j].IsOverlap(result))
                            {
                                resultMatchLength++;
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
                else if (resultMatchtIndex >= 0)
                {
                    dst.RemoveRange(resultMatchtIndex, resultMatchLength);
                    var tmpDst = MoveOverlap(dst, result);
                    dst.Clear();
                    dst.AddRange(tmpDst);
                    dst.Insert(resultMatchtIndex, result);
                }
            }
        }
    }
}