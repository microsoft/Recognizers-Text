using System;

using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;
using Microsoft.Recognizers.Text.DateTime.French;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer
    {
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

            var type = typeof(DateTimeModel);

            RegisterModel(Culture.English, type, new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration(), options),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options)
                    ));

            RegisterModel(Culture.Chinese, type, new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration(), options),
                    new MergedExtractorChs(options)
                    ));

            RegisterModel(Culture.Spanish, type, new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration(), options),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), options)
                    ));

            RegisterModel(Culture.French, type, new DateTimeModel(
                    new BaseMergedParser(new FrenchMergedParserConfiguration(), options),
                    new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), options)
                    ));
        }

        private DateTimeRecognizer(string cultureCode, DateTimeOptions options)
        {

            var type = typeof(DateTimeModel);

            switch (cultureCode) {
                case Culture.English:
                    RegisterModel(cultureCode, type, new DateTimeModel(
                                      new BaseMergedParser(new EnglishMergedParserConfiguration(), options),
                                      new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options)
                                  ));
                    break;
                case Culture.Chinese:
                    RegisterModel(cultureCode, type, new DateTimeModel(
                                      new FullDateTimeParser(new ChineseDateTimeParserConfiguration(), options),
                                      new MergedExtractorChs(options)
                                  ));
                    break;
                case Culture.Spanish:
                    RegisterModel(Culture.Spanish, type, new DateTimeModel(
                                      new BaseMergedParser(new SpanishMergedParserConfiguration(), options),
                                      new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), options)
                                  ));
                    break;
                case Culture.French:
                    RegisterModel(Culture.French, type, new DateTimeModel(
                                      new BaseMergedParser(new FrenchMergedParserConfiguration(), options),
                                      new BaseMergedExtractor(new FrenchMergedExtractorConfiguration(), options)
                                  ));
                    break;
                default:
                    throw new ArgumentException($"Culture {cultureCode} not yet supported in timex.");
            }
            
        }

        public DateTimeModel GetDateTimeModel()
        {
            return GetDateTimeModel("", false);
        }

        public DateTimeModel GetDateTimeModel(string culture, bool fallbackToDefaultCulture = true)
        {

            DateTimeModel model;
            if (string.IsNullOrEmpty(culture))
            {
                model = (DateTimeModel)GetSingleModel<DateTimeModel>();
            }
            else
            {
                model = (DateTimeModel)GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
            }

            return model;
        }
  
    }
}
