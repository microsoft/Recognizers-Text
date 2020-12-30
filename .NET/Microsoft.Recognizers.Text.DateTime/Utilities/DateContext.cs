using System;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    // Currently only Year is enabled as context, we may support Month or Week in the future
    public class DateContext
    {
        public int Year { get; set; } = Constants.InvalidYear;

        public static List<DateObject> GetFuturePastDate(bool noYear, DateObject referenceDate, int year, int month, int day)
        {
            // Get future/past date, especially for Feb 29.
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

            return new List<DateObject> { futureDate, pastDate };
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

        // Judge the date is Feb 29th
        private static bool IsFeb29th(int year, int month, int day)
        {
            return month == 2 && day == 29;
        }

        private DateObject SetDateWithContext(DateObject originalDate)
        {
            return new DateObject(Year, originalDate.Month, originalDate.Day);
        }

        private Tuple<DateObject, DateObject> SetDateRangeWithContext(Tuple<DateObject, DateObject> originalDateRange)
        {
            var startDate = SetDateWithContext(originalDateRange.Item1);
            var endDate = SetDateWithContext(originalDateRange.Item2);

            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }
    }
}
