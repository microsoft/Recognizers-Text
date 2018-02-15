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

        internal abstract System.Collections.Immutable.ImmutableDictionary<Regex, string> Regexes { get; }

        protected virtual string ExtractType { get; } = "";

        public BaseNumberRangeExtractor(BaseNumberExtractor numberExtractor, BaseNumberExtractor ordinalExtractor, BaseNumberParser numberParser)
        {
            this.numberExtractor = numberExtractor;
            this.ordinalExtractor = ordinalExtractor;
            this.numberParser = numberParser;
        }

        public virtual List<ExtractResult> Extract(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new List<ExtractResult>();
            }

            var result = new List<ExtractResult>();
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
                        for (var j = 0; j < length; j++)
                        {
                            matched[start + j] = true;
                        }

                        // Keep Source Data for extra information
                        matchSource.Add(new Tuple<int, int>(start, length), collection.Value);
                    }
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
                        var length = i - last;
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
                                Data = matchSource.ContainsKey(srcMatch) ? matchSource[srcMatch] : null
                            };
                            result.Add(er);
                        }
                    }
                }
                else
                {
                    last = i;
                }
            }

            return result;
        }

        private void GetMatchedStartAndLength(Match match, string type, string source, out int start, out int length)
        {
            start = NumberRangeConstants.INVALID_NUM;
            length = NumberRangeConstants.INVALID_NUM;

            var numberStr1 = match.Groups["number1"].Value;
            var numberStr2 = match.Groups["number2"].Value;

            if (type.Contains(NumberRangeConstants.TWONUM))
            {
                var extractNumList1 = ExtractNumberAndOrdinalFromStr(numberStr1);
                var extractNumList2 = ExtractNumberAndOrdinalFromStr(numberStr2);

                if (extractNumList1 != null && extractNumList2 != null)
                {
                    if (type.Contains(NumberRangeConstants.TWONUMTILL))
                    {
                        // num1 must less than num2
                        var num1 = (double)(numberParser.Parse(extractNumList1[0]).Value ?? 0);
                        var num2 = (double)(numberParser.Parse(extractNumList2[0]).Value ?? 0);

                        if (num1 > num2)
                        {
                            return;
                        }
                    }

                    bool validNum1 = false, validNum2 = false;
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

                var extractNumList = ExtractNumberAndOrdinalFromStr(numberStr);

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

        private bool ValidateMatchAndGetStartAndLength(List<ExtractResult> extractNumList, string numberStr, Match match, string source, ref int start, ref int length)
        {
            bool validNum = false;

            foreach (var extractNum in extractNumList)
            {
                if (numberStr.Trim().EndsWith(extractNum.Text) && match.Value.StartsWith(numberStr))
                {
                    start = source.IndexOf(numberStr) + extractNum.Start ?? 0;
                    length = length - extractNum.Start ?? 0;
                    validNum = true;
                }
                else if (extractNum.Start == 0 && match.Value.EndsWith(numberStr))
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

        private List<ExtractResult> ExtractNumberAndOrdinalFromStr(string numberStr)
        {
            var extractNumber = numberExtractor.Extract(numberStr);
            var extractOrdinal = ordinalExtractor.Extract(numberStr);

            if (extractNumber.Count == 0)
            {
                return extractOrdinal.Count == 0 ? null : extractOrdinal;
            }
            else
            {
                if (extractOrdinal.Count == 0)
                {
                    return extractNumber;
                }

                extractNumber.AddRange(extractOrdinal);
                extractNumber = extractNumber.OrderBy(num => num.Start).ThenByDescending(num => num.Length).ToList();
                return extractNumber;
            }
        }
    }

    public static class NumberRangeConstants
    {
        // Number range regex type
        public const string TWONUM = "TwoNum";
        public const string TWONUMBETWEEN = "TwoNumBetween";
        public const string TWONUMTILL = "TwoNumTill";
        public const string MORE = "More";
        public const string LESS = "Less";
        public const string EQUAL = "Equal";

        // Brackets and comma for number range resolution value
        public const char LEFT_OPEN = '(';
        public const char RIGHT_OPEN = ')';
        public const char LEFT_CLOSED = '[';
        public const char RIGHT_CLOSED = ']';
        public const char INTERVAL_SEPARATOR = ',';

        // Invalid number
        public const int INVALID_NUM = -1;
    }
}
