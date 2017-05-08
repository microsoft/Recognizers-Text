using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitExtractor : IExtractor
    {
        private readonly INumberWithUnitExtractorConfiguration config;

        private readonly HashSet<Regex> SuffixRegexes = new HashSet<Regex>();
        private readonly HashSet<Regex> PrefixRegexes = new HashSet<Regex>();

        private readonly int _maxPrefixMatchLen;
        
        public NumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config)
        {
            this.config = config;
            if (this.config.SuffixList?.Count > 0)
            {
                SuffixRegexes = BuildRegexFromSet(this.config.SuffixList.Values);
            }
            if (this.config.PrefixList?.Count > 0)
            {
                foreach (var preMatch in this.config.PrefixList.Values)
                {
                    var matchList = preMatch.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var match in matchList)
                    {
                        _maxPrefixMatchLen = _maxPrefixMatchLen >= match.Length ? _maxPrefixMatchLen : match.Length;
                    }
                }
                // 2 is the maxium length of spaces.
                _maxPrefixMatchLen += 2;
                PrefixRegexes = BuildRegexFromSet(this.config.PrefixList.Values);
            }
        }
        
        protected HashSet<Regex> BuildRegexFromSet(IEnumerable<string> collection, bool ignoreCase = true)
        {
            var regexes = new HashSet<Regex>();
            foreach (var regexString in collection)
            {
                var regexTokens = new List<string>();
                foreach (var token in regexString.Split('|'))
                {
                    regexTokens.Add(Regex.Escape(token));
                }
                var pattern = $@"{this.config.BuildPrefix}({string.Join("|", regexTokens)}){this.config.BuildSuffix}";
                var options = RegexOptions.Singleline | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                var regex = new Regex(pattern, options);
                regexes.Add(regex);
            }
            return regexes;
        }

        public List<ExtractResult> Extract(string source)
        {
            if (!PreCheckStr(source))
            {
                return new List<ExtractResult>();
            }

            var mappingPrefix = new Dictionary<int, PrefixUnitResult>();
            var matched = new bool[source.Length];
            var numbers = this.config.UnitNumExtractor.Extract(source);
            var result = new List<ExtractResult>();
            var sourceLen = source.Length;

            /* Mix prefix and numbers, make up a prefix-number combination */
            if (_maxPrefixMatchLen != 0)
            {
                foreach (var number in numbers)
                {
                    if (number.Start == null || number.Length == null)
                    {
                        continue;
                    }
                    var maxFindPref = Math.Min(_maxPrefixMatchLen, number.Start.Value);
                    if (maxFindPref == 0)
                    {
                        continue;
                    }
                    /* Scan from left to right , find the longest match */
                    var leftStr = source.Substring(number.Start.Value - maxFindPref, maxFindPref);
                    var lastIndex = leftStr.Length;
                    Match bestMatch = null;
                    foreach (var regex in PrefixRegexes)
                    {
                        var collection = regex.Matches(leftStr);
                        if (collection.Count == 0)
                        {
                            continue;
                        }
                        foreach (Match match in collection)
                        {
                            if (match.Success &&
                                leftStr.Substring(match.Index, lastIndex - match.Index).Trim() == match.Value)
                            {
                                if (bestMatch == null || bestMatch.Index >= match.Index)
                                {
                                    bestMatch = match;
                                }
                            }
                        }
                    }
                    if (bestMatch != null)
                    {
                        var offSet = lastIndex - bestMatch.Index;
                        var unitStr = leftStr.Substring(bestMatch.Index, offSet);
                        mappingPrefix.Add(number.Start.Value, new PrefixUnitResult { Offset = offSet, UnitStr = unitStr });
                    }
                }
            }

            foreach (var number in numbers)
            {
                if (number.Start == null || number.Length == null)
                {
                    continue;
                }
                int start = (int)number.Start, length = (int)number.Length;
                var maxFindLen = sourceLen - start - length;
                PrefixUnitResult prefixUnit;
                mappingPrefix.TryGetValue(start, out prefixUnit);

                if (maxFindLen > 0)
                {
                    var rightSub = source.Substring(start + length, maxFindLen);
                    var unitMatch = SuffixRegexes.Select(r => r.Matches(rightSub)).ToList();

                    var maxlen = 0;
                    for (var i = 0; i < unitMatch.Count; i++)
                    {
                        foreach (Match m in unitMatch[i])
                        {
                            if (m.Length > 0)
                            {
                                var endpos = m.Index + m.Length;
                                if (m.Index >= 0)
                                {
                                    var midStr = rightSub.Substring(0, Math.Min(m.Index, rightSub.Length));
                                    if (maxlen < endpos && (string.IsNullOrWhiteSpace(midStr) || midStr.Trim().Equals(this.config.ConnectorToken)))
                                    {
                                        maxlen = endpos;
                                    }
                                }
                            }
                        }
                    }
                    if (maxlen != 0)
                    {
                        for (var i = 0; i < length + maxlen; i++)
                        {
                            matched[i + start] = true;
                        }
                        var substr = source.Substring(start, length + maxlen);
                        var er = new ExtractResult
                        {
                            Start = start,
                            Length = length + maxlen,
                            Text = substr,
                            Type = this.config.ExtractType
                        };
                        if (prefixUnit != null)
                        {
                            er.Start -= prefixUnit.Offset;
                            er.Length += prefixUnit.Offset;
                            er.Text = prefixUnit.UnitStr + er.Text;
                        }
                        /* Relative position will be used in Parser */
                        number.Start = start - er.Start;
                        er.Data = number;
                        result.Add(er);
                        continue;
                    }
                }

                if (prefixUnit != null)
                {
                    var er = new ExtractResult
                    {
                        Start = number.Start - prefixUnit.Offset,
                        Length = number.Length + prefixUnit.Offset,
                        Text = prefixUnit.UnitStr + number.Text,
                        Type = this.config.ExtractType
                    };
                    /* Relative position will be used in Parser */
                    number.Start = start - er.Start;
                    er.Data = number;
                    result.Add(er);
                }
            }

            return result;
        }

        protected virtual bool PreCheckStr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return true;
        }
    }

    public class PrefixUnitResult
    {
        public int Offset;
        public string UnitStr;
    }
}
