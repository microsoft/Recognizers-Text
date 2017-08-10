﻿using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishMergedExtractorConfiguration : IMergedExtractorConfiguration
    {
        private static readonly Regex BeforeRegex = new Regex(@"(antes(\s+de(\s+las?)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex AfterRegex = new Regex(@"(despues(\s*de(\s+las?)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public IExtractor DateExtractor { get; }

        public IExtractor TimeExtractor { get; }

        public IExtractor DateTimeExtractor { get; }

        public IExtractor DatePeriodExtractor { get; }

        public IExtractor TimePeriodExtractor { get; }

        public IExtractor DateTimePeriodExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IExtractor GetExtractor { get; }

        public IExtractor HolidayExtractor { get; }

        public SpanishMergedExtractorConfiguration()
        {
            DateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            TimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            DateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            DatePeriodExtractor = new BaseDatePeriodExtractor(new SpanishDatePeriodExtractorConfiguration());
            TimePeriodExtractor = new BaseTimePeriodExtractor(new SpanishTimePeriodExtractorConfiguration());
            DateTimePeriodExtractor = new BaseDateTimePeriodExtractor(new SpanishDateTimePeriodExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
            GetExtractor = new BaseSetExtractor(new SpanishSetExtractorConfiguration());
            HolidayExtractor = new BaseHolidayExtractor(new SpanishHolidayExtractorConfiguration());
        }

        public bool HasBeforeTokenIndex(string text, out int index)
        {
            index = -1;
            var match = BeforeRegex.Match(text);
            if (match.Success)
            {
                index = match.Index;
            }
            return match.Success;
        }

        public bool HasAfterTokenIndex(string text, out int index)
        {
            index = -1;
            var match = AfterRegex.Match(text);
            if (match.Success)
            {
                index = match.Index;
            }
            return match.Success;
        }
    }
}
