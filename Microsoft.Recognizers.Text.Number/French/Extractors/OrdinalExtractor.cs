using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public const string FrOrdinalSuffixRegex = @"i[eè]me"; //unsure if needed 

        public const string RoundNumberOrdinalRegex = @"(centi[eè]me|milli[eè]me|millioni[eè]me|milliardi[eè]me|billioni[eè]me)";

        public const string BasicOrdinalRegex =
                       @"(z[eé]roi[eè]me|premier|premi[eè]re|deuxi[eè]me|second|seconde|troisi[eè]me|tiers|tierce|quatri[eè]me|cinqui[eè]me|sixi[eè]me|septi[eè]me|huiti[eè]me|neuvi[eè]me|dixi[eè]me|onzi[eè]me
                          |douzi[eè]me|treizi[eè]me|quatorzi[eè]me|quinzi[eè]me|seizi[eè]me|dix-septi[eè]me|dix-huiti[eè]me|dix-neuvi[eè]me|vingti[eè]me)";

        public static string SuffixBasicOrdinalRegex
            =>
                $@"((((({IntegerExtractor.TensNumberIntegerRegex}(\s+(et\s+)?|\s*-\s*){
                    IntegerExtractor.ZeroToNineIntegerRegex})|{IntegerExtractor
                        .TensNumberIntegerRegex}|{IntegerExtractor.ZeroToNineIntegerRegex}|{IntegerExtractor.AnIntRegex
                    })(\s+{IntegerExtractor
                        .RoundNumberIntegerRegex})+)\s+(et\s+)?)*({IntegerExtractor.TensNumberIntegerRegex
                    }(\s+|\s*-\s*))?{BasicOrdinalRegex})";

        public static string SuffixRoundNumberOrdinalRegex
            =>
                $@"(({IntegerExtractor.AllIntRegex}\s+){RoundNumberOrdinalRegex})";

        public static string AllOrdinalRegex
            =>
                $@"({SuffixBasicOrdinalRegex}|{SuffixRoundNumberOrdinalRegex})";

        public OrdinalExtractor()
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)((\d*(1er|2e|2eme|3e|3eme|4e|4eme|5e|5eme|6e|6eme|7e|7eme|8e|8eme|9e|9eme|0e|0eme))|(11e|11eme|12e|12eme))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex(@"(?<=\b)(\d{1,3}(\s*,\s*\d{3})*\s*e)(?=\b)", // 'e' instead of 'th'
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)", RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdFr"
                },
                {
                    new Regex($@"(?<!(a|un|une)\s+){RoundNumberOrdinalRegex}",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdFr"
                }
            };
            Regexes = _regexes.ToImmutableDictionary();
        }
    }
}
