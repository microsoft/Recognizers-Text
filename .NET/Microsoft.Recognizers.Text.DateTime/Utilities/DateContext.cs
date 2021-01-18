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
    }
}
