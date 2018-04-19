using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.Number.Chinese
{
    public class ChineseNumberParserConfiguration : INumberParserConfiguration, ICJKNumberParserConfiguration
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
            ZeroToNineMap = NumbersDefinitions.ZeroToNineMap.ToImmutableDictionary();
            RoundNumberMapChar = NumbersDefinitions.RoundNumberMapChar.ToImmutableDictionary();
            FullToHalfMap = NumbersDefinitions.FullToHalfMap.ToImmutableDictionary();
            TratoSimMap = NumbersDefinitions.TratoSimMap.ToImmutableDictionary();
            UnitMap = NumbersDefinitions.UnitMap.ToImmutableDictionary();
            RoundDirectList = NumbersDefinitions.RoundDirectList.ToImmutableList();

            HalfADozenRegex = null;
            DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DigitNumRegex = new Regex(NumbersDefinitions.DigitNumRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DozenRegex = new Regex(NumbersDefinitions.DozenRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PercentageRegex = new Regex(NumbersDefinitions.PercentageRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DoubleAndRoundRegex = new Regex(NumbersDefinitions.DoubleAndRoundRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FracSplitRegex = new Regex(NumbersDefinitions.FracSplitRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            PointRegex = new Regex(NumbersDefinitions.PointRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            SpeGetNumberRegex = new Regex(NumbersDefinitions.SpeGetNumberRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            PairRegex = new Regex(NumbersDefinitions.PairRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            FractionPrepositionRegex = null;
        }

        public NumberOptions Options { get; }

        public CultureInfo CultureInfo { get; private set; }

        public char DecimalSeparatorChar { get; private set; }

        public Regex DigitalNumberRegex { get; private set; }

        public Regex FractionPrepositionRegex { get; }

        public string FractionMarkerToken { get; private set; }

        public Regex HalfADozenRegex { get; private set; }

        public string HalfADozenText { get; private set; }

        public string LangMarker { get; private set; }

        public char NonDecimalSeparatorChar { get; private set; }

        public string NonDecimalSeparatorText { get; private set; }

        public Regex DigitNumRegex { get; private set; }

        public Regex DozenRegex { get; private set; }

        public Regex PercentageRegex { get; private set; }

        public Regex DoubleAndRoundRegex { get; private set; }

        public Regex FracSplitRegex { get; private set; }

        public Regex NegativeNumberSignRegex { get; private set; }

        public Regex PointRegex { get; private set; }

        public Regex SpeGetNumberRegex { get; private set; }

        public Regex PairRegex { get; private set; }

        public ImmutableDictionary<string, long> OrdinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> CardinalNumberMap { get; private set; }

        public ImmutableDictionary<string, long> RoundNumberMap { get; private set; }

        public ImmutableDictionary<char, double> ZeroToNineMap { get; private set; }

        public ImmutableDictionary<char, long> RoundNumberMapChar { get; private set; }

        public ImmutableDictionary<char, char> FullToHalfMap { get; private set; }

        public ImmutableDictionary<string, string> UnitMap { get; private set; }

        public ImmutableDictionary<char, char> TratoSimMap { get; private set; }

        public ImmutableList<char> RoundDirectList { get; private set; }

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