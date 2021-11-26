// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class MatchingUtil
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;
        private static readonly Regex InvalidDayNumberPrefix =
            new Regex(Definitions.BaseDateTime.InvalidDayNumberPrefix, RegexFlags);

        public static bool IsInvalidDayNumberPrefix(string prefix)
        {
            return InvalidDayNumberPrefix.IsMatch(prefix);
        }

        public static bool GetAgoLaterIndex(string text, Regex regex, out int index, bool inSuffix)
        {
            index = -1;
            var match = inSuffix ? regex.MatchBegin(text, trim: true) : regex.MatchEnd(text, trim: true);

            if (match.Success)
            {
                index = match.Index + (inSuffix ? match.Length : 0);
                return true;
            }

            return false;
        }

        public static bool GetTermIndex(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text.Trim().Split(' ').Last());
            if (match.Success)
            {
                index = text.Length - text.LastIndexOf(match.Value, StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public static bool ContainsAgoLaterIndex(string text, Regex regex, bool inSuffix)
        {
            return GetAgoLaterIndex(text, regex, out var index, inSuffix);
        }

        public static bool ContainsTermIndex(string text, Regex regex)
        {
            return GetTermIndex(text, regex, out var index);
        }

        // Temporary solution for remove superfluous words only under the Preview mode
        public static string PreProcessTextRemoveSuperfluousWords(string text, StringMatcher matcher, out List<MatchResult<string>> superfluousWordMatches)
        {
            superfluousWordMatches = RemoveSubMatches(matcher.Find(text));

            var bias = 0;

            foreach (var match in superfluousWordMatches)
            {
                text = text.Remove(match.Start - bias, match.Length);
                bias += match.Length;
            }

            return text;
        }

        // Temporary solution for recover superfluous words only under the Preview mode
        public static List<ExtractResult> PostProcessRecoverSuperfluousWords(List<ExtractResult> extractResults, List<MatchResult<string>> superfluousWordMatches, string originText)
        {
            foreach (var match in superfluousWordMatches)
            {
                foreach (var extractResult in extractResults)
                {
                    var extractResultEnd = extractResult.Start + extractResult.Length;
                    if (match.Start > extractResult.Start && extractResultEnd >= match.Start)
                    {
                        extractResult.Length += match.Length;
                    }

                    if (match.Start <= extractResult.Start)
                    {
                        extractResult.Start += match.Length;
                    }
                }
            }

            foreach (var extractResult in extractResults)
            {
                extractResult.Text = originText.Substring((int)extractResult.Start, (int)extractResult.Length);
            }

            return extractResults;
        }

        public static List<MatchResult<string>> RemoveSubMatches(IEnumerable<MatchResult<string>> matchResults)
        {
            var matchList = matchResults.ToList();

            return matchList.Where(item =>
                !matchList.Any(
                    ritem => (ritem.Start < item.Start && ritem.End >= item.End) ||
                             (ritem.Start <= item.Start && ritem.End > item.End))).ToList();
        }
    }
}