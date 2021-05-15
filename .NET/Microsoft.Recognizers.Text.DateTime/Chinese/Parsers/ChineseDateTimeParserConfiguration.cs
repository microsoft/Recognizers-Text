using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Chinese;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimeParserConfiguration
    {
        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexFlags);

        public static readonly Regex SimpleAmRegex = new Regex(DateTimeDefinitions.DateTimeSimpleAmRegex, RegexFlags);

        public static readonly Regex SimplePmRegex = new Regex(DateTimeDefinitions.DateTimeSimplePmRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public ChineseDateTimeParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            IntegerExtractor = config.IntegerExtractor;
            DateExtractor = config.DateExtractor;
            TimeExtractor = config.TimeExtractor;
            DurationExtractor = config.DurationExtractor;

            DateParser = config.DateParser;
            TimeParser = config.TimeParser;
            NumberParser = config.NumberParser;

            UnitMap = DateTimeDefinitions.ParserConfigurationUnitMap.ToImmutableDictionary();
            NowRegex = ChineseDateTimeExtractorConfiguration.NowRegex;
            TimeOfTodayRegex = ChineseDateTimeExtractorConfiguration.TimeOfTodayRegex;
            DateTimePeriodUnitRegex = ChineseDateTimeExtractorConfiguration.DateTimePeriodUnitRegex;
            BeforeRegex = ChineseDateTimeExtractorConfiguration.BeforeRegex;
            AfterRegex = ChineseDateTimeExtractorConfiguration.AfterRegex;
        }

        public IDateTimeExtractor DateExtractor { get; }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateTimeParser DateParser { get; }

        public IDateTimeParser TimeParser { get; }

        public IExtractor IntegerExtractor { get; }

        public IParser NumberParser { get; }

        public ImmutableDictionary<string, string> UnitMap { get; }

        public Regex NowRegex { get; }

        public Regex TimeOfTodayRegex { get; }

        public Regex DateTimePeriodUnitRegex { get; }

        public Regex BeforeRegex { get; }

        public Regex AfterRegex { get; }

        Regex ICJKDateTimeParserConfiguration.LunarRegex => LunarRegex;

        Regex ICJKDateTimeParserConfiguration.LunarHolidayRegex => LunarHolidayRegex;

        Regex ICJKDateTimeParserConfiguration.SimpleAmRegex => SimpleAmRegex;

        Regex ICJKDateTimeParserConfiguration.SimplePmRegex => SimplePmRegex;

        public bool GetMatchedNowTimex(string text, out string timex)
        {
            var trimmedText = text.Trim();

            // @TODO move hardcoded values to resources file
            if (trimmedText.EndsWith("现在", StringComparison.Ordinal))
            {
                timex = "PRESENT_REF";
            }
            else if (trimmedText is "刚刚才" or "刚刚" or "刚才")
            {
                timex = "PAST_REF";
            }
            else if (trimmedText is "立刻" or "马上")
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
            if (text is "今天" or "今日" or "最近")
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
            else if (text is "大后天" or "大後天")
            {
                value = 3;
            }
            else if (text is "大前天")
            {
                value = -3;
            }
            else if (text is "后天" or "後天")
            {
                value = 2;
            }
            else if (text is "前天")
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
