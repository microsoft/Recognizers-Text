using System.Collections.Generic;
using DateObject = System.DateTime;

using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeALTParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEALT; // "DateTimeALT";

        private readonly IDateTimeALTParserConfiguration config;

        public BaseDateTimeALTParser(IDateTimeALTParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseDateTimeAndTimeALT(er, referenceTime);

                if (innerResult.Success)
                {
                    var context = ((ExtractResult)((Dictionary<string, object>)er.Data)[Constants.Context]).Text;
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime((DateObject) innerResult.FutureValue)},
                        {Constants.Context, context}
                    };

                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, FormatUtil.FormatDateTime((DateObject) innerResult.PastValue)},
                        {Constants.Context, context}
                    };

                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? "" : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = ""
            };

            return ret;
        }

        // merge a Date entity and a Time entity
        private DateTimeResolutionResult ParseDateTimeAndTimeALT(ExtractResult er, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();
            var contextEr = (ExtractResult)((Dictionary<string, object>)er.Data)[Constants.Context];
            var dateTimeEr = new ExtractResult();
            dateTimeEr.Text = $"{contextEr.Text} {er.Text}";
            dateTimeEr.Type = Constants.SYS_DATETIME_DATETIME;
            var datetimePr = this.config.DateTimeParser.Parse(dateTimeEr, referenceTime);

            if (datetimePr.Value != null)
            {
                ret.FutureValue = ((DateTimeResolutionResult)datetimePr.Value).FutureValue;
                ret.PastValue = ((DateTimeResolutionResult)datetimePr.Value).PastValue;
                ret.Timex = datetimePr.TimexStr;
                ret.Success = true;
            }

            return ret;
        }
    }
}