// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.Spanish
{
    public class SpanishPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public SpanishPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
