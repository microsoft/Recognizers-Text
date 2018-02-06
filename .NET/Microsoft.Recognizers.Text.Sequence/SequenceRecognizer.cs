using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Sequence.English;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer
    {
        public static readonly SequenceRecognizer Instance = new SequenceRecognizer(SequenceOptions.None);

        private SequenceRecognizer(SequenceOptions options)
        {
            RegisterModel(Culture.English, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });

            RegisterModel(Culture.Chinese, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });

            RegisterModel(Culture.French, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });

            RegisterModel(Culture.German, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });

            
            RegisterModel(Culture.Spanish, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });

            RegisterModel(Culture.Portuguese, options.ToString(), new Dictionary<Type, IModel>
            {
                [typeof(PhoneNumberModel)] = new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor())
            });
        }

        public IModel GetPhoneNumberModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PhoneNumberModel>(culture, fallbackToDefaultCulture, SequenceOptions.None.ToString());
        }

    }
}
