using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public static string GenerateCompoundDurationTimex(Dictionary<string, string> unitToTimexComponents, IImmutableDictionary<string, long> unitValueMap)
        {
            var unitList = new List<string>(unitToTimexComponents.Keys);
            unitList.Sort((x, y) => (unitValueMap[x] < unitValueMap[y] ? 1 : -1));
            var isTimeDurationAlreadyExist = false;
            var timexBuilder = new StringBuilder(Constants.GeneralPeriodPrefix);

            for (int i = 0; i < unitList.Count; i++)
            {
                var timexComponent = unitToTimexComponents[unitList[i]];

                // The Time Duration component occurs first time, 
                if (!isTimeDurationAlreadyExist && IsTimeDurationTimex(timexComponent))
                {
                    timexBuilder.Append($"{Constants.TimeTimexPrefix}{GetDurationTimexWithoutPrefix(timexComponent)}");
                    isTimeDurationAlreadyExist = true;
                }
                else
                {
                    timexBuilder.Append($"{GetDurationTimexWithoutPrefix(timexComponent)}");
                }
            }

            return timexBuilder.ToString();
        }

        private static bool IsTimeDurationTimex(string timex)
        {
            return timex.StartsWith($"{Constants.GeneralPeriodPrefix}{Constants.TimeTimexPrefix}");
        }

        private static string GetDurationTimexWithoutPrefix(string timex)
        {
            // Remove "PT" prefix for TimeDuration, Remove "P" prefix for DateDuration
            if (IsTimeDurationTimex(timex))
            {
                return timex.Substring(2);
            }
            else
            {
                return timex.Substring(1);
            }
        }

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
                return $"{Constants.TimexFuzzyYear}{Constants.DateTimexConnector}{Constants.TimexFuzzyWeek}";
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
                return $"{Constants.TimexFuzzyYear}{Constants.DateTimexConnector}{Constants.TimexFuzzyWeek}{Constants.DateTimexConnector}{Constants.TimexWeekend}";
            }
            else
            {
                return $"{FormatUtil.ToIsoWeekTimex(date)}{Constants.DateTimexConnector}{Constants.TimexWeekend}";
            }
        }

        public static string GenerateMonthTimex(DateObject date = default(DateObject))
        {
            if (date.IsDefaultValue())
            {
                return $"{Constants.TimexFuzzyYear}{Constants.DateTimexConnector}{Constants.TimexFuzzyMonth}";
            }
            else
            {
                return $"{date.Year.ToString("D4")}{Constants.DateTimexConnector}{date.Month.ToString("D2")}";
            }
        }

        public static string GenerateYearTimex(DateObject date = default(DateObject))
        {
            return date.IsDefaultValue() ? Constants.TimexFuzzyYear : date.Year.ToString("D4");
        }

        public static string GenerateDurationTimex(double number, string unitStr, bool isLessThanDay)
        {
            if (!unitStr.Equals(Constants.TimexBusinessDay))
            {
                if (unitStr.Equals("10Y"))
                {
                    number = number * 10;
                    unitStr = Constants.TimexYear;
                }
                else
                {
                    unitStr = unitStr.Substring(0, 1);
                }
            }

            return Constants.GeneralPeriodPrefix + (isLessThanDay ? Constants.TimeTimexPrefix : string.Empty) + number.ToString(CultureInfo.InvariantCulture) + unitStr;
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

        public static TimeOfDayResolutionResult ParseTimeOfDay(string timex)
        {
            var result = new TimeOfDayResolutionResult();
            switch (timex)
            {
                case Constants.Morning:
                    result.BeginHour = 8;
                    result.EndHour = 12;
                    break;
                case Constants.Afternoon:
                    result.BeginHour = 12;
                    result.EndHour = 16;
                    break;
                case Constants.Evening:
                    result.BeginHour = 16;
                    result.EndHour = 20;
                    break;
                case Constants.Daytime:
                    result.BeginHour = 8;
                    result.EndHour = 18;
                    break;
                case Constants.BusinessHour:
                    result.BeginHour = 8;
                    result.EndHour = 18;
                    break;
                case Constants.Night:
                    result.BeginHour = 20;
                    result.EndHour = 23;
                    result.EndMin = 59;
                    break;
               default:
                    break;
            }
         
            return result;
        }
    }
}
