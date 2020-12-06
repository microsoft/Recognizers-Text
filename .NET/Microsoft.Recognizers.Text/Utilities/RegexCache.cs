using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text
{
    public static class RegexCache
    {
        private static ConcurrentDictionary<(string pattern, RegexOptions options), Regex> _cache = new ConcurrentDictionary<(string pattern, RegexOptions options), Regex>();

        public static bool Compiled { get; set; } = false;

        public static Regex Get(string pattern, RegexOptions options)
        {
            if (Compiled)
            {
                return _cache.GetOrAdd((pattern, options), k => new Regex(k.pattern, k.options | RegexOptions.Compiled));
            }
            else
            {
                return _cache.GetOrAdd((pattern, options), k => new Regex(k.pattern, k.options));
            }
        }

        public static Regex Get(string pattern) => Get(pattern, default);
    }
}