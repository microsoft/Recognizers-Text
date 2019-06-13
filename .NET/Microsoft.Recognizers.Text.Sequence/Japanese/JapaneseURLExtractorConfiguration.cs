using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Japanese
{
    public class JapaneseURLExtractorConfiguration : SequenceConfiguration
    {

        public JapaneseURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            UrlRegex = new Regex(URLDefinitions.UrlRegex, RegexOptions.Compiled);
            IpUrlRegex = new Regex(URLDefinitions.IpUrlRegex, RegexOptions.Compiled);
        }
    }
}