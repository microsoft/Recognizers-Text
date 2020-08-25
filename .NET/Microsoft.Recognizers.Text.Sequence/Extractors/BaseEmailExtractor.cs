using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.Sequence
{
    public class BaseEmailExtractor : BaseSequenceExtractor
    {
        private static readonly Regex Rfc5322ValidationRegex = new Regex(BaseEmail.RFC5322Regex, RegexOptions.Compiled);

        private static readonly char[] TrimmableChars = { '.' };

        private readonly BaseSequenceConfiguration config;

        public BaseEmailExtractor(BaseSequenceConfiguration config)
        {
            this.config = config;

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

        protected override List<ExtractResult> PostFilter(List<ExtractResult> results)
        {
            // If Relaxed is on, no extra validation is applied
            if ((config.Options & SequenceOptions.Relaxed) != 0)
            {
                return results;
            }
            else
            {
                // Not return malformed e-mail addresses and trim ending '.'
                // (?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])

                foreach (var result in results)
                {
                    if (result.Text.EndsWith(".", StringComparison.Ordinal))
                    {
                        result.Text = result.Text.TrimEnd(TrimmableChars);
                        result.Length--;
                    }
                }

                return results.Where(r => Rfc5322ValidationRegex.IsMatch(r.Text)).ToList();
            }
        }

    }
}
