using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FractionExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_FRACTION; // "Fraction";

        public FractionExtractor()
        {
            var regexes = new Dictionary<Regex, string>
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
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+((et)\s+)?)?({IntegerExtractor.AllIntRegex})(\s+((et)\s)?)((({OrdinalExtractor.AllOrdinalRegex})s?|({OrdinalExtractor.SuffixOrdinalRegex})s?)|demis?|tiers?|quarts?)(?=\b)",    
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex($@"(?<=\b)({IntegerExtractor.AllIntRegex}\s+(et\s+)?)?(un|une)(\s+)(({OrdinalExtractor.AllOrdinalRegex})|({OrdinalExtractor.SuffixOrdinalRegex})|(et\s+)?demis?)(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                },
                {
                    new Regex($@"(?<=\b)(({IntegerExtractor.AllIntRegex})|((?<!\.)\d+))\s+sur\s+(({IntegerExtractor.AllIntRegex})|((\d+)(?!\.)))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "FracFr"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
