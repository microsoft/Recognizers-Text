// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.parsers.config;

public class PrefixAdjustResult {
    public final int hour;
    public final int minute;
    public final boolean hasMin;

    public PrefixAdjustResult(int hour, int minute, boolean hasMin) {
        this.hour = hour;
        this.minute = minute;
        this.hasMin = hasMin;
    }
}
