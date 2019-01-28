using System.Collections.Generic;

using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.Dutch;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer<DateTimeOptions>
    {
        public DateTimeRecognizer(string targetCulture, DateTimeOptions options = DateTimeOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public DateTimeRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOptions(options), lazyInitialization)
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

        public static List<ModelResult> RecognizeDateTime(string query, string culture, DateTimeOptions options = DateTimeOptions.None, System.DateTime? refTime = null, bool fallbackToDefaultCulture = true)
        {
            var recognizer = new DateTimeRecognizer(options);
            var model = recognizer.GetDateTimeModel(culture, fallbackToDefaultCulture);
            return model.Parse(query, refTime ?? System.DateTime.Now);
        }

        public DateTimeModel GetDateTimeModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<DateTimeModel>(
                Culture.English,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new EnglishMergedParserConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: false))),
                    new BaseMergedDateTimeExtractor(
                        new EnglishMergedExtractorConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: false)))));

            RegisterModel<DateTimeModel>(
                Culture.EnglishOthers,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new EnglishMergedParserConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: true))),
                    new BaseMergedDateTimeExtractor(
                        new EnglishMergedExtractorConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: true)))));

            RegisterModel<DateTimeModel>(
                Culture.Chinese,
                options => new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration(options)),
                    new ChineseMergedExtractorConfiguration(options)));

            RegisterModel<DateTimeModel>(
                Culture.Spanish,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new SpanishMergedParserConfiguration(new BaseOptionsConfiguration(options))),
                    new BaseMergedDateTimeExtractor(new SpanishMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.French,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new FrenchMergedParserConfiguration(new BaseOptionsConfiguration(options))),
                    new BaseMergedDateTimeExtractor(new FrenchMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.Portuguese,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new PortugueseMergedParserConfiguration(new BaseOptionsConfiguration(options))),
                    new BaseMergedDateTimeExtractor(new PortugueseMergedExtractorConfiguration(options))));

            RegisterModel<DateTimeModel>(
                Culture.German,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new GermanMergedParserConfiguration(new BaseOptionsConfiguration(options))),
                    new BaseMergedDateTimeExtractor(new GermanMergedExtractorConfiguration(options))));

            // TODO to be uncommented when all tests for Dutch are green.
            // RegisterModel<DateTimeModel>(
            //     Culture.Dutch,
            //     options => new DateTimeModel(
            //         new BaseMergedDateTimeParser(
            //             new DutchMergedParserConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: true))),
            //         new BaseMergedDateTimeExtractor(
            //             new DutchMergedExtractorConfiguration(new BaseOptionsConfiguration(options, dmyDateFormat: true)))));

            // TODO to be uncommented when all tests for Japanese are green.
            // RegisterModel<DateTimeModel>(
            //    Culture.Japanese,
            //    options => new DateTimeModel(
            //      new FullDateTimeParser(new JapaneseDateTimeParserConfiguration(options)),
            //      new JapaneseMergedExtractor(options)));
        }
    }
}