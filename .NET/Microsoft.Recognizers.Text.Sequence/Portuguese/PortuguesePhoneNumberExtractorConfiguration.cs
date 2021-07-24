// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Sequence.Portuguese
{
    public class PortuguesePhoneNumberExtractorConfiguration : BasePhoneNumberExtractorConfiguration
    {
        public PortuguesePhoneNumberExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            FalsePositivePrefixRegex = new Regex(PhoneNumbersDefinitions.FalsePositivePrefixRegex);
        }
    }
}
