using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Text.Matcher;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class MatchingUtil
    {
        public static bool GetAgoLaterIndex(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text.TrimStart().ToLower());
            if (match.Success && match.Index == 0)
            {
                index = text.ToLower().LastIndexOf(match.Value, StringComparison.Ordinal) + match.Value.Length;
                return true;
            }

            return false;
        }

        public static bool GetTermIndex(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text.Trim().ToLower().Split(' ').Last());
            if (match.Success)
            {
                index = text.Length - text.ToLower().LastIndexOf(match.Value, StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public static bool ContainsAgoLaterIndex(string text, Regex regex)
        {
            int index = -1;
            return GetAgoLaterIndex(text, regex, out index);
        }

        public static bool ContainsTermIndex(string text, Regex regex)
        {
            int index = -1;
            return GetTermIndex(text, regex, out index);
        }

        // Temporary solution for remove superfluous words only under the Preview mode
        public static string PreProcessTextRemoveSuperfluousWords(string text, StringMatcher matcher, out List<MatchResult<string>> superfluousWordMatches)
        {
            superfluousWordMatches = matcher.Find(text).ToList();
            var bias = 0;

            foreach (var match in superfluousWordMatches)
            {
                text = text.Remove(match.Start - bias, match.Length);
                bias += match.Length;
            }

            return text;
        }

        // Temporary solution for recover superfluous words only under the Preview mode
        public static List<ExtractResult> PosProcessExtractionRecoverSuperfluousWords(List<ExtractResult> extractResults,
            List<MatchResult<string>> superfluousWordMatches, string originText)
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

    }
}