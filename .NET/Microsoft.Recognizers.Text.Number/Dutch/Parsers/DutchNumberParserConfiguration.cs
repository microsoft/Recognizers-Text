using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Dutch;

namespace Microsoft.Recognizers.Text.Number.Dutch
{
    public class DutchNumberParserConfiguration : BaseNumberParserConfiguration
    {
        public DutchNumberParserConfiguration(NumberOptions options)
            : this()
        {
            this.Options = options;
        }

        public DutchNumberParserConfiguration()
            : this(new CultureInfo(Culture.Dutch))
        {
        }

        public DutchNumberParserConfiguration(CultureInfo ci)
        {
            this.LangMarker = NumbersDefinitions.LangMarker;
            this.CultureInfo = ci;

            this.DecimalSeparatorChar = NumbersDefinitions.DecimalSeparatorChar;
            this.FractionMarkerToken = NumbersDefinitions.FractionMarkerToken;
            this.NonDecimalSeparatorChar = NumbersDefinitions.NonDecimalSeparatorChar;
            this.HalfADozenText = NumbersDefinitions.HalfADozenText;
            this.WordSeparatorToken = NumbersDefinitions.WordSeparatorToken;

            this.WrittenDecimalSeparatorTexts = NumbersDefinitions.WrittenDecimalSeparatorTexts;
            this.WrittenGroupSeparatorTexts = NumbersDefinitions.WrittenGroupSeparatorTexts;
            this.WrittenIntegerSeparatorTexts = NumbersDefinitions.WrittenIntegerSeparatorTexts;
            this.WrittenFractionSeparatorTexts = NumbersDefinitions.WrittenFractionSeparatorTexts;

            this.CardinalNumberMap = NumbersDefinitions.CardinalNumberMap.ToImmutableDictionary();
            this.OrdinalNumberMap = NumbersDefinitions.OrdinalNumberMap.ToImmutableDictionary();
            this.RelativeReferenceMap = NumbersDefinitions.RelativeReferenceMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            // @TODO Change init to follow design in other languages
            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexOptions.Singleline);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexOptions.Singleline);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexOptions.Singleline);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexOptions.Singleline);
        }

        public override ImmutableDictionary<string, long> CardinalNumberMap { get; set; }

        public override NumberOptions Options { get; set; }

        public override CultureInfo CultureInfo { get; set; }

        public override char DecimalSeparatorChar { get; set; }

        public override Regex DigitalNumberRegex { get; set; }

        public override Regex FractionPrepositionRegex { get; }

        public override Regex NegativeNumberSignRegex { get; set; }

        public override string FractionMarkerToken { get; set; }

        public override Regex HalfADozenRegex { get; set; }

        public override string HalfADozenText { get; set; }

        public override string LangMarker { get; set; }

        public override char NonDecimalSeparatorChar { get; set; }

        public string NonDecimalSeparatorText { get; private set; }

        public override ImmutableDictionary<string, long> OrdinalNumberMap { get; set; }

        public override ImmutableDictionary<string, string> RelativeReferenceMap { get; set; }

        public override ImmutableDictionary<string, long> RoundNumberMap { get; set; }

        public override string WordSeparatorToken { get; set; }

        public override IEnumerable<string> WrittenDecimalSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenGroupSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenIntegerSeparatorTexts { get; set; }

        public override IEnumerable<string> WrittenFractionSeparatorTexts { get; set; }
    }
}
