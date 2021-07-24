﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    [Flags]
    public enum UnspecificDateTimeTerms
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// NonspecificYear
        /// </summary>
        NonspecificYear = 1,

        /// <summary>
        /// NonspecificMonth
        /// </summary>
        NonspecificMonth = 2,

        /// <summary>
        /// NonspecificDay
        /// </summary>
        NonspecificDay = 4,
    }

    public static class TimexUtility
    {
        private static readonly Calendar Cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private static readonly HashSet<char> NumberComponents = new HashSet<char>()
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.',
        };

        private static readonly Dictionary<DatePeriodTimexType, string> DatePeriodTimexTypeToTimexSuffix = new Dictionary<DatePeriodTimexType, string>()
        {
            { DatePeriodTimexType.ByDay, Constants.TimexDay },
            { DatePeriodTimexType.ByWeek, Constants.TimexWeek },
            { DatePeriodTimexType.ByFortnight, Constants.TimexFortnight },
            { DatePeriodTimexType.ByMonth, Constants.TimexMonth },
            { DatePeriodTimexType.ByYear, Constants.TimexYear },
        };

        public static string GenerateCompoundDurationTimex(Dictionary<string, string> unitToTimexComponents, IImmutableDictionary<string, long> unitValueMap)
        {
            var unitList = new List<string>(unitToTimexComponents.Keys);
            unitList.Sort((x, y) => (unitValueMap[x] < unitValueMap[y] ? 1 : -1));
            unitList = unitList.Select(t => unitToTimexComponents[t]).ToList();
            return TimexHelpers.GenerateCompoundDurationTimex(unitList);
        }

        // TODO: Unify this two methods. This one here detect if "begin/end" have same year, month and day with "alter begin/end" and make them nonspecific.
        public static string GenerateDatePeriodTimex(DateObject begin, DateObject end, DatePeriodTimexType timexType, DateObject alternativeBegin = default(DateObject), DateObject alternativeEnd = default(DateObject), bool hasYear = true)
        {
            // If the year is not specified, the combined range timex will use fuzzy years.
            if (!hasYear)
            {
                return GenerateDatePeriodTimex(begin, end, timexType, UnspecificDateTimeTerms.NonspecificYear);
            }

            var equalDurationLength = (end - begin) == (alternativeEnd - alternativeBegin);

            if (alternativeBegin.IsDefaultValue() || alternativeEnd.IsDefaultValue())
            {
                equalDurationLength = true;
            }

            var unitCount = equalDurationLength ? GetDatePeriodTimexUnitCount(begin, end, timexType) : "XX";

            var datePeriodTimex = $"P{unitCount}{DatePeriodTimexTypeToTimexSuffix[timexType]}";

            return $"({DateTimeFormatUtil.LuisDate(begin, alternativeBegin)},{DateTimeFormatUtil.LuisDate(end, alternativeEnd)},{datePeriodTimex})";
        }

        public static string GenerateDatePeriodTimex(DateObject begin, DateObject end, DatePeriodTimexType timexType, UnspecificDateTimeTerms terms)
        {
            var beginYear = begin.Year;
            var endYear = end.Year;
            var beginMonth = begin.Month;
            var endMonth = end.Month;
            var beginDay = begin.Day;
            var endDay = end.Day;

            if ((terms & UnspecificDateTimeTerms.NonspecificYear) != 0)
            {
                beginYear = endYear = -1;
            }

            if ((terms & UnspecificDateTimeTerms.NonspecificMonth) != 0)
            {
                beginMonth = endMonth = -1;
            }

            if ((terms & UnspecificDateTimeTerms.NonspecificDay) != 0)
            {
                beginDay = endDay = -1;
            }

            var unitCount = GetDatePeriodTimexUnitCount(begin, end, timexType);

            var datePeriodTimex = $"P{unitCount}{DatePeriodTimexTypeToTimexSuffix[timexType]}";

            return $"({DateTimeFormatUtil.LuisDate(beginYear, beginMonth, beginDay)},{DateTimeFormatUtil.LuisDate(endYear, endMonth, endDay)},{datePeriodTimex})";
        }

        public static string GenerateDatePeriodTimex(DateObject begin, DateObject end, DatePeriodTimexType timexType, string timex1, string timex2)
        {
            var boundaryValid = !begin.IsDefaultValue() && !end.IsDefaultValue();
            var unitCount = boundaryValid ? GetDatePeriodTimexUnitCount(begin, end, timexType) : "X";
            var datePeriodTimex = $"P{unitCount}{DatePeriodTimexTypeToTimexSuffix[timexType]}";
            return $"({timex1},{timex2},{datePeriodTimex})";
        }

        public static string GenerateWeekTimex(DateObject monday = default(DateObject))
        {
            if (monday.IsDefaultValue())
            {
                return $"{Constants.TimexFuzzyYear}{Constants.DateTimexConnector}{Constants.TimexFuzzyWeek}";
            }
            else
            {
                return DateTimeFormatUtil.ToIsoWeekTimex(monday);
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
                return $"{DateTimeFormatUtil.ToIsoWeekTimex(date)}{Constants.DateTimexConnector}{Constants.TimexWeekend}";
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
                return $"{date.Year:D4}{Constants.DateTimexConnector}{date.Month:D2}";
            }
        }

        public static string GenerateYearTimex(DateObject date = default(DateObject), string specialYearPrefixes = null)
        {
            var yearTimex = date.IsDefaultValue() ? Constants.TimexFuzzyYear : $"{date.Year:D4}";
            return specialYearPrefixes == null ? yearTimex : specialYearPrefixes + yearTimex;
        }

        public static string GenerateYearTimex(int year, string specialYearPrefixes = null)
        {
            var yearTimex = DateTimeFormatUtil.LuisDate(year);
            return specialYearPrefixes == null ? yearTimex : specialYearPrefixes + yearTimex;
        }

        public static string GenerateDurationTimex(double number, string unitStr, bool isLessThanDay)
        {
            if (!Constants.TimexBusinessDay.Equals(unitStr, StringComparison.Ordinal))
            {
                switch (unitStr)
                {
                    case Constants.DECADE_UNIT:
                        number = number * 10;
                        unitStr = Constants.TimexYear;
                        break;
                    case Constants.FORTNIGHT_UNIT:
                        number = number * 2;
                        unitStr = Constants.TimexWeek;
                        break;
                    case Constants.WEEKEND_UNIT:
                        unitStr = Constants.TimexWeekend;
                        break;
                    default:
                        unitStr = unitStr.Substring(0, 1);
                        break;
                }
            }

            return Constants.GeneralPeriodPrefix +
                   (isLessThanDay ? Constants.TimeTimexPrefix : string.Empty) +
                   number.ToString(CultureInfo.InvariantCulture) + unitStr;
        }

        public static DatePeriodTimexType GetDatePeriodTimexType(string durationTimex)
        {
            DatePeriodTimexType result;

            var minimumUnit = durationTimex.Substring(durationTimex.Length - 1);

            switch (minimumUnit)
            {
                case Constants.TimexYear:
                    result = DatePeriodTimexType.ByYear;
                    break;
                case Constants.TimexMonth:
                    result = DatePeriodTimexType.ByMonth;
                    break;
                case Constants.TimexWeek:
                    result = DatePeriodTimexType.ByWeek;
                    break;
                default:
                    result = DatePeriodTimexType.ByDay;
                    break;
            }

            return result;
        }

        public static DateObject OffsetDateObject(DateObject date, int offset, DatePeriodTimexType timexType)
        {
            DateObject result;

            switch (timexType)
            {
                case DatePeriodTimexType.ByYear:
                    result = date.AddYears(offset);
                    break;
                case DatePeriodTimexType.ByMonth:
                    result = date.AddMonths(offset);
                    break;
                case DatePeriodTimexType.ByWeek:
                    result = date.AddDays(7 * offset);
                    break;
                case DatePeriodTimexType.ByDay:
                    result = date.AddDays(offset);
                    break;
                default:
                    result = date;
                    break;
            }

            return result;
        }

        public static string MergeTimexAlternatives(string timex1, string timex2)
        {
            if (timex1.Equals(timex2, StringComparison.Ordinal))
            {
                return timex1;
            }

            return $"{timex1}{Constants.CompositeTimexDelimiter}{timex2}";
        }

        public static void ProcessDoubleTimex(Dictionary<string, object> resolutionDic, string futureKey, string pastKey, string originTimex)
        {
            string[] timexes = originTimex.Split(Constants.CompositeTimexDelimiter);

            if (!resolutionDic.ContainsKey(futureKey) || !resolutionDic.ContainsKey(pastKey) || timexes.Length != 2)
            {
                return;
            }

            var futureResolution = (Dictionary<string, string>)resolutionDic[futureKey];
            var pastResolution = (Dictionary<string, string>)resolutionDic[pastKey];
            futureResolution[DateTimeResolutionKey.Timex] = timexes[0];
            pastResolution[DateTimeResolutionKey.Timex] = timexes[1];
        }

        public static bool HasDoubleTimex(string comment)
        {
            return comment.Equals(Constants.Comment_DoubleTimex, StringComparison.Ordinal);
        }

        public static TimeOfDayResolutionResult ResolveTimeOfDay(string tod)
        {
            var result = new TimeOfDayResolutionResult();
            switch (tod)
            {
                case Constants.EarlyMorning:
                    result.Timex = Constants.EarlyMorning;
                    result.BeginHour = Constants.EarlyMorningBeginHour;
                    result.EndHour = Constants.EarlyMorningEndHour;
                    break;
                case Constants.Morning:
                    result.Timex = Constants.Morning;
                    result.BeginHour = Constants.MorningBeginHour;
                    result.EndHour = Constants.MorningEndHour;
                    break;
                case Constants.MidDay:
                    result.Timex = Constants.MidDay;
                    result.BeginHour = Constants.MidDayBeginHour;
                    result.EndHour = Constants.MidDayEndHour;
                    break;
                case Constants.Afternoon:
                    result.Timex = Constants.Afternoon;
                    result.BeginHour = Constants.AfternoonBeginHour;
                    result.EndHour = Constants.AfternoonEndHour;
                    break;
                case Constants.Evening:
                    result.Timex = Constants.Evening;
                    result.BeginHour = Constants.EveningBeginHour;
                    result.EndHour = Constants.EveningEndHour;
                    break;
                case Constants.Daytime:
                    result.Timex = Constants.Daytime;
                    result.BeginHour = Constants.DaytimeBeginHour;
                    result.EndHour = Constants.DaytimeEndHour;
                    break;
                case Constants.Nighttime:
                    result.Timex = Constants.Nighttime;
                    result.BeginHour = Constants.NighttimeBeginHour;
                    result.EndHour = Constants.NighttimeEndHour;
                    break;
                case Constants.BusinessHour:
                    result.Timex = Constants.BusinessHour;
                    result.BeginHour = Constants.BusinessBeginHour;
                    result.EndHour = Constants.BusinessEndHour;
                    break;
                case Constants.Night:
                    result.Timex = Constants.Night;
                    result.BeginHour = Constants.NightBeginHour;
                    result.EndHour = Constants.NightEndHour;
                    result.EndMin = Constants.NightEndMin;
                    break;
                case Constants.MealtimeBreakfast:
                    result.Timex = Constants.MealtimeBreakfast;
                    result.BeginHour = Constants.MealtimeBreakfastBeginHour;
                    result.EndHour = Constants.MealtimeBreakfastEndHour;
                    break;
                case Constants.MealtimeBrunch:
                    result.Timex = Constants.MealtimeBrunch;
                    result.BeginHour = Constants.MealtimeBrunchBeginHour;
                    result.EndHour = Constants.MealtimeBrunchEndHour;
                    break;
                case Constants.MealtimeLunch:
                    result.Timex = Constants.MealtimeLunch;
                    result.BeginHour = Constants.MealtimeLunchBeginHour;
                    result.EndHour = Constants.MealtimeLunchEndHour;
                    break;
                case Constants.MealtimeDinner:
                    result.Timex = Constants.MealtimeDinner;
                    result.BeginHour = Constants.MealtimeDinnerBeginHour;
                    result.EndHour = Constants.MealtimeDinnerEndHour;
                    break;
                default:
                    break;
            }

            return result;
        }

        public static string CombineDateAndTimeTimex(string dateTimex, string timeTimex)
        {
            return $"{dateTimex}{timeTimex}";
        }

        public static string GenerateEndInclusiveTimex(string originalTimex, DatePeriodTimexType datePeriodTimexType,
            DateObject startDate, DateObject endDate)
        {

            var timexEndInclusive = GenerateDatePeriodTimex(startDate, endDate, datePeriodTimexType);

            // Sometimes the original timex contains fuzzy part like "XXXX-05-31"
            // The fuzzy part needs to stay the same in the new end-inclusive timex
            if (originalTimex.Contains(Constants.TimexFuzzy) && originalTimex.Length == timexEndInclusive.Length)
            {
                var timexCharSet = new char[timexEndInclusive.Length];

                for (int i = 0; i < originalTimex.Length; i++)
                {
                    if (originalTimex[i] != Constants.TimexFuzzy)
                    {
                        timexCharSet[i] = timexEndInclusive[i];
                    }
                    else
                    {
                        timexCharSet[i] = Constants.TimexFuzzy;
                    }
                }

                timexEndInclusive = new string(timexCharSet);
            }

            return timexEndInclusive;
        }

        public static string GenerateWeekOfYearTimex(int year, int weekNum)
        {
            var weekTimex = GenerateWeekTimex(weekNum);
            var yearTimex = DateTimeFormatUtil.LuisDate(year);

            return $"{yearTimex}-{weekTimex}";
        }

        public static string GenerateWeekOfMonthTimex(int year, int month, int weekNum)
        {
            var weekTimex = GenerateWeekTimex(weekNum);
            var monthTimex = DateTimeFormatUtil.LuisDate(year, month);

            return $"{monthTimex}-{weekTimex}";
        }

        public static string GenerateWeekTimex(int weekNum)
        {
            return $"W{weekNum.ToString("D2", CultureInfo.InvariantCulture)}";
        }

        public static string GenerateDateTimePeriodTimex(string beginTimex, string endTimex, string durationTimex)
        {
            return $"({beginTimex},{endTimex},{durationTimex})";
        }

        public static RangeTimexComponents GetRangeTimexComponents(string rangeTimex)
        {
            rangeTimex = rangeTimex.Replace("(", string.Empty).Replace(")", string.Empty);
            var components = rangeTimex.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var result = new RangeTimexComponents();

            if (components.Length == 3)
            {
                result.BeginTimex = components[0];
                result.EndTimex = components[1];
                result.DurationTimex = components[2];
                result.IsValid = true;
            }

            return result;
        }

        public static bool IsRangeTimex(string timex)
        {
            return !string.IsNullOrEmpty(timex) && timex.StartsWith("(", StringComparison.Ordinal);
        }

        public static string SetTimexWithContext(string timex, DateContext context)
        {
            return timex.Replace(Constants.TimexFuzzyYear, context.Year.ToString("D4", CultureInfo.InvariantCulture));
        }

        public static string GenerateSetTimex(string durationType, float durationLength, float multiplier = 1)
        {
            return $"P{durationLength * multiplier:0.#}{durationType}";
        }

        public static string ModifyAmbiguousCenturyTimex(string timex)
        {
            return "XX" + timex.Substring(2);
        }

        private static bool IsTimeDurationTimex(string timex)
        {
            return timex.StartsWith($"{Constants.GeneralPeriodPrefix}{Constants.TimeTimexPrefix}", StringComparison.Ordinal);
        }

        private static string GetDatePeriodTimexUnitCount(DateObject begin, DateObject end, DatePeriodTimexType timexType)
        {
            string unitCount;

            switch (timexType)
            {
                case DatePeriodTimexType.ByDay:
                    unitCount = (end - begin).TotalDays.ToString(CultureInfo.InvariantCulture);
                    break;
                case DatePeriodTimexType.ByWeek:
                    unitCount = ((end - begin).TotalDays / 7).ToString(CultureInfo.InvariantCulture);
                    break;
                case DatePeriodTimexType.ByFortnight:
                    unitCount = ((end - begin).TotalDays / 7).ToString(CultureInfo.InvariantCulture);
                    break;
                case DatePeriodTimexType.ByMonth:
                    unitCount = (((end.Year - begin.Year) * 12) + (end.Month - begin.Month)).ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    unitCount = ((end.Year - begin.Year) + ((end.Month - begin.Month) / 12.0)).ToString(CultureInfo.InvariantCulture);
                    break;
            }

            return unitCount;
        }

    }
}
