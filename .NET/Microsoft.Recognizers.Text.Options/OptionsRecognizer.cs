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
        public static readonly OptionsRecognizer Instance = new OptionsRecognizer();

        public OptionsRecognizer()
        {
            RegisterModel(Culture.English, "None", new Dictionary<Type, IModel>
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
