using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class RegExpUtility
    {
        private const string NameGrou = "name";

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

        public static ConditionalMatch MatchEnd(this Regex regex, string text, bool trim)
        {
            var match = Regex.Match(text, regex.ToString(), RegexOptions.RightToLeft | regex.Options);
            var strAfter = text.Substring(match.Index + match.Length);

            if (trim)
            {
                strAfter = strAfter.Trim();
            }

            return new ConditionalMatch(match, match.Success && string.IsNullOrEmpty(strAfter));
        }

        // We can't trim before match as we may use the match index later
        public static ConditionalMatch MatchBegin(this Regex regex, string text, bool trim)
        {
            var match = regex.Match(text);
            var strBefore = text.Substring(0, match.Index);

            if (trim)
            {
                strBefore = strBefore.Trim();
            }

            return new ConditionalMatch(match, match.Success && string.IsNullOrEmpty(strBefore));
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