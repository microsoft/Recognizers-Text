// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

public class TimexSet {
    private TimexProperty timex;

    public TimexSet(String timex) {
        this.timex = new TimexProperty(timex);
    }

    public TimexProperty getTimex() {
        return timex;
    }

    public void setTimex(TimexProperty withTimex) {
        this.timex = withTimex;
    }
}
