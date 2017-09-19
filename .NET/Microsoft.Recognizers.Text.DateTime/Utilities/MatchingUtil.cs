using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class MatchingUtil
    {
        public static bool GetAgoLaterIndex(string text, Regex regex, out int index)
        {
            index = -1;
            var match = regex.Match(text.TrimStart().ToLower());
            if (match.Success)
            {
                index = text.ToLower().LastIndexOf(match.Value, StringComparison.Ordinal) + match.Value.Length;
                return true;
            }

            return false;
        }

        public static bool GetInIndex(string text, Regex regex, out int index)
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

        public static bool ContainsInIndex(string text, Regex regex)
        {
            int index = -1;
            return GetInIndex(text, regex, out index);
        }
    }
}