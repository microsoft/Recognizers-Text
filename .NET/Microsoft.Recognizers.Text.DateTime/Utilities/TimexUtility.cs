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

        private static readonly Dictionary<DatePeriodTimexType, string> DatePeriodTimexTypeToTimexSuffix = new Dictionary<DatePeriodTimexType, string>()
        {
            {DatePeriodTimexType.ByDay, Constants.TimexDay },
            {DatePeriodTimexType.ByWeek, Constants.TimexWeek },
            {DatePeriodTimexType.ByMonth, Constants.TimexMonth },
            {DatePeriodTimexType.ByYear, Constants.TimexYear }
        };

        public static string GenerateDatePeriodTimex(DateObject begin, DateObject end, DatePeriodTimexType timexType, DateObject alternativeBegin = default(DateObject), DateObject alternativeEnd = default(DateObject))
        {
            var equalDurationLength = ((end - begin) == (alternativeEnd - alternativeBegin));

            if (alternativeBegin.IsDefaultValue() || alternativeEnd.IsDefaultValue())
            {
                equalDurationLength = true;
            }

            var unitCount = "XX";

            if (equalDurationLength)
            {
                if (timexType == DatePeriodTimexType.ByDay)
                {
                    unitCount = (end - begin).TotalDays.ToString();
                }
                else if (timexType == DatePeriodTimexType.ByWeek)
                {
                    unitCount = ((end - begin).TotalDays / 7).ToString();
                }
                else if (timexType == DatePeriodTimexType.ByMonth)
                {
                    unitCount = (((end.Year - begin.Year) * 12) + (end.Month - begin.Month)).ToString();
                }
                else
                {
                    unitCount = ((end.Year - begin.Year) + (end.Month - begin.Month) / 12.0).ToString();
                }
            }
            
            var datePeriodTimex = $"P{unitCount}{DatePeriodTimexTypeToTimexSuffix[timexType]}";

            return $"({FormatUtil.LuisDate(begin, alternativeBegin)},{FormatUtil.LuisDate(end, alternativeEnd)},{datePeriodTimex})";
        }

        public static string GenerateWeekTimex(DateObject monday = default(DateObject))
        {
            if (monday.IsDefaultValue())
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
            if (date.IsDefaultValue())
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
            if (date.IsDefaultValue())
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
            return date.IsDefaultValue() ? "XXXX" : date.Year.ToString("D4");
        }

        public static string GenerateDurationTimex(double number, string unitStr, bool isLessThanDay)
        {
            if (!unitStr.Equals(Constants.TimexBusinessDay))
            {
                if (unitStr.Equals("10Y"))
                {
                    number = number * 10;
                    unitStr = "Y";
                }
                else
                {
                    unitStr = unitStr.Substring(0, 1);
                }
            }

            return "P" + (isLessThanDay ? "T" : "") + number.ToString(CultureInfo.InvariantCulture) + unitStr;
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
