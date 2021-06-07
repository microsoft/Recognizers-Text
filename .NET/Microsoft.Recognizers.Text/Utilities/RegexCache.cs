using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text
{
    public static class RegexCache
    {
        private static ConcurrentDictionary<(string pattern, RegexOptions options), Regex> _cache = new ConcurrentDictionary<(string pattern, RegexOptions options), Regex>();
        private static FieldInfo _patternFieldGetter = typeof(Regex).GetField("pattern", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Compiled { get; set; } = false;

        public static bool StripPatterns { get; set; } = false;

        public static Regex Get(string pattern, RegexOptions options)
        {
            if (Compiled)
            {
                return _cache.GetOrAdd((pattern, options), k => StripPattern(new Regex(k.pattern, k.options | RegexOptions.Compiled)));
            }
            else
            {
                return _cache.GetOrAdd((pattern, options), k => new Regex(k.pattern, k.options));
            }

            static Regex StripPattern(Regex regex)
            {
                if (StripPatterns)
                {
                    _patternFieldGetter.SetValue(regex, null);
                }

                return regex;
            }
        }

        public static Regex Get(string pattern) => Get(pattern, default);

        public static bool IsMatch(string text, string pattern)
        {
            return Get(pattern).IsMatch(text);
        }
    }
}