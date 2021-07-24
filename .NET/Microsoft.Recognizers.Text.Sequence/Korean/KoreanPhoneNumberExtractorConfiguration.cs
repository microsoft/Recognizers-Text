// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.Korean
{
    public class KoreanPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public KoreanPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
