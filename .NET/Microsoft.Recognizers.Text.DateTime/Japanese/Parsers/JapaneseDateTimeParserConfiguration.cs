using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Japanese;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseDateTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKDateTimeParserConfiguration
    {
        public static readonly Regex LunarRegex = new Regex(DateTimeDefinitions.LunarRegex, RegexFlags);

        public static readonly Regex LunarHolidayRegex = new Regex(DateTimeDefinitions.LunarHolidayRegex, RegexFlags);

        public static readonly Regex SimpleAmRegex = new Regex(DateTimeDefinitions.DateTimeSimpleAmRegex, RegexFlags);

        public static readonly Regex SimplePmRegex = new Regex(DateTimeDefinitions.DateTimeSimplePmRegex, RegexFlags);

        private const RegexOptions RegexFlags = RegexOptions.Singleline | RegexOptions.ExplicitCapture;

        public JapaneseDateTimeParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
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
            NowRegex = JapaneseDateTimeExtractorConfiguration.NowRegex;
            TimeOfTodayRegex = JapaneseDateTimeExtractorConfiguration.TimeOfTodayRegex;
            DateTimePeriodUnitRegex = JapaneseDateTimeExtractorConfiguration.DateTimePeriodUnitRegex;
            BeforeRegex = JapaneseDateTimeExtractorConfiguration.BeforeRegex;
            AfterRegex = JapaneseDateTimeExtractorConfiguration.AfterRegex;
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
            else if (trimmedText.Equals("刚刚才", StringComparison.Ordinal) ||
                     trimmedText.Equals("刚刚", StringComparison.Ordinal) ||
                     trimmedText.Equals("刚才", StringComparison.Ordinal))
            {
                timex = "PAST_REF";
            }
            else if (trimmedText.Equals("立刻", StringComparison.Ordinal) ||
                     trimmedText.Equals("马上", StringComparison.Ordinal))
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
