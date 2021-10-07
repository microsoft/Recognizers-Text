// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
            var timexList = new List<string> { };
            if (timex.Years != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Year, timex.Years ?? Constants.InvalidValue));
            }

            if (timex.Months != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Month, timex.Months ?? Constants.InvalidValue));
            }

            if (timex.Weeks != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Week, timex.Weeks ?? Constants.InvalidValue));
            }

            if (timex.Days != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Day, timex.Days ?? Constants.InvalidValue));
            }

            if (timex.Hours != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Hour, timex.Hours ?? Constants.InvalidValue));
            }

            if (timex.Minutes != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Minute, timex.Minutes ?? Constants.InvalidValue));
            }

            if (timex.Seconds != null)
            {
                timexList.Add(TimexHelpers.GenerateDurationTimex(TimexUnit.Second, timex.Seconds ?? Constants.InvalidValue));
            }

            return TimexHelpers.GenerateCompoundDurationTimex(timexList);
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
            return TimexHelpers.GenerateDateTimex(timex.Year ?? Constants.InvalidValue, timex.WeekOfYear ?? (timex.Month ?? Constants.InvalidValue), timex.DayOfWeek != null ? timex.DayOfWeek.Value : timex.DayOfMonth ?? Constants.InvalidValue, timex.WeekOfMonth ?? Constants.InvalidValue, timex.DayOfWeek != null);
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

            if (timex.Year != null && timex.Month != null && timex.WeekOfMonth != null)
            {
                return $"{TimexDateHelpers.FixedFormatNumber(timex.Year, 4)}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-W{TimexDateHelpers.FixedFormatNumber(timex.WeekOfMonth, 2)}";
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
                return $"{Constants.TimexFuzzyYear}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-{Constants.TimexFuzzyWeek}-{timex.WeekOfMonth}-{timex.DayOfWeek}";
            }

            if (timex.Month != null && timex.WeekOfMonth != null)
            {
                return $"{Constants.TimexFuzzyYear}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}-W{timex.WeekOfMonth?.ToString("D2", CultureInfo.InvariantCulture)}";
            }

            if (timex.Month != null)
            {
                return $"{Constants.TimexFuzzyYear}-{TimexDateHelpers.FixedFormatNumber(timex.Month, 2)}";
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
