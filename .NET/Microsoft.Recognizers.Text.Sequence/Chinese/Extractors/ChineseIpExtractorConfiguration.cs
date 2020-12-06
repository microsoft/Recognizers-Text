using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChineseIpExtractorConfiguration : IpConfiguration
    {
        public ChineseIpExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            Ipv4Regex = RegexCache.Get(IpDefinitions.Ipv4Regex, RegexOptions.Compiled);
            Ipv6Regex = RegexCache.Get(IpDefinitions.Ipv6Regex, RegexOptions.Compiled);
        }
    }
}
