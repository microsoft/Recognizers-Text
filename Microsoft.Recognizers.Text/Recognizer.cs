using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer : IRecognizer
    {
        private readonly ModelContainer ModelContainer = new ModelContainer();

        protected void RegisterModel(string culture, Type type, IModel model)
        {
            ModelContainer.RegisterModel(culture, type, model);
        }

        protected void RegisterModel(string culture, Dictionary<Type, IModel> models)
        {
            ModelContainer.RegisterModel(culture, models);
        }

        public IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            return ModelContainer.GetModel<TModel>(culture, fallbackToDefaultCulture);
        }

        public bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true)
        {
            return ModelContainer.TryGetModel<TModel>(culture, out model, fallbackToDefaultCulture);
        }

        public bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            return ModelContainer.ContainsModel<TModel>(culture, fallbackToDefaultCulture);
        }
    }
}
