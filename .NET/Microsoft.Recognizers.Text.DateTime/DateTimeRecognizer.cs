using System.Collections.Generic;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
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

        public DateTimeModel GetDateTimeModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }
        
        public static List<ModelResult> RecognizeDateTime(string query, string culture, DateTimeOptions options = DateTimeOptions.None, System.DateTime? refTime = null, bool fallbackToDefaultCulture = true)
        {
            var recognizer = new DateTimeRecognizer(options);
            var model = recognizer.GetDateTimeModel(culture, fallbackToDefaultCulture);
            return model.Parse(query, refTime ?? System.DateTime.Now);

            instanceOptions = options;

            var type = typeof(DateTimeModel);

            RegisterModel(Culture.English, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new EnglishMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(options))
                          ));

            RegisterModel(Culture.Chinese, type, options.ToString(), new DateTimeModel(
                              new FullDateTimeParser(new ChineseDateTimeParserConfiguration(options)),
                              new MergedExtractorChs(options)
                          ));

            RegisterModel(Culture.Spanish, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new SpanishMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(options))
                          ));

            RegisterModel(Culture.French, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new FrenchMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(options))
                          ));

            RegisterModel(Culture.Portuguese, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new PortugueseMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new PortugueseMergedExtractorConfiguration(options))
                          ));
            RegisterModel(Culture.German, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new GermanMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new GermanMergedExtractorConfiguration(options))
                          ));
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

                case Culture.Portuguese:
                    RegisterModel(Culture.Portuguese, type, options.ToString(), new DateTimeModel(
                                      new BaseMergedParser(new PortugueseMergedParserConfiguration(options)),
                                      new BaseMergedExtractor(new PortugueseMergedExtractorConfiguration(options))
                                  ));
                    break;

                case Culture.German:
                    RegisterModel(Culture.German, type, options.ToString(), new DateTimeModel(
                              new BaseMergedParser(new GermanMergedParserConfiguration(options)),
                              new BaseMergedExtractor(new GermanMergedExtractorConfiguration(options))
                          ));
                    break;

                default:
                    throw new ArgumentException($"Culture {cultureCode} not yet supported in timex.");
            }
            
        }

        // Uninitialized recognizer constructor
        private DateTimeRecognizer()
        {
        }

        private DateTimeOptions SanityCheck(DateTimeOptions options)
        {
            if (!ContainsModels())
            {
                Initialize(options);
            }

            if (options == DateTimeOptions.None)
            {
                options = instanceOptions;
            }

            return options;
        }

        public DateTimeModel GetDateTimeModel(DateTimeOptions options = DateTimeOptions.None)
        {

            options = SanityCheck(options);

            return GetDateTimeModel("", false, options);
        }
        
        public DateTimeModel GetDateTimeModel(string culture, bool fallbackToDefaultCulture = true, DateTimeOptions options = DateTimeOptions.None)
        {

            options = SanityCheck(options);

            DateTimeModel model;
            if (string.IsNullOrEmpty(culture))
            {
                model = (DateTimeModel)GetSingleModel<DateTimeModel>();
            }
            else
            {
                model = (DateTimeModel)GetModel<DateTimeModel>(culture, fallbackToDefaultCulture, options.ToString());
            }

            return model;
        }
    }
}