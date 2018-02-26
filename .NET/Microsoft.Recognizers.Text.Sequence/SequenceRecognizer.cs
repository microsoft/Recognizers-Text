using System;
using System.Collections.Generic;
using Microsoft.Recognizers.Text.Sequence.English;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceRecognizer : Recognizer<SequenceOptions>
    {
        public SequenceRecognizer(string targetCulture, SequenceOptions options = SequenceOptions.None, bool lazyInitialization = false)
            : base(targetCulture, options, lazyInitialization)
        {
        }

        public SequenceRecognizer(string targetCulture, int options, bool lazyInitialization = false)
            : this(targetCulture, GetOption(options), lazyInitialization)
        {
        }

        public SequenceRecognizer(SequenceOptions options = SequenceOptions.None, bool lazyInitialization = true)
            : base(null, options, lazyInitialization)
        {
        }

        public SequenceRecognizer(int options, bool lazyInitialization = true)
            : this(null, GetOption(options), lazyInitialization)
        {
        }

        public IModel GetPhoneNumberModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PhoneNumberModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetIpAddressModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<IpAddressModel>(Culture.English, fallbackToDefaultCulture);
        }

        public static List<ModelResult> RecognizePhoneNumber(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetPhoneNumberModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeIpAddress(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetIpAddressModel(culture, fallbackToDefaultCulture), query, options);
        }

        private static List<ModelResult> RecognizeByModel(Func<SequenceRecognizer, IModel> getModelFunc, string query, SequenceOptions options)
        {
            var recognizer = new SequenceRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<PhoneNumberModel>(
                Culture.English,
                (options) => new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor()));

            RegisterModel<IpAddressModel>(
                Culture.English,
                (options) => new IpAddressModel(new IpParser(), new IpExtractor()));
        }
    }
}