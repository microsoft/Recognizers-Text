using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitExtractor : IExtractor
    {
        private readonly INumberWithUnitExtractorConfiguration config;

        private readonly StringMatcher suffixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
        private readonly StringMatcher prefixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
        private readonly Regex separateRegex;

        private readonly int maxPrefixMatchLen;

        private readonly char[] separators = { '|' };

        public NumberWithUnitExtractor(INumberWithUnitExtractorConfiguration config)
        {
            this.config = config;

            if (this.config.SuffixList?.Count > 0)
            {         
                suffixMatcher = BuildMatcherFromSet(this.config.SuffixList.Values.ToArray());
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

                // 2 is the maximum length of spaces.
                maxPrefixMatchLen += 2;  
                prefixMatcher = BuildMatcherFromSet(this.config.PrefixList.Values.ToArray());
            }

            separateRegex = BuildSeparateRegexFromSet();
        }

        protected StringMatcher BuildMatcherFromSet(IEnumerable<string> collection, bool ignoreCase = true)
        {
            StringMatcher matcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
            List<string> matcherList = new List<string>();

            foreach (var mapping in collection)
            {
                foreach (var token in mapping.Split('|'))
                {
                    matcherList.Add(token);
                }
            }

            matcher.Init(matcherList);

            return matcher;
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
            var separateWords = new HashSet<string>();
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

            // Sort separateWords using descending length.
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

            // Mix prefix and numbers, make up a prefix-number combination
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

                    // Scan from left to right , find the longest match 
                    var leftStr = source.Substring(number.Start.Value - maxFindPref, maxFindPref);
                    var lastIndex = leftStr.Length;

                    MatchResult<String> bestMatch = null;
                    var prefixMatch = prefixMatcher.Find(leftStr.ToLower());

                    foreach (var m in prefixMatch)
                    {
                        if (m.Length > 0 && leftStr.Substring(m.Start, lastIndex - m.Start).ToLower().Trim() == m.Text)
                        {
                            if (bestMatch == null || bestMatch.Start >= m.Start)
                            {
                                bestMatch = m;
                            }
                        }
                    }

                    if (bestMatch != null)
                    {
                        var offSet = lastIndex - bestMatch.Start;
                        var unitStr = leftStr.Substring(bestMatch.Start, offSet);
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
                    var suffixMatch = suffixMatcher.Find(rightSub.ToLower()).ToList();

                    var maxlen = 0;
                    foreach (var m in suffixMatch)
                    {
                        if (m.Length > 0)
                        {
                            var endpos = m.Start + m.Length;
                            if (m.Start >= 0)
                            {
                                var midStr = rightSub.Substring(0, Math.Min(m.Start, rightSub.Length));

                                if (maxlen < endpos && 
                                    (string.IsNullOrWhiteSpace(midStr) || midStr.Trim().Equals(this.config.ConnectorToken)))
                                {
                                    maxlen = endpos;
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

                        // Relative position will be used in Parser
                        number.Start = start - er.Start;
                        er.Data = number;

                        // Special treatment, handle cases like '2:00 pm', '00 pm' is not dimension
                        var isNotUnit = false;
                        if (er.Type.Equals(Constants.SYS_UNIT_DIMENSION))
                        {
                            var nonUnitMatch = this.config.PmNonUnitRegex.Matches(source);                           

                            foreach (Match time in nonUnitMatch)
                            {
                                if (er.Start >= time.Index && er.Start + er.Length <= time.Index + time.Length)
                                {
                                    isNotUnit = true;
                                    break;
                                }
                            }
                        }

                        if (isNotUnit)
                        {
                            continue;
                        }

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
                    
                    // Relative position will be used in Parser
                    number.Start = start - er.Start;
                    er.Data = number;
                    result.Add(er);
                }
            }
            
            // Extract Separate unit
            if (separateRegex != null)
            {
                ExtractSeparateUnits(source, result);

                // Remove common ambiguous cases
                result = FilterAmbiguity(result, source);
            }

            return result;
        }
        
        public void ExtractSeparateUnits(string source, List<ExtractResult> numDependResults)
        {
            // Default is false
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

            // Extract all SeparateUnits, then merge it with numDependResults
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
                            // Mark as extracted
                            for (int j = 0; j < i; j++)
                            {
                                matchResult[j] = true;
                            }

                            // Special treatment, handle cases like '2:00 pm', both '00 pm' and 'pm' are not dimension
                            var isNotUnit = false;
                            if (match.Value.Equals(Constants.AMBIGUOUS_TIME_TERM))
                            {
                                var nonUnitMatch = this.config.PmNonUnitRegex.Matches(source);

                                foreach (Match time in nonUnitMatch)
                                {
                                    if (DimensionInsideTime(match, time))
                                    {
                                        isNotUnit = true;
                                        break;
                                    }
                                }
                            }

                            if (isNotUnit)
                            {
                                continue;
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

        private bool DimensionInsideTime(Match dimension, Match time)
        {
            bool isSubMatch = dimension.Index >= time.Index && dimension.Index + dimension.Length <= time.Index + time.Length;
            return isSubMatch;        
        }

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> ers, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in config.AmbiguityFiltersDict)
                {
                    if (regex.Key.IsMatch(text))
                    {
                        var matches = regex.Value.Matches(text).Cast<Match>();
                        ers = ers.Where(er =>
                                            !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                 .ToList();
                    }
                }
            }

            return ers;
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
                    // If x is null and y is null, they're equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater. 
                    return 1;
                }
            }
            else
            {
                // If x is not null...
                if (y == null) // ...and y is null, x is greater.
                {
                    return -1;
                }
                else
                {
                    // ...and y is not null, compare the lengths of the two strings.
                    int retval = y.Length.CompareTo(x.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length, the longer string is greater.
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length, sort them with ordinary string comparison.
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