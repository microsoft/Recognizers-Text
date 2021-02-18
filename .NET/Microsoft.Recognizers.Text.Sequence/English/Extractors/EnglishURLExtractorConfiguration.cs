using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishURLExtractorConfiguration : URLConfiguration
    {
        public EnglishURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            IpUrlRegex = RegexCache.Get(BaseURL.IpUrlRegex, RegexOptions.Compiled);
            UrlRegex = RegexCache.Get(BaseURL.UrlRegex, RegexOptions.Compiled);
        }

    }
}
