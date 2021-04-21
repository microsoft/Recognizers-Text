using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Utilities;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseCJKTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_TIME; // "Time";

        private readonly ICJKTimeParserConfiguration config;

        public BaseCJKTimeParser(ICJKTimeParserConfiguration configuration)
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
                var result = this.config.TimeExtractor.Extract(er.Text, refDate);
                extra = result[0]?.Data as DateTimeExtra<TimeType>;
            }

            if (extra != null)
            {
                var timeResult = this.config.FunctionMap[extra.Type](extra);
                var parseResult = this.config.TimeFunc.PackTimeResult(extra, timeResult, referenceTime);
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