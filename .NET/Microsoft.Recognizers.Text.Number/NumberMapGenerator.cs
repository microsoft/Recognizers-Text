using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Recognizers.Text.Number
{
    public static class NumberMapGenerator
    {
        public static ImmutableDictionary<string, long> InitOrdinalNumberMap(Dictionary<string, long> ordinalNumberMap, Dictionary<string, long> prefixCardinalMap, Dictionary<string, long> suffixOrdinalMap)
        {
            var simpleOrdinalDictionary = ordinalNumberMap.ToDictionary(
                                          entry => entry.Key, entry => entry.Value);

            foreach (var suffix in suffixOrdinalMap)
            {
                foreach (var prefix in prefixCardinalMap)
                {
                    simpleOrdinalDictionary.Add(prefix.Key + suffix.Key, prefix.Value * suffix.Value);
                }
            }

            return new Dictionary<string, long>(simpleOrdinalDictionary).ToImmutableDictionary();
        }
    }
}
