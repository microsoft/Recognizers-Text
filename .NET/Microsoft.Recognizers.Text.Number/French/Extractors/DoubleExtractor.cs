using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE;

        public DoubleExtractor(string placeholder = NumbersDefinitions.PlaceHolderDefault)
        {
            this.Regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(NumbersDefinitions.DoubleDecimalPointRegex(placeholder), RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithoutIntegralRegex(placeholder), RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithMultiplierRegex, RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(NumbersDefinitions.DoubleWithRoundNumber, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                },
                {
                    new Regex(NumbersDefinitions.DoubleAllFloatRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleFr"
                },
                {
                    new Regex(NumbersDefinitions.DoubleExponentialNotationRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                },
                {
                    new Regex(NumbersDefinitions.DoubleCaretExponentialNotationRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                },
                {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumDotComma, placeholder),
                    "DoubleNum"
                }
            }.ToImmutableDictionary();
        }

    }
}
