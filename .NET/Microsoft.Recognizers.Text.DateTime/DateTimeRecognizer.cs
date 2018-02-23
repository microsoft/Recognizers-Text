using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer<DateTimeOptions>
    {
        public DateTimeRecognizer(string targetCulture, DateTimeOptions options = DateTimeOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public DateTimeRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOption(options), lazyInitialization)
        {
        }

        public DateTimeRecognizer(DateTimeOptions options = DateTimeOptions.None, bool lazyInitialization = true)
            : this(null, options, lazyInitialization)
        {
        }

        public DateTimeRecognizer(int options, bool lazyInitialization = true)
            : this(null, options, lazyInitialization)
        {
        }

        public DateTimeModel GetDateTimeModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }
        
        public static List<ModelResult> RecognizeDateTime(string query, string culture, DateTimeOptions options = DateTimeOptions.None, System.DateTime? refTime = null, bool fallbackToDefaultCulture = true)
        {
            var recognizer = new DateTimeRecognizer(options);
            var model = recognizer.GetDateTimeModel(culture, fallbackToDefaultCulture);
            return model.Parse(query, refTime ?? System.DateTime.Now);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<DateTimeModel>(
                Culture.English,
                (options) => new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration(options)),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.Chinese,
                (options) => new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration(options)),
                    new MergedExtractorChs(options)));

            RegisterModel<DateTimeModel>(
                Culture.Spanish,
                (options) => new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration(options)),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.French,
                (options) => new DateTimeModel(
                    new BaseMergedParser(new FrenchMergedParserConfiguration(options)),
                    new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.Portuguese,
                (options) => new DateTimeModel(
                    new BaseMergedParser(new PortugueseMergedParserConfiguration(options)),
                    new BaseMergedExtractor(new PortugueseMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.German,
                (options) => new DateTimeModel(
                            new BaseMergedParser(new GermanMergedParserConfiguration(options)),
                            new BaseMergedExtractor(new GermanMergedExtractorConfiguration(options))));
        }
    }
}