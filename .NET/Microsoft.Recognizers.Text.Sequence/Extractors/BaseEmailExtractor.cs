// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
