using System.Collections.Immutable;

using Microsoft.Recognizers.Text.DateTime.Italian.Utilities;
using Microsoft.Recognizers.Definitions.Italian;
using Microsoft.Recognizers.Text.Number.Italian;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Italian
{
    public class ItalianCommonDateTimeParserConfiguration : BaseDateParserConfiguration
    {
        public ItalianCommonDateTimeParserConfiguration(IOptionsConfiguration options) : base(options)
        {
            UtilityConfiguration = new ItalianDatetimeUtilityConfiguration();

            UnitMap = DateTimeDefinitions.UnitMap.ToImmutableDictionary();
            UnitValueMap = DateTimeDefinitions.UnitValueMap.ToImmutableDictionary();
            SeasonMap = DateTimeDefinitions.SeasonMap.ToImmutableDictionary();
            CardinalMap = DateTimeDefinitions.CardinalMap.ToImmutableDictionary();
            DayOfWeek = DateTimeDefinitions.DayOfWeek.ToImmutableDictionary();
            MonthOfYear = DateTimeDefinitions.MonthOfYear.ToImmutableDictionary();
            Numbers = DateTimeDefinitions.Numbers.ToImmutableDictionary();
            DoubleNumbers = DateTimeDefinitions.DoubleNumbers.ToImmutableDictionary();
            WrittenDecades = DateTimeDefinitions.WrittenDecades.ToImmutableDictionary();
            SpecialDecadeCases = DateTimeDefinitions.SpecialDecadeCases.ToImmutableDictionary();

            CardinalExtractor = new CardinalExtractor();
            IntegerExtractor = new IntegerExtractor();
            OrdinalExtractor = new OrdinalExtractor();

            NumberParser = new BaseNumberParser(new ItalianNumberParserConfiguration());
            DateExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration(this));
            TimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration(this));
            DateTimeExtractor = new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ItalianDatePeriodExtractorConfiguration(this));
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration(this));
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ItalianDateTimePeriodExtractorConfiguration(this));
            DateParser = new BaseDateParser(new ItalianDateParserConfiguration(this));
            TimeParser = new BaseTimeParser(new ItalianTimeParserConfiguration(this));
            DateTimeParser = new BaseDateTimeParser(new ItalianDateTimeParserConfiguration(this));
            DurationParser = new BaseDurationParser(new ItalianDurationParserConfiguration(this));
            DatePeriodParser = new BaseDatePeriodParser(new ItalianDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new ItalianTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new BaseDateTimePeriodParser(new ItalianDateTimePeriodParserConfiguration(this));
            DateTimeAltParser = new BaseDateTimeAltParser(new ItalianDateTimeAltParserConfiguration(this));
        }
    }
}
