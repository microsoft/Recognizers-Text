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

        internal abstract System.Collections.Immutable.ImmutableDictionary<Regex, string> Regexes { get; }

        protected virtual string ExtractType { get; } = "";

        public BaseNumberRangeExtractor(BaseNumberExtractor numberExtractor, BaseNumberExtractor ordinalExtractor)
        {
            this.numberExtractor = numberExtractor;
            this.ordinalExtractor = ordinalExtractor;
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
            for (var i = 0; i < source.Length; i++)
            {
                matched[i] = false;
            }

            var collections = Regexes.ToDictionary(o => o.Key.Matches(source), p => p.Value);
            foreach (var collection in collections)
            {
                foreach (Match m in collection.Key)
                {
                    GetMatchedStartAndLength(m, collection.Value, source, out int start, out int length);

                    if (start > 0 && length > 0)
                    {
                        for (var j = 0; j < length; j++)
                        {
                            matched[start + j] = true;
                        }

                        //Keep Source Data for extra information
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
            start = -1;
            length = -1;
            if (type.Contains("TwoNum"))
            {
                var numberStr1 = match.Groups["number1"].Value;
                var numberStr2 = match.Groups["number2"].Value;

                var extractNum1 = numberExtractor.Extract(numberStr1);
                if (extractNum1 == null)
                {
                    extractNum1 = ordinalExtractor.Extract(numberStr1);
                }

                var extractNum2 = numberExtractor.Extract(numberStr2);
                if (extractNum2 == null)
                {
                    extractNum2 = ordinalExtractor.Extract(numberStr2);
                }

                if (extractNum1.Count == 1 && extractNum2.Count == 1)
                {
                    if (extractNum1[0].Start == 0 && extractNum1[0].Length == numberStr1.Trim().Length
                        && extractNum2[0].Start == 0 && extractNum2[0].Length == numberStr2.Trim().Length)
                    {
                        start = match.Index;
                        length = match.Length;
                    }
                    else if (extractNum1[0].Start == 0 && extractNum1[0].Length == numberStr1.Trim().Length
                        && extractNum2[0].Start == 0 && source.EndsWith(numberStr2))
                    {
                        start = match.Index;
                        length = match.Length - numberStr2.Length + extractNum2[0].Length ?? 0;
                    }
                    else if (numberStr1.Trim().EndsWith(extractNum1[0].Text) && source.StartsWith(numberStr1)
                        && extractNum2[0].Start == 0 && source.EndsWith(numberStr2))
                    {
                        start = extractNum1[0].Start ?? 0;
                        length = match.Length - numberStr2.Length + extractNum2[0].Length ?? 0;
                    }
                }
            }
            else
            {
                var numberStr = match.Groups["number"].Value;

                var extractNum = numberExtractor.Extract(numberStr);
                if (extractNum == null)
                {
                    extractNum = ordinalExtractor.Extract(numberStr);
                }

                if (extractNum.Count == 1)
                {
                    if (extractNum[0].Start == 0 && extractNum[0].Length == numberStr.Trim().Length)
                    {
                        start = match.Index;
                        length = match.Length;
                    }
                    else if (extractNum[0].Start == 0 && source.EndsWith(numberStr))
                    {
                        start = match.Index;
                        length = match.Length - numberStr.Length + extractNum[0].Length ?? 0;
                    }
                    else if (numberStr.Trim().EndsWith(extractNum[0].Text) && source.StartsWith(numberStr))
                    {
                        start = extractNum[0].Start ?? 0;
                        length = match.Length - numberStr.Length + extractNum[0].Length ?? 0;
                    }
                }
            }
        }
    }
}
