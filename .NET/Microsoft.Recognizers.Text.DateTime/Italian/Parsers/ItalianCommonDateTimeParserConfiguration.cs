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
            DateExtractor = new BaseDateExtractor(new ItalianDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new ItalianTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new ItalianDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new ItalianDurationExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new ItalianDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new ItalianTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new ItalianDateTimePeriodExtractorConfiguration());
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
