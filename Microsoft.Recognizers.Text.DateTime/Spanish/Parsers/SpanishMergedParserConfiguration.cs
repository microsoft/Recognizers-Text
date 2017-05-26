using Microsoft.Recognizers.Text.DateTime.Parsers;
using System.Text.RegularExpressions;

namespace Microsoft.Recognizers.Text.DateTime.Spanish.Parsers
{
    public class SpanishMergedParserConfiguration : SpanishCommonDateTimeParserConfiguration, IMergedParserConfiguration
    {
        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public IDateTimeParser DatePeriodParser { get; }

        public IDateTimeParser TimePeriodParser { get; }

        public IDateTimeParser DateTimePeriodParser { get; }

        public IDateTimeParser SetParser { get; }

        public IDateTimeParser HolidayParser { get; }

        public SpanishMergedParserConfiguration() : base()
        {
            BeforeRegex = new Regex(@"^\s*antes\s+",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
            AfterRegex = new Regex(@"^\s*despues\s+",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
            DatePeriodParser = new BaseDatePeriodParser(new SpanishDatePeriodParserConfiguration(this));
            TimePeriodParser = new BaseTimePeriodParser(new SpanishTimePeriodParserConfiguration(this));
            DateTimePeriodParser = new DateTimePeriodParser(new SpanishDateTimePeriodParserConfiguration(this));
            SetParser = new BaseSetParser(new SpanishSetParserConfiguration(this));
            HolidayParser = new BaseHolidayParser(new SpanishHolidayParserConfiguration());
        }
    }
}
