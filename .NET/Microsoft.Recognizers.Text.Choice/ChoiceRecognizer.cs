using System.Collections.Generic;
using Microsoft.Recognizers.Text.Choice.Bulgarian;
using Microsoft.Recognizers.Text.Choice.Chinese;
using Microsoft.Recognizers.Text.Choice.Dutch;
using Microsoft.Recognizers.Text.Choice.English;
using Microsoft.Recognizers.Text.Choice.French;
using Microsoft.Recognizers.Text.Choice.German;
using Microsoft.Recognizers.Text.Choice.Japanese;
using Microsoft.Recognizers.Text.Choice.Portuguese;
using Microsoft.Recognizers.Text.Choice.Spanish;
using Microsoft.Recognizers.Text.Choice.Swedish;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceRecognizer : Recognizer<ChoiceOptions>
    {
        public ChoiceRecognizer(string targetCulture, ChoiceOptions options = ChoiceOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public ChoiceRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOptions(options), lazyInitialization)
        {
        }

        public ChoiceRecognizer(ChoiceOptions options = ChoiceOptions.None, bool lazyInitialization = true)
            : base(null, options, lazyInitialization)
        {
        }

        public ChoiceRecognizer(int options, bool lazyInitialization = true)
            : this(null, GetOptions(options), lazyInitialization)
        {
        }

        public static List<ModelResult> RecognizeBoolean(string query, string culture, ChoiceOptions options = ChoiceOptions.None, bool fallbackToDefaultCulture = true)
        {
            var recognizer = new ChoiceRecognizer(options);
            var model = recognizer.GetBooleanModel(culture, fallbackToDefaultCulture);
            return model.Parse(query);
        }

        public IModel GetBooleanModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<BooleanModel>(culture, fallbackToDefaultCulture);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<BooleanModel>(
                Culture.Chinese,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new ChineseBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Dutch,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new DutchBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.English,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new EnglishBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.French,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new FrenchBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.German,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new GermanBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Japanese,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new JapaneseBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Portuguese,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new PortugueseBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Spanish,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new SpanishBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Swedish,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new SwedishBooleanExtractorConfiguration())));

            RegisterModel<BooleanModel>(
                Culture.Bulgarian,
                (options) => new BooleanModel(new BooleanParser(), new BooleanExtractor(new BulgarianBooleanExtractorConfiguration())));
        }
    }
}
