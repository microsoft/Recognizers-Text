using System;
using System.Collections.Generic;
using System.Text;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimexUtility
    {
        public static string GenerateDatePeriodTimex(DateObject begin, DateObject end, DatePeriodTimexType timexType)
        {
            var datePeriodTimex = string.Empty;

            if (timexType == DatePeriodTimexType.ByDay)
            {
                datePeriodTimex = $"P{(end - begin).TotalDays}D";
            }
            else if (timexType == DatePeriodTimexType.ByWeek)
            {
                datePeriodTimex = $"P{(end - begin).TotalDays / 7}W";
            }
            else if (timexType == DatePeriodTimexType.ByMonth)
            {
                var monthDiff = ((end.Year - begin.Year) * 12) + (end.Month - begin.Month);
                datePeriodTimex = $"P{monthDiff}M";
            }
            else
            {
                var yearDiff = (end.Year - begin.Year) + (end.Month - begin.Month) / 12.0;
                datePeriodTimex = $"P{yearDiff}Y";
            }

            return $"({FormatUtil.LuisDate(begin)},{FormatUtil.LuisDate(end)},{datePeriodTimex})";
        }
    }
}
