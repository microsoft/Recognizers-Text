using System.Collections.Generic;

using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.Dutch;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.German;
using Microsoft.Recognizers.Text.DateTime.Hindi;
using Microsoft.Recognizers.Text.DateTime.Italian;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.Turkish;

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
                        new EnglishMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.English, options, dmyDateFormat: false))),
                    new BaseMergedDateTimeExtractor(
                        new EnglishMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.English, options, dmyDateFormat: false)))));

            RegisterModel<DateTimeModel>(
                Culture.EnglishOthers,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new EnglishMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, options, dmyDateFormat: true))),
                    new BaseMergedDateTimeExtractor(
                        new EnglishMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.EnglishOthers, options, dmyDateFormat: true)))));

            RegisterModel<DateTimeModel>(
                Culture.Chinese,
                options => new DateTimeModel(
                    new FullDateTimeParser(
                        new ChineseDateTimeParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Chinese, options))),
                    new ChineseMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Chinese, options))));

            RegisterModel<DateTimeModel>(
                Culture.Spanish,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new SpanishMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Spanish, options))),
                    new BaseMergedDateTimeExtractor(
                        new SpanishMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Spanish, options)))));

            RegisterModel<DateTimeModel>(
                Culture.French,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new FrenchMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.French, options))),
                    new BaseMergedDateTimeExtractor(
                        new FrenchMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.French, options)))));

            RegisterModel<DateTimeModel>(
                Culture.Portuguese,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new PortugueseMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Portuguese, options))),
                    new BaseMergedDateTimeExtractor(
                        new PortugueseMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Portuguese, options)))));

            RegisterModel<DateTimeModel>(
                Culture.German,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new GermanMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.German, options))),
                    new BaseMergedDateTimeExtractor(
                        new GermanMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.German, options)))));

            RegisterModel<DateTimeModel>(
                Culture.Italian,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new ItalianMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Italian, options))),
                    new BaseMergedDateTimeExtractor(
                        new ItalianMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Italian, options)))));

            RegisterModel<DateTimeModel>(
                Culture.Turkish,
                options => new DateTimeModel(
                    new BaseMergedDateTimeParser(
                        new TurkishMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Turkish, options))),
                    new BaseMergedDateTimeExtractor(
                        new TurkishMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Turkish, options)))));

            // RegisterModel<DateTimeModel>(
            //     Culture.Hindi,
            //     options => new DateTimeModel(
            //         new BaseMergedDateTimeParser(
            //             new HindiMergedParserConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Hindi, options))),
            //         new BaseMergedDateTimeExtractor(
            //             new HindiMergedExtractorConfiguration(new BaseDateTimeOptionsConfiguration(Culture.Hindi, options)))));

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