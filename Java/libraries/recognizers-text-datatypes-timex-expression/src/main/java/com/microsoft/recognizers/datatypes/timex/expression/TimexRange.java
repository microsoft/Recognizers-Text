// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

public class TimexRange {
    private TimexProperty start;

    private TimexProperty end;

    private TimexProperty duration;

    public TimexProperty getStart() {
        return start;
    }

    public void setStart(TimexProperty withStart) {
        this.start = withStart;
    }

    public TimexProperty getEnd() {
        return end;
    }

    public void setEnd(TimexProperty withEnd) {
        this.end = withEnd;
    }

    public TimexProperty getDuration() {
        return duration;
    }

    public void setDuration(TimexProperty withDuration) {
        this.duration = withDuration;
    }
}
