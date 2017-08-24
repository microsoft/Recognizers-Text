using System;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer
    {
        public static DateTimeRecognizer GetInstance(DateTimeOptions options=DateTimeOptions.None)
        {
            return new DateTimeRecognizer(options);
        }

        private DateTimeRecognizer(DateTimeOptions options = DateTimeOptions.None)
        {
            var type = typeof(DateTimeModel);

            RegisterModel(Culture.English, type, new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration()),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration(), options)
                    ));

            RegisterModel(Culture.Chinese, type, new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration()),
                    new MergedExtractorChs()
                    ));

            RegisterModel(Culture.Spanish, type, new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration()),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration(), options)
                    ));
        }

        public DateTimeModel GetDateTimeModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return (DateTimeModel)GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }

        
    }
}
