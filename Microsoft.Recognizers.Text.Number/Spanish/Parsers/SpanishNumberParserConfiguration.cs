using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Spanish
{
    public class SpanishNumberParserConfiguration : INumberParserConfiguration
    {
        public SpanishNumberParserConfiguration() : this(new CultureInfo(Culture.Spanish)) { }

        public SpanishNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = "Spa";
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = ',';
            this.FractionMarkerToken = "sobre";
            this.NonDecimalSeparatorChar = '.';
            this.HalfADozenText = "seis";
            this.WordSeparatorToken = "y";

            this.WrittenDecimalSeparatorTexts = new List<string> { "coma", "con" };
            this.WrittenGroupSeparatorTexts = new List<string> { "punto" };
            this.WrittenIntegerSeparatorTexts = new List<string> { "y" };
            this.WrittenFractionSeparatorTexts = new List<string> { "con" };

            this.CardinalNumberMap = InitCardinalNumberMap();
            this.OrdinalNumberMap = InitOrdinalNumberMap();
            this.RoundNumberMap = InitRoundNumberMap();
            this.HalfADozenRegex = new Regex(@"media\s+docena", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(
                @"((?<=\b)(mil|millones|mill[oó]n|billones|bill[oó]n|trillones|trill[oó]n|docenas?)(?=\b))|((?<=(\d|\b))(k|t|m|g)(?=\b))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public CultureInfo CultureInfo { get; private set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public string NonDecimalSeparatorText { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; private set; }

        public string WordSeparatorToken { get; private set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; private set; }

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, object context)
        {
            var result = new List<string>();

            foreach (var token in tokens)
            {
                var tempWord = token.Trim('s');
                if (this.OrdinalNumberMap.ContainsKey(tempWord))
                {
                    result.Add(tempWord);
                    continue;
                }

                if (tempWord.EndsWith("avo") || tempWord.EndsWith("ava"))
                {
                    var origTempWord = tempWord;
                    var newLength = origTempWord.Length;
                    tempWord = origTempWord.Remove(newLength - 3);
                    if (this.CardinalNumberMap.ContainsKey(tempWord))
                    {
                        result.Add(tempWord);
                        continue;
                    }
                    else
                    {
                        tempWord = origTempWord.Remove(newLength - 2);
                        if (this.CardinalNumberMap.ContainsKey(tempWord))
                        {
                            result.Add(tempWord);
                            continue;
                        }
                    }
                }

                result.Add(token);
            }

            return result;
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
                {"cero", 0},
                {"un", 1},
                {"una", 1},
                {"uno", 1},
                {"dos", 2},
                {"tres", 3},
                {"cuatro", 4},
                {"cinco", 5},
                {"seis", 6},
                {"siete", 7},
                {"ocho", 8},
                {"nueve", 9},
                {"diez", 10},
                {"once", 11},
                {"doce", 12},
                {"docena", 12},
                {"docenas", 12},
                {"trece", 13},
                {"catorce", 14},
                {"quince", 15},
                {"dieciseis", 16},
                {"dieciséis", 16},
                {"diecisiete", 17},
                {"dieciocho", 18},
                {"diecinueve", 19},
                {"veinte", 20},
                {"ventiuna", 21 },
                {"ventiuno", 21 },
                {"veintiun", 21 },
                {"veintiún", 21 },
                {"veintiuno", 21 },
                {"veintiuna", 21 },
                {"veintidos", 22 },
                {"veintidós", 22 },
                {"veintitres", 23 },
                {"veintitrés", 23 },
                {"veinticuatro", 24 },
                {"veinticinco", 25 },
                {"veintiseis", 26 },
                {"veintiséis", 26 },
                {"veintisiete", 27 },
                {"veintiocho", 28 },
                {"veintinueve", 29 },
                {"treinta", 30},
                {"cuarenta", 40},
                {"cincuenta", 50},
                {"sesenta", 60},
                {"setenta", 70},
                {"ochenta", 80},
                {"noventa", 90},
                {"cien", 100},
                {"ciento", 100},
                {"doscientas", 200},
                {"doscientos", 200},
                {"trescientas", 300},
                {"trescientos", 300},
                {"cuatrocientas", 400},
                {"cuatrocientos", 400},
                {"quinientas", 500},
                {"quinientos", 500},
                {"seiscientas", 600},
                {"seiscientos", 600},
                {"setecientas", 700},
                {"setecientos", 700},
                {"ochocientas", 800},
                {"ochocientos", 800},
                {"novecientas", 900},
                {"novecientos", 900},
                {"mil", 1000},
                {"millon", 1000000},
                {"millón", 1000000},
                {"millones", 1000000},
                {"billon", 1000000000000},
                {"billón", 1000000000000},
                {"billones", 1000000000000},
                {"trillon", 1000000000000000000},
                {"trillón", 1000000000000000000},
                {"trillones", 1000000000000000000}
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitOrdinalNumberMap()
        {
            var simpleOrdinalDictionary = new Dictionary<string, long>
            {
                {"primero", 1},
                {"primera", 1},
                {"primer", 1},
                {"segundo", 2},
                {"segunda", 2},
                {"medio", 2},
                {"media", 2},
                {"tercero", 3},
                {"tercera", 3},
                {"tercer", 3},
                {"tercio", 3},
                {"cuarto", 4},
                {"cuarta", 4},
                {"quinto", 5},
                {"quinta", 5},
                {"sexto", 6},
                {"sexta", 6},
                {"septimo", 7},
                {"septima", 7},
                {"octavo", 8},
                {"octava", 8},
                {"noveno", 9},
                {"novena", 9},
                {"decimo", 10},
                {"decima", 10},
                {"undecimo", 11},
                {"undecima", 11},
                {"duodecimo", 12},
                {"duodecima", 12},
                {"decimotercero", 13},
                {"decimotercera", 13},
                {"decimocuarto", 14},
                {"decimocuarta", 14},
                {"decimoquinto", 15},
                {"decimoquinta", 15},
                {"decimosexto", 16},
                {"decimosexta", 16},
                {"decimoseptimo", 17},
                {"decimoseptima", 17},
                {"decimoctavo", 18},
                {"decimoctava", 18},
                {"decimonoveno", 19},
                {"decimonovena", 19},
                {"vigesimo", 20},
                {"vigesima", 20},
                {"trigesimo", 30},
                {"trigesima", 30},
                {"cuadragesimo", 40},
                {"cuadragesima", 40},
                {"quincuagesimo", 50},
                {"quincuagesima", 50},
                {"sexagesimo", 60},
                {"sexagesima", 60},
                {"septuagesimo", 70},
                {"septuagesima", 70},
                {"octogesimo", 80},
                {"octogesima", 80},
                {"nonagesimo", 90},
                {"nonagesima", 90},
                {"centesimo", 100},
                {"centesima", 100},
                {"ducentesimo", 200},
                {"ducentesima", 200},
                {"tricentesimo", 300},
                {"tricentesima", 300},
                {"cuadringentesimo", 400},
                {"cuadringentesima", 400},
                {"quingentesimo", 500},
                {"quingentesima", 500},
                {"sexcentesimo", 600},
                {"sexcentesima", 600},
                {"septingentesimo", 700},
                {"septingentesima", 700},
                {"octingentesimo", 800},
                {"octingentesima", 800},
                {"noningentesimo", 900},
                {"noningentesima", 900},
                {"milesimo", 1000},
                {"milesima", 1000},
                {"millonesimo", 1000000},
                {"millonesima", 1000000},
                {"billonesimo", 1000000000000},
                {"billonesima", 1000000000000}
            };

            var PrefixCardinalDictionary = new Dictionary<string, long>()
            {
                {"dos", 2 },
                {"tres", 3 },
                {"cuatro", 4 },
                {"cinco", 5 },
                {"seis", 6 },
                {"siete", 7 },
                {"ocho", 8 },
                {"nueve", 9 },
                {"diez", 10 },
                {"once", 11 },
                {"doce", 12 },
                {"trece", 13},
                {"catorce", 14},
                {"quince", 15},
                {"dieciseis", 16},
                {"dieciséis", 16},
                {"diecisiete", 17},
                {"dieciocho", 18},
                {"diecinueve", 19},
                {"veinte", 20},
                {"ventiuna", 21 },
                {"veintiun", 21 },
                {"veintiún", 21 },
                {"veintidos", 22 },
                {"veintitres", 23 },
                {"veinticuatro", 24 },
                {"veinticinco", 25 },
                {"veintiseis", 26 },
                {"veintisiete", 27 },
                {"veintiocho", 28 },
                {"veintinueve", 29 },
                {"treinta", 30},
                {"cuarenta", 40},
                {"cincuenta", 50},
                {"sesenta", 60},
                {"setenta", 70},
                {"ochenta", 80},
                {"noventa", 90},
                {"cien", 100 },
                {"doscientos", 200},
                {"trescientos", 300},
                {"cuatrocientos", 400},
                {"quinientos", 500},
                {"seiscientos", 600},
                {"setecientos", 700},
                {"ochocientos", 800},
                {"novecientos", 900},
            };

            var SufixOrdinalDictionary = new Dictionary<string, long>()
            {
                {"milesimo", 1000 },
                {"millonesimo", 1000000 },
                {"billonesimo", 1000000000000 },
            };

            foreach (var sufix in SufixOrdinalDictionary)
            {
                foreach (var prefix in PrefixCardinalDictionary)
                {
                    simpleOrdinalDictionary.Add(prefix.Key + sufix.Key, prefix.Value * sufix.Value);
                }
            }

            return new Dictionary<string, long>(simpleOrdinalDictionary).ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitRoundNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"mil", 1000},
                {"milesimo", 1000},
                {"millon", 1000000},
                {"millón", 1000000},
                {"millones", 1000000},
                {"millonesimo", 1000000},
                {"billon", 1000000000000},
                {"billón", 1000000000000},
                {"billones", 1000000000000},
                {"billonesimo", 1000000000000},
                {"trillon", 1000000000000000000},
                {"trillón", 1000000000000000000},
                {"trillones", 1000000000000000000},
                {"trillonesimo", 1000000000000000000},
                // long support up to ~trillon - see if we want to support higher numbers
                //{"cuatrill[oó]n", 1000000000000000000000000},
                //{"cuatrillones", 1000000000000000000000000},
                //{"quintill[oó]n", 1000000000000000000000000000000},
                //{"quintillones", 1000000000000000000000000000000},
                //{"sextill[oó]n", 1000000000000000000000000000000000000},
                //{"sextillones", 1000000000000000000000000000000000000},
                //{"septill[oó]n", 1000000000000000000000000000000000000000000},
                //{"septillones", 1000000000000000000000000000000000000000000},
                {"docena", 12},
                {"docenas", 12},
                {"k", 1000},
                {"m", 1000000},
                {"g", 1000000000},
                {"t", 1000000000000}
            }.ToImmutableDictionary();
        }
    }
}