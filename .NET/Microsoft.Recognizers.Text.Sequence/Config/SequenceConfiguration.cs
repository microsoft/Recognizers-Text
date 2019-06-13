﻿using System;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class SequenceConfiguration
    {
        public SequenceConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public Regex IpUrlRegex { get; set; }

        public Regex UrlRegex { get; set; }

    }
}
