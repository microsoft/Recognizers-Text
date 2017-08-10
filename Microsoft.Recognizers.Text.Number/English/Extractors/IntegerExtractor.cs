using Microsoft.Recognizers.Resources.English;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class IntegerExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_INTEGER; // "Integer";

        public IntegerExtractor(string placeholder = Numeric.PlaceHolderDefault)
        {
            var regexes = new Dictionary<Regex, string> {
                {
                    new Regex(Numeric.NumbersWithPlaceHolder(placeholder),
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                }, {
                    new Regex(Numeric.NumbersWithSuffix, RegexOptions.Singleline), "IntegerNum"
                }, {
                    new Regex(Numeric.RoundNumberIntegerRegexWithLocks,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                }, {
                    new Regex(Numeric.NumbersWithDozenSuffix,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerNum"
                }, {
                    new Regex(Numeric.AllIntRegexWithLocks,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerEng"
                }, {
                    new Regex(Numeric.AllIntRegexWithDozenSuffixLocks,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "IntegerEng"
                }, {
                    GenerateLongFormatNumberRegexes(LongFormatType.IntegerNumComma, placeholder), "IntegerNum"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}