using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    internal class ModelFactory<TModelOptions> : Dictionary<(string culture, Type modelType), Func<TModelOptions, IModel>>
    {
        private static ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel> cache = new ConcurrentDictionary<(string culture, Type modelType, string modelOptions), IModel>();

        public T GetModel<T>(string culture, string defaultCulture, TModelOptions options) where T : IModel
        {
            string selectedCulture = string.IsNullOrEmpty(culture) ? defaultCulture : culture;
            if (string.IsNullOrEmpty(selectedCulture))
            {
                throw new ArgumentNullException("culture", "Culture is required.");
            }

            if (TryGetModel(selectedCulture, options, out T model))
            {
                return model;
            }
            else if (TryGetModel(defaultCulture, options, out model))
            {
                return model;
            }

            throw new ArgumentException($"Could not find Model with the specified configuration: {culture}, {typeof(T).ToString()}");
        }

        private bool TryGetModel<T>(string culture, TModelOptions options, out T model) where T : IModel
        {
            // Look in cache
            var cacheKey = (culture.ToLowerInvariant(), typeof(T), options.ToString());
            if (cache.ContainsKey(cacheKey))
            {
                model = (T)cache[cacheKey];
                return true;
            }

            // Use Factory to create instance
            var key = GenerateKey(culture, typeof(T));
            if (this.ContainsKey(key))
            {
                var factoryMethod = this[key];
                model = (T)factoryMethod(options);

                // Store in cache
                cache[cacheKey] = model;

                return true;
            }

            model = default(T);
            return false;
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
