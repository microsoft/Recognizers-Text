using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }

        protected override sealed string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public FractionExtractor()
        {
            string specialFractionInteger = $@"((({IntegerExtractor.AllIntRegex})i?({IntegerExtractor.ZeroToNineIntegerRegex})|({IntegerExtractor.AllIntRegex}))a?v[oa]s?)";

            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+((y|con)\s+)?)?({IntegerExtractor.AllIntRegex})(\s+((y|con)\s)?)((({OrdinalExtractor.AllOrdinalRegex})s?|({specialFractionInteger})|({OrdinalExtractor.SufixRoundOrdinalRegex})s?)|medi[oa]s?|tercios?)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracSpa"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(y\s+)?)?(un|un[oa])(\s+)(({OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.SufixRoundOrdinalRegex})|(y\s+)?medi[oa]s?)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracSpa"
                },
                {
                    new Regex($@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+sobre\s+(({IntegerExtractor.AllIntRegex})|((\d+)(?!\.)))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracSpa"
                },
            };

            this.Regexes = _regexes.ToImmutableDictionary();
        }
    }
}