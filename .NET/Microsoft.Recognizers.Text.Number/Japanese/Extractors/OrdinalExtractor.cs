using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL;

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    //だい一百五十四
                    new Regex(NumbersDefinitions.OrdinalRegex, RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.JAPANESE)
                },
                {
                    //だい２５６５
                    new Regex(NumbersDefinitions.OrdinalNumbersRegex, RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.JAPANESE)
                },
                {
                    //2折 ２.５折
                    new Regex(NumbersDefinitions.NumbersFoldsPercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.PERCENT_PREFIX, Constants.SPECIAL_SUFFIX)
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
