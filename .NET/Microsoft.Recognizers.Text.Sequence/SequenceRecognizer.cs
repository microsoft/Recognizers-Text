using System;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer
    {
        public static readonly SequenceRecognizer Instance = new SequenceRecognizer(SequenceOptions.None);

        private SequenceRecognizer(SequenceOptions options)
        {
            RegisterModel(Culture.English, options.ToString(), new Dictionary<Type, IModel>
            {

            });

        }

        public IModel GetPhoneNumberModel(string culture, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PhoneNumberModel>(culture, fallbackToDefaultCulture, SequenceOptions.None.ToString());
        }

    }
}
