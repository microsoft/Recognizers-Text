// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Utilities
{
    public class ModAndDateResult
    {
        public ModAndDateResult(DateObject beginDate, DateObject endDate, string mod, List<DateObject> dateList)
        {
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Mod = mod;
            this.DateList = dateList;
        }

        public ModAndDateResult(DateObject beginDate, DateObject endDate)
        {
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Mod = string.Empty;
            this.DateList = null;
        }

        public DateObject BeginDate { get; set; }

        public DateObject EndDate { get; set; }

        public string Mod { get; set; }

        public List<DateObject> DateList { get; set; }

        public static ModAndDateResult GetModAndDate(DateObject beginDate, DateObject endDate, DateObject referenceDate, string timex, bool future)
        {
            DateObject beginDateResult = beginDate;
            DateObject endDateResult = endDate;
            var isBusinessDay = timex.EndsWith(Constants.TimexBusinessDay, StringComparison.Ordinal);
            var businessDayCount = 0;
            List<DateObject> dateList = null;

            if (isBusinessDay)
            {
                businessDayCount = int.Parse(timex.Substring(1, timex.Length - 3), CultureInfo.InvariantCulture);
            }

            if (future)
            {
                string mod = Constants.AFTER_MOD;

                // For future the beginDate should add 1 first
                if (isBusinessDay)
                {
                    beginDateResult = DurationParsingUtil.GetNextBusinessDay(referenceDate);
                    endDateResult = DurationParsingUtil.GetNthBusinessDay(beginDateResult, businessDayCount - 1, true, out dateList);
                    endDateResult = endDateResult.AddDays(1);
                    return new ModAndDateResult(beginDateResult, endDateResult, mod, dateList);
                }
                else
                {
                    beginDateResult = referenceDate.AddDays(1);
                    endDateResult = DurationParsingUtil.ShiftDateTime(timex, beginDateResult, true);
                    return new ModAndDateResult(beginDateResult, endDateResult, mod, null);
                }
            }
            else
            {
                const string mod = Constants.BEFORE_MOD;

                if (isBusinessDay)
                {
                    endDateResult = DurationParsingUtil.GetNextBusinessDay(endDateResult, false);
                    beginDateResult = DurationParsingUtil.GetNthBusinessDay(endDateResult, businessDayCount - 1, false, out dateList);
                    endDateResult = endDateResult.AddDays(1);
                    return new ModAndDateResult(beginDateResult, endDateResult, mod, dateList);
                }
                else
                {
                    beginDateResult = DurationParsingUtil.ShiftDateTime(timex, endDateResult, false);
                    return new ModAndDateResult(beginDateResult, endDateResult, mod, null);
                }
            }
        }
    }
}
