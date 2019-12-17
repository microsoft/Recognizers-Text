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
            return !source.StartsWith("-", StringComparison.Ordinal);
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
            var unitIsPrefix = new List<bool>();

            MatchCollection nonUnitMatches = null;
            var prefixMatches = prefixMatcher.Find(source).OrderBy(o => o.Start).ToList();
            var suffixMatches = suffixMatcher.Find(source).OrderBy(o => o.Start).ToList();

            if (prefixMatches.Count > 0 || suffixMatches.Count > 0)
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

                    var closeMatch = false;
                    if (maxFindPref != 0)
                    {
                        // Scan from left to right, find the longest match
                        var lastIndex = start;
                        MatchResult<string> bestMatch = null;

                        foreach (var m in prefixMatches)
                        {
                            if (m.Length > 0 && m.End > start)
                            {
                                break;
                            }

                            var unitStr = source.Substring(m.Start, lastIndex - m.Start);
                            if (m.Length > 0 && unitStr.Trim() == m.Text)
                            {
                                if (unitStr == m.Text)
                                {
                                    closeMatch = true;
                                }

                                bestMatch = m;
                                break;
                            }
                        }

                        if (bestMatch != null)
                        {
                            var offSet = lastIndex - bestMatch.Start;
                            var unitStr = source.Substring(bestMatch.Start, offSet);
                            mappingPrefix[number.Start.Value] = new PrefixUnitResult { Offset = offSet, UnitStr = unitStr };
                        }
                    }

                    mappingPrefix.TryGetValue(start, out PrefixUnitResult prefixUnit);

                    // For currency unit, such as "$ 10 $ 20", get candidate "$ 10" "10 $" "$20" then select to get result.
                    // So add "$ 10" to result here, then get "10 $" in the suffixMatch.
                    // But for case like "摄氏温度10度", "摄氏温度10" will skip this and continue to extend the suffix.
                    if (prefixUnit != null && !prefixMatched && CheckExtractorType(Constants.SYS_UNIT_CURRENCY))
                    {
                        var er = new ExtractResult
                        {
                            Start = number.Start - prefixUnit.Offset,
                            Length = number.Length + prefixUnit.Offset,
                            Text = prefixUnit.UnitStr + number.Text,
                            Type = this.config.ExtractType,
                        };

                        // Relative position will be used in Parser
                        var numberData = number.Clone();
                        numberData.Start = start - er.Start;
                        er.Data = numberData;

                        result.Add(er);
                        unitIsPrefix.Add(true);
                    }

                    if (maxFindSuff > 0)
                    {
                        // If the number already get close prefix currency unit, skip the suffix match.
                        if (CheckExtractorType(Constants.SYS_UNIT_CURRENCY) && closeMatch)
                        {
                            continue;
                        }

                        // find the best suffix unit
                        var maxlen = 0;
                        var firstIndex = start + length;

                        foreach (var m in suffixMatches)
                        {
                            if (m.Length > 0 && m.Start >= firstIndex)
                            {
                                var endpos = m.Start + m.Length - firstIndex;
                                if (maxlen < endpos)
                                {
                                    var midStr = source.Substring(firstIndex, m.Start - firstIndex);
                                    if (string.IsNullOrWhiteSpace(midStr) || midStr.Trim().Equals(this.config.ConnectorToken, StringComparison.Ordinal))
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

                            if (prefixUnit != null && !CheckExtractorType(Constants.SYS_UNIT_CURRENCY))
                            {
                                prefixMatched = true;
                                er.Start -= prefixUnit.Offset;
                                er.Length += prefixUnit.Offset;
                                er.Text = prefixUnit.UnitStr + er.Text;
                            }

                            // Relative position will be used in Parser
                            var numberData = number.Clone();
                            numberData.Start = start - er.Start;
                            er.Data = numberData;

                            // Special treatment, handle cases like '2:00 pm', '00 pm' is not dimension
                            var isNotUnit = false;
                            if (er.Type.Equals(Constants.SYS_UNIT_DIMENSION, StringComparison.Ordinal))
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
                            unitIsPrefix.Add(false);
                        }
                    }

                    if (prefixUnit != null && !prefixMatched && !CheckExtractorType(Constants.SYS_UNIT_CURRENCY))
                    {
                        var er = new ExtractResult
                        {
                            Start = number.Start - prefixUnit.Offset,
                            Length = number.Length + prefixUnit.Offset,
                            Text = prefixUnit.UnitStr + number.Text,
                            Type = this.config.ExtractType,
                        };

                        // Relative position will be used in Parser
                        var numberData = number.Clone();
                        numberData.Start = start - er.Start;
                        er.Data = numberData;

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

            if (CheckExtractorType(Constants.SYS_UNIT_CURRENCY))
            {
                result = SelectCandidates(source, result, unitIsPrefix);
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
                            if (match.Value.Equals(Constants.AMBIGUOUS_TIME_TERM, StringComparison.Ordinal))
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
                var options = RegexOptions.Singleline | RegexOptions.ExplicitCapture | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

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
            var options = RegexOptions.Singleline | RegexOptions.ExplicitCapture | (ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

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

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text)
        {
            if (this.config.AmbiguityFiltersDict != null)
            {
                foreach (var regex in this.config.AmbiguityFiltersDict)
                {
                    foreach (var extractResult in extractResults)
                    {
                        if (regex.Key.IsMatch(extractResult.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            extractResults = extractResults.Where(er => !matches.Any(m => m.Index < er.Start + er.Length && m.Index + m.Length > er.Start))
                                .ToList();
                        }
                    }
                }
            }

            return extractResults;
        }

        private bool CheckExtractorType(string extractorType)
        {
            return this.config.ExtractType.Equals(extractorType, StringComparison.Ordinal);
        }

        private List<ExtractResult> SelectCandidates(string source, List<ExtractResult> extractResults, List<bool> unitIsPrefix)
        {
            int totalCandidate = unitIsPrefix.Count;
            bool haveConflict = false;
            for (var index = 1; index < totalCandidate; index++)
            {
                if (extractResults[index - 1].Start + extractResults[index - 1].Length > extractResults[index].Start)
                {
                    haveConflict = true;
                }
            }

            if (!haveConflict)
            {
                return extractResults;
            }

            var prefixResult = new List<ExtractResult>();
            var suffixResult = new List<ExtractResult>();
            int currentEnd = -1;
            for (var index = 0; index < totalCandidate; index++)
            {
                if (currentEnd < extractResults[index].Start)
                {
                    currentEnd = (int)(extractResults[index].Start + extractResults[index].Length);
                    prefixResult.Add(extractResults[index]);
                }
                else
                {
                    if (unitIsPrefix[index])
                    {
                        prefixResult.RemoveAt(prefixResult.Count - 1);
                        currentEnd = (int)(extractResults[index].Start + extractResults[index].Length);
                        prefixResult.Add(extractResults[index]);
                    }
                }
            }

            currentEnd = source.Length;
            for (var index = totalCandidate - 1; index >= 0; index--)
            {
                if (currentEnd >= extractResults[index].Start + extractResults[index].Length)
                {
                    currentEnd = (int)extractResults[index].Start;
                    suffixResult.Add(extractResults[index]);
                }
                else
                {
                    if (!unitIsPrefix[index])
                    {
                        suffixResult.RemoveAt(suffixResult.Count - 1);
                        currentEnd = (int)extractResults[index].Start;
                        suffixResult.Add(extractResults[index]);
                    }
                }
            }

            // Add Separate unit
            for (var index = totalCandidate; index < extractResults.Count; index++)
            {
                prefixResult.Add(extractResults[index]);
                suffixResult.Add(extractResults[index]);
            }

            if (suffixResult.Count > prefixResult.Count)
            {
                return suffixResult;
            }

            return prefixResult;
        }
    }
}