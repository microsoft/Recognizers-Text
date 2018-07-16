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
                datePeriodTimex = $"P{(end - begin).TotalDays}{Constants.TimexDay}";
            }
            else if (timexType == DatePeriodTimexType.ByWeek)
            {
                datePeriodTimex = $"P{(end - begin).TotalDays / 7}{Constants.TimexWeek}";
            }
            else if (timexType == DatePeriodTimexType.ByMonth)
            {
                var monthDiff = ((end.Year - begin.Year) * 12) + (end.Month - begin.Month);
                datePeriodTimex = $"P{monthDiff}{Constants.TimexMonth}";
            }
            else
            {
                var yearDiff = (end.Year - begin.Year) + (end.Month - begin.Month) / 12.0;
                datePeriodTimex = $"P{yearDiff}{Constants.TimexYear}";
            }

            return $"({FormatUtil.LuisDate(begin)},{FormatUtil.LuisDate(end)},{datePeriodTimex})";
        }

        // In some mode, we would need to alter the timex, e.g. alter the timex to include the period end
        // However, sometimes the originalTimex is fuzzy, like "(XXXX-05-31,XXXX-06-05,P5D)"
        // For these cases, we first generate a concreteTimex without any fuzzy character, then we merge the altered Timex with the originalTimex to keep the fuzzy part
        public static string GenerateAlterTimex(string originalTimex, string alterTimex)
        {
            if (originalTimex.Length == alterTimex.Length)
            {
                var timexCharSet = new char[alterTimex.Length];

                for (int i = 0; i < originalTimex.Length; i++)
                {
                    if (originalTimex[i] != Constants.TimexFuzzy)
                    {
                        timexCharSet[i] = alterTimex[i];
                    }
                    else
                    {
                        timexCharSet[i] = Constants.TimexFuzzy;
                    }
                }

                originalTimex = new string(timexCharSet);
            }

            return originalTimex;
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

            if (minimumUnit == Constants.TimexYear)
            {
                ret = DatePeriodTimexType.ByYear;
            }
            else if (minimumUnit == Constants.TimexMonth)
            {
                ret = DatePeriodTimexType.ByMonth;
            }
            else if (minimumUnit == Constants.TimexWeek)
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
