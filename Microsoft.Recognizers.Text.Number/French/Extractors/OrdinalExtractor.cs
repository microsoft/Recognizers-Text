using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public const string SimpleRoundOrdinalRegex = @"(centi[eè]me|milli[eè]me|millioni[eè]me|milliardi[eè]me|billioni[eè]me)";

        public const string OneToNineOrdinalRegex = @"(premier|premi[eè]re|deuxi[eè]me|second[e]|troisi[eè]me|tiers|tierce|quatri[eè]me|cinqui[eè]me|sixi[eè]me|septi[eè]me|huiti[eè]me|neuvi[eè]me)";

        public const string SpecialUnderHundredOrdinalRegex = @"(onzi[eè]me|douzi[eè]me)";

        public const string TensOrdinalRegex =
                       @"(quatre-vingt-dixi[eè]me|quatre-vingti[eè]me|huitanti[eè]me|octanti[eè]me|soixante-dixi[eè]me|septanti[eè]me|soixanti[eè]me|cinquanti[eè]me|quaranti[eè]me|trenti[eè]me|vingti[eè]me)";

        public static string HundredOrdinalRegex = $@"({IntegerExtractor.AllIntRegex}(\s+(centi[eè]me\s)))"; // un centieme, deux centieme, trois centieme, etc 

        public static string UnderHundredOrdinalRegex => $@"((({IntegerExtractor.AllIntRegex}(\W)?)?{OneToNineOrdinalRegex})|({IntegerExtractor.TensNumberIntegerRegex}(\W)?)?{OneToNineOrdinalRegex}|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})";

        public static string UnderThousandOrdinalRegex => $@"((({HundredOrdinalRegex}(\s)?)?{UnderHundredOrdinalRegex})|(({IntegerExtractor.AllIntRegex}(\W)?)?{SimpleRoundOrdinalRegex})|{HundredOrdinalRegex})";

        public static string OverThousandOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})(i[eè]me))";

        public static string ComplexOrdinalRegex => $@"(({OverThousandOrdinalRegex}(\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex}|{UnderHundredOrdinalRegex})";

        public static string SuffixOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})({SimpleRoundOrdinalRegex}))";

        public static string ComplexRoundOrdinalRegex => $@"((({SuffixOrdinalRegex}(\s)?)?{ComplexOrdinalRegex})|{SuffixOrdinalRegex})";

        public static string AllOrdinalRegex => $@"{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}";

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)((\d*(1er|2e|2eme|3e|3eme|4e|4eme|5e|5eme|6e|6eme|7e|7eme|8e|8eme|9e|9eme|0e|0eme))|(11e|11eme|12e|12eme))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdFr"
                }
            };

            Regexes = regexes.ToImmutableDictionary();
        }
    }
}
