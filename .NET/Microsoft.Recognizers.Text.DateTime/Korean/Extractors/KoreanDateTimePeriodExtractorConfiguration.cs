﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Korean;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration,
        ICJKDateTimePeriodExtractorConfiguration
    {

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DateTimePeriodTillRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.DateTimePeriodPrepositionRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex ZhijianRegex = new Regex(DateTimeDefinitions.ZhijianRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SpecificTimeOfDayRegex = new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.DateTimePeriodFollowedUnit, RegexFlags, RegexTimeOut);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex TimePeriodLeftRegex = new Regex(DateTimeDefinitions.TimePeriodLeftRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RelativeRegex = new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex RestOfDateRegex = new Regex(DateTimeDefinitions.RestOfDateRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex AmPmDescRegex = new Regex(DateTimeDefinitions.AmPmDescRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex BeforeAfterRegex = new Regex(DateTimeDefinitions.BeforeAfterRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex HourRegex = new Regex(DateTimeDefinitions.HourRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateTimePeriodThisRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateTimePeriodLastRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateTimePeriodNextRegex, RegexFlags, RegexTimeOut);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DateTimePeriodNumberCombinedWithUnit, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public KoreanDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = new CardinalExtractor(numConfig);

            SingleDateExtractor = new BaseCJKDateExtractor(new KoreanDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseCJKTimeExtractor(new KoreanTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseCJKDateTimeExtractor(new KoreanDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new KoreanDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new KoreanTimePeriodExtractorConfiguration(this));
        }

        public IExtractor CardinalExtractor { get; }

        public IDateTimeExtractor SingleDateExtractor { get; }

        public IDateTimeExtractor SingleTimeExtractor { get; }

        public IDateTimeExtractor SingleDateTimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeExtractor TimePeriodExtractor { get; }

        Regex ICJKDateTimePeriodExtractorConfiguration.PrepositionRegex => PrepositionRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.TillRegex => TillRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.SpecificTimeOfDayRegex => SpecificTimeOfDayRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.TimeOfDayRegex => TimeOfDayRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.FollowedUnit => FollowedUnit;

        Regex ICJKDateTimePeriodExtractorConfiguration.UnitRegex => UnitRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.PastRegex => PastRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.FutureRegex => FutureRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.TimePeriodLeftRegex => TimePeriodLeftRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.RelativeRegex => RelativeRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.RestOfDateRegex => RestOfDateRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.AmPmDescRegex => AmPmDescRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.ThisRegex => ThisRegex;

        Regex ICJKDateTimePeriodExtractorConfiguration.BeforeAfterRegex => BeforeAfterRegex;

        public bool GetFromTokenIndex(string text, out int index)
        {
            index = -1;

            // @TODO move hardcoded values to resources file
            if (text.Trim().EndsWith("从", StringComparison.Ordinal))
            {
                index = text.LastIndexOf("从", StringComparison.Ordinal);
                return true;
            }

            return false;
        }

        public bool GetBetweenTokenIndex(string text, out int index)
        {
            index = -1;
            var match = ZhijianRegex.Match(text);
            if (match.Success)
            {
                index = match.Length;
                return true;
            }

            return false;
        }

        public bool HasConnectorToken(string text)
        {
            // @TODO move hardcoded values to resources file
            return text.Equals("和", StringComparison.Ordinal) ||
                    text.Equals("与", StringComparison.Ordinal) ||
                    text.Equals("到", StringComparison.Ordinal);
        }
    }
}