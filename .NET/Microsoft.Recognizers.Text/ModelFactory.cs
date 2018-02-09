using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    internal class ModelFactory<TModelOptions> : Dictionary<(string culture, Type modelType), Func<TModelOptions, IModel>>
    {
        private static ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel> cache = new ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel>();

        public T GetModel<T>(string culture, TModelOptions options) where T : IModel
        {
            if (string.IsNullOrEmpty(culture))
            {
                throw new ArgumentNullException("culture", "Culture is required.");
            }

            // Look in cache
            var cacheKey = (culture.ToLowerInvariant(), typeof(T), options.ToString());
            if (cache.ContainsKey(cacheKey))
            {
                return (T)cache[cacheKey];
            }

            // Use Factory to create instance
            var key = GenerateKey(culture, typeof(T));
            if (this.ContainsKey(key))
            {
                var factoryMethod = this[key];
                var model = (T)factoryMethod(options);

                // Store in cache
                cache[cacheKey] = model;

                return model;
            }

            throw new ArgumentException($"Could not find Model with the specified configuration: {culture}, {typeof(T).ToString()}");
        }

        public new void Add((string culture, Type modelType) config, Func<TModelOptions, IModel> modelCreator)
        {
            base.Add(GenerateKey(config.culture, config.modelType), modelCreator);
        }

        private static (string culture, Type modelType) GenerateKey(string culture, Type modelType)
        {
            return (culture.ToLowerInvariant(), modelType);
        }
    }
}
