using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberParserConfiguration : INumberParserConfiguration
    {
        public ChineseNumberParserConfiguration()
            : this(new CultureInfo(Culture.Chinese))
        {
        }

        public ChineseNumberParserConfiguration(CultureInfo ci)
        {
            LangMarker = "Chs";
            CultureInfo = ci;

            DecimalSeparatorChar = '.';
            FractionMarkerToken = "";
            NonDecimalSeparatorChar = ' ';
            HalfADozenText = "";
            WordSeparatorToken = "";

            WrittenDecimalSeparatorTexts = Enumerable.Empty<string>();
            WrittenGroupSeparatorTexts = Enumerable.Empty<string>();
            WrittenIntegerSeparatorTexts = Enumerable.Empty<string>();
            WrittenFractionSeparatorTexts = Enumerable.Empty<string>();

            CardinalNumberMap = InitCardinalNumberMap();
            OrdinalNumberMap = InitOrdinalNumberMap();
            RoundNumberMap = InitRoundNumberMap();
            ZeroToNineMapChs = InitZeroToNineMapChs();
            RoundNumberMapChs = InitRoundNumberMapChs();
            FullToHalfMapChs = InitFullToHalfMapChs();
            TratoSimMapChs = InitTratoSimMapChs();
            UnitMapChs = InitUnitMapChs();
            RoundDirectListChs = InitRoundDirectListChs();

            HalfADozenRegex = null;
            DigitalNumberRegex = new Regex(
                @"((?<=(\d|\b))(k|t|m|g)(?=\b))",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DigitNumRegex = new Regex($@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DozenRegex = new Regex(@".*打$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PercentageRegex = new Regex(@"(?<=百\s*分\s*之).+|.+(?=个\s*百\s*分\s*点)|.*(?=[％%])",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DoubleAndRoundChsRegex =
                new Regex(
                    $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}+(\.{IntegerExtractor
                        .ZeroToNineChsFullHalfRegexChs}+)?\s*[多几余]?[万亿萬億]"
                    + @"{1,2}",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FracSplitRegex = new Regex(@"又|分\s*之",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SymbolRegex =
                new Regex($@"^{IntegerExtractor.SignSymbolRegexChs}.*|^{IntegerExtractor.SignSymbolRegexNum}.*",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PointRegexChs = new Regex(DoubleExtractor.PointRegexChsStr,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SpeGetNumberRegex = new Regex(
                $@"{IntegerExtractor.ZeroToNineChsFullHalfRegexChs}|{IntegerExtractor
                    .ZeroToNineIntegerRegexChs}|[十拾半对對]",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);
            PairRegex = new Regex(@".*[双对雙對]$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }


        public CultureInfo CultureInfo { get; private set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public string NonDecimalSeparatorText { get; private set; }

        public Regex DigitNumRegex { get; private set; }

        public Regex DozenRegex { get; private set; }

        public Regex PercentageRegex { get; private set; }

        public Regex DoubleAndRoundChsRegex { get; private set; }

        public Regex FracSplitRegex { get; private set; }

        public Regex SymbolRegex { get; private set; }

        public Regex PointRegexChs { get; private set; }

        public Regex SpeGetNumberRegex { get; private set; }

        public Regex PairRegex { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; private set; }

        public ImmutableDictionary<char, double> ZeroToNineMapChs { get; private set; }

        public ImmutableDictionary<char, long> RoundNumberMapChs { get; private set; }

        public ImmutableDictionary<char, char> FullToHalfMapChs { get; private set; }

        public ImmutableDictionary<string, string> UnitMapChs { get; private set; }

        public ImmutableDictionary<char, char> TratoSimMapChs { get; private set; }

        public ImmutableList<char> RoundDirectListChs { get; private set; }

        public string WordSeparatorToken { get; private set; }

        public IEnumerable<string> WrittenDecimalSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenGroupSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenIntegerSeparatorTexts { get; private set; }

        public IEnumerable<string> WrittenFractionSeparatorTexts { get; private set; }

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, object context)
        {
            return tokens;
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            return 0;
        }

        private static ImmutableDictionary<string, long> InitCardinalNumberMap()
        {
            return new Dictionary<string, long>().ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitOrdinalNumberMap()
        {
            return new Dictionary<string, long>().ToImmutableDictionary();
        }

        private static ImmutableDictionary<string, long> InitRoundNumberMap()
        {
            return new Dictionary<string, long>
            {
                {"k", 1000},
                {"m", 1000000},
                {"g", 1000000000},
                {"t", 1000000000000}
            }.ToImmutableDictionary();
        }

        private ImmutableDictionary<char, double> InitZeroToNineMapChs()
        {
            return new Dictionary<char, double>
            {
                {'零', 0},
                {'一', 1},
                {'二', 2},
                {'三', 3},
                {'四', 4},
                {'五', 5},
                {'六', 6},
                {'七', 7},
                {'八', 8},
                {'九', 9},
                {'〇', 0},
                {'壹', 1},
                {'贰', 2},
                {'貳', 2},
                {'叁', 3},
                {'肆', 4},
                {'伍', 5},
                {'陆', 6},
                {'陸', 6},
                {'柒', 7},
                {'捌', 8},
                {'玖', 9},
                {'０', 0},
                {'１', 1},
                {'２', 2},
                {'３', 3},
                {'４', 4},
                {'５', 5},
                {'６', 6},
                {'７', 7},
                {'８', 8},
                {'９', 9},
                {'0', 0},
                {'1', 1},
                {'2', 2},
                {'3', 3},
                {'4', 4},
                {'5', 5},
                {'6', 6},
                {'7', 7},
                {'8', 8},
                {'9', 9},
                {'半', 0.5},
                {'两', 2},
                {'兩', 2},
                {'俩', 2},
                {'倆', 2},
                {'仨', 3}
            }.ToImmutableDictionary();
        }

        private ImmutableDictionary<char, long> InitRoundNumberMapChs()
        {
            return new Dictionary<char, long>
            {
                {'十', 10},
                {'百', 100},
                {'千', 1000},
                {'万', 10000},
                {'亿', 100000000},
                {'兆', 1000000000000},
                {'拾', 10},
                {'佰', 100},
                {'仟', 1000},
                {'萬', 10000},
                {'億', 100000000}
            }.ToImmutableDictionary();
        }

        private ImmutableDictionary<char, char> InitFullToHalfMapChs()
        {
            return new Dictionary<char, char>()
            {
                {'０', '0'},
                {'１', '1'},
                {'２', '2'},
                {'３', '3'},
                {'４', '4'},
                {'５', '5'},
                {'６', '6'},
                {'７', '7'},
                {'８', '8'},
                {'９', '9'},
                {'／', '/'},
                {'－', '-'},
                {'，', ','},
                {'Ｇ', 'G'},
                {'Ｍ', 'M'},
                {'Ｔ', 'T'},
                {'Ｋ', 'K'},
                {'ｋ', 'k'},
                {'．', '.'}
            }.ToImmutableDictionary();
        }

        private ImmutableDictionary<string, string> InitUnitMapChs()
        {
            return new Dictionary<string, string>()
            {
                {"萬萬", "億"},
                {"億萬", "兆"},
                {"萬億", "兆"},
                {"万万", "亿"},
                {"万亿", "兆"},
                {"亿万", "兆"},
                {" ", ""},
                {"多", ""},
                {"余", ""},
                {"几", ""}
            }.ToImmutableDictionary();
        }

        private ImmutableList<char> InitRoundDirectListChs()
        {
            return new List<char>
            {
                '万',
                '萬',
                '亿',
                '兆',
                '億'
            }.ToImmutableList();
        }

        private ImmutableDictionary<char, char> InitTratoSimMapChs()
        {
            return new Dictionary<char, char>()
            {
                {'佰', '百'},
                {'點', '点'},
                {'個', '个'},
                {'幾', '几'},
                {'對', '对'},
                {'雙', '双'}
            }.ToImmutableDictionary();
        }
    }
}