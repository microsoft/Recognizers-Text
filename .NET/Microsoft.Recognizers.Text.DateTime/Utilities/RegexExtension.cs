using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class RegexExtension
    {
        // Regex match with match length equals to text length
        public static bool IsExactMatch(this Regex regex, string text)
        {
            var match = regex.Match(text);

            return (match.Success && match.Length == text.Trim().Length);
        }

        // Regex match and the match locate at the end of the text
        public static bool IsEndMatch(this Regex regex, string text)
        {
            var match = Regex.Match(text, regex.ToString(),
                    RegexOptions.RightToLeft | regex.Options);

            return (match.Success && string.IsNullOrWhiteSpace(text.Substring(match.Index + match.Length)));
        }

        // Regex match and the match locate at the beginning of the text
        public static bool IsBeginMatch(this Regex regex, string text)
        {
            var match = regex.Match(text);

            return (match.Success && string.IsNullOrWhiteSpace(text.Substring(0, match.Index)));
        }

        public static ConditionalMatch MatchExact(this Regex regex, string text)
        {
            var match = regex.Match(text);

            return new ConditionalMatch(match, (match.Success && match.Length == text.Trim().Length));
        }

        public static ConditionalMatch MatchEnd(this Regex regex, string text)
        {
            var match = Regex.Match(text, regex.ToString(),
                    RegexOptions.RightToLeft | regex.Options);

            return new ConditionalMatch(match, (match.Success && string.IsNullOrWhiteSpace(text.Substring(match.Index + match.Length))));
        }

        public static ConditionalMatch MatchBegin(this Regex regex, string text)
        {
            var match = regex.Match(text);

            return new ConditionalMatch(match, (match.Success && string.IsNullOrWhiteSpace(text.Substring(0, match.Index))));
        }
    }
}
