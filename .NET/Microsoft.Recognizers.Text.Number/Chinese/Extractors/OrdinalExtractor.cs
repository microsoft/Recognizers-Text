using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
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
                    //第一百五十四
                    new Regex(NumbersDefinitions.OrdinalRegex, RegexOptions.Singleline), RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.CHINESE)
                },
                {
                    //第２５６５,  第1234
                    new Regex(NumbersDefinitions.OrdinalNumbersRegex, RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.ORDINAL_PREFIX, Constants.CHINESE)
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}