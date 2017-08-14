using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        NumbersDefinitions.OrdinalSuffixRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex(NumbersDefinitions.OrdinalNumericRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex(NumbersDefinitions.OrdinalEnglishRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdEng"
                },
                {
                    new Regex(NumbersDefinitions.OrdinalRoundNumberRegex,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdEng"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}