using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.Number.English
{
    public class DoubleExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_DOUBLE; // "Double";

        public DoubleExtractor(string placeholder = Numeric.PlaceHolderDefault)
        {
            var regexes = new Dictionary<Regex, string> {
                {
                    new Regex(Numeric.DoubleDecimalPointRegex(placeholder),
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                }, {
                    new Regex(Numeric.DoubleWithoutIntegralRegex(placeholder),
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                }, {
                    new Regex(Numeric.DoubleWithMultiplierRegex,
                              RegexOptions.Singleline),
                    "DoubleNum"
                }, {
                    new Regex(Numeric.DoubleWithRoundNumber,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleNum"
                }, {
                    new Regex(Numeric.DoubleAllFloatRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoubleEng"
                }, {
                    new Regex(Numeric.DoubleExponentialNotationRegex,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                }, {
                    new Regex(Numeric.DoubleCaretExponentialNotationRegex,
                              RegexOptions.IgnoreCase | RegexOptions.Singleline),
                    "DoublePow"
                }, {
                    GenerateLongFormatNumberRegexes(LongFormatType.DoubleNumCommaDot, placeholder), "DoubleNum"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}