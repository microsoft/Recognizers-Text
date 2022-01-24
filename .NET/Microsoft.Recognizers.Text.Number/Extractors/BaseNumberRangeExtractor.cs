// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberRangeExtractor : IExtractor
    {
        private readonly BaseNumberExtractor numberExtractor;

        private readonly BaseNumberExtractor ordinalExtractor;

        private readonly BaseNumberParser numberParser;

        protected BaseNumberRangeExtractor(BaseNumberExtractor numberExtractor, BaseNumberExtractor ordinalExtractor, BaseNumberParser numberParser,
                                           INumberOptionsConfiguration config)
        {
            this.numberExtractor = numberExtractor;
            this.ordinalExtractor = ordinalExtractor;
            this.numberParser = numberParser;
            Config = config;
        }

        internal abstract System.Collections.Immutable.ImmutableDictionary<Regex, string> Regexes { get; }

        internal abstract Regex AmbiguousFractionConnectorsRegex { get; }

        protected virtual INumberOptionsConfiguration Config { get; }

        protected virtual string ExtractType { get; } = string.Empty;

        public virtual List<ExtractResult> Extract(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var results = new List<ExtractResult>();
            var matchSource = new Dictionary<Tuple<int, int>, string>();
            var matched = new bool[source.Length];

            var collections = Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value);

            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    GetMatchedStartAndLength(m, collection.Value, source, out int start, out int length);

                    if (start >= 0 && length > 0)
                    {
                        // Add match if not already in matchSource (it can happen that a certain pattern is extracted by more than one regex,
                        // but if the same tuple is added more than once execution breaks).
                        if (!matchSource.ContainsKey(Tuple.Create(start, length)))
                        {
                            // Keep Source Data for extra information
                            matchSource.Add(new Tuple<int, int>(start, length), collection.Value);
                        }
                    }
                }
            }

            foreach (var match in matchSource)
            {
                var start = match.Key.Item1;
                var length = match.Key.Item2;

                // Filter wrong two number ranges such as "more than 20 and less than 10" and "大于20小于10".
                if (match.Value.Equals(NumberRangeConstants.TWONUM, StringComparison.Ordinal))
                {
                    int moreIndex = 0, lessIndex = 0;

                    var text = source.Substring(match.Key.Item1, match.Key.Item2);

                    var er = numberExtractor.Extract(text);

                    if (er.Count != 2)
                    {
                        er = ordinalExtractor.Extract(text);

                        if (er.Count != 2)
                        {
                            continue;
                        }
                    }

                    var nums = er.Select(r => (double)(numberParser.Parse(r).Value ?? 0)).ToList();

                    // Order matchSource by decreasing match length so that "no less than x" is before "less than x"
                    var matchList = matchSource.ToList();
                    matchList.Sort((pair1, pair2) => pair2.Key.Item2.CompareTo(pair1.Key.Item2));

                    moreIndex = matchList.First(r =>
                                                    r.Value.Equals(NumberRangeConstants.MORE, StringComparison.Ordinal) &&
                                                    r.Key.Item1 >= start && r.Key.Item1 + r.Key.Item2 <= start + length).Key.Item1;

                    lessIndex = matchList.First(r =>
                                                    r.Value.Equals(NumberRangeConstants.LESS, StringComparison.Ordinal) &&
                                                    r.Key.Item1 >= start && r.Key.Item1 + r.Key.Item2 <= start + length).Key.Item1;

                    if (!((nums[0] < nums[1] && moreIndex <= lessIndex) || (nums[0] > nums[1] && moreIndex >= lessIndex)))
                    {
                        continue;
                    }
                }

                // The entity is longer than 1, so don't mark the last char to represent the end.
                // To avoid no connector cases like "大于20小于10" being marked as a whole entity.
                for (var j = 0; j < length - 1; j++)
                {
                    matched[start + j] = true;
                }

            }

            var last = -1;
            for (var i = 0; i < source.Length; i++)
            {
                if (matched[i])
                {
                    if (i + 1 == source.Length || !matched[i + 1])
                    {
                        var start = last + 1;
                        var length = i - last + 1;
                        var substr = source.Substring(start, length);

                        if (matchSource.Keys.Any(o => o.Item1 == start && o.Item2 == length))
                        {
                            var srcMatch = matchSource.Keys.First(o => o.Item1 == start && o.Item2 == length);
                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length,
                                Text = substr,
                                Type = ExtractType,
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null,
                            };

                            results.Add(er);
                        }
                    }
                }
                else
                {
                    last = i;
                }
            }

            // In ExperimentalMode, cases like "from 3 to 5" and "between 10 and 15" are set to closed at both start and end
            if ((Config.Options & NumberOptions.ExperimentalMode) != 0)
            {
                foreach (var result in results)
                {
                    var data = result.Data.ToString();
                    if (data == NumberRangeConstants.TWONUMBETWEEN ||
                        data == NumberRangeConstants.TWONUMTILL)
                    {
                        result.Data = NumberRangeConstants.TWONUMCLOSED;
                    }
                }
            }

            return results;
        }

        private static bool ValidateMatchAndGetStartAndLength(List<ExtractResult> extractNumList, string numberStr, Match match,
                                                              string source, ref int start, ref int length)
        {
            bool validNum = false;

            foreach (var extractNum in extractNumList)
            {
                if (numberStr.Trim().EndsWith(extractNum.Text, StringComparison.Ordinal) &&
                    match.Value.StartsWith(numberStr, StringComparison.Ordinal))
                {
                    start = match.Index + extractNum.Start ?? 0;
                    length = length - extractNum.Start ?? 0;
                    validNum = true;
                }
                else if (extractNum.Start == 0 && match.Value.EndsWith(numberStr, StringComparison.Ordinal))
                {
                    length = length - numberStr.Length + extractNum.Length ?? 0;
                    validNum = true;
                }
                else if (extractNum.Start == 0 && extractNum.Length == numberStr.Trim().Length)
                {
                    validNum = true;
                }

                if (validNum)
                {
                    break;
                }
            }

            return validNum;
        }

        // Judge whether it's special cases like "more than 30000 in 2010"
        // For these specific cases, we will not treat "30000 in 2010" as a fraction number
        private static bool IsAmbiguousRangeOrFraction(Match match, string type, string numberStr)
        {
            return (type == NumberRangeConstants.MORE || type == NumberRangeConstants.LESS) && match.Value.Trim().EndsWith(numberStr, StringComparison.Ordinal);
        }

        private void GetMatchedStartAndLength(Match match, string type, string source, out int start, out int length)
        {
            start = NumberRangeConstants.INVALID_NUM;
            length = NumberRangeConstants.INVALID_NUM;

            var numberStr1 = match.Groups["number1"].Value?.TrimStart();
            var numberStr2 = match.Groups["number2"].Value?.TrimStart();

            if (type.Contains(NumberRangeConstants.TWONUM))
            {
                var extractNumList1 = ExtractNumberAndOrdinalFromStr(numberStr1);
                var extractNumList2 = ExtractNumberAndOrdinalFromStr(numberStr2);

                if (extractNumList1 != null && extractNumList2 != null)
                {
                    if (type.Contains(NumberRangeConstants.TWONUMTILL))
                    {
                        // num1 must have same type with num2
                        if (extractNumList1[0].Type != extractNumList2[0].Type)
                        {
                            return;
                        }

                        // num1 must less than num2
                        var num1 = (double)(numberParser.Parse(extractNumList1[0]).Value ?? 0);
                        var num2 = (double)(numberParser.Parse(extractNumList2[0]).Value ?? 0);

                        if (num1 > num2)
                        {
                            return;
                        }

                        extractNumList1.RemoveRange(1, extractNumList1.Count - 1);
                        extractNumList2.RemoveRange(1, extractNumList2.Count - 1);
                    }

                    bool validNum1, validNum2;
                    start = match.Index;
                    length = match.Length;

                    validNum1 = ValidateMatchAndGetStartAndLength(extractNumList1, numberStr1, match, source, ref start, ref length);
                    validNum2 = ValidateMatchAndGetStartAndLength(extractNumList2, numberStr2, match, source, ref start, ref length);

                    if (!validNum1 || !validNum2)
                    {
                        start = NumberRangeConstants.INVALID_NUM;
                        length = NumberRangeConstants.INVALID_NUM;
                    }
                }
            }
            else
            {
                var numberStr = string.IsNullOrEmpty(numberStr1) ? numberStr2 : numberStr1;

                var isAmbiguousRangeOrFraction = IsAmbiguousRangeOrFraction(match, type, numberStr);
                var extractNumList = ExtractNumberAndOrdinalFromStr(numberStr, isAmbiguousRangeOrFraction);

                if (extractNumList != null)
                {
                    start = match.Index;
                    length = match.Length;

                    if (!ValidateMatchAndGetStartAndLength(extractNumList, numberStr, match, source, ref start, ref length))
                    {
                        start = NumberRangeConstants.INVALID_NUM;
                        length = NumberRangeConstants.INVALID_NUM;
                    }
                }
            }
        }

        // TODO: this should not be in the NumberRangeExtractor as it doesn't handle duration concepts
        private List<ExtractResult> ExtractNumberAndOrdinalFromStr(string numberStr, bool isAmbiguousRangeOrFraction = false)
        {
            List<ExtractResult> ret = null;
            var extractNumber = numberExtractor.Extract(numberStr);
            var extractOrdinal = ordinalExtractor.Extract(numberStr);

            if (extractNumber.Count == 0)
            {
                ret = extractOrdinal.Count == 0 ? null : extractOrdinal;
            }
            else if (extractOrdinal.Count == 0)
            {
                ret = extractNumber;
            }
            else
            {
                ret = new List<ExtractResult>();
                ret.AddRange(extractNumber);
                ret.AddRange(extractOrdinal);
                ret = ret.OrderByDescending(num => num.Length).ThenByDescending(num => num.Start).ToList();
            }

            var removeFractionWithInConnector = ShouldRemoveFractionWithInConnector(numberStr);

            if (ret != null && (removeFractionWithInConnector || isAmbiguousRangeOrFraction))
            {
                ret = RemoveAmbiguousFractions(ret);
            }

            return ret;
        }

        private bool ShouldRemoveFractionWithInConnector(string numberStr)
        {
            var removeFractionWithInConnector = false;

            if ((Config.Options & NumberOptions.ExperimentalMode) != 0)
            {
                removeFractionWithInConnector = IsFractionWithInConnector(numberStr);
            }

            return removeFractionWithInConnector;
        }

        // Fraction with InConnector may lead to some ambiguous cases like "more than 30000 in 2010"
        // In ExperimentalMode, we will remove all FractionWithInConnector numbers to avoid such cases
        private bool IsFractionWithInConnector(string numberStr)
        {
            return AmbiguousFractionConnectorsRegex.Match(numberStr).Success;
        }

        // For cases like "more than 30000 in 2010", we will not treat "30000 in 2010" as a fraction number
        // In this method, "30000 in 2010" will be changed to "30000"
        private List<ExtractResult> RemoveAmbiguousFractions(List<ExtractResult> ers)
        {
            foreach (var er in ers)
            {
                if (er.Data != null && er.Data.ToString() ==
                    RegexTagGenerator.GenerateRegexTag(Constants.FRACTION_PREFIX, Constants.ENGLISH).Name)
                {
                    var match = AmbiguousFractionConnectorsRegex.Match(er.Text);

                    if (match.Success)
                    {
                        var beforeText = er.Text.Substring(0, match.Index).TrimEnd();

                        er.Length = beforeText.Length;
                        er.Text = beforeText;
                        er.Type = Constants.SYS_NUM;
                        er.Data = null;
                    }
                }
            }

            return ers;
        }
    }
}
