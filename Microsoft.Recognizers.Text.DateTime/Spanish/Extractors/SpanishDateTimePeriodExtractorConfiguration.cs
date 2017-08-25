using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.DateTime.Spanish
{
    public class SpanishDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public static readonly Regex NumberCombinedWithUnit =
            new Regex($@"\b(?<num>\d+(\.\d*)?)\s*{SpanishTimePeriodExtractorConfiguration.UnitRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(@"((desde|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(@"(y\s*(la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(@"(entre\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public SpanishDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
            SingleDateExtractor = new BaseDateExtractor(new SpanishDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new SpanishTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new SpanishDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new SpanishDurationExtractorConfiguration());
        }

        public IExtractor CardinalExtractor { get; }

        public IExtractor SingleDateExtractor { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IExtractor SingleDateTimeExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] 
            {
                SpanishTimePeriodExtractorConfiguration.PureNumFromTo,
                SpanishTimePeriodExtractorConfiguration.PureNumBetweenAnd
            };

        public Regex PrepositionRegex => SpanishDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => SpanishDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificTimeOfDayRegex => SpanishDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;

        public Regex TimeOfDayRegex => SpanishDateTimeExtractorConfiguration.TimeOfDayRegex;

        public Regex FollowedUnit => SpanishTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex TimeUnitRegex => SpanishTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PastPrefixRegex => SpanishDatePeriodExtractorConfiguration.PastRegex;

        public Regex NextPrefixRegex => SpanishDatePeriodExtractorConfiguration.FutureRegex;

        //TODO: add this
        public static readonly Regex WeekDayRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this
        public static readonly Regex PeriodTimeOfDayWithDateRegex = new Regex(@"\b(((y|a|en|por)\s+la|al)\s+)?(?<timeOfDay>mañana|madrugada|(pasado\s+(el\s+)?)?medio\s?d[ií]a|tarde|noche|anoche)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this according to English
        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(
                $@"({SpanishDatePeriodExtractorConfiguration.PastRegex}|{
                    SpanishDatePeriodExtractorConfiguration.FutureRegex})\s+{
                    SpanishTimePeriodExtractorConfiguration.UnitRegex
                    }",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => NumberCombinedWithUnit;

        Regex IDateTimePeriodExtractorConfiguration.WeekDayRegex => WeekDayRegex;

        Regex IDateTimePeriodExtractorConfiguration.PeriodTimeOfDayWithDateRegex => PeriodTimeOfDayWithDateRegex;

        Regex IDateTimePeriodExtractorConfiguration.RelativeTimeUnitRegex => RelativeTimeUnitRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            var fromMatch = FromRegex.Match(text);
            if (fromMatch.Success)
            {
                index = fromMatch.Index;
            }
            return fromMatch.Success;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var beforeMatch = BeforeRegex.Match(text);
            if (beforeMatch.Success)
            {
                index = beforeMatch.Index;
            }
            return beforeMatch.Success;
        }

        public bool HasConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }
    }
}
