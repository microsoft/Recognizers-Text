namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimeALTParserConfiguration : IDateTimeALTParserConfiguration
    {
        public IDateTimeParser DateTimeParser { get; }

        public SpanishDateTimeALTParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
        }

    }
}
