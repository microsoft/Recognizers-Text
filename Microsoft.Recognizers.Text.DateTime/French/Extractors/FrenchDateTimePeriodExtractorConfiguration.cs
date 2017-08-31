using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.French;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchDateTimePeriodExtractorConfiguration : IDatePeriodExtractorConfiguration
    {
        public static readonly Regex NumberCombinedWithUnit =
           new Regex($@"\b(?<num>\d+(\.\d*)?)\s*{FrenchTimePeriodExtractorConfiguration.UnitRegex}", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex FromRegex = new Regex(@"((depuis|de)(\s*la(s)?)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex ConnectorAndRegex = new Regex(@"(y\s*(et\s)?)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex BeforeRegex = new Regex(@"(avant\s*(la(s)?)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public FrenchDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = Number.French.CardinalExtractor.GetInstance();
            SingleDateExtractor = new BaseDateExtractor(new FrenchDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new FrenchDateTimeExtractorConfiguration());
            DurationExtractor = new BaseDurationExtractor(new FrenchDurationExtractorConfiguration());
        }

        public IExtractor CardinalExtractor { get; }

        public IExtractor SingleDateExtractor { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IExtractor SingleDateTimeExtractor { get; }

        public IExtractor DurationExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[]
        {
                FrenchTimePeriodExtractorConfiguration.PureNumFromTo,
                FrenchTimePeriodExtractorConfiguration.PureNumBetweenAnd
        };

        public Regex PrepositionRegex => FrenchDateTimeExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => FrenchDatePeriodExtractorConfiguration.TillRegex;

        public Regex SpecificTimeOfDayRegex => FrenchDateTimeExtractorConfiguration.SpecificTimeOfDayRegex;

        public Regex TimeOfDayRegex => FrenchDateTimeExtractorConfiguration.TimeOfDayRegex;

        public Regex FollowedUnit => FrenchTimePeriodExtractorConfiguration.FollowedUnit;

        public Regex TimeUnitRegex => FrenchTimePeriodExtractorConfiguration.UnitRegex;

        public Regex PastPrefixRegex => FrenchDatePeriodExtractorConfiguration.PastRegex;

        public Regex NextPrefixRegex => FrenchDatePeriodExtractorConfiguration.FutureRegex;

        //TODO: add this
        public static readonly Regex WeekDayRegex = new Regex(@"^[\.]", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this
        public static readonly Regex PeriodTimeOfDayWithDateRegex = new Regex(@"\b(((y|a|en|por)\s+la|al)\s+)?(?<timeOfDay>mañana|madrugada|(pasado\s+(el\s+)?)?medio\s?d[ií]a|tarde|noche|anoche)\b", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        //TODO: add this according to English
        public static readonly Regex RelativeTimeUnitRegex =
            new Regex(
                $@"({FrenchDatePeriodExtractorConfiguration.PastRegex}|{
                    FrenchDatePeriodExtractorConfiguration.FutureRegex})\s+{
                    FrenchTimePeriodExtractorConfiguration.UnitRegex
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
