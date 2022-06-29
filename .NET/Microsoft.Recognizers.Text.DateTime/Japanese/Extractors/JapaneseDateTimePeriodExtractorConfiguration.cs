// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Config;
using Microsoft.Recognizers.Text.Number.Japanese;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateTimePeriodExtractorConfiguration : BaseDateTimeOptionsConfiguration,
        ICJKDateTimePeriodExtractorConfiguration
    {

        public static readonly Regex TillRegex = new Regex(DateTimeDefinitions.DateTimePeriodTillRegex, RegexFlags);

        public static readonly Regex FromPrefixRegex = new Regex(DateTimeDefinitions.DateTimePeriodFromPrefixRegex, RegexFlags);

        public static readonly Regex FromSuffixRegex = new Regex(DateTimeDefinitions.DateTimePeriodFromSuffixRegex, RegexFlags);

        public static readonly Regex ConnectorRegex = new Regex(DateTimeDefinitions.DateTimePeriodConnectorRegex, RegexFlags);

        public static readonly Regex PrepositionRegex = new Regex(DateTimeDefinitions.DateTimePeriodPrepositionRegex, RegexFlags);

        public static readonly Regex ZhijianRegex = new Regex(DateTimeDefinitions.ZhijianRegex, RegexFlags);

        public static readonly Regex TimeOfDayRegex = new Regex(DateTimeDefinitions.TimeOfDayRegex, RegexFlags);

        public static readonly Regex SpecificTimeOfDayRegex = new Regex(DateTimeDefinitions.SpecificTimeOfDayRegex, RegexFlags);

        public static readonly Regex UnitRegex = new Regex(DateTimeDefinitions.DateTimePeriodUnitRegex, RegexFlags);

        public static readonly Regex FollowedUnit = new Regex(DateTimeDefinitions.DateTimePeriodFollowedUnit, RegexFlags);

        public static readonly Regex PastRegex = new Regex(DateTimeDefinitions.PastRegex, RegexFlags);

        public static readonly Regex FutureRegex = new Regex(DateTimeDefinitions.FutureRegex, RegexFlags);

        public static readonly Regex WeekDayRegex = new Regex(DateTimeDefinitions.WeekDayRegex, RegexFlags);

        public static readonly Regex TimePeriodLeftRegex = new Regex(DateTimeDefinitions.TimePeriodLeftRegex, RegexFlags);

        public static readonly Regex RelativeRegex = new Regex(DateTimeDefinitions.RelativeRegex, RegexFlags);

        public static readonly Regex RestOfDateRegex = new Regex(DateTimeDefinitions.RestOfDateRegex, RegexFlags);

        public static readonly Regex AmPmDescRegex = new Regex(DateTimeDefinitions.AmPmDescRegex, RegexFlags);

        public static readonly Regex BeforeAfterRegex = new Regex(DateTimeDefinitions.BeforeAfterRegex, RegexFlags);

        public static readonly Regex HourRegex = new Regex(DateTimeDefinitions.HourRegex, RegexFlags);
        public static readonly Regex HourNumRegex = new Regex(DateTimeDefinitions.HourNumRegex, RegexFlags);
        public static readonly Regex ThisRegex = new Regex(DateTimeDefinitions.DateTimePeriodThisRegex, RegexFlags);
        public static readonly Regex LastRegex = new Regex(DateTimeDefinitions.DateTimePeriodLastRegex, RegexFlags);
        public static readonly Regex NextRegex = new Regex(DateTimeDefinitions.DateTimePeriodNextRegex, RegexFlags);
        public static readonly Regex NumberCombinedWithUnit = new Regex(DateTimeDefinitions.DateTimePeriodNumberCombinedWithUnit, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseDateTimePeriodExtractorConfiguration(IDateTimeOptionsConfiguration config)
            : base(config)
        {
            var numOptions = NumberOptions.None;
            if ((config.Options & DateTimeOptions.NoProtoCache) != 0)
            {
                numOptions = NumberOptions.NoProtoCache;
            }

            var numConfig = new BaseNumberOptionsConfiguration(config.Culture, numOptions);

            CardinalExtractor = new CardinalExtractor(numConfig, CJKNumberExtractorMode.ExtractAll);

            SingleDateExtractor = new BaseCJKDateExtractor(new JapaneseDateExtractorConfiguration(this));
            SingleTimeExtractor = new BaseCJKTimeExtractor(new JapaneseTimeExtractorConfiguration(this));
            SingleDateTimeExtractor = new BaseCJKDateTimeExtractor(new JapaneseDateTimeExtractorConfiguration(this));
            DurationExtractor = new BaseCJKDurationExtractor(new JapaneseDurationExtractorConfiguration(this));
            TimePeriodExtractor = new BaseCJKTimePeriodExtractor(new JapaneseTimePeriodExtractorConfiguration(this));
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

            var match = FromPrefixRegex.MatchEnd(text, trim: true);
            if (match.Success)
            {
                index = match.Index;
                return true;
            }
            else
            {
                match = FromSuffixRegex.MatchBegin(text, trim: true);
                if (match.Success)
                {
                    index = match.Index + match.Length;
                    return true;
                }
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
            return ConnectorRegex.IsExactMatch(text, trim: true);
        }
    }
}