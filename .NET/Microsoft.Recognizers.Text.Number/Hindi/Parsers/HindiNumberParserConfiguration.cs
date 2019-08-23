using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;

namespace Microsoft.Recognizers.Text.Number.Hindi
{
    public class HindiNumberParserConfiguration : BaseNumberParserConfiguration
    {
        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public HindiNumberParserConfiguration(INumberOptionsConfiguration config)
        {

            this.Config = config;
            this.LangMarker = NumbersDefinitions.LangMarker;

            // @TODO Temporary workaround
            var culture = config.Culture;
            if (culture.IndexOf("*", StringComparison.Ordinal) != -1)
            {
                culture = config.Culture.Replace("*", "in");
            }

            this.CultureInfo = new CultureInfo(culture);

            this.IsCompoundNumberLanguage = NumbersDefinitions.CompoundNumberLanguage;
            this.IsMultiDecimalSeparatorCulture = NumbersDefinitions.MultiDecimalSeparatorCulture;

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
            this.RelativeReferenceOffsetMap = NumbersDefinitions.RelativeReferenceOffsetMap.ToImmutableDictionary();
            this.RelativeReferenceRelativeToMap = NumbersDefinitions.RelativeReferenceRelativeToMap.ToImmutableDictionary();
            this.RoundNumberMap = NumbersDefinitions.RoundNumberMap.ToImmutableDictionary();

            this.HalfADozenRegex = new Regex(NumbersDefinitions.HalfADozenRegex, RegexFlags);
            this.DigitalNumberRegex = new Regex(NumbersDefinitions.DigitalNumberRegex, RegexFlags);
            this.NegativeNumberSignRegex = new Regex(NumbersDefinitions.NegativeNumberSignRegex, RegexFlags);
            this.FractionPrepositionRegex = new Regex(NumbersDefinitions.FractionPrepositionRegex, RegexFlags);
        }

        public string NonDecimalSeparatorText { get; private set; }
    }
}
