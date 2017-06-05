namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishMergedExtractorConfiguration : IMergedExtractorConfiguration
    {
        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IExtractor SetExtractor { get; }

        public IExtractor HolidayExtractor { get; }

        public EnglishMergedExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new EnglishDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new EnglishTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new EnglishDateTimePeriodExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new EnglishDurationExtractorConfiguration());
            SetExtractor = new BaseSetExtractor(new EnglishSetExtractorConfiguration());
            HolidayExtractor = new BaseHolidayExtractor(new EnglishHolidayExtractorConfiguration());
        }

        public bool HasAfterTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("after"))
            {
                index = text.LastIndexOf("after");
                return true;
            }
            return false;
        }

        public bool HasBeforeTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("before"))
            {
                index = text.LastIndexOf("before");
                return true;
            }
            return false;
        }
    }
}
