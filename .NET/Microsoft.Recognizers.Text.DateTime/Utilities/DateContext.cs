// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    // Currently only Year is enabled as context, we may support Month or Week in the future
    public class DateContext
    {
        public int Year { get; set; } = Constants.InvalidYear;

        // Generate future/past date for cases without specific year like "Feb 29th"
        public static (DateObject future, DateObject past) GenerateDates(bool noYear, DateObject referenceDate, int year, int month, int day)
        {
            var futureDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var pastDate = DateObject.MinValue.SafeCreateFromValue(year, month, day);
            var futureYear = year;
            var pastYear = year;
            if (noYear)
            {
                if (IsFeb29th(year, month, day))
                {
                    if (DateObject.IsLeapYear(year))
                    {
                        if (futureDate < referenceDate)
                        {
                            futureDate = DateObject.MinValue.SafeCreateFromValue(futureYear + 4, month, day);
                        }
                        else
                        {
                            pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear - 4, month, day);
                        }
                    }
                    else
                    {
                        pastYear = pastYear >> 2 << 2;
                        if (!DateObject.IsLeapYear(pastYear))
                        {
                            pastYear -= 4;
                        }

                        futureYear = pastYear + 4;
                        if (!DateObject.IsLeapYear(futureYear))
                        {
                            futureYear += 4;
                        }

                        futureDate = DateObject.MinValue.SafeCreateFromValue(futureYear, month, day);
                        pastDate = DateObject.MinValue.SafeCreateFromValue(pastYear, month, day);
                    }
                }
                else
                {
                    if (futureDate < referenceDate && !futureDate.IsDefaultValue())
                    {
                        futureDate = DateObject.MinValue.SafeCreateFromValue(year + 1, month, day);
                    }

                    if (pastDate >= referenceDate && !pastDate.IsDefaultValue())
                    {
                        pastDate = DateObject.MinValue.SafeCreateFromValue(year - 1, month, day);
                    }
                }
            }

            return (futureDate, pastDate);
        }

        // This method is to ensure the begin date is less than the end date.
        // As DateContext only supports common Year as context, so it subtracts one year from beginDate. @TODO problematic in other usages.
        public static DateObject SwiftDateObject(DateObject beginDate, DateObject endDate)
        {
            if (beginDate > endDate)
            {
                beginDate = beginDate.AddYears(-1);
            }

            return beginDate;
        }

        public static bool IsFeb29th(DateObject date)
        {
            return date.Month == 2 && date.Day == 29;
        }

        public static bool IsFeb29th(int year, int month, int day)
        {
            return month == 2 && day == 29;
        }

        // this method is to validate whether the match is part of date range and is a correct split
        // For example: in case "10-1 - 11-7", "10-1 - 11" can be matched by some of the Regexes, but the full text is a date range, so "10-1 - 11" is not a correct split
        public static bool ValidateMatch(Match match, string text, IEnumerable<Regex> dateRegexList, Regex rangeConnectorSymbolRegex)
        {
            // If the match doesn't contains "year" part, it will not be ambiguous and it's a valid match
            var isValidMatch = !match.Groups[Constants.YearGroupName].Success;

            if (!isValidMatch)
            {
                var yearGroup = match.Groups[Constants.YearGroupName];

                // If the "year" part is not at the end of the match, it's a valid match
                if (yearGroup.Index + yearGroup.Length != match.Index + match.Length)
                {
                    isValidMatch = true;
                }
                else
                {
                    var subText = text.Substring(yearGroup.Index);

                    // If the following text (include the "year" part) doesn't start with a Date entity, it's a valid match
                    if (!StartsWithBasicDate(subText, dateRegexList))
                    {
                        isValidMatch = true;
                    }
                    else
                    {
                        // If the following text (include the "year" part) starts with a Date entity, but the following text (doesn't include the "year" part) also starts with a valid Date entity, the current match is still valid
                        // For example, "10-1-2018-10-2-2018". Match "10-1-2018" is valid because though "2018-10-2" a valid match (indicates the first year "2018" might belongs to the second Date entity), but "10-2-2018" is also a valid match.
                        subText = text.Substring(yearGroup.Index + yearGroup.Length).Trim();
                        subText = TrimStartRangeConnectorSymbols(subText, rangeConnectorSymbolRegex);
                        isValidMatch = StartsWithBasicDate(subText, dateRegexList);
                    }
                }

                // Expressions with mixed separators are not considered valid dates e.g. "30/4.85" (unless one is a comma "30/4, 2016")
                if (match.Groups[Constants.DayGroupName].Success && match.Groups[Constants.MonthGroupName].Success)
                {
                    var noDateText = match.Value.Replace(match.Groups[Constants.YearGroupName].Value, string.Empty)
                        .Replace(match.Groups[Constants.MonthGroupName].Value, string.Empty)
                        .Replace(match.Groups[Constants.DayGroupName].Value, string.Empty);
                    noDateText = match.Groups[Constants.WeekdayGroupName].Success ? noDateText.Replace(match.Groups[Constants.WeekdayGroupName].Value, string.Empty) : noDateText;
                    var separators = new List<char> { '/', '\\', '-', '.' };

                    if (separators.Count(separator => noDateText.Contains(separator)) > 1)
                    {
                        isValidMatch = false;
                    }
                }
            }

            return isValidMatch;
        }

        // This method is to ensure the year of begin date is same with the end date in no year situation.
        public (DateTimeParseResult pr1, DateTimeParseResult pr2) SyncYear(DateTimeParseResult pr1, DateTimeParseResult pr2)
        {
            if (IsEmpty())
            {
                int futureYear;
                int pastYear;
                if (IsFeb29th((DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue))
                {
                    futureYear = ((DateObject)((DateTimeResolutionResult)pr1.Value).FutureValue).Year;
                    pastYear = ((DateObject)((DateTimeResolutionResult)pr1.Value).PastValue).Year;
                    pr2.Value = SyncDateEntityResolutionInFeb29th((DateTimeResolutionResult)pr2.Value, futureYear, pastYear);
                }
                else if (IsFeb29th((DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue))
                {
                    futureYear = ((DateObject)((DateTimeResolutionResult)pr2.Value).FutureValue).Year;
                    pastYear = ((DateObject)((DateTimeResolutionResult)pr2.Value).PastValue).Year;
                    pr1.Value = SyncDateEntityResolutionInFeb29th((DateTimeResolutionResult)pr1.Value, futureYear, pastYear);
                }
            }

            return (pr1, pr2);
        }

        public DateTimeResolutionResult SyncDateEntityResolutionInFeb29th(DateTimeResolutionResult resolutionResult, int futureYear, int pastYear)
        {
            resolutionResult.FutureValue = SetDateWithContext((DateObject)resolutionResult.FutureValue, futureYear);
            resolutionResult.PastValue = SetDateWithContext((DateObject)resolutionResult.PastValue, pastYear);

            return resolutionResult;
        }

        public DateTimeParseResult ProcessDateEntityParsingResult(DateTimeParseResult originalResult)
        {
            if (!IsEmpty())
            {
                originalResult.TimexStr = TimexUtility.SetTimexWithContext(originalResult.TimexStr, this);
                originalResult.Value = ProcessDateEntityResolution((DateTimeResolutionResult)originalResult.Value);
            }

            return originalResult;
        }

        public DateTimeResolutionResult ProcessDateEntityResolution(DateTimeResolutionResult resolutionResult)
        {
            if (!IsEmpty())
            {
                resolutionResult.Timex = TimexUtility.SetTimexWithContext(resolutionResult.Timex, this);
                resolutionResult.FutureValue = SetDateWithContext((DateObject)resolutionResult.FutureValue);
                resolutionResult.PastValue = SetDateWithContext((DateObject)resolutionResult.PastValue);
            }

            return resolutionResult;
        }

        public DateTimeResolutionResult ProcessDatePeriodEntityResolution(DateTimeResolutionResult resolutionResult)
        {
            if (!IsEmpty())
            {
                resolutionResult.Timex = TimexUtility.SetTimexWithContext(resolutionResult.Timex, this);
                resolutionResult.FutureValue = SetDateRangeWithContext((Tuple<DateObject, DateObject>)resolutionResult.FutureValue);
                resolutionResult.PastValue = SetDateRangeWithContext((Tuple<DateObject, DateObject>)resolutionResult.PastValue);
            }

            return resolutionResult;
        }

        public bool IsEmpty()
        {
            return this.Year == Constants.InvalidYear;
        }

        // TODO: Simplify this method to improve its performance
        private static string TrimStartRangeConnectorSymbols(string text, Regex rangeConnectorSymbolRegex)
        {
            var rangeConnectorSymbolMatches = rangeConnectorSymbolRegex.Matches(text);

            foreach (Match symbolMatch in rangeConnectorSymbolMatches)
            {
                var startSymbolLength = -1;

                if (symbolMatch.Success && symbolMatch.Index == 0 && symbolMatch.Length > startSymbolLength)
                {
                    startSymbolLength = symbolMatch.Length;
                }

                if (startSymbolLength > 0)
                {
                    text = text.Substring(startSymbolLength);
                }
            }

            return text.Trim();
        }

        // TODO: Simplify this method to improve its performance
        private static bool StartsWithBasicDate(string text, IEnumerable<Regex> dateRegexList)
        {
            foreach (var regex in dateRegexList)
            {
                var match = regex.MatchBegin(text, trim: true);

                if (match.Success)
                {
                    return true;
                }
            }

            return false;
        }

        private DateObject SetDateWithContext(DateObject originalDate, int year = -1)
        {
            if (!originalDate.IsDefaultValue())
            {
                return DateObject.MinValue.SafeCreateFromValue(year == -1 ? Year : year, originalDate.Month, originalDate.Day);
            }

            return originalDate;
        }

        private Tuple<DateObject, DateObject> SetDateRangeWithContext(Tuple<DateObject, DateObject> originalDateRange)
        {
            var startDate = SetDateWithContext(originalDateRange.Item1);
            var endDate = SetDateWithContext(originalDateRange.Item2);

            return new Tuple<DateObject, DateObject>(startDate, endDate);
        }
    }
}
