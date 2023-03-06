// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.French;
using Microsoft.Recognizers.Text.DateTime.French.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.French
{
    public class FrenchTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly string ExtractorName = Constants.SYS_DATETIME_TIMEPERIOD; // "TimePeriod";

        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourRegex =
            new Regex(DateTimeDefinitions.HourRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PeriodHourNumRegex =
            new Regex(DateTimeDefinitions.PeriodHourNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PeriodDescRegex =
            new Regex(DateTimeDefinitions.PeriodDescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PureNumFromTo =
            new Regex(DateTimeDefinitions.PureNumFromTo, RegexFlags, RegexTimeOut);

        public static readonly Regex PureNumBetweenAnd =
            new Regex(DateTimeDefinitions.PureNumBetweenAnd, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificTimeFromTo =
            new Regex(DateTimeDefinitions.SpecificTimeFromTo, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificTimeBetweenAnd =
            new Regex(DateTimeDefinitions.SpecificTimeBetweenAnd, RegexFlags, RegexTimeOut);

        public static readonly Regex PrepositionRegex =
            new Regex(DateTimeDefinitions.PrepositionRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfDayRegex =
            new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificTimeOfDayRegex =
            new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeUnitRegex =
            new Regex(DateTimeDefinitions.TimeUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeFollowedUnit =
            new Regex(DateTimeDefinitions.TimeFollowedUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeNumberCombinedWithUnit =
            new Regex(DateTimeDefinitions.TimeNumberCombinedWithUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex GeneralEndingRegex =
            new Regex(DateTimeDefinitions.GeneralEndingRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex FromRegex =
            new Regex(DateTimeDefinitions.FromRegex2, RegexFlags, RegexTimeOut);

        private static readonly Regex ConnectorAndRegex =
            new Regex(DateTimeDefinitions.ConnectorAndRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex BeforeRegex =
            new Regex(DateTimeDefinitions.BeforeRegex2, RegexFlags, RegexTimeOut);

        public FrenchTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new FrenchTimeExtractorConfiguration(this));
            UtilityConfiguration = new FrenchDatetimeUtilityConfiguration();

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.English.IntegerExtractor.GetInstance(numConfig);

            TimeZoneExtractor = new BaseTimeZoneExtractor(new FrenchTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd, PmRegex, AmRegex };

        public IEnumerable<Regex> PureNumberRegex => new Regex[] { PureNumFromTo, PureNumBetweenAnd };

        bool ITimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => FrenchDateTimeExtractorConfiguration.TimeOfDayRegex;

        Regex ITimePeriodExtractorConfiguration.GeneralEndingRegex => GeneralEndingRegex;

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

        public bool IsConnectorToken(string text)
        {
            return ConnectorAndRegex.IsMatch(text);
        }

        public List<ExtractResult> ApplyPotentialPeriodAmbiguityHotfix(string text, List<ExtractResult> timePeriodErs) => TimePeriodFunctions.ApplyPotentialPeriodAmbiguityHotfix(text, timePeriodErs);
    }
}
