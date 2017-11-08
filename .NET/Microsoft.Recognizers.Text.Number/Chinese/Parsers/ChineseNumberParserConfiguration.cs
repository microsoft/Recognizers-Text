using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberParserConfiguration : INumberParserConfiguration
    {
        public ChineseNumberParserConfiguration() : this(new CultureInfo(Culture.Chinese))
        {
        }

        public ChineseNumberParserConfiguration(CultureInfo ci)
        {
            LangMarker = NumbersDefinitions.LangMarker;
            CultureInfo = ci;

            DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            FractionMarkerToken = NumbersDefinitions.FractionMarkerToken;
            NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            HalfADozenText = NumbersDefinitions.HalfADozenText;
            WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;

            WrittenDecimalSeparatorTexts = Enumerable.Empty<string>();
            WrittenGroupSeparatorTexts = Enumerable.Empty<string>();
            WrittenIntegerSeparatorTexts = Enumerable.Empty<string>();
            WrittenFractionSeparatorTexts = Enumerable.Empty<string>();

            CardinalNumberMap = new Dictionary<string, long>().ToImmutableDictionary();
            OrdinalNumberMap = new Dictionary<string, long>().ToImmutableDictionary();
            RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            ZeroToNineMapChs = NumbersDefinitions.ZeroToNineMapChs.ToImmutableDictionary();
            RoundNumberMapChs = NumbersDefinitions.RoundNumberMapChs.ToImmutableDictionary();
            FullToHalfMapChs = NumbersDefinitions.FullToHalfMapChs.ToImmutableDictionary();
            TratoSimMapChs = NumbersDefinitions.TratoSimMapChs.ToImmutableDictionary();
            UnitMapChs = NumbersDefinitions.UnitMapChs.ToImmutableDictionary();
            RoundDirectListChs = NumbersDefinitions.RoundDirectListChs.ToImmutableList();

            HalfADozenRegex = null;
            DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DigitNumRegex = new Regex(NumbersDefinitions.DigitNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DozenRegex = new Regex(NumbersDefinitions.DozenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PercentageRegex = new Regex(NumbersDefinitions.PercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DoubleAndRoundChsRegex = new Regex(NumbersDefinitions.DoubleAndRoundChsRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FracSplitRegex = new Regex(NumbersDefinitions.FracSplitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SymbolRegex = new Regex(NumbersDefinitions.SymbolRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PointRegexChs = new Regex(NumbersDefinitions.PointRegexChs, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SpeGetNumberRegex = new Regex(NumbersDefinitions.SpeGetNumberRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            PairRegex = new Regex(NumbersDefinitions.PairRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
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

        public IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public long ResolveCompositeNumber(string numberStr)
        {
            return 0;
        }
    }
}