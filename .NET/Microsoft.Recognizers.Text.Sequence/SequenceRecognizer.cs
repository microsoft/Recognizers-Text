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
            : this(targetCulture, GetOptions(options), lazyInitialization)
        {
        }

        public SequenceRecognizer(SequenceOptions options = SequenceOptions.None, bool lazyInitialization = true)
            : base(null, options, lazyInitialization)
        {
        }

        public SequenceRecognizer(int options, bool lazyInitialization = true)
            : this(null, GetOptions(options), lazyInitialization)
        {
        }

        public static List<ModelResult> RecognizePhoneNumber(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetPhoneNumberModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeIpAddress(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetIpAddressModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeMention(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetMentionModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeHashtag(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetHashtagModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeEmail(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetEmailModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeURL(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetURLModel(culture, fallbackToDefaultCulture), query, options);
        }

        public static List<ModelResult> RecognizeGUID(string query, string culture, SequenceOptions options = SequenceOptions.None, bool fallbackToDefaultCulture = true)
        {
            return RecognizeByModel(recognizer => recognizer.GetGUIDModel(culture, fallbackToDefaultCulture), query, options);
        }

        public IModel GetPhoneNumberModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<PhoneNumberModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetIpAddressModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<IpAddressModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetMentionModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<MentionModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetHashtagModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<HashtagModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetEmailModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<EmailModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetURLModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<URLModel>(Culture.English, fallbackToDefaultCulture);
        }

        public IModel GetGUIDModel(string culture = null, bool fallbackToDefaultCulture = true)
        {
            return GetModel<GUIDModel>(Culture.English, fallbackToDefaultCulture);
        }

        protected override void InitializeConfiguration()
        {
            RegisterModel<PhoneNumberModel>(
                Culture.English,
                (options) => new PhoneNumberModel(new PhoneNumberParser(), new PhoneNumberExtractor()));

            RegisterModel<IpAddressModel>(
                Culture.English,
                (options) => new IpAddressModel(new IpParser(), new IpExtractor()));

            RegisterModel<MentionModel>(
                Culture.English,
                (options) => new MentionModel(new MentionParser(), new MentionExtractor()));

            RegisterModel<HashtagModel>(
                Culture.English,
                (options) => new HashtagModel(new HashtagParser(), new HashtagExtractor()));

            RegisterModel<EmailModel>(
                Culture.English,
                (options) => new EmailModel(new EmailParser(), new EmailExtractor()));

            RegisterModel<URLModel>(
                Culture.English,
                (options) => new URLModel(new URLParser(), new URLExtractor()));

            RegisterModel<GUIDModel>(
                Culture.English,
                (options) => new GUIDModel(new GUIDParser(), new GUIDExtractor()));
        }

        private static List<ModelResult> RecognizeByModel(Func<SequenceRecognizer, IModel> getModelFunc, string query, SequenceOptions options)
        {
            var recognizer = new SequenceRecognizer(options);
            var model = getModelFunc(recognizer);
            return model.Parse(query);
        }
    }
}