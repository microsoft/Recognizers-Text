namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeALTParserConfiguration : IDateTimeALTParserConfiguration
    {
        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public SpanishDateTimeALTParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
        }

    }
}
