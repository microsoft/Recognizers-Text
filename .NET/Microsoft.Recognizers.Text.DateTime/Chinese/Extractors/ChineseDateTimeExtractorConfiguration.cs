using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimeExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_DATETIME; // "DateTime";

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.PrepositionRegex, RegexFlags);

        public static readonly Regex NowRegex = new Regex(DateTimeDefinitions.NowRegex, RegexFlags);

        public static readonly Regex NightRegex = new Regex(DateTimeDefinitions.NightRegex, RegexFlags);

        public static readonly Regex TimeOfTodayRegex = new Regex(DateTimeDefinitions.TimeOfTodayRegex, RegexFlags);

        public static readonly Regex BeforeRegex = new Regex(DateTimeDefinitions.BeforeRegex, RegexFlags);

        public static readonly Regex AfterRegex = new Regex(DateTimeDefinitions.AfterRegex, RegexFlags);

        public static readonly Regex DateTimePeriodUnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseDateTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {

            DatePointExtractor = new BaseCJKDateExtractor(new ChineseDateExtractorConfiguration(this));
            TimePointExtractor = new BaseCJKTimeExtractor(new ChineseTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new ChineseDurationExtractorConfiguration(this));
        }

        public IDateTimeExtractor DatePointExtractor { get; }

        public IDateTimeExtractor TimePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        Regex ICJKDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex ICJKDateTimeExtractorConfiguration.PrepositionRegex => PrepositionRegex;

        Regex ICJKDateTimeExtractorConfiguration.NightRegex => NightRegex;

        Regex ICJKDateTimeExtractorConfiguration.TimeOfTodayRegex => TimeOfTodayRegex;

        Regex ICJKDateTimeExtractorConfiguration.BeforeRegex => BeforeRegex;

        Regex ICJKDateTimeExtractorConfiguration.AfterRegex => AfterRegex;
    }
}