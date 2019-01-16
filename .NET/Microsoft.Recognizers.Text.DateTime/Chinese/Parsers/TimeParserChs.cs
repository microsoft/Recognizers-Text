using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Definitions.Chinese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Chinese
{
    public class TimeParserChs : IDateTimeParser
    {
        public static readonly IDateTimeExtractor TimeExtractor = new TimeExtractorChs();

        private delegate TimeResult TimeFunction(DateTimeExtra<TimeType> extra);

        private static TimeFunctions timeFunc = new TimeFunctions
        {
            NumberDictionary = DateTimeDefinitions.TimeNumberDictionary,
            LowBoundDesc = DateTimeDefinitions.TimeLowBoundDesc,
            DayDescRegex = TimeExtractorChs.DayDescRegex,

        };

        private static readonly Dictionary<TimeType, TimeFunction> FunctionMap =
            new Dictionary<TimeType, TimeFunction>
            {
                {TimeType.DigitTime, timeFunc.HandleDigit},
                {TimeType.CountryTime, timeFunc.HandleChinese},
                {TimeType.LessTime, timeFunc.HandleLess}
            };

        private readonly IFullDateTimeParserConfiguration config;

        public TimeParserChs(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

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
                var parseResult = timeFunc.PackTimeResult(extra, timeResult, referenceTime);
                if (parseResult.Success)
                {
                    parseResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject) parseResult.FutureValue)}
                    };

                    parseResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.TIME, DateTimeFormatUtil.FormatTime((DateObject) parseResult.PastValue)}
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
                    ResolutionStr = "",
                    TimexStr = parseResult.Timex
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