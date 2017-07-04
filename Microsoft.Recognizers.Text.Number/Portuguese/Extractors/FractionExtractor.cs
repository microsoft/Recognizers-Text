using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public FractionExtractor()
        {
            string specialFractionInteger = $@"((({IntegerExtractor.AllIntRegex})i?({IntegerExtractor.ZeroToNineIntegerRegex})|({IntegerExtractor.AllIntRegex}))\s+a?v[oa]s?)";

            var regexes = new Dictionary<Regex, string>
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
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+((e|com)\s+)?)?({IntegerExtractor.AllIntRegex})(\s+((e|com)\s)?)((({OrdinalExtractor.AllOrdinalRegex})s?|({specialFractionInteger})|({OrdinalExtractor.SufixRoundOrdinalRegex})s?)|mei[oa]?|ter[çc]o?)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracPor"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(e\s+)?)?(um|um[as])(\s+)(({OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.SufixRoundOrdinalRegex})|(e\s+)?mei[oa]?)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracPor"
                },
                {
                    new Regex($@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+sobre\s+(({IntegerExtractor.AllIntRegex})|((\d+)(?!\.)))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracPor"
                },
            };

            this.Regexes = regexes.ToImmutableDictionary();
        }
    }
}