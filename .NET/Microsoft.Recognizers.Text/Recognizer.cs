using System;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<TRecognizerOptions>
           where TRecognizerOptions : struct
    {
        private readonly ModelFactory<TRecognizerOptions> factory;

        protected Recognizer(string targetCulture, TRecognizerOptions options, bool lazyInitialization)
        {
            this.Options = options;
            this.TargetCulture = targetCulture;

            this.factory = new ModelFactory<TRecognizerOptions>();
            InitializeConfiguration();

            if (!lazyInitialization)
            {
                this.InitializeModels(targetCulture, options);
            }
        }

        public string TargetCulture { get; private set; }

        public TRecognizerOptions Options { get; private set; }

        public static TRecognizerOptions GetOptions(int value) => EnumUtils.Convert<TRecognizerOptions>(value);

        protected T GetModel<T>(string culture, bool fallbackToDefaultCulture)
                  where T : IModel
        {
            return this.factory.GetModel<T>(culture ?? TargetCulture, fallbackToDefaultCulture, Options);
        }

        protected void RegisterModel<T>(string culture, Func<TRecognizerOptions, IModel> modelCreator)
        {
            this.factory.Add((culture, typeof(T)), modelCreator);
        }

        protected abstract void InitializeConfiguration();

        private void InitializeModels(string targetCulture, TRecognizerOptions options)
        {
            this.factory.InitializeModels(targetCulture, options);
        }
    }
}