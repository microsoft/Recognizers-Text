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
            UrlRegex = RegexCache.Get(URLDefinitions.UrlRegex, RegexOptions.Compiled);
            IpUrlRegex = RegexCache.Get(URLDefinitions.IpUrlRegex, RegexOptions.Compiled);
        }
    }
}