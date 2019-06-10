using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChineseURLExtractorConfiguration : SequenceConfiguration, IURLExtractorConfiguration
    {
        public static readonly Regex IpUrlRegex = new Regex(BaseURL.IpUrlRegex, RegexOptions.Compiled);

        public static readonly Regex UrlRegex = new Regex(BaseURL.UnicodeUrlRegex, RegexOptions.Compiled);

        public static readonly Regex UrlRegex2 = new Regex(BaseURL.UrlRegex2, RegexOptions.Compiled);

        public ChineseURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
        }

        Regex IURLExtractorConfiguration.IpUrlRegex => IpUrlRegex;

        Regex IURLExtractorConfiguration.UrlRegex => UrlRegex;

        Regex IURLExtractorConfiguration.UrlRegex2 => UrlRegex2;

    }
}