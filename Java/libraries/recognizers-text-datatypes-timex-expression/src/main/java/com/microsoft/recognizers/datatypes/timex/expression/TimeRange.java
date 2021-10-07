// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

public class TimeRange {
    private Time start;

    private Time end;

    public Time getStart() {
        return start;
    }

    public void setStart(Time withStart) {
        this.start = withStart;
    }

    public Time getEnd() {
        return end;
    }

    public void setEnd(Time withEnd) {
        this.end = withEnd;
    }
}
