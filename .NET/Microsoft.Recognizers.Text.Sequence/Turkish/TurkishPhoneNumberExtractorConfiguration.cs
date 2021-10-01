// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.Turkish
{
    public class TurkishPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public TurkishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
