// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public static class TasksModeSetHandler
    {
        public static DateTimeResolutionResult TasksModeResolveSet(ref DateTimeResolutionResult result, string innerTimex, DateTimeParseResult pr = null)
        {
            result.Timex = innerTimex;

            result.FutureValue = result.PastValue = "Set: " + innerTimex;

            if (pr != null)
            {
                DateTimeResolutionResult value = (DateTimeResolutionResult)pr.Value;
                if (value.FutureValue != null)
                {
                    if (pr.TimexStr.EndsWith("WE"))
                    {
                        result.FutureValue = ((Tuple<DateObject, DateObject>)value.FutureValue).Item1;
                        result.PastValue = ((Tuple<DateObject, DateObject>)value.PastValue).Item1;
                    }
                }
            }

            result.Success = true;

            return result;
        }

        public static Dictionary<string, string> TasksModeGenerateResolutionSetParser(Dictionary<string, string> resolutionDic, string mod, string timex)
        {
            var res = new Dictionary<string, string>();

            TasksModeAddAltSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIMEALT, mod, res);
            if (timex.StartsWith("P") && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("period", timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : string.Empty);
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : string.Empty);
            }
            else if (timex.StartsWith("XXXX-") && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract("period", timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : "1");
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : "W");
            }
            else if (timex.StartsWith("T") && res.Count > 0)
            {
                res.Add("intervalSize", "1");
                res.Add("intervalType", "D");
            }

            return res;
        }

        public static string TasksModeTimexIntervalExt(string timex)
        {
            if (timex.Contains(Constants.TimexFuzzyWeek))
            {
                timex = timex + "P1W";
            }
            else if (timex.Contains(Constants.TimexFuzzyYear))
            {
                timex = timex + "P1Y";
            }
            else if (!timex.EndsWith("WE") && !timex.EndsWith("WD"))
            {
                timex = timex + "P1D";
            }

            return timex;
        }

        public static DateTimeResolutionResult TasksModeAddResolution(ref DateTimeResolutionResult result, ExtractResult er, DateObject refDate)
        {
            if (result.Timex.EndsWith("WE"))
            {
                if (refDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    result.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.DATE,
                            DateTimeFormatUtil.FormatDate((DateObject)refDate)
                        },
                    };

                    result.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.DATE,
                            DateTimeFormatUtil.FormatDate((DateObject)refDate)
                        },
                    };
                }
                else
                {
                    var tempdate = refDate.Upcoming(DayOfWeek.Sunday).Date;
                    var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day);
                    result.FutureResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.DATE,
                            DateTimeFormatUtil.FormatDate(dateTimeToSet)
                        },
                    };

                    result.PastResolution = new Dictionary<string, string>
                    {
                        {
                            TimeTypeConstants.DATE,
                            DateTimeFormatUtil.FormatDate(dateTimeToSet)
                        },
                    };
                }
            }
            else if (result.Timex.EndsWith("WD"))
            {
                if (refDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    result.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate.AddDays(2)) },
                    };

                    result.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate.AddDays(2)) },
                    };
                }
                else if (refDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    result.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate.AddDays(1)) },
                    };

                    result.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate.AddDays(1)) },
                    };
                }
                else
                {
                    result.FutureResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate) },
                    };

                    result.PastResolution = new Dictionary<string, string>
                    {
                        { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate) },
                    };
                }
            }
            else if (result.Timex.StartsWith("P"))
            {
                result.FutureResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate) },
                };

                result.PastResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.DATE, DateTimeFormatUtil.FormatDate((DateObject)refDate) },
                };
            }
            else if (result.Timex.StartsWith("XXXX-"))
            {
                var timexRes = TimexResolver.Resolve(new[] { result.Timex }, refDate);

                string value = timexRes.Values[1].Value;

                var resKey = TimeTypeConstants.DATETIME;

                if (!result.Timex.Contains("T"))
                {
                    resKey = TimeTypeConstants.DATE;
                }

                var futureValue = refDate.AddDays(7);

                if (DateTimeFormatUtil.FormatDate(futureValue).Equals(value.Substring(0, 10)) && result.Timex.StartsWith("XXXX-WXX-"))
                {
                    if (result.Timex.Contains("T"))
                    {
                        if (DateTimeFormatUtil.FormatTime(refDate).CompareTo(value.Substring(11)) <= 0)
                        {
                            value = DateTimeFormatUtil.FormatDate(refDate) + " " + value.Substring(11);
                        }
                    }
                    else
                    {
                        value = DateTimeFormatUtil.FormatDate(refDate);
                    }
                }

                result.FutureResolution = new Dictionary<string, string>
                {
                    { resKey, (string)value },
                };

                result.PastResolution = new Dictionary<string, string>
                {
                    { resKey, (string)value },
                };
            }
            else if (result.Timex.StartsWith("T"))
            {
                var timexRes = TimexResolver.Resolve(new[] { result.Timex }, refDate);

                string value = timexRes.Values[0].Start;
                if (value == null)
                {
                    value = timexRes.Values[0].Value;
                }

                DateObject resDate = refDate;
                if (DateTimeFormatUtil.FormatTime(resDate).CompareTo(value) > 0)
                {
                    resDate = resDate.AddDays(1);
                }

                result.FutureResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.DATETIME, DateTimeFormatUtil.FormatDate((DateObject)resDate) + " " + (string)value },
                };

                result.PastResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.DATETIME, DateTimeFormatUtil.FormatDate((DateObject)resDate) + " " + (string)value },
                };
            }
            else
            {
                result.FutureResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.SET, (string)result.FutureValue },
                };

                result.PastResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.SET, (string)result.PastValue },
                };

            }

            return result;
        }

        internal static void TasksModeAddAltSingleDateTimeToResolution(Dictionary<string, string> resolutionDic, string type, string mod,
                                                            Dictionary<string, string> res)
        {
            if (resolutionDic.ContainsKey(TimeTypeConstants.DATE))
            {
                res.Add("setTypename", TimeTypeConstants.DATE);
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.DATETIME))
            {
                res.Add("setTypename", Constants.SYS_DATETIME_DATETIME);
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATETIME, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.TIME))
            {
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.TIME, mod, res);
            }
        }
    }
}
