using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Recognizers.Text
{
    internal class ModelContainer
    {
        public static readonly string DefaultCulture = Culture.English;

        private readonly ConcurrentDictionary<Tuple<string, Type, string>, IModel> modelInstances = 
            new ConcurrentDictionary<Tuple<string, Type, string>, IModel>();

        public IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture, string options)
        {
            if (!TryGetModel<TModel>(culture, out IModel model, fallbackToDefaultCulture, options))
            {
                throw new ArgumentException($"ERROR: No IModel instance for {culture}-{typeof(TModel)}--{options}");
            }

            return model;
        }

        public bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture, string options)
        {
            model = null;
            var ret = true;

            var key = GenerateKey(culture, typeof(TModel), options);

            if (!modelInstances.ContainsKey(key))
            {
                if (fallbackToDefaultCulture)
                {
                    culture = DefaultCulture;
                    key = GenerateKey(culture, typeof(TModel), options);
                }

                if (!modelInstances.ContainsKey(key))
                {
                    ret = false;
                }
            }

            if (ret)
            {
                model = modelInstances[key];
            }

            return ret;
        }

        public bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture, string options)
        {
            return TryGetModel<TModel>(culture, out IModel model, fallbackToDefaultCulture, options);
        }

        private static Tuple<string, Type, string> GenerateKey(string culture, Type type, string options)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                throw new ArgumentNullException(nameof(culture));
            }

            culture = culture.ToLower(); // Ignore Case

            return new Tuple<string, Type, string>(culture, type, options);
        }

        public void RegisterModel(string culture, Type type, IModel model, string options)
        {
            var key = GenerateKey(culture, type, options);
            if (modelInstances.ContainsKey(key))
            {
                throw new ArgumentException($"ERROR: {culture}-{type} has already been registered.");
            }

            modelInstances.TryAdd(key, model);
        }

        public void RegisterModel(string culture, Dictionary<Type, IModel> models, string options)
        {
            foreach (var model in models)
            {
                RegisterModel(culture, model.Key, model.Value, options);
            }
        }

        public bool IsSingleModel()
        {
            return modelInstances.Count == 1;
        }

        public IModel GetSingleModel<TModel>()
        {
            if (IsSingleModel())
            {
                return modelInstances[modelInstances.Keys.FirstOrDefault()];
            }
            else
            {
                throw new InvalidOperationException($"Please request a specific culture for {typeof(TModel)}.");
            }
        }

        public bool ContainsModels() {
            return modelInstances.Keys.Any();
        }

    }
}
