using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public class ModelContainer
    {
        public static readonly string DefaultCulture = Culture.English;

        private Dictionary<KeyValuePair<string, Type>, IModel> ModelInstances = new Dictionary<KeyValuePair<string, Type>, IModel>();

        public IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            IModel model;
            if (!TryGetModel<TModel>(culture, out model, fallbackToDefaultCulture))
            {
                throw new Exception($"ERROR: No IModel instance for {culture}-{typeof(TModel)}");
            }

            return model;
        }

        public bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true)
        {
            model = null;
            var ret = true;
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
                    ret = false;
                }
            }

            if (ret)
            {
                model = ModelInstances[key];
            }

            return ret;
        }

        public bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            IModel model;
            return TryGetModel<TModel>(culture, out model, fallbackToDefaultCulture);
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

        public void RegisterModel(string culture, Type type, IModel model)
        {
            var key = GenerateKey(culture, type);
            if (ModelInstances.ContainsKey(key))
            {
                throw new ArgumentException($"ERROR: {culture}-{type} has been registered.");
            }

            ModelInstances.Add(key, model);
        }

        public void RegisterModel(string culture, Dictionary<Type, IModel> models)
        {
            foreach (var model in models)
            {
                RegisterModel(culture, model.Key, model.Value);
            }
        }
    }
}
