﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
                    if (pr.TimexStr.EndsWith(TasksModeConstants.WeekEndPrefix))
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
            if (timex.StartsWith(TasksModeConstants.GeneralPeriodPrefix) && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract(TasksModeConstants.PeriodString, timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : string.Empty);
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : string.Empty);
            }
            else if (timex.StartsWith(TasksModeConstants.FuzzyYear) && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract(TasksModeConstants.PeriodString, timex, extracted);
                res.Add("intervalSize", extracted.TryGetValue("amount", out var intervalSize) ? intervalSize : "1");
                res.Add("intervalType", extracted.TryGetValue("dateUnit", out var intervalType) ? intervalType : TasksModeConstants.TimexWeek);
            }
            else if (timex.StartsWith(TasksModeConstants.TimeTimexPrefix) && res.Count > 0)
            {
                res.Add("intervalSize", "1");
                res.Add("intervalType",  TasksModeConstants.TimexDay);
            }

            return res;
        }

        public static string TasksModeTimexIntervalExt(string timex)
        {
            string periodicity;
            if (timex.Contains(Constants.TimexFuzzyWeek))
            {
                periodicity = TasksModeConstants.WeeklyPeriodic;
            }
            else if (timex.Contains(Constants.TimexFuzzyYear))
            {
                periodicity = TasksModeConstants.YearlyPeriodic;
            }
            else if (!timex.EndsWith(TasksModeConstants.WeekEndPrefix) && !timex.EndsWith(TasksModeConstants.WeekDayPrefix))
            {
                periodicity = TasksModeConstants.DailyPeriodic;
            }
            else
            {
                periodicity = string.Empty;
            }

            timex = ExtendSetTimex(timex, periodicity);
            return timex;
        }

        public static DateTimeResolutionResult TasksModeAddResolution(ref DateTimeResolutionResult result, ExtractResult er, DateObject refDate)
        {
            if (result.Timex.EndsWith(TasksModeConstants.WeekEndPrefix))
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
            else if (result.Timex.EndsWith(TasksModeConstants.WeekDayPrefix))
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
            else if (result.Timex.StartsWith(TasksModeConstants.GeneralPeriodPrefix))
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
            else if (result.Timex.StartsWith(TasksModeConstants.FuzzyYear))
            {
                var timexRes = TimexResolver.Resolve(new[] { result.Timex }, refDate);

                string value = timexRes.Values[1].Value;

                var resKey = TimeTypeConstants.DATETIME;

                if (!result.Timex.Contains(TasksModeConstants.TimeTimexPrefix))
                {
                    resKey = TimeTypeConstants.DATE;
                }

                var futureValue = refDate.AddDays(7);

                if (DateTimeFormatUtil.FormatDate(futureValue).Equals(value.Substring(0, 10)) && result.Timex.StartsWith(TasksModeConstants.FuzzyYearAndWeek))
                {
                    if (result.Timex.Contains(TasksModeConstants.TimeTimexPrefix))
                    {
                        if (DateTimeFormatUtil.FormatTime(refDate).CompareTo(value.Substring(11)) <= 0)
                        {
                            value = JoinDateWithValue(refDate, value.Substring(11));
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
            else if (result.Timex.StartsWith(TasksModeConstants.TimeTimexPrefix))
            {
                var timexRes = TimexResolver.Resolve(new[] { result.Timex }, refDate);

                string value = GetStartValue(timexRes);
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
                    { TimeTypeConstants.DATETIME, JoinDateWithValue(resDate, (string)value) },
                };

                result.PastResolution = new Dictionary<string, string>
                {
                    { TimeTypeConstants.DATETIME, JoinDateWithValue(resDate, (string)value) },
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

        internal static string JoinDateWithValue(DateObject resDate, string value)
        {
            return string.Join(" ", DateTimeFormatUtil.FormatDate((DateObject)resDate), (string)value);
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

        internal static string GetStartValue(Resolution timexRes)
        {
            switch (timexRes.Values[0].Timex)
            {
                case TasksModeConstants.Morning: return TasksModeConstants.StringMorningHHMMSS;
                case TasksModeConstants.Afternoon: return TasksModeConstants.StringAfternoonHHMMSS;
                case TasksModeConstants.Evening: return TasksModeConstants.StringEveningHHMMSS;
                case TasksModeConstants.Night: return TasksModeConstants.StringNightHHMMSS;
                default: return timexRes.Values[0].Start;
            }
        }

        // function replaces P1 with P2 when parsing values i.e. every other day at 2pm
        internal static string TasksModeTimexIntervalReplace(string timex)
        {
            timex = timex.Replace(TasksModeConstants.DailyPeriodPrefix, TasksModeConstants.AlternatePeriodPrefix);

            return timex;
        }

        internal static string ExtendSetTimex(string timex, string extTimex)
        {
            return timex + extTimex;
        }

    }
}
