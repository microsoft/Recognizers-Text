using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitExtractor : IExtractor
    {
        private readonly INumberWithUnitExtractorConfiguration config;

        private readonly HashSet<Regex> suffixRegexes = new HashSet<Regex>();
        private readonly HashSet<Regex> prefixRegexes = new HashSet<Regex>();
        private readonly Regex separateRegex;

        private readonly int maxPrefixMatchLen;

        private readonly char[] separators = { '|' };

        public NumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config)
        {
            this.config = config;

            if (this.config.SuffixList?.Count > 0)
            {
                suffixRegexes = BuildRegexFromSet(this.config.SuffixList.Values);
            }

            if (this.config.PrefixList?.Count > 0)
            {

                foreach (var preMatch in this.config.PrefixList.Values)
                {
                    var matchList = preMatch.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var match in matchList)
                    {
                        maxPrefixMatchLen = maxPrefixMatchLen >= match.Length ? maxPrefixMatchLen : match.Length;
                    }
                }

                // 2 is the maxium length of spaces.
                maxPrefixMatchLen += 2;
                prefixRegexes = BuildRegexFromSet(this.config.PrefixList.Values);
            }

            separateRegex = BuildSeparateRegexFromSet();
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

        protected Regex BuildSeparateRegexFromSet(bool ignoreCase = true)
        {
            HashSet<string> separateWords = new HashSet<string>();
            if (config.PrefixList?.Count > 0)
            {
                foreach (var addWord in config.PrefixList.Values)
                {
                    foreach (var word in addWord.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (ValidateUnit(word))
                        {
                            separateWords.Add(word);
                        }
                    }
                }
            }

            if (config.SuffixList?.Count > 0)
            {
                foreach (var addWord in config.SuffixList.Values)
                {
                    foreach (var word in addWord.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (ValidateUnit(word))
                        {
                            separateWords.Add(word);
                        }
                    }
                }
            }

            if (config.AmbiguousUnitList?.Count > 0)
            {
                var abandonWords = config.AmbiguousUnitList;
                foreach (var abandonWord in abandonWords)
                {
                    if (separateWords.Contains(abandonWord))
                    {
                        separateWords.Remove(abandonWord);
                    }
                }
            }

            //Sort separateWords using descending length.
            var regexTokens = separateWords.Select(Regex.Escape).ToList();
            if (regexTokens.Count == 0)
            {
                return null;
            }

            regexTokens.Sort(new DinoComparer());
            var pattern = $@"{this.config.BuildPrefix}({string.Join("|", regexTokens)}){this.config.BuildSuffix}";
            var options = RegexOptions.Singleline | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            var regex = new Regex(pattern, options);
            return regex;
        }

        public bool ValidateUnit(string source)
        {
            return !source.StartsWith("-");
        }

        public List<ExtractResult> Extract(string source)
        {

            var result = new List<ExtractResult>();

            if (!PreCheckStr(source))
            {
                return result;
            }

            var mappingPrefix = new Dictionary<int, PrefixUnitResult>();
            var matched = new bool[source.Length];
            var numbers = this.config.UnitNumExtractor.Extract(source);
            var sourceLen = source.Length;

            /* Mix prefix and numbers, make up a prefix-number combination */
            if (maxPrefixMatchLen != 0)
            {
                foreach (var number in numbers)
                {
                    if (number.Start == null || number.Length == null)
                    {
                        continue;
                    }

                    var maxFindPref = Math.Min(maxPrefixMatchLen, number.Start.Value);
                    if (maxFindPref == 0)
                    {
                        continue;
                    }

                    /* Scan from left to right , find the longest match */
                    var leftStr = source.Substring(number.Start.Value - maxFindPref, maxFindPref);
                    var lastIndex = leftStr.Length;

                    Match bestMatch = null;
                    foreach (var regex in prefixRegexes)
                    {
                        var collection = regex.Matches(leftStr);
                        if (collection.Count == 0)
                        {
                            continue;
                        }

                        foreach (Match match in collection)
                        {
                            if (match.Success && leftStr.Substring(match.Index, lastIndex - match.Index).Trim() == match.Value)
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
                mappingPrefix.TryGetValue(start, out PrefixUnitResult prefixUnit);

                if (maxFindLen > 0)
                {
                    var rightSub = source.Substring(start + length, maxFindLen);
                    var unitMatch = suffixRegexes.Select(r => r.Matches(rightSub)).ToList();

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
            
            //extract Separate unit
            if (separateRegex != null)
            {
                ExtractSeparateUnits(source, result);
            }

            return result;
        }
        
        public void ExtractSeparateUnits(string source, List<ExtractResult> numDependResults)
        {
            //Default is false
            bool[] matchResult = new bool[source.Length];
            foreach (var numDependResult in numDependResults)
            {
                int start = numDependResult.Start.Value;
                int i = 0;
                do
                {
                    matchResult[start + i++] = true;
                } while (i < numDependResult.Length.Value);
            }

            //Extract all SeparateUnits, then merge it with numDependResults
            var matchCollection = separateRegex.Matches(source);
            if (matchCollection.Count != 0)
            {
                foreach (Match match in matchCollection)
                {
                    if (match.Success)
                    {
                        int i = 0;
                        while (i < match.Length && !matchResult[match.Index + i])
                        {
                            i++;
                        }

                        if (i == match.Length)
                        {
                            //Mark as extracted
                            for (int j = 0; j < i; j++)
                            {
                                matchResult[j] = true;
                            }

                            numDependResults.Add(new ExtractResult
                            {
                                Start = match.Index,
                                Length = match.Length,
                                Text = match.Value,
                                Type = this.config.ExtractType,
                                Data = null
                            });
                        }
                    }
                }
            }
        }
        
        protected virtual bool PreCheckStr(string str)
        {
            return !string.IsNullOrEmpty(str);
        }

    }

    public class DinoComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return 1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return -1;
                }
                else
                {
                    // ...and y is not null, compare the 
                    // lengths of the two strings.
                    //
                    int retval = y.Length.CompareTo(x.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return string.Compare(x, y, StringComparison.Ordinal);
                    }
                }
            }
        }
    }

    public class PrefixUnitResult
    {
        public int Offset;
        public string UnitStr;
    }
 
}