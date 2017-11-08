using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER;

        public IntegerExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(NumbersDefinitions.NumbersWithPlaceHolder(placeholder),
                    RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithSuffix,
                        RegexOptions.Singleline)
                    , "IntegerNum"
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumDot, placeholder),
                    "IntegerNum"
                },
                {
                    new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithDozen2Suffix,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithDozenSuffix,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerPor"
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithDozenSuffixLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerPor"
                }
            };

            this.Regexes = regexes.ToImmutableDictionary();
        }
    }
}