using Microsoft.Recognizers.Text.Number.Extractors;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish.Extractors
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal override sealed ImmutableDictionary<Regex, string> Regexes { get; }

        protected override sealed string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public const string SimpleRoundOrdinalRegex = @"(mil[eé]simo|millon[eé]sim[oa]|billon[eé]sim[oa]|trillon[eé]sim[oa]|cuatrillon[eé]sim[oa]|quintillon[eé]sim[oa]|sextillon[eé]sim[oa]|septillon[eé]sim[oa])";

        public const string OneToNineOrdinalRegex = @"(primer[oa]|segund[oa]|tercer[oa]|cuart[oa]|quint[oa]|sext[oa]|s[eé]ptim[oa]|octav[oa]|noven[oa])";

        public const string TensOrdinalRegex = @"(nonag[eé]sim[oa]|octog[eé]sim[oa]|septuag[eé]sim[oa]|sexag[eé]sim[oa]|quincuag[eé]sim[oa]|cuadrag[eé]sim[oa]|trig[eé]sim[oa]|vig[eé]sim[oa]|d[eé]cim[oa])";

        public const string HundredOrdinalRegex = @"(cent[eé]sim[oa]|ducent[eé]sim[oa]|tricent[eé]sim[oa]|cuadringent[eé]sim[oa]|quingent[eé]sim[oa]|sexcent[eé]sim[oa]|septingent[eé]sim[oa]|octingent[eé]sim[oa]|noningent[eé]sim[oa])";

        public const string SpecialUnderHundredOrdinalRegex = @"(und[eé]cim[oa]|duod[eé]cimo|decimoctav[oa])";

        public static string UnderHundredOrdinalRegex => $@"((({TensOrdinalRegex}(\s)?)?{OneToNineOrdinalRegex})|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})";

        public static string UnderThousandOrdinalRegex => $@"((({HundredOrdinalRegex}(\s)?)?{UnderHundredOrdinalRegex})|{HundredOrdinalRegex})";

        public static string OverThousandOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})([eé]sim[oa]))";

        public static string ComplexOrdinalRegex => $@"(({OverThousandOrdinalRegex}(\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex})";

        public static string SufixRoundOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})({SimpleRoundOrdinalRegex}))";

        public static string ComplexRoundOrdinalRegex => $@"((({SufixRoundOrdinalRegex}(\s)?)?{ComplexOrdinalRegex})|{SufixRoundOrdinalRegex})";

        public static string AllOrdinalRegex => $@"{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}";


        public OrdinalExtractor()
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)(\d*(1r[oa]|2d[oa]|3r[oa]|4t[oa]|5t[oa]|6t[oa]|7m[oa]|8v[oa]|9n[oa]|0m[oa]|11[vm][oa]|12[vm][oa]))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalSpa"
                }
            };

            this.Regexes = _regexes.ToImmutableDictionary();
        }
    }
}