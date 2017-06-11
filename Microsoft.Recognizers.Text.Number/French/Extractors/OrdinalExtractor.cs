using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.French
{
    public class OrdinalExtractor : BaseNumberExtractor
    {
        internal sealed override ImmutableDictionary<Regex, string> Regexes { get; }
        protected sealed override string ExtractType { get; } = Constants.SYS_NUM_ORDINAL;

        // NOTE: Ordinals in FR are fairly simple, with a few exceptions (0-3) everything ends with '-ième' to signify ordinance 

        public const string SimpleRoundOrdinalRegex = @"(centi[eè]me|milli[eè]me|millioni[eè]me|milliardi[eè]me|billioni[eè]me)";

        public const string OneToNineOrdinalRegex = @"(premier|premi[èe]re|deuxi[èe]me|troisi[èe]me|quatri[èe]me|cinqui[èe]me|sixi[èe]me|septi[èe]me|huiti[èe]me|neuvi[èe]me)";

        public const string FrenchOrdinalSuffixRegex = @"(i[èe]me|i[èe]mes)";

        public const string OneToThreeOrdinalRegex = @"(premier|premi[èe]re|second|seconde|tiers|tierce)";

        public const string TensOrdinalRegex = @"(dixi[eè]me|vingti[eè]me|trenti[eè]me|quaranti[eè]me|cinquanti[eè]me|soixanti[eè]me|soixante-dixi[eè]me|quatre-vingti[eè]me|quatre-vingt-dixi[eè]me)";

        public static string HundredOrdinalRegex = $@"(centi[eè]me|({IntegerExtractor.ZeroToNineIntegerRegex}+)(\s+|\s*-\s*)centi[eè]me|)"; // check this one

        public static string UnderHundredOrdinalRegex => $@"((({TensOrdinalRegex}(\s)?)?{OneToNineOrdinalRegex})|{TensOrdinalRegex}|{BasicOrdinalRegex})";

        public static string UnderThousandOrdinalRegex => $@"((({HundredOrdinalRegex}(\s)?)?{UnderHundredOrdinalRegex})|{HundredOrdinalRegex})";

        public static string OverThousandOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex}+)(\s+|\s*-\s*)(mille)";

        public static string ComplexOrdinalRegex => $@"(({OverThousandOrdinalRegex}(\s)?)?{UnderThousandOrdinalRegex}|{OverThousandOrdinalRegex})";

        public static string SufixRoundOrdinalRegex => $@"(({IntegerExtractor.AllIntRegex})({SimpleRoundOrdinalRegex}))";

        public static string ComplexRoundOrdinalRegex => $@"((({SufixRoundOrdinalRegex}(\s)?)?{ComplexOrdinalRegex})|{SufixRoundOrdinalRegex})";

        public static string AllOrdinalRegex = $@"{ComplexOrdinalRegex}|{SimpleRoundOrdinalRegex}|{ComplexRoundOrdinalRegex}";

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
        $@"(({IntegerExtractor.AllIntRegex}\s+){SimpleRoundOrdinalRegex})";

        
        public OrdinalExtractor()
        {
            var _regexes = new Dictionary<Regex, string>
            {
                {
                    new Regex(
                        @"(?<=\b)((\d*(1[er][re]|2e|3e|4e|5e|6e|7e|8e|9e|0th))|(11e|12e))(?=\b)",
                        RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Singleline)
                        , "OrdinalNum"
                },
                {
                    new Regex(
                        @"",
                        RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Singleline)
                        , "OrdinalNum"        
                },
                {
                    new Regex($@"(?<=\b){AllOrdinalRegex}(?=\b)", RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Singleline)
                        , "OrdFr" 
                }    
                
            };

            Regexes = _regexes.ToImmutableDictionary();
        }

    }
}
