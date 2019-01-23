using System;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class BaseDateTimeAltParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIMEALT; // "DateTimeALT";

        private readonly IDateTimeAltParserConfiguration config;

        public BaseDateTimeAltParser(IDateTimeAltParserConfiguration configuration)
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
                var innerResult = ParseDateTimeAndTimeAlt(er, referenceTime);

                if (innerResult.Success)
                {
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
                TimexStr = value == null ? string.Empty : ((DateTimeResolutionResult)value).Timex,
                ResolutionStr = string.Empty,
            };

            return ret;
        }

        public List<DateTimeParseResult> FilterResults(string query, List<DateTimeParseResult> candidateResults)
        {
            return candidateResults;
        }

        // merge the entity with its related contexts and then parse the combine text
        private static DateTimeResolutionResult GetResolution(ExtractResult er, DateTimeParseResult pr, DateTimeResolutionResult ret)
        {
            var parentText = (string)((Dictionary<string, object>)er.Data)[ExtendedModelResult.ParentTextKey];
            var type = pr.Type;

            var singlePointResolution = string.Empty;
            var pastStartPointResolution = string.Empty;
            var pastEndPointResolution = string.Empty;
            var futureStartPointResolution = string.Empty;
            var futureEndPointResolution = string.Empty;
            var singlePointType = string.Empty;
            var startPointType = string.Empty;
            var endPointType = string.Empty;

            if (type == Constants.SYS_DATETIME_DATEPERIOD || type == Constants.SYS_DATETIME_TIMEPERIOD ||
                type == Constants.SYS_DATETIME_DATETIMEPERIOD)
            {
                switch (type)
                {
                    case Constants.SYS_DATETIME_DATEPERIOD:
                        startPointType = TimeTypeConstants.START_DATE;
                        endPointType = TimeTypeConstants.END_DATE;
                        pastStartPointResolution = DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)ret.PastValue).Item1);
                        pastEndPointResolution = DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)ret.PastValue).Item2);
                        futureStartPointResolution = DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)ret.FutureValue).Item1);
                        futureEndPointResolution = DateTimeFormatUtil.FormatDate(((Tuple<DateObject, DateObject>)ret.FutureValue).Item2);
                        break;

                    case Constants.SYS_DATETIME_DATETIMEPERIOD:
                        startPointType = TimeTypeConstants.START_DATETIME;
                        endPointType = TimeTypeConstants.END_DATETIME;

                        if (ret.PastValue is Tuple<DateObject, DateObject> tuple)
                        {
                            pastStartPointResolution = DateTimeFormatUtil.FormatDateTime(tuple.Item1);
                            pastEndPointResolution = DateTimeFormatUtil.FormatDateTime(tuple.Item2);
                            futureStartPointResolution =
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)ret.FutureValue).Item1);
                            futureEndPointResolution =
                                DateTimeFormatUtil.FormatDateTime(((Tuple<DateObject, DateObject>)ret.FutureValue).Item2);
                        }
                        else if (ret.PastValue is DateObject datetime)
                        {
                            pastStartPointResolution = DateTimeFormatUtil.FormatDateTime(datetime);
                            futureStartPointResolution = DateTimeFormatUtil.FormatDateTime((DateObject)ret.FutureValue);
                        }

                        break;

                    case Constants.SYS_DATETIME_TIMEPERIOD:
                        startPointType = TimeTypeConstants.START_TIME;
                        endPointType = TimeTypeConstants.END_TIME;
                        pastStartPointResolution = DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)ret.PastValue).Item1);
                        pastEndPointResolution = DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)ret.PastValue).Item2);
                        futureStartPointResolution = DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)ret.FutureValue).Item1);
                        futureEndPointResolution = DateTimeFormatUtil.FormatTime(((Tuple<DateObject, DateObject>)ret.FutureValue).Item2);
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case Constants.SYS_DATETIME_DATE:
                        singlePointType = TimeTypeConstants.DATE;
                        singlePointResolution = DateTimeFormatUtil.FormatDate((DateObject)ret.FutureValue);
                        break;

                    case Constants.SYS_DATETIME_DATETIME:
                        singlePointType = TimeTypeConstants.DATETIME;
                        singlePointResolution = DateTimeFormatUtil.FormatDateTime((DateObject)ret.FutureValue);
                        break;

                    case Constants.SYS_DATETIME_TIME:
                        singlePointType = TimeTypeConstants.TIME;
                        singlePointResolution = DateTimeFormatUtil.FormatTime((DateObject)ret.FutureValue);
                        break;
                }
            }

            ret.FutureResolution = new Dictionary<string, string>();
            ret.PastResolution = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(futureStartPointResolution))
            {
                ret.FutureResolution.Add(startPointType, futureStartPointResolution);
            }

            if (!string.IsNullOrEmpty(futureEndPointResolution))
            {
                ret.FutureResolution.Add(endPointType, futureEndPointResolution);
            }

            if (!string.IsNullOrEmpty(pastStartPointResolution))
            {
                ret.PastResolution.Add(startPointType, pastStartPointResolution);
            }

            if (!string.IsNullOrEmpty(pastEndPointResolution))
            {
                ret.PastResolution.Add(endPointType, pastEndPointResolution);
            }

            if (!string.IsNullOrEmpty(singlePointResolution))
            {
                ret.FutureResolution.Add(singlePointType, singlePointResolution);
                ret.PastResolution.Add(singlePointType, singlePointResolution);
            }

            if (!string.IsNullOrEmpty(parentText))
            {
                ret.FutureResolution.Add(ExtendedModelResult.ParentTextKey, parentText);
                ret.PastResolution.Add(ExtendedModelResult.ParentTextKey, parentText);
            }

            if (((DateTimeResolutionResult)pr.Value).Mod != null)
            {
                ret.Mod = ((DateTimeResolutionResult)pr.Value).Mod;
            }

            if (((DateTimeResolutionResult)pr.Value).TimeZoneResolution != null)
            {
                ret.TimeZoneResolution = ((DateTimeResolutionResult)pr.Value).TimeZoneResolution;
            }

            return ret;
        }

        private DateTimeResolutionResult ParseDateTimeAndTimeAlt(ExtractResult er, DateObject referenceTime)
        {
            var ret = new DateTimeResolutionResult();

            // Original type of the extracted entity
            var subType = ((Dictionary<string, object>)er.Data)[Constants.SubType].ToString();
            var dateTimeEr = new ExtractResult();

            // e.g. {next week Mon} or {Tue}, former--"next week Mon" doesn't contain "context" key
            var hasContext = false;
            ExtractResult contextEr = null;
            if (((Dictionary<string, object>)er.Data).ContainsKey(Constants.Context))
            {
                contextEr = (ExtractResult)((Dictionary<string, object>)er.Data)[Constants.Context];
                if (contextEr.Type.Equals(Constants.ContextType_RelativeSuffix))
                {
                    dateTimeEr.Text = $"{er.Text} {contextEr.Text}";
                }
                else
                {
                    dateTimeEr.Text = $"{contextEr.Text} {er.Text}";
                }

                hasContext = true;
            }
            else
            {
                dateTimeEr.Text = er.Text;
            }

            dateTimeEr.Data = er.Data;
            var dateTimePr = new DateTimeParseResult();

            if (subType == Constants.SYS_DATETIME_DATE)
            {
                dateTimeEr.Type = Constants.SYS_DATETIME_DATE;
                dateTimePr = this.config.DateParser.Parse(dateTimeEr, referenceTime);
            }
            else if (subType == Constants.SYS_DATETIME_TIME)
            {
                if (!hasContext)
                {
                    dateTimeEr.Type = Constants.SYS_DATETIME_TIME;
                    dateTimePr = this.config.TimeParser.Parse(dateTimeEr, referenceTime);
                }
                else if (contextEr.Type == Constants.SYS_DATETIME_DATE || contextEr.Type == Constants.ContextType_RelativePrefix)
                {
                    // For cases:
                    //      Monday 9 am or 11 am
                    //      next 9 am or 11 am
                    dateTimeEr.Type = Constants.SYS_DATETIME_DATETIME;
                    dateTimePr = this.config.DateTimeParser.Parse(dateTimeEr, referenceTime);
                }
                else if (contextEr.Type == Constants.ContextType_AmPm)
                {
                    // For cases: in the afternoon 3 o'clock or 5 o'clock
                    dateTimeEr.Type = Constants.SYS_DATETIME_TIME;
                    dateTimePr = this.config.TimeParser.Parse(dateTimeEr, referenceTime);
                }
            }
            else if (subType == Constants.SYS_DATETIME_DATETIME)
            {
                // "next week Mon 9 am or Tue 1 pm"
                dateTimeEr.Type = Constants.SYS_DATETIME_DATETIME;
                dateTimePr = this.config.DateTimeParser.Parse(dateTimeEr, referenceTime);
            }
            else if (subType == Constants.SYS_DATETIME_TIMEPERIOD)
            {
                if (!hasContext)
                {
                    dateTimeEr.Type = Constants.SYS_DATETIME_TIMEPERIOD;
                    dateTimePr = this.config.TimePeriodParser.Parse(dateTimeEr, referenceTime);
                }
                else if (contextEr.Type == Constants.SYS_DATETIME_DATE || contextEr.Type == Constants.ContextType_RelativePrefix)
                {
                    dateTimeEr.Type = Constants.SYS_DATETIME_DATETIMEPERIOD;
                    dateTimePr = this.config.DateTimePeriodParser.Parse(dateTimeEr, referenceTime);
                }
            }
            else if (subType == Constants.SYS_DATETIME_DATETIMEPERIOD)
            {
                dateTimeEr.Type = Constants.SYS_DATETIME_DATETIMEPERIOD;
                dateTimePr = this.config.DateTimePeriodParser.Parse(dateTimeEr, referenceTime);
            }
            else if (subType == Constants.SYS_DATETIME_DATEPERIOD)
            {
                dateTimeEr.Type = Constants.SYS_DATETIME_DATEPERIOD;
                dateTimePr = this.config.DatePeriodParser.Parse(dateTimeEr, referenceTime);
            }

            if (dateTimePr.Value != null)
            {
                ret.FutureValue = ((DateTimeResolutionResult)dateTimePr.Value).FutureValue;
                ret.PastValue = ((DateTimeResolutionResult)dateTimePr.Value).PastValue;
                ret.Timex = dateTimePr.TimexStr;

                // Create resolution
                ret = GetResolution(er, dateTimePr, ret);

                ret.Success = true;
            }

            return ret;
        }
    }
}
