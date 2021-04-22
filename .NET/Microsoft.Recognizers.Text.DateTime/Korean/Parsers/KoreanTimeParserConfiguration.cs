using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.Korean;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Korean
{
    public class KoreanTimeParserConfiguration : BaseDateTimeOptionsConfiguration, ICJKTimeParserConfiguration
    {
        private static TimeFunctions timeFunc = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = KoreanTimeExtractorConfiguration.DayDescRegex,
        };

        private static readonly Dictionary<TimeType, TimeFunction> FunctionMap =
            new Dictionary<TimeType, TimeFunction>
            {
                { TimeType.DigitTime, timeFunc.HandleDigit },
                { TimeType.CjkTime, timeFunc.HandleKanji },
                { TimeType.LessTime, timeFunc.HandleLess },
            };

        public KoreanTimeParserConfiguration(ICJKCommonDateTimeParserConfiguration config)
         : base(config)
        {
            TimeExtractor = config.TimeExtractor;
        }

        // public delegate TimeResult TimeFunction(DateTimeExtra<TimeType> extra);

        public IDateTimeExtractor TimeExtractor { get; }

        TimeFunctions ICJKTimeParserConfiguration.TimeFunc => timeFunc;

        Dictionary<TimeType, TimeFunction> ICJKTimeParserConfiguration.FunctionMap => FunctionMap;
    }
}