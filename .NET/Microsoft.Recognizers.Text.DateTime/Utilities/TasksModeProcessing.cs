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

        // Change resolution value of datetime value under tasksmode.
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
                endHour = Constants.MealtimeBrunchEndHour;
            }
            else if (todSymbol == Constants.MealtimeLunch)
            {
                beginHour = Constants.MealtimeLunchBeginHour;
                endHour = Constants.MealtimeLunchEndHour;
            }
            else if (todSymbol == Constants.MealtimeDinner)
            {
                beginHour = 20;
                endHour = 21;
            }
            else
            {
                return false;
            }

            return true;
        }

    }
}