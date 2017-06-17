using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public const string OneHalfOneThirdOneQuarterRegex = @"(demi|tiers|quarts)";

        public FractionExtractor()

        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))",
                       RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(
                        $@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(et\s+)?)?({IntegerExtractor.AllIntRegex
                            })(\s+|\s*-\s*)((({OrdinalExtractor.AllOrdinalRegex})|({
                            OrdinalExtractor.RoundNumberOrdinalRegex}))s|demis|quarts|tiers)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex(
                        $@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(et\s+)?)?(a|un|une)(\s+|\s*-\s*)(({
                            OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.RoundNumberOrdinalRegex
                            })|demi|quart|tier?=\b)", RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex(
                        $@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+sur\s+(({
                            IntegerExtractor.AllIntRegex})|(\d+)(?!\.))(?=\b)",     
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                }
            };

            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}
