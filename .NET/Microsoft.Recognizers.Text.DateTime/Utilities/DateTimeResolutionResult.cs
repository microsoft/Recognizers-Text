﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.DateTime
{
    public class DateTimeResolutionResult
    {
        public DateTimeResolutionResult()
        {
            Success = false;
        }

        public bool Success { get; set; }

        public string Timex { get; set; }

        public bool IsLunar { get; set; }

        public string Mod { get; set; }

        public bool HasRangeChangingMod { get; set; } = false;

        public string Comment { get; set; }

        public Dictionary<string, string> FutureResolution { get; set; }

        public Dictionary<string, string> PastResolution { get; set; }

        public object FutureValue { get; set; }

        public object PastValue { get; set; }

        public List<object> SubDateTimeEntities { get; set; }

        public TimeZoneResolutionResult TimeZoneResolution { get; set; }

        public List<object> List { get; set; }
    }
}