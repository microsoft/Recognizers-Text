// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Dutch;
using Microsoft.Recognizers.Text.DateTime.Dutch.Utilities;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Dutch
{
    public class DutchTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration, ITimePeriodExtractorConfiguration
    {
        public static readonly Regex TillRegex =
            new Regex(DateTimeDefinitions.TillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourRegex =
            new Regex(DateTimeDefinitions.HourRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PeriodHourNumRegex =
            new Regex(DateTimeDefinitions.PeriodHourNumRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PeriodDescRegex =
            new Regex(DateTimeDefinitions.DescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PureNumFromTo =
            new Regex(DateTimeDefinitions.PureNumFromTo, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeDateFromTo =
            new Regex(DateTimeDefinitions.TimeDateFromTo, RegexFlags, RegexTimeOut);

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
            new Regex(DateTimeDefinitions.FromRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex BetweenRegex =
            new Regex(DateTimeDefinitions.BetweenTokenRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex RangeConnectorRegex =
            new Regex(DateTimeDefinitions.RangeConnectorRegex, RegexFlags, RegexTimeOut);

        public DutchTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            TokenBeforeDate = DateTimeDefinitions.TokenBeforeDate;
            SingleTimeExtractor = new BaseTimeExtractor(new DutchTimeExtractorConfiguration(this));
            UtilityConfiguration = new DutchDatetimeUtilityConfiguration();

            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            IntegerExtractor = Number.Dutch.IntegerExtractor.GetInstance(numConfig);

            TimeZoneExtractor = new BaseTimeZoneExtractor(new DutchTimeZoneExtractorConfiguration(this));
        }

        public string TokenBeforeDate { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor TimeZoneExtractor { get; }

        public IExtractor IntegerExtractor { get; }

        public IEnumerable<Regex> SimpleCasesRegex => new[]
        {
            PureNumFromTo, PureNumBetweenAnd, SpecificTimeFromTo, SpecificTimeBetweenAnd,
        };

        public IEnumerable<Regex> PureNumberRegex => new[]
        {
            PureNumFromTo, PureNumBetweenAnd,
        };

        bool ITimePeriodExtractorConfiguration.CheckBothBeforeAfter => DateTimeDefinitions.CheckBothBeforeAfter;

        Regex ITimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ITimePeriodExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

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
            var betweenMatch = BetweenRegex.Match(text);
            if (betweenMatch.Success)
            {
                index = betweenMatch.Index;
            }

            return betweenMatch.Success;
        }

        public bool IsConnectorToken(string text)
        {
            return RangeConnectorRegex.IsExactMatch(text, trim: true);
        }

        // This method is used to disambiguate extractions containing 'morgen' (that can mean both 'tomorrow' and 'morning').
        // It discards isolated occurrences of 'morgen', keeping as valid extractions only those cases
        // where it is part of a bigger match (e.g. 'diensdag morgen')
        public List<ExtractResult> ApplyPotentialPeriodAmbiguityHotfix(string text, List<ExtractResult> timePeriodErs)
        {
            {
                var morgenStr = DateTimeDefinitions.MorningTermList[0];
                List<ExtractResult> timePeriodErsResult = new List<ExtractResult>();
                foreach (var timePeriodEr in timePeriodErs)
                {
                    if (!timePeriodEr.Text.Equals(morgenStr, StringComparison.Ordinal))
                    {
                        timePeriodErsResult.Add(timePeriodEr);
                    }
                }

                return timePeriodErsResult;
            }
        }
    }
}