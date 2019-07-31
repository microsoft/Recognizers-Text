namespace Microsoft.Recognizers.Text.DateTime.Turkish
{
    public class TurkishDateTimeAltParserConfiguration : IDateTimeAltParserConfiguration
    {
        public TurkishDateTimeAltParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimePeriodParser = config.DateTimePeriodParser;
            TimePeriodParser = config.TimePeriodParser;
            DatePeriodParser = config.DatePeriodParser;
        }

        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DatePeriodParser { get; }
    }
}
