using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number
{
    public abstract class BaseNumberRecognizer : Recognizer<Dictionary<string, Dictionary<Type, IModel>>>
    {
        public static IModel GetModel<TModel>(string culture, bool fallbackToDefaultCulture = true)
        {
            if (!ModelInstances.ContainsKey(culture) || !ModelInstances[culture].ContainsKey(typeof(TModel)))
            {
                if (fallbackToDefaultCulture)
                {
                    culture = DefaultCulture;
                }
                else
                {
                    throw new Exception($"ERROR: No IModel instance for {typeof(TModel)}");
                }
            }

            var model = ModelInstances[culture][typeof(TModel)];

            return model;
        }
    }
}
