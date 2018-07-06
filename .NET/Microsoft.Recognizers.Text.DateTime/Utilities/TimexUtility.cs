using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimexUtility
    {
        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private static readonly HashSet<char> NumberComponents = new HashSet<char>()
        {
            '0','1','2','3','4','5','6','7','8','9','.'
        };

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

        public static string GenerateWeekTimex(DateObject monday = default(DateObject))
        {
            if (monday == default(DateObject))
            {
                return "XXXX-WXX";
            }
            else
            {
                return FormatUtil.ToIsoWeekTimex(monday);
            }
        }

        public static string GenerateWeekendTimex(DateObject date = default(DateObject))
        {
            if (date == default(DateObject))
            {
                return "XXXX-WXX-WE";
            }
            else
            {
                return date.Year.ToString("D4") + "-W" +
                       Cal.GetWeekOfYear(date,
                               CalendarWeekRule.FirstFourDayWeek,
                               DayOfWeek.Monday)
                           .ToString("D2") + "-WE";
            }
        }

        public static string GenerateMonthTimex(DateObject date = default(DateObject))
        {
            if (date == default(DateObject))
            {
                return "XXXX-XX";
            }
            else
            {
                return date.Year.ToString("D4") + "-" +
                       date.Month.ToString("D2");
            }
        }

        public static string GenerateYearTimex(DateObject date = default(DateObject))
        {
            if (date == default(DateObject))
            {
                return "XXXX";
            }
            else
            {
                return date.Year.ToString("D4");
            }
        }
        
        public static DatePeriodTimexType GetDatePeriodTimexType(string durationTimex)
        {
            var minimumUnit = durationTimex.Substring(durationTimex.Length - 1);
            var ret = DatePeriodTimexType.ByDay;

            if (minimumUnit == "Y")
            {
                ret = DatePeriodTimexType.ByYear;
            }
            else if (minimumUnit == "M")
            {
                ret = DatePeriodTimexType.ByMonth;
            }
            else if (minimumUnit == "W")
            {
                ret = DatePeriodTimexType.ByWeek;
            }

            return ret;
        }

        public static DateObject OffsetDateObject(DateObject date, int offset, DatePeriodTimexType timexType)
        {
            var ret = date;

            if (timexType == DatePeriodTimexType.ByYear)
            {
                ret = date.AddYears(offset);
            }
            else if (timexType == DatePeriodTimexType.ByMonth)
            {
                ret = date.AddMonths(offset);
            }
            else if (timexType == DatePeriodTimexType.ByWeek)
            {
                ret = date.AddDays(7 * offset);
            }
            else if (timexType == DatePeriodTimexType.ByDay)
            {
                ret = date.AddDays(offset);
            }

            return ret;
        }
    }
}
