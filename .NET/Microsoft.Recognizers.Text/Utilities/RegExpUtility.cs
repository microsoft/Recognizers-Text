using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Utilities
{
    public static class RegExpUtility
    {
        // These three variables use "String.raw" method, that does not exist in dotnet and I could not find the equivalent
        private static Regex matchGroup;

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

        /*
        public static ConditionalMatch MatchEnd(this Regex regex, string text, bool trim) // Can not import ConditionalMatch
        {
            var match = Regex.Match(text, regex.ToString(), RegexOptions.RightToLeft | regex.Options);
            var strAfter = text.Substring(match.Index + match.Length);

            if (trim)
            {
                strAfter = strAfter.Trim();
            }

            return new ConditionalMatch(match, match.Success && string.IsNullOrEmpty(strAfter));
        }

        */

        public static Regex GetFirstMatchIndex(Regex regex, string source)
        {
            bool matched;
            int index;
            string value;
        }

        /*
        {
        var matches = GetMatches(regex, source);

            // Do not have length property even if matches is recognized as a Match objects list, because Match does not have it
            if (matches.Length)
            {
                return { }
            }

            return regex;
        }
        */

        public static GetFirstMatchIndex(Regex regex, string source)
        {
            var matches = GetMatches(regex, source);
            if(matches.)
        }

        // Is this right?
        public static string[] Split(Regex regex, string source)
        {
            return regex.Split(source);
        }

        private static string SanitizeGroups(string source)
        {

            var index = 0;
            matchGroup = new Regex(@"\?< (?<name>\w +) >");

            // Cannot convert lambda expression to type int
            var result = Regex.Replace(source, matchGroup, MatchEvaluator(matchGroup, name, index));
            return result;
        }

        private static string MatchEvaluator(Regex regex, string name, int index)
        {
            return regex.Replace(name, $"{name}__{index++}");
        }
    }
}