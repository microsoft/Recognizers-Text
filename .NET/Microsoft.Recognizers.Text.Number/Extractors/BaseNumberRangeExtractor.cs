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

                    if (start >= 0 && length > 0)
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

            var numberStr1 = match.Groups["number1"].Value;
            var numberStr2 = match.Groups["number2"].Value;

            if (type.Contains("TwoNum"))
            {
                var extractNumList1 = ExtractNumberFromStr(numberStr1);
                var extractNumList2 = ExtractNumberFromStr(numberStr2);

                if (extractNumList1 != null && extractNumList2 != null)
                {
                    bool validNum1 = false, validNum2 = false;
                    start = match.Index;
                    length = match.Length;

                    foreach (var extractNum1 in extractNumList1)
                    {
                        if (numberStr1.Trim().EndsWith(extractNum1.Text) && match.Value.StartsWith(numberStr1))
                        {
                            start = source.IndexOf(extractNum1.Text);
                            length = length - extractNum1.Start ?? 0;
                            validNum1 = true;
                        }
                        else if (extractNum1.Start == 0 && match.Value.EndsWith(numberStr1))
                        {
                            length = length - numberStr1.Length + extractNum1.Length ?? 0;
                            validNum1 = true;
                        }
                        else if (extractNum1.Start == 0 && extractNum1.Length == numberStr1.Trim().Length)
                        {
                            validNum1 = true;
                        }

                        if (validNum1)
                        {
                            break;
                        }
                    }

                    foreach (var extractNum2 in extractNumList2)
                    {
                        if (numberStr2.Trim().EndsWith(extractNum2.Text) && match.Value.StartsWith(numberStr2))
                        {
                            start = source.IndexOf(extractNum2.Text);
                            length = length - extractNum2.Start ?? 0;
                            validNum2 = true;
                        }
                        else if (extractNum2.Start == 0 && match.Value.EndsWith(numberStr2))
                        {
                            length = length - numberStr2.Length + extractNum2.Length ?? 0;
                            validNum2 = true;
                        }
                        else if (extractNum2.Start == 0 && extractNum2.Length == numberStr2.Trim().Length)
                        {
                            validNum2 = true;
                        }

                        if (validNum2)
                        {
                            break;
                        }
                    }
                    
                    if (!validNum1 || !validNum2)
                    {
                        start = -1;
                        length = -1;
                    }
                }
            }
            else
            {
                var numberStr = string.IsNullOrEmpty(numberStr1) ? numberStr2 : numberStr1;

                var extractNumList = ExtractNumberFromStr(numberStr);

                if (extractNumList != null)
                {
                    foreach (var extractNum in extractNumList)
                    {
                        if (extractNum.Start == 0 && match.Value.EndsWith(numberStr))
                        {
                            start = match.Index;
                            length = match.Length - numberStr.Length + extractNum.Length ?? 0;
                            break;
                        }
                        else if (numberStr.Trim().EndsWith(extractNum.Text) && match.Value.StartsWith(numberStr))
                        {
                            start = source.IndexOf(extractNum.Text);
                            length = match.Length - extractNum.Start ?? 0;
                            break;
                        }
                        else if (extractNum.Start == 0 && extractNum.Length == numberStr.Trim().Length)
                        {
                            start = match.Index;
                            length = match.Length;
                            break;
                        }
                    }
                }
            }
        }

        private List<ExtractResult> ExtractNumberFromStr(string numberStr)
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
}
