using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text; 

namespace Microsoft.Recognizers.Text.Number.French
{
    public class FrenchNumberParserConfiguration : INumberParserConfiguration
    {
        public FrenchNumberParserConfiguration(): this(new CultureInfo(Culture.French)) { }

        public FrenchNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = "Fr";
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = ',';
            this.FractionMarkerToken = "sur"; 
            this.NonDecimalSeparatorChar = '.';
            this.HalfADozenText = "six";
            this.WordSeparatorToken = "et"; // EN - 'and'

            this.WrittenDecimalSeparatorTexts = new List<string> { "virgule" };
            this.WrittenGroupSeparatorTexts = new List<string> { "point", "points" };
            this.WrittenIntegerSeparatorTexts = new List<string> { "et","-" };
            this.WrittenFractionSeparatorTexts = new List<string> { "et", "sur" };

            this.CardinalNumberMap = InitCardinalNumberMap();
            this.OrdinalNumberMap = InitOrdinalNumberMap();
            this.RoundNumberMap = InitRoundNumberMap();
            this.HalfADozenRegex = new Regex(@"(?<=\b)+demi\s+douzaine", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(
                @"((?<=\b)(cent|mille|million|milliard|billion|douzaine(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public CultureInfo CultureInfo { get; set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; private set; }

        public string WordSeparatorToken { get; private set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; private set; }

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }

            long value = 0;
            long finalValue = 0;
            var strBuilder = new StringBuilder();
            int lastGoodChar = 0;
            for (int i = 0; i < numberStr.Length; i++)
            {
                strBuilder.Append(numberStr[i]);

                if (this.CardinalNumberMap.ContainsKey(strBuilder.ToString()) && this.CardinalNumberMap[strBuilder.ToString()] > value)
                {
                    lastGoodChar = i;
                    value = this.CardinalNumberMap[strBuilder.ToString()];
                }

                if ((i + 1) == numberStr.Length)
                {
                    finalValue += value;
                    strBuilder.Clear();
                    i = lastGoodChar++;
                    value = 0;
                }
            }

            return finalValue;
        }

        private static ImmutableDictionary<string, long> InitCardinalNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"zéro", 0},
                {"zero", 0 },
                {"un", 1},
                {"une", 1},
                {"deux", 2},
                {"trois", 3},
                {"quatre", 4},
                {"cinq", 5},
                {"six", 6 },
                {"sept", 7},
                {"huit", 8},
                {"neuf", 9},
                {"dix", 10},
                {"onze", 11},
                {"douze", 12},
                {"treize", 13},
                {"quatorze", 14},
                {"quinze", 15},
                {"seize", 16},
                {"dix-sept", 17},
                {"dix-huit", 18},
                {"dix-neuf", 19},
                {"vingt", 20}, 
                {"trente", 30},
                {"quarante", 40},
                {"cinquante", 50},
                {"soixante", 60},
                {"soixante-dix", 70},
                {"septante", 70 },
                {"quatre-vingts", 80},
                {"quatre-vingts-dix", 90},
                {"quatre-vingts-onze", 91},
                {"quatre-vingts-douze", 92},
                {"quatre-vingts-treize", 93 },
                {"quatre-vingts-quatorze", 94 },
                {"quatre-vingts-quinze", 95 },
                {"quatre-vingts-seize", 96 },
                {"quatre-vingt-dix-sept", 97 },
                {"quatre-vingt-dix-neuf", 98 },
                {"nonante", 90 },
                {"cent", 100},
                {"mille", 1000},
                {"un million", 1000000},
                {"million", 1000000 },
                {"un milliard", 1000000000},
                {"milliard", 1000000000},
                {"un mille milliards", 1000000000000},
                {"un billion", 1000000000000 }
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitOrdinalNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"premier", 1},
                {"première", 1},
                {"premiere", 1 },
                {"deuxième", 2},
                {"deuxieme", 2},
                {"second", 2},
                {"seconde", 2},
                {"troisième", 3},
                {"demi", 2}, 
                {"tiers", 3},
                {"tierce", 3 },
                {"quart", 4 },
                {"quarts", 4 },
                {"troisieme", 3},
                {"quatrième", 4},
                {"quatrieme", 4},
                {"cinquième", 5},
                {"cinquieme", 5},
                {"sixième", 6},
                {"sixieme", 6},
                {"septième", 7 },
                {"septieme", 7 },
                {"huitième", 8 },
                {"huitieme", 8 },
                {"neuvième", 9 },
                {"neuvieme", 9 },
                {"dixième", 10 },
                {"dixieme", 10 },
                {"onzième", 11 },
                {"onzieme", 11 },
                {"douzième", 12 },
                {"douzieme", 12 },
                {"treizième", 13 },
                {"treizieme", 13 },
                {"quatorzième", 14 },
                {"quatorizieme", 14 },
                {"quinzième", 15 },
                {"quinzieme", 15 },
                {"seizième", 16 },
                {"seizieme", 16 },
                {"dix-septième", 17 },
                {"dix-septieme", 17 },
                {"dix-huitième", 18 },
                {"dix-huitieme", 18 },
                {"dix-neuvième", 19 },
                {"dix-neuvieme", 19 },
                {"vingtième", 20 },
                {"vingtieme", 20 },
                {"trentième", 30 },
                {"trentieme", 30 },
                {"quarantième", 40 },
                {"quarantieme", 40 },
                {"cinquantième", 50 },
                {"cinquantieme", 50 },
                {"soixantième", 60 },
                {"soixantieme", 60 },
                {"soixante-dixième", 70 },
                {"soixante-dixieme", 70 },
                {"septantième", 70 },
                {"septantieme", 70 },
                {"quatre-vingtième", 80 },
                {"quatre-vingtieme", 80 },
                {"huitantième", 80 },
                {"huitantieme", 80 },
                {"octantième", 80 },
                {"octantieme", 80 },
                {"quatre-vingt-dixième", 90 },
                {"quatre-vingt-dixieme", 90 },
                {"nonantième", 90 },
                {"nonantieme", 90},
                {"centième", 100 },
                {"centieme", 100 },
                {"millième", 1000 },
                {"millieme", 1000 },
                {"millionième", 1000000 },
                {"millionieme", 1000000 },
                {"milliardième", 1000000000 },
                {"milliardieme", 1000000000 },
                {"billionieme", 1000000000000},
                {"billionième", 1000000000000},
                {"trillionième", 1000000000000000000},
                {"trillionieme", 1000000000000000000}

            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitRoundNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"cent", 100}, // hundred
                {"mille", 1000}, // thousand
                {"million", 1000000}, // million
                {"milliard", 1000000000}, //billion
                {"billion", 1000000000000}, //trillion
                {"centieme", 100}, // hundredth
                {"centième", 100},
                {"millieme", 1000},
                {"millième", 1000},
                {"millionième", 1000000},// millionth
                {"millionieme", 1000000},
                {"milliardième", 1000000000}, //billionth
                {"milliardieme", 1000000000},
                {"billionième", 1000000000000}, // trillionth
                {"billionieme", 1000000000000},
                {"centiemes", 100}, // hundredths
                {"centièmes", 100},
                {"millièmes", 1000}, // thousandths
                {"milliemes", 1000},
                {"millionièmes", 1000000},
                {"millioniemes", 1000000},
                {"milliardièmes", 1000000000},
                {"milliardiemes", 1000000000},
                {"billionièmes", 1000000000000},
                {"billioniemes", 1000000000000},
                {"douzaine", 12},
                {"douzaines", 12},
                {"k", 1000},
                {"m", 1000000},
                {"g", 1000000000},
                {"b", 1000000000},
                {"t", 1000000000000}
            }.ToImmutableDictionary();
        }
    }
}
