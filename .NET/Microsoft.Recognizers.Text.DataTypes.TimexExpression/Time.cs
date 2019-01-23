// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Recognizers.Text.DataTypes.TimexExpression
{
    public class Time
    {
        public Time(int seconds)
        {
            Hour = (int)Math.Floor(seconds / 3600000m);
            Minute = (int)Math.Floor((seconds - (Hour * 3600000)) / 60000m);
            Second = (seconds - (Hour * 3600000) - (Minute * 60000)) / 1000;
        }

        public Time(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }

        public int GetTime()
        {
            return (Second * 1000) + (Minute * 60000) + (Hour * 3600000);
        }
    }
}
