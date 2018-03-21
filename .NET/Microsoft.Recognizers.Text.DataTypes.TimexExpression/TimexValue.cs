// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexValue
    {
        public static string DateValue(TimexProperty obj)
        {
            if (obj.Year != null && obj.Month != null && obj.DayOfMonth != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(obj.Year, 4)}-{TimexDateHelpers.FixedFormatNumber(obj.Month, 2)}-{TimexDateHelpers.FixedFormatNumber(obj.DayOfMonth, 2)}";
            }

            return string.Empty;
        }

        public static string TimeValue(TimexProperty obj)
        {
            if (obj.Hour != null && obj.Minute != null && obj.Second != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(obj.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(obj.Minute, 2)}:{TimexDateHelpers.FixedFormatNumber(obj.Second, 2)}";
            }

            return string.Empty;
        }

        public static string DatetimeValue(TimexProperty obj)
        {
            return $"{DateValue(obj)} {TimeValue(obj)}";
        }

        public static string DurationValue(TimexProperty obj)
        {
            if (obj.Years != null)
            {
                return (31536000 * obj.Years).ToString();
            }

            if (obj.Months != null)
            {
                return (2592000 * obj.Months).ToString();
            }

            if (obj.Weeks != null)
            {
                return (604800 * obj.Weeks).ToString();
            }

            if (obj.Days != null)
            {
                return (86400 * obj.Days).ToString();
            }

            if (obj.Hours != null)
            {
                return (3600 * obj.Hours).ToString();
            }

            if (obj.Minutes != null)
            {
                return (60 * obj.Minutes).ToString();
            }

            if (obj.Seconds != null)
            {
                return obj.Seconds.ToString();
            }

            return string.Empty;
        }
    }
}
