using System;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<TModelOptions>
    {
        private readonly ModelFactory<TModelOptions> factory;

        public string RecognizerCulture { get; private set; }

        public TModelOptions RecognizerOptions { get; private set; }

        protected Recognizer(string culture, TModelOptions options)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                throw new ArgumentException("culture", "The Culture is required");
            }

            this.RecognizerCulture = culture;
            this.RecognizerOptions = options;

            this.factory = new ModelFactory<TModelOptions>();
            InitializeConfiguration();
        }

        protected T GetModel<T>() where T : IModel
        {
            return this.factory.GetModel<T>(RecognizerCulture, RecognizerOptions);
        }

        protected void RegisterModel<T>(string culture, Func<TModelOptions, IModel> modelCreator)
        {
            this.factory.Add((culture, typeof(T)), modelCreator);
        }

        protected abstract void InitializeConfiguration();
    }
}