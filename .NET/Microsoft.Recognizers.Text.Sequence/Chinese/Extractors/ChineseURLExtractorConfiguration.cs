// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChineseURLExtractorConfiguration : URLConfiguration
    {
        public ChineseURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            UrlRegex = new Regex(URLDefinitions.UrlRegex, RegexOptions.Compiled);
            IpUrlRegex = new Regex(URLDefinitions.IpUrlRegex, RegexOptions.Compiled);
        }
    }
}