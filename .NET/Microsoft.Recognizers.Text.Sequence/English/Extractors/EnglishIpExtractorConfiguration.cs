// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishIpExtractorConfiguration : IpConfiguration
    {
        public EnglishIpExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            Ipv4Regex = new Regex(BaseIp.Ipv4Regex, RegexOptions.Compiled);
            Ipv6Regex = new Regex(BaseIp.Ipv6Regex, RegexOptions.Compiled);
        }

    }
}
