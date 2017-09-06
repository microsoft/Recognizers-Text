using System.Collections.Generic;
using System.Globalization;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Text;

namespace Microsoft.Recognizers.Text.Number.Portuguese
{
    public class PortugueseNumberParserConfiguration : INumberParserConfiguration
    {
        public PortugueseNumberParserConfiguration() : this(new CultureInfo(Culture.Portuguese)) { }

        public PortugueseNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = "Por";
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = ',';
            this.FractionMarkerToken = "sobre";
            this.NonDecimalSeparatorChar = '.';
            this.HalfADozenText = "seis";
            this.WordSeparatorToken = "e";

            this.WrittenDecimalSeparatorTexts = new List<string> { "virgula", "vírgula" };
            this.WrittenGroupSeparatorTexts = new List<string> { "ponto" };
            this.WrittenIntegerSeparatorTexts = new List<string> { "e" };
            this.WrittenFractionSeparatorTexts = new List<string> { "com" }; // @TODO Using the correct 'e' will cause issues as it's a common number separator.

            this.CardinalNumberMap = InitCardinalNumberMap();
            this.OrdinalNumberMap = InitOrdinalNumberMap();
            this.RoundNumberMap = InitRoundNumberMap();

            this.HalfADozenRegex = new Regex(@"meia\s+d[uú]zia", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(
                @"((?<=\b)(mil|cem|milh[oõ]es|milh[aã]o|bilh[oõ]es|bilh[aã]o|trilh[oõ]es|trilh[aã]o|milhares|centena|centenas|dezena|dezenas?)(?=\b))|((?<=(\d|\b))(k|t|m|g)(?=\b))",
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

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
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

                    if (string.IsNullOrWhiteSpace(tempWord)) {
                        // Ignore avos in fractions.
                        continue;
                    }
                    else if (this.CardinalNumberMap.ContainsKey(tempWord))
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
                {"zero", 0},
                {"hum", 1},
                {"um", 1},
                {"uma", 1},
                {"dois", 2},
                {"duas", 2},
                {"meia", 2},
                {"meio", 2},
                {"tres", 3},
                {"três", 3},
                {"quatro", 4},
                {"cinco", 5},
                {"seis", 6},
                {"sete", 7},
                {"oito", 8},
                {"nove", 9},
                {"dez", 10},
                {"dezena", 10},
                {"déz", 10},
                {"onze", 11},
                {"doze", 12},
                {"dúzia", 12},
                {"duzia", 12},
                {"dúzias", 12},
                {"duzias", 12},
                {"treze", 13},
                {"catorze", 14},
                {"quatorze", 14},
                {"quinze", 15},
                {"dezesseis", 16},
                {"dezasseis", 16},
                {"dezessete", 17},
                {"dezassete", 17},
                {"dezoito", 18},
                {"dezenove", 19},
                {"dezanove", 19},
                {"vinte", 20},
                {"trinta", 30},
                {"quarenta", 40},
                {"cinquenta", 50},
                {"cincoenta", 50},
                {"sessenta", 60},
                {"setenta", 70},
                {"oitenta", 80},
                {"noventa", 90},
                {"cem", 100},
                {"cento", 100},
                {"duzentos", 200},
                {"duzentas", 200},
                {"trezentos", 300},
                {"trezentas", 300},
                {"quatrocentos", 400},
                {"quatrocentas", 400},
                {"quinhentos", 500},
                {"quinhentas", 500},
                {"seiscentos", 600},
                {"seiscentas", 600},
                {"setecentos", 700},
                {"setecentas", 700},
                {"oitocentos", 800},
                {"oitocentas", 800},
                {"novecentos", 900},
                {"novecentas", 900},
                {"mil", 1000},
                {"milhão", 1000000},
                {"milhao", 1000000},
                {"milhões", 1000000},
                {"milhoes", 1000000},
                {"bilhão", 1000000000},
                {"bilhao", 1000000000},
                {"bilhões", 1000000000},
                {"bilhoes", 1000000000},
                {"trilhão", 1000000000000},
                {"trilhao", 1000000000000},
                {"trilhões", 1000000000000},
                {"trilhoes", 1000000000000},
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitOrdinalNumberMap()
        {
            var simpleOrdinalDictionary = new Dictionary<string, long>
            {
                {"primeiro", 1},
                {"primeira", 1},
                {"segundo", 2},
                {"segunda", 2},
                {"terceiro", 3},
                {"terceira", 3},
                {"quarto", 4},
                {"quarta", 4},
                {"quinto", 5},
                {"quinta", 5},
                {"sexto", 6},
                {"sexta", 6},
                {"sétimo", 7},
                {"setimo", 7},
                {"sétima", 7},
                {"setima", 7},
                {"oitavo", 8},
                {"oitava", 8},
                {"nono", 9},
                {"nona", 9},
                {"décimo", 10},
                {"decimo", 10},
                {"décima", 10},
                {"decima", 10},
                {"undécimo", 11},
                {"undecimo", 11},
                {"undécima", 11},
                {"undecima", 11},
                {"duodécimo", 11},
                {"duodecimo", 11},
                {"duodécima", 11},
                {"duodecima", 11},
                {"vigésimo", 20},
                {"vigesimo", 20},
                {"vigésima", 20},
                {"vigesima", 20},
                {"trigésimo", 30},
                {"trigesimo", 30},
                {"trigésima", 30},
                {"trigesima", 30},
                {"quadragésimo", 40},
                {"quadragesimo", 40},
                {"quadragésima", 40},
                {"quadragesima", 40},
                {"quinquagésimo", 50},
                {"quinquagesimo", 50},
                {"quinquagésima", 50},
                {"quinquagesima", 50},
                {"sexagésimo", 60},
                {"sexagesimo", 60},
                {"sexagésima", 60},
                {"sexagesima", 60},
                {"septuagésimo", 70},
                {"septuagesimo", 70},
                {"septuagésima", 70},
                {"septuagesima", 70},
                {"setuagésimo", 70},
                {"setuagesimo", 70},
                {"setuagésima", 70},
                {"setuagesima", 70},
                {"octogésimo", 80},
                {"octogesimo", 80},
                {"octogésima", 80},
                {"octogesima", 80},
                {"nonagésimo", 90},
                {"nonagesimo", 90},
                {"nonagésima", 90},
                {"nonagesima", 90},
                {"centesimo", 100},
                {"centésimo", 100},
                {"centesima", 100},
                {"centésima", 100},
                {"ducentésimo", 200},
                {"ducentesimo", 200},
                {"ducentésima", 200},
                {"ducentesima", 200},
                {"tricentésimo", 300},
                {"tricentesimo", 300},
                {"tricentésima", 300},
                {"tricentesima", 300},
                {"trecentésimo", 300},
                {"trecentesimo", 300},
                {"trecentésima", 300},
                {"trecentesima", 300},
                {"quadringentésimo", 400},
                {"quadringentesimo", 400},
                {"quadringentésima", 400},
                {"quadringentesima", 400},
                {"quingentésimo", 500},
                {"quingentesimo", 500},
                {"quingentésima", 500},
                {"quingentesima", 500},
                {"sexcentésimo", 600},
                {"sexcentesimo", 600},
                {"sexcentésima", 600},
                {"sexcentesima", 600},
                {"seiscentésimo", 600},
                {"seiscentesimo", 600},
                {"seiscentésima", 600},
                {"seiscentesima", 600},
                {"septingentésimo", 700},
                {"septingentesimo", 700},
                {"septingentésima", 700},
                {"septingentesima", 700},
                {"setingentésimo", 700},
                {"setingentesimo", 700},
                {"setingentésima", 700},
                {"setingentesima", 700},
                {"octingentésimo", 800},
                {"octingentesimo", 800},
                {"octingentésima", 800},
                {"octingentesima", 800},
                {"noningentésimo", 900},
                {"noningentesimo", 900},
                {"noningentésima", 900},
                {"noningentesima", 900},
                {"nongentésimo", 900},
                {"nongentesimo", 900},
                {"nongentésima", 900},
                {"nongentesima", 900},
                {"milésimo", 1000},
                {"milesimo", 1000},
                {"milésima", 1000},
                {"milesima", 1000},
                {"milionésimo", 1000000},
                {"milionesimo", 1000000},
                {"milionésima", 1000000},
                {"milionesima", 1000000},
                {"bilionésimo", 1000000000},
                {"bilionesimo", 1000000000},
                {"bilionésima", 1000000000},
                {"bilionesima", 1000000000}
            };

            var prefixCardinalDictionary = new Dictionary<string, long>()
            {
                {"hum", 1 },
                {"dois", 2 },
                {"tres", 3 },
                {"três", 3 },
                {"quatro", 4 },
                {"cinco", 5 },
                {"seis", 6 },
                {"sete", 7 },
                {"oito", 8 },
                {"nove", 9 },
                {"dez", 10 },
                {"onze", 11 },
                {"doze", 12 },
                {"treze", 13},
                {"catorze", 14},
                {"quatorze", 14},
                {"quinze", 15},
                {"dezesseis", 16},
                {"dezasseis", 16},
                {"dezessete", 17},
                {"dezassete", 17},
                {"dezoito", 18},
                {"dezenove", 19},
                {"dezanove", 19},
                {"vinte", 20},
                {"trinta", 30},
                {"quarenta", 40},
                {"cinquenta", 50},
                {"cincoenta", 50},
                {"sessenta", 60},
                {"setenta", 70},
                {"oitenta", 80},
                {"noventa", 90},
                {"cem", 100 },
                {"duzentos", 200},
                {"trezentos", 300},
                {"quatrocentos", 400},
                {"quinhentos", 500},
                {"seiscentos", 600},
                {"setecentos", 700},
                {"oitocentos", 800},
                {"novecentos", 900},
            };

            var sufixOrdinalDictionary = new Dictionary<string, long>()
            {
                {"milesimo", 1000 },
                {"milionesimo", 1000000 },
                {"bilionesimo", 1000000000 },
                {"trilionesimo", 1000000000000 },
            };

            foreach (var sufix in sufixOrdinalDictionary)
            {
                foreach (var prefix in prefixCardinalDictionary)
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
                {"milhão", 1000000},
                {"milhao", 1000000},
                {"milhões", 1000000},
                {"milhoes", 1000000},
                {"milionésimo", 1000000},
                {"milionesimo", 1000000},
                {"bilhão", 1000000000},
                {"bilhao", 1000000000},
                {"bilhões", 1000000000},
                {"bilhoes", 1000000000},
                {"bilionésimo", 1000000000},
                {"bilionesimo", 1000000000},
                {"trilhão", 1000000000000},
                {"trilhao", 1000000000000},
                {"trilhões", 1000000000000},
                {"trilhoes", 1000000000000},
                {"trilionésimo", 1000000000000},
                {"trilionesimo", 1000000000000},
                {"dezena", 10},
                {"dezenas", 10},
                {"dúzia", 12},
                {"duzia", 12},
                {"dúzias", 12},
                {"duzias", 12},
                {"k", 1000},
                {"m", 1000000},
                {"g", 1000000000},
                {"t", 1000000000000}
            }.ToImmutableDictionary();
        }
    }
}