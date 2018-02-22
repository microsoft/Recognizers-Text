using Microsoft.Recognizers.Text.Utilities;
using System;
using System.Linq;

namespace Microsoft.Recognizers.Text
{
    public abstract class Recognizer<TModelOptions> where TModelOptions : struct
    {
        private readonly ModelFactory<TModelOptions> factory;

        public string DefaultCulture { get; private set; }

        public TModelOptions Options { get; private set; }

        protected Recognizer(string defaultCulture, TModelOptions options, bool lazyInitialization = true)
        {
            if (string.IsNullOrWhiteSpace(defaultCulture))
            {
                throw new ArgumentException("culture", "The default culture is required");
            }

            this.DefaultCulture = defaultCulture;
            this.Options = options;

            this.factory = new ModelFactory<TModelOptions>();
            InitializeConfiguration();

            if (!lazyInitialization) this.InitializeModels(options);
        }

        protected Recognizer(string defaultCulture, int options, bool lazyInitialization = true)
            : this(defaultCulture, EnumUtils.Convert<TModelOptions>(options), lazyInitialization)
        {
        }

        protected T GetModel<T>(string culture = null) where T : IModel
        {
            return this.factory.GetModel<T>(culture, DefaultCulture, Options);
        }

        protected void RegisterModel<T>(string culture, Func<TModelOptions, IModel> modelCreator)
        {
            this.factory.Add((culture, typeof(T)), modelCreator);
        }

        protected abstract void InitializeConfiguration();

        protected void InitializeModels(TModelOptions options)
        {
            this.factory.Select((constructor) => this.factory.GetModel<IModel>(constructor.Key.culture, this.DefaultCulture, options));
        }
    }
}