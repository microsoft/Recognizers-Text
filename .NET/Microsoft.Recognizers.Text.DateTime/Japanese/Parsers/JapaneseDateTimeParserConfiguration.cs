﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimeParserConfiguration
    {
        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SimpleAmRegex = new Regex(DateTimeDefinitions.DateTimeSimpleAmRegex, RegexFlags, RegexTimeOut);

        public static readonly Regex SimplePmRegex = new Regex(DateTimeDefinitions.DateTimeSimplePmRegex, RegexFlags, RegexTimeOut);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        private static readonly Regex NowTimeRegex = new Regex(DateTimeDefinitions.NowTimeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex RecentlyTimeRegex = new Regex(DateTimeDefinitions.RecentlyTimeRegex, RegexFlags, RegexTimeOut);

        private static readonly Regex AsapTimeRegex = new Regex(DateTimeDefinitions.AsapTimeRegex, RegexFlags, RegexTimeOut);

        public JapaneseDateTimeParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DurationExtractor = config.DurationExtractor;

            DateParser = config.DateParser;
            DurationParser = config.DurationParser;
            TimeParser = config.TimeParser;
            NumberParser = config.NumberParser;

            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            NowRegex = JapaneseDateTimeExtractorConfiguration.NowRegex;
            TimeOfSpecialDayRegex = JapaneseDateTimeExtractorConfiguration.TimeOfSpecialDayRegex;
            DateTimePeriodUnitRegex = JapaneseDateTimeExtractorConfiguration.DateTimePeriodUnitRegex;
            BeforeRegex = JapaneseDateTimeExtractorConfiguration.BeforeRegex;
            AfterRegex = JapaneseDateTimeExtractorConfiguration.AfterRegex;
            DurationRelativeDurationUnitRegex = JapaneseDateTimeExtractorConfiguration.DurationRelativeDurationUnitRegex;
            AgoLaterRegex = JapaneseDateTimeExtractorConfiguration.AgoLaterRegex;
        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser DurationParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        public ImmutableDictionary<string, string> UnitMap { get; }

        public Regex NowRegex { get; }

        public Regex TimeOfSpecialDayRegex { get; }

        public Regex DateTimePeriodUnitRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        public Regex DurationRelativeDurationUnitRegex { get; }

        public Regex AgoLaterRegex { get; }

        Regex ICJKDateTimeParserConfiguration.LunarRegex => LunarRegex;

        Regex ICJKDateTimeParserConfiguration.LunarHolidayRegex => LunarHolidayRegex;

        Regex ICJKDateTimeParserConfiguration.SimpleAmRegex => SimpleAmRegex;

        Regex ICJKDateTimeParserConfiguration.SimplePmRegex => SimplePmRegex;

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            if (NowTimeRegex.MatchEnd(trimmedText, trim: true).Success)
            {
                timex = "PRESENT_REF";
            }
            else if (RecentlyTimeRegex.IsExactMatch(trimmedText, trim: true))
            {
                timex = "PAST_REF";
            }
            else if (AsapTimeRegex.IsExactMatch(trimmedText, trim: true))
            {
                timex = "FUTURE_REF";
            }
            else
            {
                timex = null;
                return false;
            }

            return true;
        }

        public int GetSwiftDay(string text)
        {
            var value = 0;

            // @TODO move hardcoded values to resources file
            if (text.Equals("今天", StringComparison.Ordinal) ||
                text.Equals("今日", StringComparison.Ordinal) ||
                text.Equals("最近", StringComparison.Ordinal))
            {
                value = 0;
            }
            else if (text.StartsWith("明", StringComparison.Ordinal))
            {
                value = 1;
            }
            else if (text.StartsWith("昨", StringComparison.Ordinal))
            {
                value = -1;
            }
            else if (text.Equals("大后天", StringComparison.Ordinal) ||
                     text.Equals("大後天", StringComparison.Ordinal))
            {
                value = 3;
            }
            else if (text.Equals("大前天", StringComparison.Ordinal))
            {
                value = -3;
            }
            else if (text.Equals("后天", StringComparison.Ordinal) ||
                     text.Equals("後天", StringComparison.Ordinal))
            {
                value = 2;
            }
            else if (text.Equals("前天", StringComparison.Ordinal))
            {
                value = -2;
            }

            return value;
        }

        public void AdjustByTimeOfDay(string matchStr, ref int hour, ref int swift)
        {
            // @TODO move hardcoded values to resources file
            switch (matchStr)
            {
                case "今晚":
                    if (hour < Constants.HalfDayHourCount)
                    {
                        hour += Constants.HalfDayHourCount;
                    }

                    break;
                case "今早":
                case "今晨":
                    if (hour >= Constants.HalfDayHourCount)
                    {
                        hour -= Constants.HalfDayHourCount;
                    }

                    break;
                case "明晚":
                    swift = 1;
                    if (hour < Constants.HalfDayHourCount)
                    {
                        hour += Constants.HalfDayHourCount;
                    }

                    break;
                case "明早":
                case "明晨":
                    swift = 1;
                    if (hour >= Constants.HalfDayHourCount)
                    {
                        hour -= Constants.HalfDayHourCount;
                    }

                    break;
                case "昨晚":
                    swift = -1;
                    if (hour < Constants.HalfDayHourCount)
                    {
                        hour += Constants.HalfDayHourCount;
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
