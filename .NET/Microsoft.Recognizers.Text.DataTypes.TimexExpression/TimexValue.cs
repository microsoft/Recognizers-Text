// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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

        public static string TimeValue(TimexProperty timexProperty)
        {
            if (timexProperty.Hour != null && timexProperty.Minute != null && timexProperty.Second != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timexProperty.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Minute, 2)}:{TimexDateHelpers.FixedFormatNumber(timexProperty.Second, 2)}";
            }

            return string.Empty;
        }

        public static string DatetimeValue(TimexProperty timexProperty)
        {
            return $"{DateValue(timexProperty)} {TimeValue(timexProperty)}";
        }

        public static string DurationValue(TimexProperty timexProperty)
        {
            if (timexProperty.Years != null)
            {
                return (31536000 * timexProperty.Years).ToString();
            }

            if (timexProperty.Months != null)
            {
                return (2592000 * timexProperty.Months).ToString();
            }

            if (timexProperty.Weeks != null)
            {
                return (604800 * timexProperty.Weeks).ToString();
            }

            if (timexProperty.Days != null)
            {
                return (86400 * timexProperty.Days).ToString();
            }

            if (timexProperty.Hours != null)
            {
                return (3600 * timexProperty.Hours).ToString();
            }

            if (timexProperty.Minutes != null)
            {
                return (60 * timexProperty.Minutes).ToString();
            }

            if (timexProperty.Seconds != null)
            {
                return timexProperty.Seconds.ToString();
            }

            return string.Empty;
        }
    }
}
