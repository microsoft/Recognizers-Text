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

            result.FutureValue = result.PastValue = ExtendSetTimex(TasksModeConstants.KeySet, innerTimex);

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
            if (timex.StartsWith(Constants.GeneralPeriodPrefix) && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract(TasksModeConstants.PeriodString, timex, extracted);
                res.Add(TasksModeConstants.KeyIntSize, extracted.TryGetValue(TasksModeConstants.AmountString, out var intervalSize) ? intervalSize : string.Empty);
                res.Add(TasksModeConstants.KeyIntType, extracted.TryGetValue(TasksModeConstants.DateUnitString, out var intervalType) ? intervalType : string.Empty);
            }
            else if (timex.StartsWith(TasksModeConstants.FuzzyYear) && res.Count > 0)
            {
                var extracted = new Dictionary<string, string>();
                TimexRegex.Extract(TasksModeConstants.PeriodString, timex, extracted);
                res.Add(TasksModeConstants.KeyIntSize, extracted.TryGetValue(TasksModeConstants.AmountString, out var intervalSize) ? intervalSize : "1");
                res.Add(TasksModeConstants.KeyIntType, extracted.TryGetValue(TasksModeConstants.DateUnitString, out var intervalType) ? intervalType : Constants.TimexWeek);
            }
            else if (timex.StartsWith(Constants.TimeTimexPrefix) && res.Count > 0)
            {
                res.Add(TasksModeConstants.KeyIntSize, "1");
                res.Add(TasksModeConstants.KeyIntType,  Constants.TimexDay);
            }

            return res;
        }

        public static string TasksModeTimexIntervalExt(string timex)
        {
            string periodicity;
            if (timex.Contains(Constants.TimexFuzzyWeek))
            {
                periodicity = TasksModeConstants.WeeklyPeriodSuffix;
            }
            else if (timex.Contains(Constants.TimexFuzzyYear))
            {
                periodicity = TasksModeConstants.YearlyPeriodSuffix;
            }
            else if (!timex.EndsWith(TasksModeConstants.WeekEndPrefix) && !timex.EndsWith(TasksModeConstants.WeekDayPrefix))
            {
                periodicity = TasksModeConstants.PeriodDaySuffix;
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
            else if (result.Timex.StartsWith(Constants.GeneralPeriodPrefix))
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

                if (!result.Timex.Contains(Constants.TimeTimexPrefix))
                {
                    resKey = TimeTypeConstants.DATE;
                }

                var futureValue = refDate.AddDays(7);

                // value = "09-04-2022 19:30" to extract only date substring from value used value[0:10].
                if (DateTimeFormatUtil.FormatDate(futureValue).Equals(value.Substring(TasksModeConstants.IntDateStartIdx, TasksModeConstants.IntDateEndIdx)) && result.Timex.StartsWith(TasksModeConstants.FuzzyYearAndWeek))
                {
                    if (result.Timex.Contains(Constants.TimeTimexPrefix))
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
            else if (result.Timex.StartsWith(Constants.TimeTimexPrefix))
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

        // function used for replacing "every" with "this" in string for parsing text i.e "19th of every month". (only with month duration)
        public static string ReplaceValueInTextWithFutTerm(string text, string value, List<string> thisTermList)
        {
            value = value.Trim();

            // the function should replace value with first term of list in text, It must agree with "month".
            string thisTerm = thisTermList[0];
            text = text.Replace(value, thisTerm);
            return text;
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
                res.Add(TasksModeConstants.KeySetTypeName, TimeTypeConstants.DATE);
                MergedParserUtil.AddSingleDateTimeToResolution(resolutionDic, TimeTypeConstants.DATE, mod, res);
            }
            else if (resolutionDic.ContainsKey(TimeTypeConstants.DATETIME))
            {
                res.Add(TasksModeConstants.KeySetTypeName, Constants.SYS_DATETIME_DATETIME);
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
                case Constants.Morning: return TasksModeConstants.StringMorningHHMMSS;
                case Constants.Afternoon: return TasksModeConstants.StringAfternoonHHMMSS;
                case Constants.Evening: return TasksModeConstants.StringEveningHHMMSS;
                case Constants.Night: return TasksModeConstants.StringNightHHMMSS;
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
