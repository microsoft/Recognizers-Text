using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer
    {
        protected static readonly string DefaultCulture = Culture.English;

        private static ConcurrentDictionary<KeyValuePair<string, Type>, IModel> ModelInstances = new ConcurrentDictionary<KeyValuePair<string, Type>, IModel>();

        protected static IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            var key = GenerateKey(culture, typeof(TModel));
            if (!ModelInstances.ContainsKey(key))
            {
                if (fallbackToDefaultCulture)
                {
                    culture = DefaultCulture;
                    key = GenerateKey(culture, typeof(TModel));
                }

                if (!ModelInstances.ContainsKey(key))
                {
                    throw new Exception($"ERROR: No IModel instance for {typeof(TModel)}");
                }
            }

            var model = ModelInstances[key];

            return model;
        }

        private static KeyValuePair<string, Type> GenerateKey(string culture, Type type)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                throw new ArgumentNullException(nameof(culture));
            }

            culture = culture.ToLower(); // Ignore Case

            return new KeyValuePair<string, Type>(culture, type);
        }

        protected static void RegisterModel(string culture, Type type, IModel model)
        {
            var key = GenerateKey(culture, type);
            if (ModelInstances.ContainsKey(key))
            {
                throw new ArgumentException($"ERROR: {culture}-{type} has been registered.");
            }

            ModelInstances.GetOrAdd(key, model);
        }

        protected static void RegisterModel(string culture, Dictionary<Type, IModel> models)
        {
            foreach (var model in models)
            {
                RegisterModel(culture, model.Key, model.Value);
            }
        }
    }
}
