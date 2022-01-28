// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    internal class DurationParsingUtil
    {
        /// <summary>
        /// Represents multi duration.
        /// </summary>
        public enum MultiDurationType
        {
            /// <summary>
            /// Represents a date
            /// </summary>
            Date = 0,

            /// <summary>
            /// Represents a time
            /// </summary>
            Time,

            /// <summary>
            /// Represents the time of the date
            /// </summary>
            DateTime,
        }

        public static bool IsTimeDurationUnit(string unitStr)
        {
            bool result;

            switch (unitStr)
            {
                case "H":
                    result = true;
                    break;
                case "M":
                    result = true;
                    break;
                case "S":
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        public static bool IsMultipleDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            return dict.Count > 1;
        }

        public static bool IsDateDuration(string timex)
        {
            var dict = ResolveDurationTimex(timex);

            return dict.All(unit => !IsTimeDurationUnit(unit.Item1));
        }

        public static DateObject ShiftDateTime(string timex, DateObject referenceDateTime, bool future)
        {
            var timexUnitMap = ResolveDurationTimex(timex);
            var result = GetShiftResult(timexUnitMap, referenceDateTime, future);
            return result;
        }

        public static DateObject GetNthBusinessDay(DateObject startDate, int n, bool isFuture, out List<DateObject> dateList)
        {
            var date = startDate;
            dateList = new List<DateObject> { date };

            for (var i = 0; i < n; i++)
            {
                date = GetNextBusinessDay(date, isFuture);
                dateList.Add(date);
            }

            if (!isFuture)
            {
                dateList.Reverse();
            }

            return date;
        }

        // By design it currently does not take holidays into account
        public static DateObject GetNextBusinessDay(DateObject startDate, bool isFuture = true)
        {
            var dateIncrement = isFuture ? 1 : -1;
            var date = startDate.AddDays(dateIncrement);

            while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(dateIncrement);
            }

            return date;
        }

        public static bool IsLessThanDay(string unit)
        {
            return unit.Equals("S", StringComparison.Ordinal) ||
                   unit.Equals("M", StringComparison.Ordinal) ||
                   unit.Equals("H", StringComparison.Ordinal);
        }

        public static DateTimeResolutionResult ParseInexactNumberUnit(string text, IDurationParserConfiguration config)
        {
            return ParseInexactNumberUnit(text, config.InexactNumberUnitRegex, config.UnitMap, config.UnitValueMap);
        }

        public static DateTimeResolutionResult ParseInexactNumberUnit(string text, ICJKDurationParserConfiguration config)
        {
            return ParseInexactNumberUnit(text, config.SomeRegex, config.UnitMap, config.UnitValueMap, isCJK: true);
        }

        private static DateTimeResolutionResult ParseInexactNumberUnit(string text, Regex inexactNumberUnitRegex, IImmutableDictionary<string, string> unitMap, IImmutableDictionary<string, long> unitValueMap, bool isCJK = false)
        {
            var ret = new DateTimeResolutionResult();

            var match = inexactNumberUnitRegex.Match(text);
            if (match.Success)
            {
                // set the inexact number "few", "some" to 3 for now
                double numVal = match.Groups["NumTwoTerm"].Success ? 2 : 3;
                var srcUnit = match.Groups["unit"].Value;

                if (unitMap.ContainsKey(srcUnit))
                {
                    var unitStr = unitMap[srcUnit];

                    if (numVal > 1000 && (unitStr.Equals(Constants.TimexYear, StringComparison.Ordinal) ||
                                          unitStr.Equals(Constants.TimexMonthFull, StringComparison.Ordinal) ||
                                          unitStr.Equals(Constants.TimexWeek, StringComparison.Ordinal)))
                    {
                        return ret;
                    }

                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, unitStr, IsLessThanDay(unitStr));

                    // In CJK implementation unitValueMap uses the unitMap values as keys while
                    // in standard implementation unitMap and unitValueMap have the same keys.
                    var unitValue = isCJK ? unitValueMap[unitStr] : unitValueMap[srcUnit];
                    ret.FutureValue = ret.PastValue = numVal * unitValue;
                    ret.Success = true;
                }
                else if (match.Groups[Constants.BusinessDayGroupName].Success)
                {
                    ret.Timex = TimexUtility.GenerateDurationTimex(numVal, Constants.TimexBusinessDay, false);

                    // The line below was containing this.config.UnitValueMap[srcUnit.Split()[1]]
                    // it was updated to accommodate single word "business day" expressions.
                    ret.FutureValue = ret.PastValue = numVal * unitValueMap[srcUnit.Split()[srcUnit.Split().Length - 1]];
                    ret.Success = true;
                }
            }

            return ret;
        }

        private static DateObject GetShiftResult(List<(string, double)> timexUnitMap, DateObject referenceDate, bool future)
        {
            var result = referenceDate;
            var futureOrPast = future ? 1 : -1;

            // timexUnitMap needs to be an ordered collection because the result depends on the order of the shifts.
            // For example "1 month 21 days later" produces different results depending on whether the day or month shift is applied first
            // (when the reference month and the following month have different numbers of days).
            foreach (var pair in timexUnitMap)
            {
                var unitStr = pair.Item1;
                var number = pair.Item2;

                switch (unitStr)
                {
                    case "H":
                        result = result.AddHours(number * futureOrPast);
                        break;
                    case "M":
                        result = result.AddMinutes(number * futureOrPast);
                        break;
                    case "S":
                        result = result.AddSeconds(number * futureOrPast);
                        break;
                    case Constants.TimexDay:
                        result = result.AddDays(number * futureOrPast);
                        break;
                    case Constants.TimexWeek:
                        result = result.AddDays(7 * number * futureOrPast);
                        break;
                    case Constants.TimexMonthFull:
                        result = result.AddMonths(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case Constants.TimexYear:
                        result = result.AddYears(Convert.ToInt32(number) * futureOrPast);
                        break;
                    case Constants.TimexBusinessDay:
                        result = GetNthBusinessDay(result, Convert.ToInt32(number), future, out _);
                        break;
                    default:
                        return result;
                }
            }

            return result;
        }

        private static List<(string, double)> ResolveDurationTimex(string timexStr)
        {
            var ret = new List<(string, double)>();

            // Resolve duration timex, such as P21DT2H (21 days 2 hours)
            var durationStr = timexStr.Replace(Constants.GeneralPeriodPrefix, string.Empty);
            var numberStart = 0;
            var isTime = false;

            // Resolve business days
            if (durationStr.EndsWith(Constants.TimexBusinessDay, StringComparison.Ordinal))
            {
                if (double.TryParse(durationStr.Substring(0, durationStr.Length - 2), out var numVal))
                {
                    ret.Add((Constants.TimexBusinessDay, numVal));
                }

                return ret;
            }

            for (var idx = 0; idx < durationStr.Length; idx++)
            {
                if (char.IsLetter(durationStr[idx]))
                {
                    if (durationStr[idx] == 'T')
                    {
                        isTime = true;
                    }
                    else
                    {
                        var numStr = durationStr.Substring(numberStart, idx - numberStart);
                        if (!double.TryParse(numStr, out var number))
                        {
                            return new List<(string, double)>();
                        }

                        var srcTimexUnit = durationStr.Substring(idx, 1);
                        if (!isTime && srcTimexUnit == Constants.TimexMonth)
                        {
                            srcTimexUnit = Constants.TimexMonthFull;
                        }

                        ret.Add((srcTimexUnit, number));
                    }

                    numberStart = idx + 1;
                }
            }

            return ret;
        }
    }
}
