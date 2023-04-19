// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Portuguese.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateTimeExtractorConfiguration : BaseDateTimeOptionsConfiguration, IDateTimeExtractorConfiguration
    {
        public static readonly Regex PrepositionRegex =
            new Regex(DateTimeDefinitions.PrepositionRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NowRegex =
            new Regex(DateTimeDefinitions.NowRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SuffixRegex =
            new Regex(DateTimeDefinitions.SuffixRegex, RegexFlags, RegexTimeOut);

        // TODO: modify it according to the corresponding English regex
        public static readonly Regex TimeOfDayRegex =
            new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfTodayAfterRegex =
            new Regex(DateTimeDefinitions.TimeOfTodayAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfTodayBeforeRegex =
            new Regex(DateTimeDefinitions.TimeOfTodayBeforeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SimpleTimeOfTodayAfterRegex =
            new Regex(DateTimeDefinitions.SimpleTimeOfTodayAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SimpleTimeOfTodayBeforeRegex =
            new Regex(DateTimeDefinitions.SimpleTimeOfTodayBeforeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificEndOfRegex =
            new Regex(DateTimeDefinitions.SpecificEndOfRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex UnspecificEndOfRegex =
            new Regex(DateTimeDefinitions.UnspecificEndOfRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearRegex =
            new Regex(DateTimeDefinitions.YearRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex YearSuffix =
            new Regex(DateTimeDefinitions.YearSuffix, RegexFlags, RegexTimeOut);

        // TODO: add this for Portuguese
        public static readonly Regex UnitRegex =
            new Regex(DateTimeDefinitions.UnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ConnectorRegex =
            new Regex(DateTimeDefinitions.ConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex NumberAsTimeRegex =
            new Regex(DateTimeDefinitions.NumberAsTimeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex DateNumberConnectorRegex =
            new Regex(DateTimeDefinitions.DateNumberConnectorRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SuffixAfterRegex =
            new Regex(DateTimeDefinitions.SuffixAfterRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public PortugueseDateTimeExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.Portuguese.IntegerExtractor.GetInstance(numConfig);

            DatePointExtractor = new BaseDateExtractor(new PortugueseDateExtractorConfiguration(this));
            TimePointExtractor = new BaseTimeExtractor(new PortugueseTimeExtractorConfiguration(this));
            DurationExtractor = new BaseDurationExtractor(new PortugueseDurationExtractorConfiguration(this));
            UtilityConfiguration = new PortugueseDatetimeUtilityConfiguration();
            HolidayExtractor = new BaseHolidayExtractor(new PortugueseHolidayExtractorConfiguration(this));

        }

        public IExtractor IntegerExtractor { get; }

        public IDateExtractor DatePointExtractor { get; }

        public IDateTimeExtractor TimePointExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor HolidayExtractor { get; }

        Regex IDateTimeExtractorConfiguration.NowRegex => NowRegex;

        Regex IDateTimeExtractorConfiguration.SuffixRegex => SuffixRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayAfterRegex => TimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayAfterRegex => SimpleTimeOfTodayAfterRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfTodayBeforeRegex => TimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.SimpleTimeOfTodayBeforeRegex => SimpleTimeOfTodayBeforeRegex;

        Regex IDateTimeExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex IDateTimeExtractorConfiguration.SpecificEndOfRegex => SpecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnspecificEndOfRegex => UnspecificEndOfRegex;

        Regex IDateTimeExtractorConfiguration.UnitRegex => UnitRegex;

        Regex IDateTimeExtractorConfiguration.NumberAsTimeRegex => NumberAsTimeRegex;

        Regex IDateTimeExtractorConfiguration.DateNumberConnectorRegex => DateNumberConnectorRegex;

        Regex IDateTimeExtractorConfiguration.YearRegex => YearRegex;

        Regex IDateTimeExtractorConfiguration.YearSuffix => YearSuffix;

        Regex IDateTimeExtractorConfiguration.SuffixAfterRegex => SuffixAfterRegex;

        public bool IsConnector(string text)
        {
            text = text.Trim();
            return string.IsNullOrEmpty(text)
                    || PrepositionRegex.IsMatch(text)
                    || ConnectorRegex.IsMatch(text);
        }
    }
}