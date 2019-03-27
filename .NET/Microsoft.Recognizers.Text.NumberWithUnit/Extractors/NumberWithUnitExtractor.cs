using System;
using System.Collections.Generic;
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

        public static bool ValidateUnit(string source)
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
            var sourceLen = source.Length;
            var prefixMatched = false;

            MatchCollection nonUnitMatches = null;
            var prefixMatch = prefixMatcher.Find(source).OrderBy(o => o.Start).ToList();
            var suffixMatch = suffixMatcher.Find(source).OrderBy(o => o.Start).ToList();

            if (prefixMatch.Count > 0 || suffixMatch.Count > 0)
            {
                var numbers = this.config.UnitNumExtractor.Extract(source).OrderBy(o => o.Start);

                // Special case for cases where number multipliers clash with unit
                var ambiguousMultiplierRegex = this.config.AmbiguousUnitNumberMultiplierRegex;
                if (ambiguousMultiplierRegex != null)
                {
                    foreach (var number in numbers)
                    {
                        var match = ambiguousMultiplierRegex.Matches(number.Text);
                        if (match.Count == 1)
                        {
                            var newLength = number.Text.Length - match[0].Length;
                            number.Text = number.Text.Substring(0, newLength);
                            number.Length = newLength;
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
                    var maxFindPref = Math.Min(maxPrefixMatchLen, number.Start.Value);
                    var maxFindSuff = sourceLen - start - length;

                    if (maxFindPref != 0)
                    {
                        // Scan from left to right, find the longest match
                        var lastIndex = start;
                        MatchResult<string> bestMatch = null;

                        foreach (var m in prefixMatch)
                        {
                            if (m.Length > 0 && m.End > start)
                            {
                                break;
                            }

                            if (m.Length > 0 && source.Substring(m.Start, lastIndex - m.Start).ToLower().Trim() == m.Text)
                            {
                                bestMatch = m;
                                break;
                            }
                        }

                        if (bestMatch != null)
                        {
                            var offSet = lastIndex - bestMatch.Start;
                            var unitStr = source.Substring(bestMatch.Start, offSet);
                            mappingPrefix.Add(number.Start.Value, new PrefixUnitResult { Offset = offSet, UnitStr = unitStr });
                        }
                    }

                    mappingPrefix.TryGetValue(start, out PrefixUnitResult prefixUnit);
                    if (maxFindSuff > 0)
                    {
                        // find the best suffix unit
                        var maxlen = 0;
                        var firstIndex = start + length;

                        foreach (var m in suffixMatch)
                        {
                            if (m.Length > 0 && m.Start >= firstIndex)
                            {
                                var endpos = m.Start + m.Length - firstIndex;
                                if (maxlen < endpos)
                                {
                                    var midStr = source.Substring(firstIndex, m.Start - firstIndex);
                                    if (string.IsNullOrWhiteSpace(midStr) || midStr.Trim().Equals(this.config.ConnectorToken))
                                    {
                                        maxlen = endpos;
                                    }
                                }
                            }
                        }

                        if (maxlen != 0)
                        {
                            var substr = source.Substring(start, length + maxlen);
                            var er = new ExtractResult
                            {
                                Start = start,
                                Length = length + maxlen,
                                Text = substr,
                                Type = this.config.ExtractType,
                            };

                            if (prefixUnit != null)
                            {
                                prefixMatched = true;
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
                                if (nonUnitMatches == null)
                                {
                                    nonUnitMatches = this.config.NonUnitRegex.Matches(source);
                                }

                                foreach (Match time in nonUnitMatches)
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
                        }
                    }

                    if (prefixUnit != null && !prefixMatched)
                    {
                        var er = new ExtractResult
                        {
                            Start = number.Start - prefixUnit.Offset,
                            Length = number.Length + prefixUnit.Offset,
                            Text = prefixUnit.UnitStr + number.Text,
                            Type = this.config.ExtractType,
                        };

                        // Relative position will be used in Parser
                        number.Start = start - er.Start;
                        er.Data = number;
                        result.Add(er);
                    }
                }
            }

            // Extract Separate unit
            if (separateRegex != null)
            {
                if (nonUnitMatches == null)
                {
                    nonUnitMatches = this.config.NonUnitRegex.Matches(source);
                }

                ExtractSeparateUnits(source, result, nonUnitMatches);

                // Remove common ambiguous cases
                result = FilterAmbiguity(result, source);
            }

            return result;
        }

        public void ExtractSeparateUnits(string source, List<ExtractResult> numDependResults, MatchCollection nonUnitMatches)
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
                }
                while (i < numDependResult.Length.Value);
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
                                foreach (Match nonUnitMatch in nonUnitMatches)
                                {
                                    if (IsMatchOverlap(match, nonUnitMatch))
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
                                Data = null,
                            });
                        }
                    }
                }
            }
        }

        protected static StringMatcher BuildMatcherFromSet(IEnumerable<string> collection, bool ignoreCase = true)
        {
            StringMatcher matcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
            List<string> matchTerms = collection.SelectMany(words =>
                words.Trim().Split('|').Where(word => !string.IsNullOrWhiteSpace(word)).Distinct()).ToList();

            matcher.Init(matchTerms);

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

        protected Regex BuildSeparateRegexFromSet(bool ignoreCase = false)
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

            regexTokens.Sort(new StringComparer());
            var pattern = $@"{this.config.BuildPrefix}({string.Join("|", regexTokens)}){this.config.BuildSuffix}";
            var options = RegexOptions.Singleline | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            var regex = new Regex(pattern, options);
            return regex;
        }

        protected virtual bool PreCheckStr(string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        private static bool IsMatchOverlap(Match match, Match nonUnitMatch)
        {
            bool isSubMatch = match.Index >= nonUnitMatch.Index && match.Index + match.Length <= nonUnitMatch.Index + nonUnitMatch.Length;
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
                        ers = ers.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                 .ToList();
                    }
                }
            }

            return ers;
        }
    }
}