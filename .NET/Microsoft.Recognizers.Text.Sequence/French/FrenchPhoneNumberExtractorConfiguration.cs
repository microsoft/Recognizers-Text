// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.French
{
    public class FrenchPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public FrenchPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
