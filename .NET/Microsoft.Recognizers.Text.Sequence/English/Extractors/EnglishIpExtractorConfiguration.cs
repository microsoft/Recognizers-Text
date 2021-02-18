using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishIpExtractorConfiguration : IpConfiguration
    {
        public EnglishIpExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            Ipv4Regex = RegexCache.Get(BaseIp.Ipv4Regex, RegexOptions.Compiled);
            Ipv6Regex = RegexCache.Get(BaseIp.Ipv6Regex, RegexOptions.Compiled);
        }

    }
}
