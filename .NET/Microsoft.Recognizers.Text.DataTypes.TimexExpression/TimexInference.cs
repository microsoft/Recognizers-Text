// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexInference
    {
        public static HashSet<string> Infer(TimexProperty timexProperty)
        {
            var types = new HashSet<string>();

            if (IsPresent(timexProperty))
            {
                types.Add(Constants.TimexTypes.Present);
            }

            if (IsDefinite(timexProperty))
            {
                types.Add(Constants.TimexTypes.Definite);
            }

            if (IsDate(timexProperty))
            {
                types.Add(Constants.TimexTypes.Date);
            }

            if (IsDateRange(timexProperty))
            {
                types.Add(Constants.TimexTypes.DateRange);
            }

            if (IsDuration(timexProperty))
            {
                types.Add(Constants.TimexTypes.Duration);
            }

            if (IsTime(timexProperty))
            {
                types.Add(Constants.TimexTypes.Time);
            }

            if (IsTimeRange(timexProperty))
            {
                types.Add(Constants.TimexTypes.TimeRange);
            }

            if (types.Contains(Constants.TimexTypes.Present))
            {
                types.Add(Constants.TimexTypes.Date);
                types.Add(Constants.TimexTypes.Time);
            }

            if (types.Contains(Constants.TimexTypes.Time) && types.Contains(Constants.TimexTypes.Duration))
            {
                types.Add(Constants.TimexTypes.TimeRange);
            }

            if (types.Contains(Constants.TimexTypes.Date) && types.Contains(Constants.TimexTypes.Time))
            {
                types.Add(Constants.TimexTypes.DateTime);
            }

            if (types.Contains(Constants.TimexTypes.Date) && types.Contains(Constants.TimexTypes.Duration))
            {
                types.Add(Constants.TimexTypes.DateRange);
            }

            if (types.Contains(Constants.TimexTypes.DateTime) && types.Contains(Constants.TimexTypes.Duration))
            {
                types.Add(Constants.TimexTypes.DateTimeRange);
            }

            if (types.Contains(Constants.TimexTypes.Date) && types.Contains(Constants.TimexTypes.TimeRange))
            {
                types.Add(Constants.TimexTypes.DateTimeRange);
            }

            return types;
        }

        private static bool IsPresent(TimexProperty timexProperty)
        {
            return timexProperty.Now == true;
        }

        private static bool IsDuration(TimexProperty timexProperty)
        {
            return timexProperty.Years != null || timexProperty.Months != null || timexProperty.Weeks != null || timexProperty.Days != null ||
                   timexProperty.Hours != null || timexProperty.Minutes != null || timexProperty.Seconds != null;
        }

        private static bool IsTime(TimexProperty timexProperty)
        {
            return timexProperty.Hour != null && timexProperty.Minute != null && timexProperty.Second != null;
        }

        private static bool IsDate(TimexProperty timexProperty)
        {
            return (timexProperty.Month != null && timexProperty.DayOfMonth != null) || timexProperty.DayOfWeek != null;
        }

        private static bool IsTimeRange(TimexProperty timexProperty)
        {
            return timexProperty.PartOfDay != null;
        }

        private static bool IsDateRange(TimexProperty timexProperty)
        {
            return (timexProperty.Year != null && timexProperty.DayOfMonth == null) ||
                   (timexProperty.Year != null && timexProperty.Month != null && timexProperty.DayOfMonth == null) ||
                   (timexProperty.Month != null && timexProperty.DayOfMonth == null) ||
                   timexProperty.Season != null || timexProperty.WeekOfYear != null || timexProperty.WeekOfMonth != null;
        }

        private static bool IsDefinite(TimexProperty timexProperty)
        {
            return timexProperty.Year != null && timexProperty.Month != null && timexProperty.DayOfMonth != null;
        }
    }
}
