using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class RegexExtension
    {
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

        // We can't trim before match as we may use the match index later
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
    }
}
