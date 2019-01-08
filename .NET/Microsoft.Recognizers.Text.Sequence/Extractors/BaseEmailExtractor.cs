using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseEmailExtractor : BaseSequenceExtractor
    {
        public BaseEmailExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(BaseEmail.EmailRegex, RegexOptions.Compiled),
                    Constants.EMAIL_REGEX
                },
                {
                    new Regex(BaseEmail.EmailRegex2, RegexOptions.Compiled),
                    Constants.EMAIL_REGEX
                },
            };

            Regexes = regexes.ToImmutableDictionary();
        }

        internal override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_EMAIL;
    }
}
