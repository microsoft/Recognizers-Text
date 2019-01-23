// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexFormat
    {
        public static string Format(TimexProperty timex)
        {
            var types = timex.Types.Count != 0 ? timex.Types : TimexInference.Infer(timex);

            if (types.Contains(Constants.TimexTypes.Present))
            {
                return "PRESENT_REF";
            }

            if ((types.Contains(Constants.TimexTypes.DateTimeRange) || types.Contains(Constants.TimexTypes.DateRange) ||
                 types.Contains(Constants.TimexTypes.TimeRange)) && types.Contains(Constants.TimexTypes.Duration))
            {
                var range = TimexHelpers.ExpandDateTimeRange(timex);
                return $"({Format(range.Start)},{Format(range.End)},{Format(range.Duration)})";
            }

            if (types.Contains(Constants.TimexTypes.DateTimeRange))
            {
                return $"{FormatDate(timex)}{FormatTimeRange(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.DateRange))
            {
                return $"{FormatDateRange(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.TimeRange))
            {
                return $"{FormatTimeRange(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.DateTime))
            {
                return $"{FormatDate(timex)}{FormatTime(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.Duration))
            {
                return $"{FormatDuration(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.Date))
            {
                return $"{FormatDate(timex)}";
            }

            if (types.Contains(Constants.TimexTypes.Time))
            {
                return $"{FormatTime(timex)}";
            }

            return string.Empty;
        }

        private static string FormatDuration(TimexProperty timex)
        {
            if (timex.Years != null)
            {
                return $"P{timex.Years}Y";
            }

            if (timex.Months != null)
            {
                return $"P{timex.Months}M";
            }

            if (timex.Weeks != null)
            {
                return $"P{timex.Weeks}W";
            }

            if (timex.Days != null)
            {
                return $"P{timex.Days}D";
            }

            if (timex.Hours != null)
            {
                return $"PT{timex.Hours}H";
            }

            if (timex.Minutes != null)
            {
                return $"PT{timex.Minutes}M";
            }

            if (timex.Seconds != null)
            {
                return $"PT{timex.Seconds}S";
            }

            return string.Empty;
        }

        private static string FormatTime(TimexProperty timex)
        {
            if (timex.Minute == 0 && timex.Second == 0)
            {
                return $"T{TimexDateHelpers.FixedFormatNumber(timex.Hour, 2)}";
            }

            if (timex.Second == 0)
            {
                return $"T{TimexDateHelpers.FixedFormatNumber(timex.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(timex.Minute, 2)}";
            }

            return $"T{TimexDateHelpers.FixedFormatNumber(timex.Hour, 2)}:{TimexDateHelpers.FixedFormatNumber(timex.Minute, 2)}:{TimexDateHelpers.FixedFormatNumber(timex.Second, 2)}";
        }

        private static string FormatDate(TimexProperty timex)
        {
            if (timex.Year != null && timex.Month != null && timex.DayOfMonth != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-{TimexDateHelpers.FixedFormatNumber(timex.DayOfMonth, 2)}";
            }

            if (timex.Month != null && timex.DayOfMonth != null)
            {
                return $"XXXX-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-{TimexDateHelpers.FixedFormatNumber(timex.DayOfMonth, 2)}";
            }

            if (timex.DayOfWeek != null)
            {
                return $"XXXX-WXX-{timex.DayOfWeek}";
            }

            return string.Empty;
        }

        private static string FormatDateRange(TimexProperty timex)
        {
            if (timex.Year != null && timex.WeekOfYear != null && timex.Weekend != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-W{TimexDateHelpers.FixedFormatNumber(timex.WeekOfYear, 2)}-WE";
            }

            if (timex.Year != null && timex.WeekOfYear != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-W{TimexDateHelpers.FixedFormatNumber(timex.WeekOfYear, 2)}";
            }

            if (timex.Year != null && timex.Season != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-{timex.Season}";
            }

            if (timex.Season != null)
            {
                return $"{timex.Season}";
            }

            if (timex.Year != null && timex.Month != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}";
            }

            if (timex.Year != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}";
            }

            if (timex.Month != null && timex.WeekOfMonth != null && timex.DayOfWeek != null)
            {
                return $"XXXX-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-WXX-{timex.WeekOfMonth}-{timex.DayOfWeek}";
            }

            if (timex.Month != null && timex.WeekOfMonth != null)
            {
                return $"XXXX-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-WXX-{timex.WeekOfMonth}";
            }

            if (timex.Month != null)
            {
                return $"XXXX-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}";
            }

            return string.Empty;
        }

        private static string FormatTimeRange(TimexProperty timex)
        {
            if (timex.PartOfDay != null)
            {
                return $"T{timex.PartOfDay}";
            }

            return string.Empty;
        }
    }
}
