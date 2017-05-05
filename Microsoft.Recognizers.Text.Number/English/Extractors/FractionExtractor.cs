using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English.Extractors
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

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
                        $@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(and\s+)?)?({IntegerExtractor.AllIntRegex
                            })(\s+|\s*-\s*)((({OrdinalExtractor.AllOrdinalRegex})|({
                            OrdinalExtractor.RoundNumberOrdinalRegex}))s|halves|quarters)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                },
                {
                    new Regex(
                        $@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(and\s+)?)?(a|an|one)(\s+|\s*-\s*)(({
                            OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.RoundNumberOrdinalRegex
                            })|half|quarter)(?=\b)", RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                },
                {
                    new Regex(
                        $@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+over\s+(({
                            IntegerExtractor.AllIntRegex})|(\d+)(?!\.))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracEng"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}