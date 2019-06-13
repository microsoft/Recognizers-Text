using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishURLExtractorConfiguration : SequenceConfiguration
    {

        public EnglishURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            IpUrlRegex = new Regex(BaseURL.IpUrlRegex, RegexOptions.Compiled);
            UrlRegex = new Regex(BaseURL.UrlRegex, RegexOptions.Compiled);
        }

    }
}
