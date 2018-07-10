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
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithSuffix,
                        RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumDot, placeholder)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumBlank, placeholder)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumNoBreakSpace, placeholder)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.RoundNumberIntegerRegexWithLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithDozen2Suffix,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.NumbersWithDozenSuffix,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.NUMBER_SUFFIX)
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.PORTUGUESE)
                },
                {
                    new Regex(NumbersDefinitions.AllIntRegexWithDozenSuffixLocks,
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                        , RegexTagGenerator.GenerateRegexTag(Constants.INTEGER_PREFIX, Constants.PORTUGUESE)
                }
            };

            this.Regexes = regexes.ToImmutableDictionary();
        }
    }
}