using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.Number.Japanese
{
    public class JapaneseNumberParserConfiguration : BaseNumberParserConfiguration, ICJKNumberParserConfiguration
    {
        public JapaneseNumberParserConfiguration()
            : this(new CultureInfo(Culture.Japanese))
        {
        }

        public JapaneseNumberParserConfiguration(CultureInfo ci)
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
            RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();
            ZeroToNineMap = NumbersDefinitions.ZeroToNineMap.ToImmutableDictionary();
            FullToHalfMap = NumbersDefinitions.FullToHalfMap.ToImmutableDictionary();
            RoundNumberMapChar = NumbersDefinitions.RoundNumberMapChar.ToImmutableDictionary();
            UnitMap = NumbersDefinitions.UnitMap.ToImmutableDictionary();
            RoundDirectList = NumbersDefinitions.RoundDirectList.ToImmutableList();

            HalfADozenRegex = null;

            // @TODO Change init to follow design in other languages
            DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            DozenRegex = new Regex(NumbersDefinitions.DozenRegex, RegexOptions.Singleline);
            PointRegex = new Regex(NumbersDefinitions.PointRegex, RegexOptions.Singleline);
            DigitNumRegex = new Regex(NumbersDefinitions.DigitNumRegex, RegexOptions.Singleline);
            DoubleAndRoundRegex = new Regex(NumbersDefinitions.DoubleAndRoundRegex, RegexOptions.Singleline);
            FracSplitRegex = new Regex(NumbersDefinitions.FracSplitRegex, RegexOptions.Singleline);
            NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
            SpeGetNumberRegex = new Regex(NumbersDefinitions.SpeGetNumberRegex, RegexOptions.Singleline);
            PercentageRegex = new Regex(NumbersDefinitions.PercentageRegex, RegexOptions.Singleline);
            PairRegex = new Regex(NumbersDefinitions.PairRegex, RegexOptions.Singleline);
            RoundNumberIntegerRegex = new Regex(NumbersDefinitions.RoundNumberIntegerRegex, RegexOptions.Singleline);
        }

        public string NonDecimalSeparatorText { get; private set; }

        public Regex DigitNumRegex { get; private set; }

        public Regex DozenRegex { get; private set; }

        public Regex PercentageRegex { get; private set; }

        public Regex DoubleAndRoundRegex { get; private set; }

        public Regex FracSplitRegex { get; private set; }

        public Regex PointRegex { get; private set; }

        public Regex SpeGetNumberRegex { get; private set; }

        public Regex PairRegex { get; private set; }

        public Regex RoundNumberIntegerRegex { get; private set; }

        public ImmutableDictionary<char, double> ZeroToNineMap { get; private set; }

        public ImmutableDictionary<char, long> RoundNumberMapChar { get; private set; }

        public ImmutableDictionary<char, char> FullToHalfMap { get; private set; }

        public ImmutableDictionary<string, string> UnitMap { get; private set; }

        public ImmutableDictionary<char, char> TratoSimMap { get; private set; }

        public ImmutableList<char> RoundDirectList { get; private set; }

        public override IEnumerable<string> NormalizeTokenSet(IEnumerable<string> tokens, ParseResult context)
        {
            return tokens;
        }

        public override long ResolveCompositeNumber(string numberStr)
        {
            return 0;
        }
    }
}