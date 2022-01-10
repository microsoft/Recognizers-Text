using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Arabic;

namespace Microsoft.Recognizers.Text.DateTime.Arabic
{
    public class ArabicTimeZoneParserConfiguration : BaseDateTimeOptionsConfiguration, ITimeZoneParserConfiguration
    {
        public static readonly Dictionary<string, int> FullToMinMapping = TimeZoneDefinitions.FullToMinMapping;

        public static readonly Regex DirectUtcRegex =
            new Regex(TimeZoneDefinitions.DirectUtcRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Dictionary<string, int> AbbrToMinMapping = TimeZoneDefinitions.AbbrToMinMapping;

        public ArabicTimeZoneParserConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
        }

        Dictionary<string, int> ITimeZoneParserConfiguration.FullToMinMapping => FullToMinMapping;

        Regex ITimeZoneParserConfiguration.DirectUtcRegex => DirectUtcRegex;

        Dictionary<string, int> ITimeZoneParserConfiguration.AbbrToMinMapping => AbbrToMinMapping;
    }
}
