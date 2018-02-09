using System;
using System.Collections.Generic;

using Microsoft.Recognizers.Text.Sequence.English;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer<SequenceOptions>
    {
        public SequenceRecognizer(string culture, SequenceOptions options = SequenceOptions.None)
        :base(culture, options)
        {

        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<PhoneNumberModel>(
                Culture.English,
                (options) => new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor()));
        }

        public IModel GetPhoneNumberModel()
        {
            return GetModel<PhoneNumberModel>();
        }
    }
}
