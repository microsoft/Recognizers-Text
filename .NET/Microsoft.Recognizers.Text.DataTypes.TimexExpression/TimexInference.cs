// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public static class TimexInference
    {
        public static HashSet<string> Infer(TimexProperty obj)
        {
            var types = new HashSet<string>();

            if (IsPresent(obj))
            {
                types.Add(Constants.TimexTypes.Present);
            }

            if (IsDefinite(obj))
            {
                types.Add(Constants.TimexTypes.Definite);
            }

            if (IsDate(obj))
            {
                types.Add(Constants.TimexTypes.Date);
            }

            if (IsDateRange(obj))
            {
                types.Add(Constants.TimexTypes.DateRange);
            }

            if (IsDuration(obj))
            {
                types.Add(Constants.TimexTypes.Duration);
            }

            if (IsTime(obj))
            {
                types.Add(Constants.TimexTypes.Time);
            }

            if (IsTimeRange(obj))
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

        private static bool IsPresent(TimexProperty obj)
        {
            return obj.Now == true;
        }

        private static bool IsDuration(TimexProperty obj)
        {
            return obj.Years != null || obj.Months != null || obj.Weeks != null || obj.Days != null ||
                   obj.Hours != null || obj.Minutes != null || obj.Seconds != null;
        }

        private static bool IsTime(TimexProperty obj)
        {
            return obj.Hour != null && obj.Minute != null && obj.Second != null;
        }

        private static bool IsDate(TimexProperty obj)
        {
            return (obj.Month != null && obj.DayOfMonth != null) || obj.DayOfWeek != null;
        }

        private static bool IsTimeRange(TimexProperty obj)
        {
            return obj.PartOfDay != null;
        }

        private static bool IsDateRange(TimexProperty obj)
        {
            return (obj.Year != null && obj.DayOfMonth == null) ||
                   (obj.Year != null && obj.Month != null && obj.DayOfMonth == null) ||
                   (obj.Month != null && obj.DayOfMonth == null) ||
                   obj.Season != null || obj.WeekOfYear != null || obj.WeekOfMonth != null;
        }

        private static bool IsDefinite(TimexProperty obj)
        {
            return obj.Year != null && obj.Month != null && obj.DayOfMonth != null;
        }
    }
}
