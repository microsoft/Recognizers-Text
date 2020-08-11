using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.Japanese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;
using TimeExtractorJpn = Microsoft.Recognizers.Text.DateTime.Japanese.JapaneseTimeExtractorConfiguration;

namespace Microsoft.Recognizers.Text.DateTime.Japanese
{
    public class JapaneseTimeParserConfiguration : IDateTimeParser
    {
        public static readonly IDateTimeExtractor TimeExtractor = new JapaneseTimeExtractorConfiguration();

        private static TimeFunctions timeFunctions = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = TimeExtractorJpn.DayDescRegex,
        };

        private static readonly Dictionary<TimeType, TimeFunction> FunctionMap =
            new Dictionary<TimeType, TimeFunction>
            {
                { TimeType.DigitTime, timeFunctions.HandleDigit },
                { TimeType.CjkTime, timeFunctions.HandleKanji },
                { TimeType.LessTime, timeFunctions.HandleLess },
            };

        private readonly IFullDateTimeParserConfiguration config;

        public JapaneseTimeParserConfiguration(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        private delegate TimeResult TimeFunction(DateTimeExtra<TimeType> extra);

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceTime = refDate;
            var extra = er.Data as DateTimeExtra<TimeType>;
            if (extra == null)
            {
                var result = TimeExtractor.Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<TimeType>;
            }

            if (extra != null)
            {
                var timeResult = FunctionMap[extra.Type](extra);
                var parseResult = timeFunctions.PackTimeResult(extra, timeResult, referenceTime);
                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject)parseResult.FutureValue) },
                    };

                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject)parseResult.PastValue) },
                    };
                }

                var ret = new DateTimeParseResult
                {
                    Start = er.Start,
                    Text = er.Text,
                    Type = er.Type,
                    Length = er.Length,
                    Value = parseResult,
                    Data = timeResult,
                    ResolutionStr = string.Empty,
                    TimexStr = parseResult.Timex,
                };

                return ret;
            }

            return null;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }
    }
}