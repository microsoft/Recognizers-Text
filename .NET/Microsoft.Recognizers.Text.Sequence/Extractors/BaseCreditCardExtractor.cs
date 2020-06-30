using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{

    public class BaseCreditCardExtractor : BaseSequenceExtractor
    {
        private CreditCardConfiguration config;

        public BaseCreditCardExtractor(CreditCardConfiguration config)
        {
            this.config = config;

            var wordBoundariesRegex = config.WordBoundariesRegex;
            var endWordBoundariesRegex = config.EndWordBoundariesRegex;

            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseCreditCard.CNCreditCardRegex(wordBoundariesRegex, endWordBoundariesRegex), RegexOptions.Compiled),
                    Constants.CREDIT_CARD_REGEX_CN
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_CREDIT_CARD;

        public override List<ExtractResult> Extract(string text)
        {
            var ers = base.Extract(text);
            foreach (var er in ers)
            {
                // Check issuer with the start of sequence
                var index = 0;
                foreach (var issuerRegex in config.IssuerRegexes)
                {
                    if (issuerRegex.Key.IsMatch(er.Text))
                    {
                        er.Issuer = issuerRegex.Value;
                        er.Validation = config.ValidationList.ElementAt(index);
                    }

                    index += 1;
                }
            }

            return ers;
        }
    }
}
