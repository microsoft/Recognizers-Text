using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class ChineseTimePeriodParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimePeriodParserConfiguration
    {
        private static TimeFunctions timeFunc = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = ChineseTimeExtractorConfiguration.DayDescRegex,
        };

        public ChineseTimePeriodParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
            : base(config)
        {
            TimeExtractor = config.TimeExtractor;
            TimeParser = config.TimeParser;
        }

        public IDateTimeExtractor TimeExtractor { get; }

        public IDateTimeParser TimeParser { get; }

        TimeFunctions ICJKTimePeriodParserConfiguration.TimeFunc => timeFunc;

        public bool GetMatchedTimexRange(string text, out string timex, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();
            beginHour = 0;
            endHour = 0;
            endMin = 0;

            var timeOfDay = string.Empty;

            if (DateTimeDefinitions.MorningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Morning;
            }
            else if (DateTimeDefinitions.MidDayTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.MidDay;
            }
            else if (DateTimeDefinitions.AfternoonTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Afternoon;
            }
            else if (DateTimeDefinitions.EveningTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Evening;
            }
            else if (DateTimeDefinitions.DaytimeTermList.Any(o => trimmedText.Equals(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Daytime;
            }
            else if (DateTimeDefinitions.NightTermList.Any(o => trimmedText.EndsWith(o, StringComparison.Ordinal)))
            {
                timeOfDay = Constants.Night;
            }
            else
            {
                timex = null;
                return false;
            }

            var parseResult = TimexUtility.ParseTimeOfDay(timeOfDay);
            timex = parseResult.Timex;
            beginHour = parseResult.BeginHour;
            endHour = parseResult.EndHour;
            endMin = parseResult.EndMin;

            return true;
        }
    }
}
