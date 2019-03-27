using System;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    // Currently only Year is enabled as context, we may support Month or Week in the future
    public class DateContext
    {
        public int Year { get; set; } = Constants.InvalidYear;

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
