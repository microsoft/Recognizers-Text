// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime
{
    public interface ISetParserConfiguration : IDateTimeOptionsConfiguration
    {
        IDateTimeExtractor DurationExtractor { get; }

        IDateTimeParser DurationParser { get; }

        IDateTimeExtractor TimeExtractor { get; }

        IDateTimeParser TimeParser { get; }

        IDateExtractor DateExtractor { get; }

        IDateTimeParser DateParser { get; }

        IDateTimeExtractor DateTimeExtractor { get; }

        IDateTimeParser DateTimeParser { get; }

        IDateTimeExtractor DatePeriodExtractor { get; }

        IDateTimeParser DatePeriodParser { get; }

        IDateTimeExtractor TimePeriodExtractor { get; }

        IDateTimeParser TimePeriodParser { get; }

        IDateTimeExtractor DateTimePeriodExtractor { get; }

        IDateTimeParser DateTimePeriodParser { get; }

        IImmutableDictionary<string, string> UnitMap { get; }

        Regex EachPrefixRegex { get; }

        Regex PeriodicRegex { get; }

        Regex EachUnitRegex { get; }

        Regex EachDayRegex { get; }

        Regex SetWeekDayRegex { get; }

        Regex SetEachRegex { get; }

        bool GetMatchedDailyTimex(string text, out string timex);

        bool GetMatchedUnitTimex(string text, out string timex);

        string WeekDayGroupMatchString(Match match);
    }
}
