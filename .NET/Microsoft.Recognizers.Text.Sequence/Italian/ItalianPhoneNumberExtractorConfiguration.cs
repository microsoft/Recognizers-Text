// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.Sequence.Italian
{
    public class ItalianPhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public ItalianPhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = null;
        }
    }
}
