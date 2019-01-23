namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IDateTimeAltParserConfiguration
    {
        IDateTimeParser DateTimeParser { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IDateTimeParser DateTimePeriodParser { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeParser DatePeriodParser { get; }
    }
}
