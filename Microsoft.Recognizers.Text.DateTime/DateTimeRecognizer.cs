using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer
    {
        private static readonly ModelContainer ModelContainer = new ModelContainer();

        static DateTimeRecognizer()
        {
            var type = typeof(DateTimeModel);
            ModelContainer.RegisterModel(Culture.English, type, new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration()),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration())
                    ));
            ModelContainer.RegisterModel(Culture.Chinese, type, new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration()),
                    new MergedExtractorChs()
                    ));
            ModelContainer.RegisterModel(Culture.Spanish, type, new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration()),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration())
                    ));
        }

        public static DateTimeModel GetDateTimeModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return (DateTimeModel)ModelContainer.GetModel<DateTimeModel>(culture, fallbackToDefaultCulture);
        }
    }
}
