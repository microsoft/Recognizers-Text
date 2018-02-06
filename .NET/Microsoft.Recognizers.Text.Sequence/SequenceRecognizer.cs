﻿using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Sequence.English;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer
    {
        public static readonly SequenceRecognizer Instance = new SequenceRecognizer(SequenceOptions.None);

        private SequenceRecognizer(SequenceOptions options)
        {
            var models = new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            };

            RegisterModel(Culture.English, options.ToString(), models);

            RegisterModel(Culture.Chinese, options.ToString(), models);

            RegisterModel(Culture.French, options.ToString(), models);

            RegisterModel(Culture.German, options.ToString(), models);

            RegisterModel(Culture.Spanish, options.ToString(), models);

            RegisterModel(Culture.Portuguese, options.ToString(), models);

        }

        public IModel GetPhoneNumberModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PhoneNumberModel>(culture, fallbackToDefaultCulture, SequenceOptions.None.ToString());
        }

    }
}
