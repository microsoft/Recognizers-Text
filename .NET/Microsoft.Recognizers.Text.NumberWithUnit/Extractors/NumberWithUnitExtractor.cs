// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.NumberWithUnit
{
    public class NumberWithUnitExtractor : IExtractor
    {

        private readonly INumberWithUnitExtractorConfiguration config;

        private readonly StringMatcher suffixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());
        private readonly StringMatcher prefixMatcher = new StringMatcher(MatchStrategy.TrieTree, new NumberWithUnitTokenizer());

        private readonly Regex separateRegex;
        private readonly Regex singleCharUnitRegex = new Regex(BaseUnits.SingleCharUnitRegex,
                                                               RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase, RegexTimeOut);

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

        protected static TimeSpan RegexTimeOut => NumberWithUnitRecognizer.GetTimeout(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool ValidateUnit(string source)
        {
            return !source.StartsWith("-", StringComparison.Ordinal);
        }

        public List<ExtractResult> Extract(string source)
        {
            var result = new List<ExtractResult>();
            IOrderedEnumerable<ExtractResult> numbers;

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

            // Remove matches with wrong length, e.g. both 'm2' and 'm 2' are extracted but only 'm2' represents a unit.
            for (int i = suffixMatches.Count - 1; i >= 0; i--)
            {
                var m = suffixMatches[i];
                if (m.CanonicalValues.All(l => l.Length != m.Length))
                {
                    suffixMatches.RemoveAt(i);
                }
            }

            if (prefixMatches.Count > 0 || suffixMatches.Count > 0)
            {
                numbers = this.config.UnitNumExtractor.Extract(source).OrderBy(o => o.Start);

                // Checking if there are conflicting interpretations between currency unit as prefix and suffix for each number.
                // For example, in Chinese, "$20，300美圆" should be broken into two entities instead of treating 20,300 as one number: "$20" and "300美圆".
                if (numbers.Any() && CheckExtractorType(Constants.SYS_UNIT_CURRENCY) && prefixMatches.Any() && suffixMatches.Any())
                {

                    foreach (var number in numbers)
                    {
                        int start = (int)number.Start, length = (int)number.Length;
                        var numberPrefix = prefixMatches.Any(o => o.Start + o.Length == number.Start);
                        var numberSuffix = suffixMatches.Any(o => o.Start == number.Start + number.Length);

                        if (numberPrefix != false && numberSuffix != false && number.Text.Contains(","))
                        {
                            int commaIndex = (int)number.Start + number.Text.IndexOf(",", StringComparison.Ordinal);
                            source = source.Substring(0, commaIndex) + " " + source.Substring(commaIndex + 1);
                        }
                    }

                    numbers = this.config.UnitNumExtractor.Extract(source).OrderBy(o => o.Start);
                }

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

                                    // Check for brackets
                                    if (m.End < source.Length && (
                                        (midStr.EndsWith("(") && source[m.End] == ')') ||
                                        (midStr.EndsWith("[") && source[m.End] == ']') ||
                                        (midStr.EndsWith("{") && source[m.End] == '}') ||
                                        (midStr.EndsWith("<") && source[m.End] == '>')))
                                    {
                                        maxlen = m.End - firstIndex + 1;
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
            else
            {
                numbers = null;
            }

            // Extract Separate unit
            if (separateRegex != null)
            {
                if (nonUnitMatches == null)
                {
                    nonUnitMatches = this.config.NonUnitRegex.Matches(source);
                }

                ExtractSeparateUnits(source, result, nonUnitMatches);

            }

            // Remove common ambiguous cases
            result = FilterAmbiguity(result, source);

            // Remove entity-specific ambiguous cases
            if (CheckExtractorType(Constants.SYS_UNIT_TEMPERATURE))
            {
                result = FilterAmbiguity(result, source, this.config.TemperatureAmbiguityFiltersDict);
            }
            else if (CheckExtractorType(Constants.SYS_UNIT_DIMENSION))
            {
                result = FilterAmbiguity(result, source, this.config.DimensionAmbiguityFiltersDict);

                // Only compound those dimensions that set within the LengthUnitToSubUnitMap, for now, it supports compound with foot and inch.
                if (this.config as English.DimensionExtractorConfiguration != null
                    && suffixMatches.Count > 0
                    && result != null
                    && result.Count >= 2)
                {
                    var compoundUnit = English.DimensionExtractorConfiguration.DimensionSuffixList
                        .Where(kvp => English.DimensionExtractorConfiguration.LengthUnitToSubUnitMap.ContainsKey(kvp.Key))
                        .Where(kvp => kvp.Value.Split('|').Contains(suffixMatches[0].Text))
                        .Select(kvp => kvp)
                        .ToList();

                    if (compoundUnit.Any())
                    {
                        result = MergeCompoundUnits(result, source);
                    }
                }
            }

            if (CheckExtractorType(Constants.SYS_UNIT_CURRENCY))
            {
                result = SelectCandidates(source, result, unitIsPrefix);
            }

            // Expand Chinese phrase to the `half` patterns when it follows closely origin phrase.
            this.config.ExpandHalfSuffix(source, ref result, numbers);

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

                var regex = new Regex(pattern, options, RegexTimeOut);
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

            var regex = new Regex(pattern, options, RegexTimeOut);
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

        private List<ExtractResult> FilterAmbiguity(List<ExtractResult> extractResults, string text, Dictionary<Regex, Regex> ambiguityFiltersDict = null)
        {
            // If no filter is specified, use common AmbiguityFilter
            if (ambiguityFiltersDict == null)
            {
                ambiguityFiltersDict = this.config.AmbiguityFiltersDict;
            }

            if (ambiguityFiltersDict != null)
            {
                foreach (var regex in ambiguityFiltersDict)
                {
                    foreach (var extractResult in extractResults)
                    {
                        if (regex.Key.IsMatch(extractResult.Text))
                        {
                            var matches = regex.Value.Matches(text).Cast<Match>();
                            extractResults = extractResults.Where(er =>
                                                                      !matches.Any(m => m.Index < er.Start + er.Length &&
                                                                                   m.Index + m.Length > er.Start)).ToList();
                        }
                    }
                }
            }

            // Filter single-char units if not exact match
            extractResults = extractResults.Where(er => !(er.Length != text.Length && singleCharUnitRegex.IsMatch(er.Text))).ToList();

            return extractResults;
        }

        /// <summary>
        /// Merge compound units when extracting, like compound 5 foot 3 inch as one entity.
        /// </summary>
        /// <param name="ers">Extract results.</param>
        /// <param name="source">Input text.</param>
        /// <returns>The compounded units.</returns>
        private List<ExtractResult> MergeCompoundUnits(List<ExtractResult> ers, string source)
        {
            var result = new List<ExtractResult>();

            MergePureNumber(source, ers);

            if (ers.Count == 0)
            {
                return result;
            }

            var groups = new int[ers.Count];
            groups[0] = 0;

            for (var idx = 0; idx < ers.Count - 1; idx++)
            {
                if (ers[idx].Type != ers[idx + 1].Type &&
                    !ers[idx].Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal) &&
                    !ers[idx + 1].Type.Equals(Constants.SYS_NUM, StringComparison.Ordinal))
                {
                    continue;
                }

                if (ers[idx].Data is ExtractResult er &&
                    !er.Data.ToString().StartsWith(Number.Constants.INTEGER_PREFIX, StringComparison.Ordinal))
                {
                    groups[idx + 1] = groups[idx] + 1;
                    continue;
                }

                var middleBegin = ers[idx].Start + ers[idx].Length ?? 0;
                var middleEnd = ers[idx + 1].Start ?? 0;
                var length = middleEnd - middleBegin;

                if (length < 0)
                {
                    continue;
                }

                var middleStr = source.Substring(middleBegin, length).Trim();

                // Separated by whitespace
                if (string.IsNullOrEmpty(middleStr))
                {
                    groups[idx + 1] = groups[idx];
                    continue;
                }

                // Separated by connectors
                var match = config.CompoundUnitConnectorRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    groups[idx + 1] = groups[idx];
                }
                else
                {
                    groups[idx + 1] = groups[idx] + 1;
                }
            }

            for (var idx = 0; idx < ers.Count; idx++)
            {
                if (idx == 0 || groups[idx] != groups[idx - 1])
                {
                    var tmpExtractResult = ers[idx].Clone();

                    tmpExtractResult.Data = new List<ExtractResult>
                    {
                        new ExtractResult
                        {
                            Data = ers[idx].Data,
                            Length = ers[idx].Length,
                            Start = ers[idx].Start,
                            Text = ers[idx].Text,
                            Type = ers[idx].Type,
                        },
                    };

                    result.Add(tmpExtractResult);
                }

                // Reduce extract results in same group
                if (idx + 1 < ers.Count && groups[idx + 1] == groups[idx])
                {
                    var group = groups[idx];

                    var periodBegin = result[group].Start ?? 0;
                    var periodEnd = (ers[idx + 1].Start ?? 0) + (ers[idx + 1].Length ?? 0);

                    result[group].Length = periodEnd - periodBegin;
                    result[group].Text = source.Substring(periodBegin, periodEnd - periodBegin);
                    result[group].Type = Constants.SYS_UNIT_CURRENCY;
                    (result[group].Data as List<ExtractResult>)?.Add(ers[idx + 1]);
                }
            }

            for (var idx = 0; idx < result.Count; idx++)
            {
                var innerData = result[idx].Data as List<ExtractResult>;
                if (innerData?.Count == 1)
                {
                    result[idx] = innerData[0];
                }
            }

            result.RemoveAll(o => o.Type == Constants.SYS_NUM);

            return result;
        }

        private void MergePureNumber(string source, List<ExtractResult> ers)
        {
            var numErs = config.UnitNumExtractor.Extract(source);

            var unitNumbers = new List<ExtractResult>();
            for (int i = 0, j = 0; i < numErs.Count; i++)
            {
                bool hasBehindExtraction = false;
                while (j < ers.Count && ers[j].Start + ers[j].Length < numErs[i].Start)
                {
                    hasBehindExtraction = true;
                    j++;
                }

                if (!hasBehindExtraction)
                {
                    continue;
                }

                // Filter cases like "1 dollars 11a", "11" is not the fraction here.
                if (source.Length > numErs[i].Start + numErs[i].Length)
                {
                    var endChar = source.Substring(numErs[i].Length + numErs[i].Start ?? 0, 1);
                    if (char.IsLetter(endChar[0]) && !SimpleTokenizer.IsCjk(endChar[0]))
                    {
                        continue;
                    }
                }

                var middleBegin = ers[j - 1].Start + ers[j - 1].Length ?? 0;
                var middleEnd = numErs[i].Start ?? 0;

                var middleStr = source.Substring(middleBegin, middleEnd - middleBegin).Trim();

                // Separated by whitespace
                if (string.IsNullOrEmpty(middleStr))
                {
                    unitNumbers.Add(numErs[i]);
                    continue;
                }

                // Separated by connectors
                var match = config.CompoundUnitConnectorRegex.Match(middleStr);
                if (match.Success && match.Index == 0 && match.Length == middleStr.Length)
                {
                    unitNumbers.Add(numErs[i]);
                }
            }

            foreach (var extractResult in unitNumbers)
            {
                var overlap = false;
                foreach (var er in ers)
                {
                    if (er.Start <= extractResult.Start && er.Start + er.Length >= extractResult.Start)
                    {
                        overlap = true;
                    }
                }

                if (!overlap)
                {
                    ers.Add(extractResult);
                }
            }

            ers.Sort((x, y) => x.Start - y.Start ?? 0);
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

            // Find prefix units with no space, e.g. '$50'.
            var noSpaceUnits = new List<Token>();
            foreach (var prefix in prefixResult)
            {
                if (prefix.Data is ExtractResult numberResult)
                {
                    var unitStr = prefix.Text.Substring(0, (int)numberResult.Start);
                    if (unitStr.Length > 0 && unitStr.Equals(unitStr.TrimEnd(), StringComparison.Ordinal))
                    {
                        noSpaceUnits.Add(new Token((int)prefix.Start, unitStr.Length));
                    }
                }
            }

            // Remove from suffixResult units that are also prefix units with no space,
            // e.g. in '1 $50', '$' should not be considered a suffix unit.
            for (var index = suffixResult.Count - 1; index >= 0; index--)
            {
                var suffix = suffixResult[index];
                if (noSpaceUnits.Any(o => suffix.Start <= o.Start && suffix.Start + suffix.Length >= o.End))
                {
                    suffixResult.RemoveAt(index);
                }
            }

            // Add Separate unit
            for (var index = totalCandidate; index < extractResults.Count; index++)
            {
                prefixResult.Add(extractResults[index]);
                suffixResult.Add(extractResults[index]);
            }

            if (suffixResult.Count >= prefixResult.Count)
            {
                suffixResult.Sort((x, y) => x.Start - y.Start ?? 0);
                return suffixResult;
            }

            return prefixResult;
        }
    }
}