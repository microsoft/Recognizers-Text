// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexValue
    {
        public static string DateValue(TimexProperty timexProperty)
        {
            if (timexProperty.Year != null && timexProperty.Month != null && timexProperty.DayOfMonth != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timexProperty.Year, 4)}-{TimexDateHelpers.FixedFormatNumber(timexProperty.Month, 2)}-{TimexDateHelpers.FixedFormatNumber(timexProperty.DayOfMonth, 2)}";
            }

            return string.Empty;
        }

        public static string TimeValue(TimexProperty timexProperty, DateTime date)
        {
            if (timexProperty.Hour != null && timexProperty.Minute != null && timexProperty.Second != null)
            {
                if (date.Kind == DateTimeKind.Utc)
                {
                    var timeString = $"{TimexDateHelpers.FixedFormatNumber(timexProperty.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Minute, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Second, 2)}";
                    var tempDateTime = DateTime.Parse(timeString, CultureInfo.InvariantCulture);
                    return tempDateTime.ToUniversalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    return $"{TimexDateHelpers.FixedFormatNumber(timexProperty.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Minute, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Second, 2)}";
                }
            }

            return string.Empty;
        }

        public static string DatetimeValue(TimexProperty timexProperty, DateTime date)
        {
            return $"{DateValue(timexProperty)} {TimeValue(timexProperty, date)}";
        }

        public static string DurationValue(TimexProperty timexProperty)
        {
            decimal duration = 0;
            if (timexProperty.Years != null)
            {
                duration += 31536000 * timexProperty.Years ?? 0;
            }

            if (timexProperty.Months != null)
            {
                duration += 2592000 * timexProperty.Months ?? 0;
            }

            if (timexProperty.Weeks != null)
            {
                duration += 604800 * timexProperty.Weeks ?? 0;
            }

            if (timexProperty.Days != null)
            {
                duration += 86400 * timexProperty.Days ?? 0;
            }

            if (timexProperty.Hours != null)
            {
                duration += 3600 * timexProperty.Hours ?? 0;
            }

            if (timexProperty.Minutes != null)
            {
                duration += 60 * timexProperty.Minutes ?? 0;
            }

            if (timexProperty.Seconds != null)
            {
                duration += timexProperty.Seconds ?? 0;
            }

            return duration.ToString(CultureInfo.InvariantCulture);
        }
    }
}
