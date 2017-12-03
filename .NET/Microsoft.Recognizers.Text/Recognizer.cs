using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer : IRecognizer
    {
        private readonly ModelContainer modelContainer = new ModelContainer();

        protected void RegisterModel(string culture, Type type, string options, IModel model)
        {
            modelContainer.RegisterModel(culture, type, model, options);
        }

        protected void RegisterModel(string culture, string options, Dictionary<Type, IModel> models)
        {
            modelContainer.RegisterModel(culture, models, options);
        }

        public IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true, string options = "")
        {
            return modelContainer.GetModel<TModel>(culture, fallbackToDefaultCulture, options);
        }

        public bool TryGetModel<TModel>(string culture, out IModel model, bool fallbackToDefaultCulture = true, string options = "")
        {
            return modelContainer.TryGetModel<TModel>(culture, out model, fallbackToDefaultCulture, options);
        }

        protected bool ContainsModels()
        {
            return modelContainer.ContainsModels();
        }

        public bool ContainsModel<TModel>(string culture, bool fallbackToDefaultCulture = true, string options = "")
        {
            return modelContainer.ContainsModel<TModel>(culture, fallbackToDefaultCulture, options);
        }

        protected IModel GetSingleModel<TModel>()
        {
            return modelContainer.GetSingleModel<TModel>();
        }
    }
}
