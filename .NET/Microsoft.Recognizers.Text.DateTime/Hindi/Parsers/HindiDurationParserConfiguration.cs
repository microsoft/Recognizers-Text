using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Hindi;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Hindi;

namespace Microsoft.Recognizers.Text.DateTime.Hindi
{
    public class HindiDurationParserConfiguration : BaseDateTimeOptionsConfiguration, IDurationParserConfiguration
    {
        public HindiDurationParserConfiguration(ICommonDateTimeParserConfiguration config)
           : base(config)
        {
            CardinalExtractor = Number.Hindi.CardinalExtractor.GetInstance();
            NumberParser = new BaseIndianNumberParser(new HindiNumberParserConfiguration(new BaseNumberOptionsConfiguration(config.Culture)));

            DurationExtractor = new BaseDurationExtractor(new HindiDurationExtractorConfiguration(this), false);

            NumberCombinedWithUnit = HindiDurationExtractorConfiguration.NumberCombinedWithDurationUnit;

            AnUnitRegex = HindiDurationExtractorConfiguration.AnUnitRegex;
            DuringRegex = HindiDurationExtractorConfiguration.DuringRegex;
            AllDateUnitRegex = HindiDurationExtractorConfiguration.AllRegex;
            HalfDateUnitRegex = HindiDurationExtractorConfiguration.HalfRegex;
            SuffixAndRegex = HindiDurationExtractorConfiguration.SuffixAndRegex;
            FollowedUnit = HindiDurationExtractorConfiguration.DurationFollowedUnit;
            ConjunctionRegex = HindiDurationExtractorConfiguration.ConjunctionRegex;
            InexactNumberRegex = HindiDurationExtractorConfiguration.InexactNumberRegex;
            InexactNumberUnitRegex = HindiDurationExtractorConfiguration.InexactNumberUnitRegex;
            DurationUnitRegex = HindiDurationExtractorConfiguration.DurationUnitRegex;

            UnitMap = config.UnitMap;
            UnitValueMap = config.UnitValueMap;
            DoubleNumbers = config.DoubleNumbers;
        }

        public IExtractor CardinalExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IParser NumberParser { get; }

        public Regex NumberCombinedWithUnit { get; }

        public Regex AnUnitRegex { get; }

        public Regex DuringRegex { get; }

        public Regex AllDateUnitRegex { get; }

        public Regex HalfDateUnitRegex { get; }

        public Regex SuffixAndRegex { get; }

        public Regex FollowedUnit { get; }

        public Regex ConjunctionRegex { get; }

        public Regex InexactNumberRegex { get; }

        public Regex InexactNumberUnitRegex { get; }

        public Regex DurationUnitRegex { get; }

        public Regex SpecialNumberUnitRegex { get; }

        bool IDurationParserConfiguration.CheckBothBeforeAfter => true;

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IImmutableDictionary<string, long> UnitValueMap { get; }

        public IImmutableDictionary<string, double> DoubleNumbers { get; }
    }
}
