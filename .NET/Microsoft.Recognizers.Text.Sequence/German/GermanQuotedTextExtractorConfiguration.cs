// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.Sequence.German
{
    public class GermanQuotedTextExtractorConfiguration : QuotedTextConfiguration
    {
        public GermanQuotedTextExtractorConfiguration(SequenceOptions options)
     : base(options)
        {
            QuotedTextRegex1 = new Regex(QuotedTextDefinitions.QuotedTextRegex1, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex2 = new Regex(QuotedTextDefinitions.QuotedTextRegex2, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex3 = new Regex(QuotedTextDefinitions.QuotedTextRegex3, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex4 = new Regex(QuotedTextDefinitions.QuotedTextRegex4, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex5 = new Regex(QuotedTextDefinitions.QuotedTextRegex5, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex6 = new Regex(QuotedTextDefinitions.QuotedTextRegex6, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex7 = new Regex(QuotedTextDefinitions.QuotedTextRegex7, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex8 = new Regex(QuotedTextDefinitions.QuotedTextRegex8, RegexOptions.Compiled, RegexTimeOut);
            QuotedTextRegex9 = new Regex(QuotedTextDefinitions.QuotedTextRegex9, RegexOptions.Compiled, RegexTimeOut);

        }
    }
}
