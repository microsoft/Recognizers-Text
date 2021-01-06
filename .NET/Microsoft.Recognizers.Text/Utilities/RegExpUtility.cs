using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class RegExpUtility
    {
        private const string NameGroup = "name";

        private static int index = 0;

        public static List<string> GetMatches(Regex regex, string input)
        {
            var successMatch = regex.Match(input);
            var matchStrs = new List<string>();

            // Store all match str.
            while (successMatch.Success)
            {
                var matchStr = successMatch.Groups[0].Value;
                matchStrs.Add(matchStr);
                successMatch = successMatch.NextMatch();
            }

            return matchStrs;
        }

        // Regex match with match length equals to text length
        public static bool IsExactMatch(this Regex regex, string text, bool trim)
        {
            var match = regex.Match(text);
            var length = trim ? text.Trim().Length : text.Length;

            return match.Success && match.Length == length;
        }

        // We can't trim before match as we may use the match index later
        public static ConditionalMatch MatchExact(this Regex regex, string text, bool trim)
        {
            var match = regex.Match(text);
            var length = trim ? text.Trim().Length : text.Length;

            return new ConditionalMatch(match, match.Success && match.Length == length);
        }

        public static bool IsNullOrEmpty(ReadOnlySpan<char> span)
        {
            return span == null || span.IsEmpty;
        }

        // @TODO Inefficient.
        public static ConditionalMatch MatchEnd(this Regex regex, string text, bool trim)
        {
            var match = Regex.Match(text, regex.ToString(), RegexOptions.RightToLeft | regex.Options);

            var strAfter = text.AsSpan(match.Index + match.Length);

            if (trim)
            {
                strAfter = strAfter.Trim();
            }

            return new ConditionalMatch(match, match.Success && IsNullOrEmpty(strAfter));
        }

        // We can't trim before match as we may use the match index later
        public static ConditionalMatch MatchBegin(this Regex regex, string text, bool trim)
        {
            var match = regex.Match(text);
            var strBefore = text.AsSpan(0, match.Index);

            if (trim)
            {
                strBefore = strBefore.Trim();
            }

            return new ConditionalMatch(match, match.Success && IsNullOrEmpty(strBefore));
        }

        // MatchBegin can fail if multiple matches are present in text (e.g. regex = "\b(A|B)\b", text = "B ... A ...")
        public static ConditionalMatch MatchesBegin(this Regex regex, string text, bool trim)
        {
            var matches = regex.Matches(text);
            foreach (Match match in matches)
            {
                var strBefore = text.AsSpan(0, match.Index);

                if (trim)
                {
                    strBefore = strBefore.Trim();
                }

                bool isMatchBegin = match.Success && IsNullOrEmpty(strBefore);
                if (isMatchBegin)
                {
                    return new ConditionalMatch(match, match.Success && IsNullOrEmpty(strBefore));
                }
            }

            return new ConditionalMatch(null, false);
        }

        public static string[] Split(Regex regex, string source)
        {
            return regex.Split(source);
        }

        private static string SanitizeGroups(string source)
        {
            Regex matchGroup = new Regex(@"\?< (?<name>\w +) >");

            var result = Regex.Replace(source, matchGroup.ToString(), ReplaceMatchGroup);
            return result;
        }

        private static string ReplaceMatchGroup(Match match)
        {
            var name = match.Groups[NameGroup]?.Value;
            return match.Value.Replace(name, $"{name}__{index++}");
        }
    }
}