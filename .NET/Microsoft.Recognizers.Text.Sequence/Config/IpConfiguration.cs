// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class IpConfiguration : ISequenceConfiguration
    {
        public IpConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public Regex Ipv4Regex { get; set; }

        public Regex Ipv6Regex { get; set; }
    }
}