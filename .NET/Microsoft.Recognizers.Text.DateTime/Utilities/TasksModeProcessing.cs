using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text.Utilities;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime
{
    public static class TasksModeProcessing
    {
        public const string ParserTypeName = "datetimeV2";

        public static readonly string DateMinString = DateTimeFormatUtil.FormatDate(DateObject.MinValue);

        /*
        TasksModeModification modifies past datetime references under tasksmode.
        Eg if input text is 22 june at 9 pm and current time is 22 june 2022, 8 am then
        under default mode pastdateime value will be 22 june 2022 9 pm, but since time has not been passed
        under tasksmode it's value will get mapped to 22 june 2021 9 pm.
        */
        public static DateTimeParseResult TasksModeModification(DateTimeParseResult slot, DateObject referenceTime)
        {
            switch (slot.Type.Substring(ParserTypeName.Length + 1))
            {
                case Constants.SYS_DATETIME_DATE:
                    slot = TasksModeModifyDateValue(slot, referenceTime);
                    break;
            }

            return slot;
        }

        /*
         Change resolution value of datetime value under tasksmode.
         */
        public static TimeOfDayResolutionResult TasksModeResolveTimeOfDay(string tod)
        {
            var result = new TimeOfDayResolutionResult();
            switch (tod)
            {
                case Constants.EarlyMorning:
                    result.Timex = Constants.EarlyMorning;
                    result.BeginHour = 6;
                    result.EndHour = 6;
                    break;

                case Constants.Morning:
                    result.Timex = Constants.Morning;
                    result.BeginHour = 6;
                    result.EndHour = 6;
                    break;

                case Constants.MidDay:
                    result.Timex = Constants.MidDay;
                    result.BeginHour = Constants.HalfDayHourCount;
                    result.EndHour = Constants.HalfDayHourCount;
                    break;

                case Constants.Afternoon:
                    result.Timex = Constants.Afternoon;
                    result.BeginHour = Constants.HalfDayHourCount;
                    result.EndHour = Constants.HalfDayHourCount;
                    break;

                case Constants.Evening:
                    result.Timex = Constants.Evening;
                    result.BeginHour = 18;
                    result.EndHour = 18;
                    break;

                case Constants.Daytime:
                    result.Timex = Constants.Daytime;
                    result.BeginHour = 16;
                    result.EndHour = 16;
                    break;

                case Constants.Nighttime:
                    result.Timex = Constants.Nighttime;
                    result.BeginHour = 21;
                    result.EndHour = 21;
                    break;

                case Constants.BusinessHour:
                    result.Timex = Constants.BusinessHour;
                    result.BeginHour = Constants.BusinessBeginHour;
                    result.EndHour = Constants.BusinessEndHour;
                    break;

                case Constants.Night:
                    result.Timex = Constants.Night;
                    result.BeginHour = 21;
                    result.EndHour = 21;
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
                    result.BeginHour = 20;
                    result.EndHour = 21;
                    break;

                default:
                    break;
            }

            return result;
        }

        /*
         Change beginHour and endHour for subjective time refereneces under TasksMode.
         morning get's mapped to 6:00 am
         */
        public static bool GetMatchedTimeRangeForTasksMode(string text, string todSymbol, out int beginHour, out int endHour, out int endMin)
        {
            var trimmedText = text.Trim();
            beginHour = 0;
            endHour = 0;
            endMin = 0;
            if (todSymbol == Constants.Morning)
            {
                beginHour = 6;
                endHour = 6;
            }
            else if (todSymbol == Constants.Afternoon)
            {
                beginHour = Constants.HalfDayHourCount;
                endHour = Constants.HalfDayHourCount;

            }
            else if (todSymbol == Constants.Evening)
            {
                beginHour = 18;
                endHour = 18;
            }
            else if (todSymbol == Constants.Night)
            {
                beginHour = 21;
                endHour = 21;
            }
            else if (todSymbol == Constants.MealtimeBreakfast)
            {
                beginHour = Constants.MealtimeBreakfastBeginHour;
                endHour = Constants.MealtimeBreakfastEndHour;
            }
            else if (todSymbol == Constants.MealtimeBrunch)
            {
                beginHour = Constants.MealtimeBrunchBeginHour;
                endHour = Constants.MealtimeBrunchBeginHour;
            }
            else if (todSymbol == Constants.MealtimeDinner)
            {
                beginHour = 20;
                endHour = 21;
            }
            else if (todSymbol == Constants.MealtimeLunch)
            {
                beginHour = Constants.MealtimeLunchBeginHour;
                endHour = Constants.MealtimeLunchEndHour;
            }
            else
            {
                return false;
            }

            return true;
        }

        /*Under TasksMode If you input today's date, future date should get mapped to current date insted of next year.
        ex if input is meet on 7 july and refrence time is 7 july 2022,
        expected future value --> 7 july 2022 &&
        past value--> 7 july 2021
        */
        private static DateTimeParseResult TasksModeModifyDateValue(DateTimeParseResult slot, DateObject referenceTime)
        {
            if (slot.TimexStr.Contains("XXXX"))
            {
                return slot;
            }

            var value = (SortedDictionary<string, object>)slot.Value;
            if (value != null && value.ContainsKey(ResolutionKey.ValueSet))
            {
                if (value[ResolutionKey.ValueSet] is IList<Dictionary<string, string>> valueSet && valueSet.Any())
                {
                    foreach (var values in valueSet)
                    {
                        if (slot.Text.Contains("next week"))
                        {
                            var tempdate = referenceTime.Upcoming(DayOfWeek.Monday).Date;
                            var dateTimeToSet = DateObject.MinValue.SafeCreateFromValue(tempdate.Year, tempdate.Month, tempdate.Day);
                            values[DateTimeResolutionKey.Value] = DateTimeFormatUtil.FormatDate(dateTimeToSet);
                            values[DateTimeResolutionKey.Timex] = $"{DateTimeFormatUtil.LuisDate(dateTimeToSet)}";
                        }

                    }
                }
            }

            slot.Value = value;

            return slot;
        }
    }
}