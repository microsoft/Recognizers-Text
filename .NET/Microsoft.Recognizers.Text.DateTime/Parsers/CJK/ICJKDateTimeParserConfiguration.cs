using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ICJKDateTimeParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DateExtractor { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeParser TimeParser { get; }

        IExtractor IntegerExtractor { get; }

        IParser NumberParser { get; }

        IDateTimeExtractor DurationExtractor { get; }

        Regex NowRegex { get; }

        Regex LunarRegex { get; }

        Regex LunarHolidayRegex { get; }

        Regex SimplePmRegex { get; }

        Regex SimpleAmRegex { get; }

        Regex TimeOfTodayRegex { get; }

        Regex DateTimePeriodUnitRegex { get; }

        Regex BeforeRegex { get; }

        Regex AfterRegex { get; }

        ImmutableDictionary<string, string> UnitMap { get; }

        bool GetMatchedNowTimex(string text, out string timex);

        int GetSwiftDay(string text);

        void AdjustByTimeOfDay(string matchStr, ref int hour, ref int swift);
    }
}
