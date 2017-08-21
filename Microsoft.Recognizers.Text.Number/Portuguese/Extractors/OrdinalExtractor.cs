using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }

        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL; // "Ordinal";

        public const string SimpleRoundOrdinalRegex = @"(mil[eé]sim[oa]|milion[eé]sim[oa]|bilion[eé]sim[oa]|trilion[eé]sim[oa]|quatrilion[eé]sim[oa]|quintilion[eé]sim[oa])";

        public const string OneToNineOrdinalRegex = @"(primeir[oa]|segund[oa]|terceir[oa]|quart[oa]|quint[oa]|sext[oa]|s[eé]tim[oa]|oitav[oa]|non[oa])";

        public const string TensOrdinalRegex = @"(nonag[eé]sim[oa]|octog[eé]sim[oa]|setuag[eé]sim[oa]|septuag[eé]sim[oa]|sexag[eé]sim[oa]|quinquag[eé]sim[oa]|quadrag[eé]sim[oa]|trig[eé]sim[oa]|vig[eé]sim[oa]|d[eé]cim[oa])";

        public const string HundredOrdinalRegex = @"(cent[eé]sim[oa]|ducent[eé]sim[oa]|tricent[eé]sim[oa]|cuadringent[eé]sim[oa]|quingent[eé]sim[oa]|sexcent[eé]sim[oa]|septingent[eé]sim[oa]|octingent[eé]sim[oa]|noningent[eé]sim[oa])";

        public const string SpecialUnderHundredOrdinalRegex = @"(und[eé]cim[oa]|duod[eé]cimo)";

        public static string UnderHundredOrdinalRegex => $@"((({TensOrdinalRegex}(\s)?)?{OneToNineOrdinalRegex})|{TensOrdinalRegex}|{SpecialUnderHundredOrdinalRegex})";

        public static string UnderThousandOrdinalRegex => $@"((({HundredOrdinalRegex}(\s)?)?{UnderHundredOrdinalRegex})|{HundredOrdinalRegex})";

        public static string OverThousandOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})([eé]sim[oa]))";

        public static string ComplexOrdinalRegex => $@"(({OverThousandOrdinalRegex}(\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex})";

        public static string SufixRoundOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})({SimpleRoundOrdinalRegex}))";

        public static string ComplexRoundOrdinalRegex => $@"((({SufixRoundOrdinalRegex}(\s)?)?{ComplexOrdinalRegex})|{SufixRoundOrdinalRegex})";

        public static string AllOrdinalRegex => $@"{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}";

        public OrdinalExtractor()
        {
            var regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)(\d*(1[oa]|2[oa]|3[oa]|4[oa]|5[oa]|6[oa]|7[oa]|8[oa]|9[oa]|0[oa]))(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalNum"
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline)
                    , "OrdinalPor"
                }
            };

            this.Regexes = regexes.ToImmutableDictionary();
        }
    }
}