using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Number.English;
using Microsoft.Recognizers.Resources.English;

namespace Microsoft.Recognizers.Text.DateTime.English
{
    public class EnglishDateTimePeriodExtractorConfiguration : IDateTimePeriodExtractorConfiguration
    {
        public EnglishDateTimePeriodExtractorConfiguration()
        {
            CardinalExtractor = new CardinalExtractor();
            SingleDateExtractor = new BaseDateExtractor(new EnglishDateExtractorConfiguration());
            SingleTimeExtractor = new BaseTimeExtractor(new EnglishTimeExtractorConfiguration());
            SingleDateTimeExtractor = new BaseDateTimeExtractor(new EnglishDateTimeExtractorConfiguration());
        }
        
        private static readonly Regex[] SimpleCases = new Regex[]
        {
            EnglishTimePeriodExtractorConfiguration.PureNumFromTo,
            EnglishTimePeriodExtractorConfiguration.PureNumBetweenAnd
        };

        public IEnumerable<Regex> SimpleCasesRegex => SimpleCases;

        public Regex PrepositionRegex => EnglishTimePeriodExtractorConfiguration.PrepositionRegex;

        public Regex TillRegex => EnglishTimePeriodExtractorConfiguration.TillRegex;

        private static readonly Regex PeriodNightRegex = new Regex(DateTimeDefinition.PeriodNightRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex PeriodSpecificNightRegex = new Regex(DateTimeDefinition.PeriodSpecificNightRegex,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex NightRegex => PeriodNightRegex;

        public Regex SpecificNightRegex => PeriodSpecificNightRegex;

        private static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinition.TimeUnitRegex,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex TimeFollowedUnit = new Regex(DateTimeDefinition.TimeFollowedUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static readonly Regex TimeNumberCombinedWithUnit = new Regex(DateTimeDefinition.TimeNumberCombinedWithUnit,
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public Regex FollowedUnit => TimeFollowedUnit;

        Regex IDateTimePeriodExtractorConfiguration.NumberCombinedWithUnit => TimeNumberCombinedWithUnit;
        
        public Regex UnitRegex => TimeUnitRegex;

        public Regex PastRegex => EnglishDatePeriodExtractorConfiguration.PastRegex;

        public Regex FutureRegex => EnglishDatePeriodExtractorConfiguration.FutureRegex;

        public IExtractor CardinalExtractor { get; }

        public IExtractor SingleDateExtractor { get; }

        public IExtractor SingleTimeExtractor { get; }

        public IExtractor SingleDateTimeExtractor { get; }

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("from"))
            {
                index = text.LastIndexOf("from", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            if (text.EndsWith("between"))
            {
                index = text.LastIndexOf("between", StringComparison.Ordinal);
                return true;
            }
            return false;
        }

        public bool HasConnectorToken(string text)
        {
            return text.Equals("and");
        }
    }
}
