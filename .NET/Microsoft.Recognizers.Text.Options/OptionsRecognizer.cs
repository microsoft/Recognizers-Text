using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Options.English.Extractors;
using Microsoft.Recognizers.Text.Options.Extractors;
using Microsoft.Recognizers.Text.Options.Models;
using Microsoft.Recognizers.Text.Options.Parsers;

namespace Microsoft.Recognizers.Text.Options
{
    public class OptionsRecognizer : Recognizer
    {
        public static readonly OptionsRecognizer Instance = new OptionsRecognizer(OptionsRecognizerOptions.None);

        public OptionsRecognizer(OptionsRecognizerOptions options)
        {
            RegisterModel(Culture.English, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(BooleanModel)] = new BooleanModel(new BooleanParser(), new BooleanExtractor(new EnglishBooleanExtractorConfiguration()))
            });
        }

        public IModel GetBooleanModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<BooleanModel>(culture, fallbackToDefaultCulture);
        }
    }
}
