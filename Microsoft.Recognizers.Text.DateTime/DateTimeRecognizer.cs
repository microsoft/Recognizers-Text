using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer
    {
        public static readonly DateTimeRecognizer Instance = new DateTimeRecognizer();

        private DateTimeRecognizer()
        {
            var type = typeof(DateTimeModel);

            RegisterModel(Culture.English, type, new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration()),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration())
                    ));

            RegisterModel(Culture.Chinese, type, new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration()),
                    new MergedExtractorChs()
                    ));

            RegisterModel(Culture.Spanish, type, new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration()),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration())
                    ));
        }

        public DateTimeModel GetDateTimeModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return (DateTimeModel)GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }
    }
}
