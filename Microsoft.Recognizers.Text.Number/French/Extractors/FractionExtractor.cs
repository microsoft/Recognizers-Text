using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }
        protected override sealed string ExtractType { get; } = Constants.SYS_NUM_FRACTION;

        public FractionExtractor()
        {
            string specialFractionInteger = $@"((({IntegerExtractor.AllIntRegex})i?({IntegerExtractor.ZeroToNineIntegerRegex})|({IntegerExtractor.AllIntRegex}))a?v[oa]s?)";

            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+[/]\d+(?=(\b[^/]|$))",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex(@"(((?<=\W|^)-\s*)|(?<=\b))\d+\s+\d+[/]\d+(?=(\b[^/]|$))",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracNum"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+((et)\s+)?)?({IntegerExtractor.AllIntRegex})(\s+((et)\s)?)((({OrdinalExtractor.AllOrdinalRegex})s?|({specialFractionInteger})|({OrdinalExtractor.SufixRoundOrdinalRegex})s?)|demis?|quarts?)(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(y\s+)?)?(un|une)(\s+)(({OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.SufixRoundOrdinalRegex})|(un\s+)?demis?)(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex($@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+sur\s+(({IntegerExtractor.AllIntRegex})|((\d+)(?!\.)))(?=\b)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
            };

            this.Regexes = _regexes.ToImmutableDictionary();
        }
    }
}
