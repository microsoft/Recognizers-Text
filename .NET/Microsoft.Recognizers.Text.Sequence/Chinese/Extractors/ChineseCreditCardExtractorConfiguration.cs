using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Sequence.Chinese
{
    public class ChineseCreditCardExtractorConfiguration : CreditCardConfiguration
    {

        public ChineseCreditCardExtractorConfiguration(SequenceOptions options)
            : base(options)
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(CreditCardDefinitions.ChinaUnionPay, RegexOptions.Compiled),
                    "ChinaUnionPay"
                },
                {
                    new Regex(CreditCardDefinitions.ChinaTUnion, RegexOptions.Compiled),
                    "ChinaTUnion"
                },
            };

            IssuerRegexes = regexes.ToImmutableDictionary();

            ValidationList = new List<bool> { true, true };
        }
    }
}
