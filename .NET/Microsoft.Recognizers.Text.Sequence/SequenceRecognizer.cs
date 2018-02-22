using System.Collections.Generic;

using Microsoft.Recognizers.Text.Sequence.English;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer<SequenceOptions>
    {
        public SequenceRecognizer(string defaultCulture, SequenceOptions options = SequenceOptions.None, bool lazyInitialization = true)
            :base(defaultCulture, options, lazyInitialization)
        {
        }

        public SequenceRecognizer(string defaultCulture, int options, bool lazyInitialization = true)
            : base(defaultCulture, options, lazyInitialization)
        {
        }

        public IModel GetPhoneNumberModel(string culture = null)
        {
            return GetModel<PhoneNumberModel>(culture);
        }

        public static List<ModelResult> RecognizePhoneNumber(string query, string culture, SequenceOptions options = SequenceOptions.None)
        {
            var recognizer = new SequenceRecognizer(Culture.English, options);
            var model = recognizer.GetPhoneNumberModel(culture);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<PhoneNumberModel>(
                Culture.English,
                (options) => new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor()));
        }
    }
}
