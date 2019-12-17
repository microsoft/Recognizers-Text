// ReSharper disable StaticMemberInGenericType

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.Recognizers.Text.InternalCache
{
    public class ResultsCache<TItem>
        where TItem : ICloneableType<TItem>
    {

        private const long BaseCacheSize = 20000;

        private const double CompactionPercentage = 0.6;

        private static readonly MemoryCacheEntryOptions CacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1);

        private readonly IMemoryCache resultsCache;

        // In recognizers usage, DateTime has 4 cache instances, while Number only has one.
        public ResultsCache(int ratioFactor = 1)
        {

            var cacheOptions = new MemoryCacheOptions
            {
                SizeLimit = BaseCacheSize * ratioFactor,
                CompactionPercentage = CompactionPercentage,
                ExpirationScanFrequency = TimeSpan.FromHours(24),
            };

            resultsCache = new MemoryCache(cacheOptions);
        }

        public List<TItem> GetOrCreate(object key, Func<List<TItem>> createItem)
        {

            if (!resultsCache.TryGetValue(key, out List<TItem> results))
            {
                results = createItem();

                resultsCache.Set(key, results, CacheEntryOptions);
            }

            return results.ConvertAll(e => e.Clone());
        }

    }
}
