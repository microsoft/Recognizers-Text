using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.DateTime.Chinese;
using Microsoft.Recognizers.Text.DateTime.English;
using Microsoft.Recognizers.Text.DateTime.Spanish;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeRecognizer : Recognizer<Dictionary<string, DateTimeModel>>
    {
        static DateTimeRecognizer()
        {
            ModelInstances = new Dictionary<string, DateTimeModel>(StringComparer.InvariantCultureIgnoreCase)
            {
                {Culture.English, new DateTimeModel(
                    new BaseMergedParser(new EnglishMergedParserConfiguration()),
                    new BaseMergedExtractor(new EnglishMergedExtractorConfiguration())
                    )},
                {Culture.Chinese, new DateTimeModel(
                    new FullDateTimeParser(new ChineseDateTimeParserConfiguration()),
                    new MergedExtractorChs()
                    )},
                {Culture.Spanish, new DateTimeModel(
                    new BaseMergedParser(new SpanishMergedParserConfiguration()),
                    new BaseMergedExtractor(new SpanishMergedExtractorConfiguration())
                    )},
            };
        }

        public static DateTimeModel GetModel(string culture, bool fallbackToDefaultCulture = true)
        {
            if (!ModelInstances.ContainsKey(culture))
            {
                if (fallbackToDefaultCulture)
                {
                    culture = DefaultCulture;
                }
                else
                {
                    throw new Exception($"ERROR: The Culture {culture} is not supported now.");
                }
            }

            var model = ModelInstances[culture];

            return model;
        }
    }
}
