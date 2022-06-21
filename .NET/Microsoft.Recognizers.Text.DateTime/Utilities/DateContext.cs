// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    // Currently only Year is enabled as context, we may support Month or Week in the future
    public class DateContext
    {
        public int Year { get; set; } = Constants.InvalidYear;

        // Generate future/past date for cases without specific year like "Feb 29th"
        public static (DateObject future, DateObject past) GenerateDates(bool noYear, DateObject referenceDate, int year, int month, int day)
        {
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var futureYear = year;
            var pastYear = year;
            if (noYear)
            {
                if (IsFeb29th(year, month, day))
                {
                    if (DateObject.IsLeapYear(year))
                    {
                        if (futureDate < referenceDate)
                        {
                            futureDate = DateObject.MinValue.SafeCreateFromValue(futureYear + 4, month, day);
                        }
                        else
                        {
                            pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear - 4, month, day);
                        }
                    }
                    else
                    {
                        pastYear = pastYear >> 2 << 2;
                        if (!DateObject.IsLeapYear(pastYear))
                        {
                            pastYear -= 4;
                        }

                        futureYear = pastYear + 4;
                        if (!DateObject.IsLeapYear(futureYear))
                        {
                            futureYear += 4;
                        }

                        futureDate = DateObject.MinValue.SafeCreateFromValue(futureYear, month, day);
                        pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear, month, day);
                    }
                }
                else
                {
                    if (futureDate < referenceDate && !futureDate.IsDefaultValue())
                    {
                        futureDate = DateObject.MinValue.SafeCreateFromValue(year + 1, month, day);
                    }

                    if (pastDate >= referenceDate && !pastDate.IsDefaultValue())
                    {
                        pastDate = DateObject.MinValue.SafeCreateFromValue(year - 1, month, day);
                    }
                }
            }

            return (futureDate, pastDate);
        }

        // This method is to ensure the begin date is less than the end date.
        // As DateContext only supports common Year as context, so it subtracts one year from beginDate. @TODO problematic in other usages.
        public static DateObject SwiftDateObject(DateObject beginDate, DateObject endDate)
        {
            if (beginDate > endDate)
            {
                beginDate = beginDate.AddYears(-1);
            }

            return beginDate;
        }

        public static bool IsFeb29th(DateObject date)
        {
            return date.Month == 2 && date.Day == 29;
        }

        public static bool IsFeb29th(int year, int month, int day)
        {
            return month == 2 && day == 29;
        }

        // Used in CJK implementation
        public static DateObject ComputeDate(int cardinal, int weekday, int month, int year)
        {
            var firstDay = DateObject.MinValue.SafeCreateFromValue(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek)weekday);
            if (weekday == 0)
            {
                weekday = 7;
            }

            if (weekday < (int)firstDay.DayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek)weekday);
            }

            return firstWeekday.AddDays(7 * (cardinal - 1));
        }

        // This method is to ensure the month of end date is same with the begin date in patterns like "april 9th through seventeenth".
        public (DateTimeParseResult pr1, DateTimeParseResult pr2) SyncMonth(DateTimeParseResult pr1, DateTimeParseResult pr2)
        {
            var pr1Value = (DateTimeResolutionResult)pr1.Value;
            var pr2Value = (DateTimeResolutionResult)pr2.Value;
            var timexList1 = pr1.TimexStr.Split(Constants.DateTimexConnector[0]);
            var timexList2 = pr2.TimexStr.Split(Constants.DateTimexConnector[0]);
            (pr1.TimexStr, pr1.Value) = SyncDateEntityResolutionMonth(pr1Value, pr2Value, timexList1, timexList2);
            (pr2.TimexStr, pr2.Value) = SyncDateEntityResolutionMonth(pr2Value, pr1Value, timexList2, timexList1, false);

            return (pr1, pr2);
        }

        // This method is to ensure the year of begin date is same with the end date in no year situation.
        public (DateTimeParseResult pr1, DateTimeParseResult pr2) SyncYear(DateTimeParseResult pr1, DateTimeParseResult pr2)
        {
            if (IsEmpty())
            {
                int futureYear;
                int pastYear;
                if (IsFeb29th((DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue))
                {
                    futureYear = ((DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue).Year;
                    pastYear = ((DateObject)((DateTimeResolutionResult)pr1.Value).PastValue).Year;
                    pr2.Value = SyncDateEntityResolutionInFeb29th((DateTimeResolutionResult)pr2.Value, futureYear, pastYear);
                }
                else if (IsFeb29th((DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue))
                {
                    futureYear = ((DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue).Year;
                    pastYear = ((DateObject)((DateTimeResolutionResult)pr2.Value).PastValue).Year;
                    pr1.Value = SyncDateEntityResolutionInFeb29th((DateTimeResolutionResult)pr1.Value, futureYear, pastYear);
                }
            }

            return (pr1, pr2);
        }

        public DateTimeResolutionResult SyncDateEntityResolutionInFeb29th(DateTimeResolutionResult resolutionResult, int futureYear, int pastYear)
        {
            resolutionResult.FutureValue = SetDateWithContext((DateObject)resolutionResult.FutureValue, futureYear);
            resolutionResult.PastValue = SetDateWithContext((DateObject)resolutionResult.PastValue, pastYear);

            return resolutionResult;
        }

        public DateTimeParseResult ProcessDateEntityParsingResult(DateTimeParseResult originalResult)
        {
            if (!IsEmpty())
            {
                originalResult.TimexStr = TimexUtility.SetTimexWithContext(originalResult.TimexStr, this);
                originalResult.Value = ProcessDateEntityResolution((DateTimeResolutionResult)originalResult.Value);
            }

            return originalResult;
        }

        public DateTimeResolutionResult ProcessDateEntityResolution(DateTimeResolutionResult resolutionResult)
        {
            if (!IsEmpty())
            {
                resolutionResult.Timex = TimexUtility.SetTimexWithContext(resolutionResult.Timex, this);
                resolutionResult.FutureValue = SetDateWithContext((DateObject)resolutionResult.FutureValue);
                resolutionResult.PastValue = SetDateWithContext((DateObject)resolutionResult.PastValue);
            }

            return resolutionResult;
        }

        public DateTimeResolutionResult ProcessDatePeriodEntityResolution(DateTimeResolutionResult resolutionResult)
        {
            if (!IsEmpty())
            {
                resolutionResult.Timex = TimexUtility.SetTimexWithContext(resolutionResult.Timex, this);
                resolutionResult.FutureValue = SetDateRangeWithContext((Tuple<DateObject, DateObject>)resolutionResult.FutureValue);
                resolutionResult.PastValue = SetDateRangeWithContext((Tuple<DateObject, DateObject>)resolutionResult.PastValue);
            }

            return resolutionResult;
        }

        public bool IsEmpty()
        {
            return this.Year == Constants.InvalidYear;
        }

        private DateObject SetDateWithContext(DateObject originalDate, int year = -1)
        {
            if (!originalDate.IsDefaultValue())
            {
                return DateObject.MinValue.SafeCreateFromValue(year == -1 ? Year : year, originalDate.Month, originalDate.Day);
            }

            return originalDate;
        }

        private Tuple<DateObject, DateObject> SetDateRangeWithContext(Tuple<DateObject, DateObject> originalDateRange)
        {
            var startDate = SetDateWithContext(originalDateRange.Item1);
            var endDate = SetDateWithContext(originalDateRange.Item2);

            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }

        private (string, DateTimeResolutionResult) SyncDateEntityResolutionMonth(DateTimeResolutionResult value1, DateTimeResolutionResult value2, string[] timexList1, string[] timexList2, bool isFirstDate = true)
        {
            if (timexList1[1].Equals(Constants.TimexFuzzyMonth, StringComparison.Ordinal) && !timexList2[1].Equals(Constants.TimexFuzzyMonth, StringComparison.Ordinal))
            {
                var futureValue1 = (DateObject)value1.FutureValue;
                var futureValue2 = (DateObject)value2.FutureValue;
                var pastValue1 = (DateObject)value1.PastValue;
                var pastValue2 = (DateObject)value2.PastValue;

                // If the ordinal number is the first date and it represents a day larger than the second date (e.g. "from the twenty-seventh to the fourth of april"),
                // it is resolved according to the reference date, otherwise it is resolved to the month of the other date.
                if (!(isFirstDate && futureValue2.Day <= futureValue1.Day))
                {
                    int swift = isFirstDate || futureValue1.Day > futureValue2.Day ? 0 : 1;
                    value1.FutureValue = DateObject.MinValue.SafeCreateFromValue(futureValue2.Year, futureValue2.Month, futureValue1.Day).AddMonths(swift);
                    value1.PastValue = DateObject.MinValue.SafeCreateFromValue(pastValue2.Year, pastValue2.Month, pastValue1.Day).AddMonths(swift);
                    timexList1[1] = ((DateObject)value1.FutureValue).Month.ToString("D2");
                }
            }

            var timexStr = string.Join(Constants.DateTimexConnector, timexList1);
            return (timexStr, value1);
        }
    }
}
