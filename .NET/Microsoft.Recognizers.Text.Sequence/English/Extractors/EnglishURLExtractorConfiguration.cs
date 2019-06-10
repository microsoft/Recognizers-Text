using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence.English
{
    public class EnglishURLExtractorConfiguration : SequenceConfiguration, IURLExtractorConfiguration
    {
        public static readonly Regex IpUrlRegex = new Regex(BaseURL.IpUrlRegex, RegexOptions.Compiled);

        public static readonly Regex UrlRegex = new Regex(BaseURL.UrlRegex, RegexOptions.Compiled);

        public static readonly Regex UrlRegex2 = new Regex(BaseURL.UrlRegex2, RegexOptions.Compiled);

        public EnglishURLExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
        }

        Regex IURLExtractorConfiguration.IpUrlRegex => IpUrlRegex;

        Regex IURLExtractorConfiguration.UrlRegex => UrlRegex;

        Regex IURLExtractorConfiguration.UrlRegex2 => UrlRegex2;

    }
}
