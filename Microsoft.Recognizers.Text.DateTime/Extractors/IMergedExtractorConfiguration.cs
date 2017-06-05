namespace Microsoft.Recognizers.Text.DateTime
{
    public interface IMergedExtractorConfiguration
    {
        IExtractor DateExtractor { get; }
        IExtractor TimeExtractor { get; }
        IExtractor DateTimeExtractor { get; }
        IExtractor DatePeriodExtractor { get; }
        IExtractor TimePeriodExtractor { get; }
        IExtractor DateTimePeriodExtractor { get; }
        IExtractor DurationExtractor { get; }
        IExtractor SetExtractor { get; }
        IExtractor HolidayExtractor { get; }

        bool HasBeforeTokenIndex(string text, out int index);
        bool HasAfterTokenIndex(string text, out int index);
    }
}