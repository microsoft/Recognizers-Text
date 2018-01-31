using System;

using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.French;
using Microsoft.Recognizers.Text.DateTime.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.German;

using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer
    {

        private DateTimeOptions instanceOptions;

        public static DateTimeOptions Convert(int value)
        {
            return EnumUtils.Convert<DateTimeOptions>(value);
        }

        public static DateTimeRecognizer GetCleanInstance()
        {
            return new DateTimeRecognizer();
        }

        public static DateTimeRecognizer GetInstance(DateTimeOptions options = DateTimeOptions.None)
        {
            return new DateTimeRecognizer(options);
        }

        public static DateTimeRecognizer GetSingleCultureInstance(string cultureCode, DateTimeOptions options = DateTimeOptions.None)
        {
            return new DateTimeRecognizer(cultureCode, options);
        }

        private DateTimeRecognizer(DateTimeOptions options)
        {
            Initialize(options);
        }

        private void Initialize(DateTimeOptions options)
        {

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
        }

        private DateTimeRecognizer(string cultureCode, DateTimeOptions options)
        {

            instanceOptions = options;

            var type = typeof(DateTimeModel);

            switch (cultureCode) {
                case Culture.English:
                    RegisterModel(cultureCode, type, options.ToString(), new DateTimeModel(
                                      new BaseMergedParser(new EnglishMergedParserConfiguration(options)),
                                      new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(options))
                                  ));
                    break;

                case Culture.Chinese:
                    RegisterModel(cultureCode, type, options.ToString(), new DateTimeModel(
                                      new FullDateTimeParser(new ChineseDateTimeParserConfiguration(options)),
                                      new MergedExtractorChs(options)
                                  ));
                    break;

                case Culture.Spanish:
                    RegisterModel(Culture.Spanish, type, options.ToString(), new DateTimeModel(
                                      new BaseMergedParser(new SpanishMergedParserConfiguration(options)),
                                      new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(options))
                                  ));
                    break;

                case Culture.French:
                    RegisterModel(Culture.French, type, options.ToString(), new DateTimeModel(
                                      new BaseMergedParser(new FrenchMergedParserConfiguration(options)),
                                      new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(options))
                                  ));
                    break;

                case Culture.Portuguese:
                    RegisterModel(Culture.Portuguese, type, options.ToString(), new DateTimeModel(
                                      new BaseMergedParser(new PortugueseMergedParserConfiguration(options)),
                                      new BaseMergedExtractor(new PortugueseMergedExtractorConfiguration(options))
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
