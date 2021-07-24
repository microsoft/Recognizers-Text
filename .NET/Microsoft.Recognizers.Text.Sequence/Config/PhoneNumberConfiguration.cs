// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class PhoneNumberConfiguration : ISequenceConfiguration
    {
        public PhoneNumberConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public string WordBoundariesRegex { get; set; }

        public string NonWordBoundariesRegex { get; set; }

        public string EndWordBoundariesRegex { get; set; }

        public Regex ColonPrefixCheckRegex { get; set; }

        public Regex FalsePositivePrefixRegex { get; set; }

        public List<char> ColonMarkers { get; set; }

        public List<char> ForbiddenPrefixMarkers { get; set; }

        public List<char> ForbiddenSuffixMarkers { get; set; }

        public Dictionary<Regex, Regex> AmbiguityFiltersDict { get; set; }

    }
}
