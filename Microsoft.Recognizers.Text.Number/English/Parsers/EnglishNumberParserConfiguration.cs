using Microsoft.Recognizers.Text.Number.Parsers;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.English.Parsers
{
    public class EnglishNumberParserConfiguration : INumberParserConfiguration
    {
        public EnglishNumberParserConfiguration() : this(new CultureInfo(Culture.English)) { }

        public EnglishNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = "Eng";
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = '.';
            this.FractionMarkerToken = "over";
            this.NonDecimalSeparatorChar = ',';
            this.HalfADozenText = "six";
            this.WordSeparatorToken = "and";

            this.WrittenDecimalSeparatorTexts = new List<string> { "point" };
            this.WrittenGroupSeparatorTexts = new List<string> { "punto" };
            this.WrittenIntegerSeparatorTexts = new List<string> { "and" };
            this.WrittenFractionSeparatorTexts = new List<string> { "and" };

            this.CardinalNumberMap = InitCardinalNumberMap();
            this.OrdinalNumberMap = InitOrdinalNumberMap();
            this.RoundNumberMap = InitRoundNumberMap();
            this.HalfADozenRegex = new Regex(@"half\s+a\s+dozen", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(
                    @"((?<=\b)(hundred|thousand|million|billion|trillion|dozen(s)?)(?=\b))|((?<=(\d|\b))(k|t|m|g|b)(?=\b))",
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
            var fracWords = new List<string>();
            var tokenList = tokens.ToList();
            var tokenLen = tokenList.Count;
            for (var i = 0; i < tokenLen; i++)
            {
                if ((i < tokenLen - 2) && tokenList[i + 1] == "-")
                {
                    fracWords.Add(tokenList[i] + tokenList[i + 1] + tokenList[i + 2]);
                    i += 2;
                }
                else
                {
                    fracWords.Add(tokenList[i]);
                }
            }
            return fracWords;
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            if (numberStr.Contains("-"))
            {
                var numbers = numberStr.Split('-');
                long ret = 0;
                foreach (var number in numbers)
                {
                    if (OrdinalNumberMap.ContainsKey(number))
                    {
                        ret += OrdinalNumberMap[number];
                    }
                    else if (CardinalNumberMap.ContainsKey(number))
                    {
                        ret += CardinalNumberMap[number];
                    }
                }
                return ret;
            }
            if (this.OrdinalNumberMap.ContainsKey(numberStr))
            {
                return this.OrdinalNumberMap[numberStr];
            }

            if (this.CardinalNumberMap.ContainsKey(numberStr))
            {
                return this.CardinalNumberMap[numberStr];
            }
            return 0;
        }

        private static ImmutableDictionary<string, long> InitCardinalNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"a", 1},
                {"zero", 0},
                {"an", 1},
                {"one", 1},
                {"two", 2},
                {"three", 3},
                {"four", 4},
                {"five", 5},
                {"six", 6},
                {"seven", 7},
                {"eight", 8},
                {"nine", 9},
                {"ten", 10},
                {"eleven", 11},
                {"twelve", 12},
                {"dozen", 12},
                {"dozens", 12},
                {"thirteen", 13},
                {"fourteen", 14},
                {"fifteen", 15},
                {"sixteen", 16},
                {"seventeen", 17},
                {"eighteen", 18},
                {"nineteen", 19},
                {"twenty", 20},
                {"thirty", 30},
                {"forty", 40},
                {"fifty", 50},
                {"sixty", 60},
                {"seventy", 70},
                {"eighty", 80},
                {"ninety", 90},
                {"hundred", 100},
                {"thousand", 1000},
                {"million", 1000000},
                {"billion", 1000000000},
                {"trillion", 1000000000000}
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitOrdinalNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"first", 1},
                {"second", 2},
                {"secondary", 2},
                {"half", 2},
                {"third", 3},
                {"fourth", 4},
                {"quarter", 4},
                {"fifth", 5},
                {"sixth", 6},
                {"seventh", 7},
                {"eighth", 8},
                {"ninth", 9},
                {"tenth", 10},
                {"eleventh", 11},
                {"twelfth", 12},
                {"thirteenth", 13},
                {"fourteenth", 14},
                {"fifteenth", 15},
                {"sixteenth", 16},
                {"seventeenth", 17},
                {"eighteenth", 18},
                {"nineteenth", 19},
                {"twentieth", 20},
                {"thirtieth", 30},
                {"fortieth", 40},
                {"fiftieth", 50},
                {"sixtieth", 60},
                {"seventieth", 70},
                {"eightieth", 80},
                {"ninetieth", 90},
                {"hundredth", 100},
                {"thousandth", 1000},
                {"millionth", 1000000},
                {"billionth", 1000000000},
                {"trillionth", 1000000000000},
                {"firsts", 1},
                {"halves", 2},
                {"thirds", 3},
                {"fourths", 4},
                {"quarters", 4},
                {"fifths", 5},
                {"sixths", 6},
                {"sevenths", 7},
                {"eighths", 8},
                {"ninths", 9},
                {"tenths", 10},
                {"elevenths", 11},
                {"twelfths", 12},
                {"thirteenths", 13},
                {"fourteenths", 14},
                {"fifteenths", 15},
                {"sixteenths", 16},
                {"seventeenths", 17},
                {"eighteenths", 18},
                {"nineteenths", 19},
                {"twentieths", 20},
                {"thirtieths", 30},
                {"fortieths", 40},
                {"fiftieths", 50},
                {"sixtieths", 60},
                {"seventieths", 70},
                {"eightieths", 80},
                {"ninetieths", 90},
                {"hundredths", 100},
                {"thousandths", 1000},
                {"millionths", 1000000},
                {"billionths", 1000000000},
                {"trillionths", 1000000000000}
            }.ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitRoundNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"hundred", 100},
                {"thousand", 1000},
                {"million", 1000000},
                {"billion", 1000000000},
                {"trillion", 1000000000000},
                {"hundredth", 100},
                {"thousandth", 1000},
                {"millionth", 1000000},
                {"billionth", 1000000000},
                {"trillionth", 1000000000000},
                {"hundredths", 100},
                {"thousandths", 1000},
                {"millionths", 1000000},
                {"billionths", 1000000000},
                {"trillionths", 1000000000000},
                {"dozen", 12},
                {"dozens", 12},
                {"k", 1000},
                {"m", 1000000},
                {"g", 1000000000},
                {"b", 1000000000},
                {"t", 1000000000000}
            }.ToImmutableDictionary();
        }
    }
}
