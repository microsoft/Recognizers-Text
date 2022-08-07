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
                case TasksModeConstants.EarlyMorning:
                    result.Timex = TasksModeConstants.EarlyMorning;
                    result.BeginHour = TasksModeConstants.EarlyMorningBeginHour;
                    result.EndHour = TasksModeConstants.EarlyMorningEndHour;
                    break;
                case TasksModeConstants.Morning:
                    result.Timex = TasksModeConstants.Morning;
                    result.BeginHour = TasksModeConstants.MorningBeginHour;
                    result.EndHour = TasksModeConstants.MorningEndHour;
                    break;
                case TasksModeConstants.MidDay:
                    result.Timex = TasksModeConstants.MidDay;
                    result.BeginHour = TasksModeConstants.MidDayBeginHour;
                    result.EndHour = TasksModeConstants.MidDayEndHour;
                    break;
                case TasksModeConstants.Afternoon:
                    result.Timex = TasksModeConstants.Afternoon;
                    result.BeginHour = TasksModeConstants.AfternoonBeginHour;
                    result.EndHour = TasksModeConstants.AfternoonEndHour;
                    break;
                case TasksModeConstants.Evening:
                    result.Timex = TasksModeConstants.Evening;
                    result.BeginHour = TasksModeConstants.EveningBeginHour;
                    result.EndHour = TasksModeConstants.EveningEndHour;
                    break;
                case TasksModeConstants.Daytime:
                    result.Timex = TasksModeConstants.Daytime;
                    result.BeginHour = TasksModeConstants.DaytimeBeginHour;
                    result.EndHour = TasksModeConstants.DaytimeEndHour;
                    break;
                case TasksModeConstants.Nighttime:
                    result.Timex = TasksModeConstants.Nighttime;
                    result.BeginHour = TasksModeConstants.NighttimeBeginHour;
                    result.EndHour = TasksModeConstants.NighttimeEndHour;
                    break;
                case TasksModeConstants.BusinessHour:
                    result.Timex = TasksModeConstants.BusinessHour;
                    result.BeginHour = TasksModeConstants.BusinessBeginHour;
                    result.EndHour = TasksModeConstants.BusinessEndHour;
                    break;
                case TasksModeConstants.Night:
                    result.Timex = TasksModeConstants.Night;
                    result.BeginHour = TasksModeConstants.NightBeginHour;
                    result.EndHour = TasksModeConstants.NightEndHour;
                    result.EndMin = TasksModeConstants.NightEndMin;
                    break;
                case TasksModeConstants.MealtimeBreakfast:
                    result.Timex = TasksModeConstants.MealtimeBreakfast;
                    result.BeginHour = TasksModeConstants.MealtimeBreakfastBeginHour;
                    result.EndHour = TasksModeConstants.MealtimeBreakfastEndHour;
                    break;
                case TasksModeConstants.MealtimeBrunch:
                    result.Timex = TasksModeConstants.MealtimeBrunch;
                    result.BeginHour = TasksModeConstants.MealtimeBrunchBeginHour;
                    result.EndHour = TasksModeConstants.MealtimeBrunchEndHour;
                    break;
                case TasksModeConstants.MealtimeLunch:
                    result.Timex = TasksModeConstants.MealtimeLunch;
                    result.BeginHour = TasksModeConstants.MealtimeLunchBeginHour;
                    result.EndHour = TasksModeConstants.MealtimeLunchEndHour;
                    break;
                case TasksModeConstants.MealtimeDinner:
                    result.Timex = TasksModeConstants.MealtimeDinner;
                    result.BeginHour = TasksModeConstants.MealtimeDinnerBeginHour;
                    result.EndHour = TasksModeConstants.MealtimeDinnerEndHour;
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
            if (todSymbol == TasksModeConstants.Morning)
            {
                beginHour = TasksModeConstants.MorningBeginHour;
                endHour = TasksModeConstants.EarlyMorningEndHour;
            }
            else if (todSymbol == TasksModeConstants.Afternoon)
            {
                beginHour = TasksModeConstants.AfternoonBeginHour;
                endHour = TasksModeConstants.AfternoonEndHour;

            }
            else if (todSymbol == TasksModeConstants.Evening)
            {
                beginHour = TasksModeConstants.EveningBeginHour;
                endHour = TasksModeConstants.EveningEndHour;
            }
            else if (todSymbol == TasksModeConstants.Night)
            {
                beginHour = TasksModeConstants.NightBeginHour;
                endHour = TasksModeConstants.NightEndHour;
            }
            else if (todSymbol == TasksModeConstants.MealtimeBreakfast)
            {
                beginHour = TasksModeConstants.MealtimeBreakfastBeginHour;
                endHour = TasksModeConstants.MealtimeBreakfastEndHour;
            }
            else if (todSymbol == TasksModeConstants.MealtimeBrunch)
            {
                beginHour = TasksModeConstants.MealtimeBrunchBeginHour;
                endHour = TasksModeConstants.MealtimeBrunchEndHour;
            }
            else if (todSymbol == TasksModeConstants.MealtimeDinner)
            {
                beginHour = TasksModeConstants.MealtimeDinnerBeginHour;
                endHour = TasksModeConstants.MealtimeDinnerEndHour;
            }
            else if (todSymbol == TasksModeConstants.MealtimeLunch)
            {
                beginHour = TasksModeConstants.MealtimeLunchBeginHour;
                endHour = TasksModeConstants.MealtimeLunchEndHour;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}