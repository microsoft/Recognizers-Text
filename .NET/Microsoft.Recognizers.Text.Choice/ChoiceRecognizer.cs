using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Choice.English.Extractors;
using Microsoft.Recognizers.Text.Choice.Extractors;
using Microsoft.Recognizers.Text.Choice.Models;
using Microsoft.Recognizers.Text.Choice.Parsers;

namespace Microsoft.Recognizers.Text.Choice
{
    public class ChoiceRecognizer : Recognizer
    {
        public static readonly ChoiceRecognizer Instance = new ChoiceRecognizer(ChoiceOptions.None);

        public ChoiceRecognizer(ChoiceOptions options)
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
