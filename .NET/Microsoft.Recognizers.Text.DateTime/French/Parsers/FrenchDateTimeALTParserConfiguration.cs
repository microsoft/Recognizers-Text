namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimeALTParserConfiguration : IDateTimeALTParserConfiguration
    {
        public IDateTimeParser DateTimeParser { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public FrenchDateTimeALTParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
        }

    }
}
