// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Recognizers.Text.DateTime
{
    public class TimeZoneResolutionResult
    {
        public string Value { get; set; }

        public int UtcOffsetMins { get; set; }

        public string TimeZoneText { get; set; }
    }
}