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
    }
}
