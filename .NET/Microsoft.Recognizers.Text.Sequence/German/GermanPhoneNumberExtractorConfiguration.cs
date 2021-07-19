// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.German
{
    public class GermanPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public GermanPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
