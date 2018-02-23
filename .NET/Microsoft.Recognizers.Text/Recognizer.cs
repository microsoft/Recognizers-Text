using Microsoft.Recognizers.Text.Utilities;
using System;
using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<TModelOptions> where TModelOptions : struct
    {
        private readonly ModelFactory<TModelOptions> factory;

        public string TargetCulture { get; private set; }

        public TModelOptions Options { get; private set; }

        protected Recognizer(string targetCulture, TModelOptions options, bool lazyInitialization)
        {
            this.Options = options;
            this.TargetCulture = targetCulture;

            this.factory = new ModelFactory<TModelOptions>();
            InitializeConfiguration();

            if (!lazyInitialization)
            {
                this.InitializeModels(targetCulture, options);
            }
        }
        
        protected T GetModel<T>(string culture, bool fallbackToDefaultCulture) where T : IModel
        {
            return this.factory.GetModel<T>(culture ?? TargetCulture, fallbackToDefaultCulture, Options);
        }

        protected void RegisterModel<T>(string culture, Func<TModelOptions, IModel> modelCreator)
        {
            this.factory.Add((culture, typeof(T)), modelCreator);
        }

        protected abstract void InitializeConfiguration();

        protected void InitializeModels(string targetCulture, TModelOptions options)
        {
            this.factory.Keys
                .Where(key => string.IsNullOrEmpty(targetCulture) || key.culture.Equals(targetCulture))
                .ToList()
                .ForEach(key => this.factory.InitializeModel(key.modelType, key.culture, options));
        }

        public static TModelOptions GetOption(int value) => EnumUtils.Convert<TModelOptions>(value);
    }
}