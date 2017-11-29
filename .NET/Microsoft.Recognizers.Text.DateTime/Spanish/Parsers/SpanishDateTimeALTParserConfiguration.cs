namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeALTParserConfiguration : IDateTimeALTParserConfiguration
    {
        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public SpanishDateTimeALTParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            DateTimePeriodParser = config.DateTimePeriodParser;
        }

    }
}
