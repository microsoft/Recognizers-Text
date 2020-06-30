using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class CreditCardConfiguration : ISequenceConfiguration
    {
        public CreditCardConfiguration(SequenceOptions options = SequenceOptions.None)
        {
            Options = options;
        }

        public SequenceOptions Options { get; }

        public string WordBoundariesRegex { get; set; }

        public string EndWordBoundariesRegex { get; set; }

        public List<bool> ValidationList { get; set; }

        public ImmutableDictionary<Regex, string> IssuerRegexes { get; set; }
    }
}
