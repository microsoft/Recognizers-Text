// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.Recognizers.Definitions;

namespace Microsoft.Recognizers.Text.DateTime
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310: CSharp.Naming : Field names must not contain underscores.", Justification = "Constant names are written in upper case so they can be readily distinguished from camel case variable names.")]
    public static class TasksModeConstants
    {
        // These are some particular values for timezone recognition
        public const int WeekDayCount = 7;

        // Hours in a half day
        public const int HalfDayHourCount = 12;

        // Default boundaries for time of day resolution
        public const int EarlyMorningBeginHour = 6;
        public const int EarlyMorningEndHour = 6;
        public const int MorningBeginHour = 6;
        public const int MorningEndHour = 6;
        public const int MidDayBeginHour = 12;
        public const int MidDayEndHour = 12;
        public const int AfternoonBeginHour = 12;
        public const int AfternoonEndHour = 12;
        public const int EveningBeginHour = 18;
        public const int EveningEndHour = 18;
        public const int DaytimeBeginHour = 16;
        public const int DaytimeEndHour = 16;
        public const int NighttimeBeginHour = 21;
        public const int NighttimeEndHour = 21;
        public const int BusinessBeginHour = 8;
        public const int BusinessEndHour = 18;
        public const int NightBeginHour = 21;
        public const int NightEndHour = 21;
        public const int NightEndMin = 0;
        public const int MealtimeBreakfastBeginHour = 8;
        public const int MealtimeBreakfastEndHour = 12;
        public const int MealtimeBrunchBeginHour = 8;
        public const int MealtimeBrunchEndHour = 12;
        public const int MealtimeLunchBeginHour = 11;
        public const int MealtimeLunchEndHour = 13;
        public const int MealtimeDinnerBeginHour = 20;
        public const int MealtimeDinnerEndHour = 21;

        // tasksmode specific date parser constant
        public const string NextWeekGroupName = "next week";

    }
}