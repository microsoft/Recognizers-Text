using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Sequence.Dutch
{
    public class DutchQuotedTextExtractorConfiguration : QuotedTextConfiguration
    {
        public DutchQuotedTextExtractorConfiguration(SequenceOptions options)
     : base(options)
        {
            QuotedTextRegex1 = new Regex(QuotedTextDefinitions.QuotedTextRegex1, RegexOptions.Compiled);
            QuotedTextRegex2 = new Regex(QuotedTextDefinitions.QuotedTextRegex2, RegexOptions.Compiled);
            QuotedTextRegex3 = new Regex(QuotedTextDefinitions.QuotedTextRegex3, RegexOptions.Compiled);
            QuotedTextRegex4 = new Regex(QuotedTextDefinitions.QuotedTextRegex4, RegexOptions.Compiled);
            QuotedTextRegex5 = new Regex(QuotedTextDefinitions.QuotedTextRegex5, RegexOptions.Compiled);
            QuotedTextRegex6 = new Regex(QuotedTextDefinitions.QuotedTextRegex6, RegexOptions.Compiled);
            QuotedTextRegex7 = new Regex(QuotedTextDefinitions.QuotedTextRegex7, RegexOptions.Compiled);
            QuotedTextRegex8 = new Regex(QuotedTextDefinitions.QuotedTextRegex8, RegexOptions.Compiled);
            QuotedTextRegex9 = new Regex(QuotedTextDefinitions.QuotedTextRegex9, RegexOptions.Compiled);

        }
    }
}
