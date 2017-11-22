namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimeALTParserConfiguration : IDateTimeALTParserConfiguration
    {
        public IDateTimeParser DateTimeParser { get; }

        public EnglishDateTimeALTParserConfiguration(ICommonDateTimeParserConfiguration config)
        {
            DateTimeParser = config.DateTimeParser;
        }

    }
}
