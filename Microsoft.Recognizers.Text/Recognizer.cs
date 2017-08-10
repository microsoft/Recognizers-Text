using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer : IRecognizer
    {
        private readonly ModelContainer modelContainer = new ModelContainer();

        protected void RegisterModel(string culture, Type type, IModel model)
        {
            modelContainer.RegisterModel(culture, type, model);
        }

        protected void RegisterModel(string culture, Dictionary<Type, IModel> models)
        {
            modelContainer.RegisterModel(culture, models);
        }

        public IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            return modelContainer.GetModel<TModel>(culture, fallbackToDefaultCulture);
        }

        public bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true)
        {
            return modelContainer.TryGetModel<TModel>(culture, out model, fallbackToDefaultCulture);
        }

        public bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            return modelContainer.ContainsModel<TModel>(culture, fallbackToDefaultCulture);
        }
    }
}
