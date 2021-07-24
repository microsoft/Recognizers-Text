// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.Hindi
{
    public class HindiPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public HindiPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
