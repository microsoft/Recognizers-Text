// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.Time;
import org.junit.Assert;
import org.junit.Test;

public class TestTime {

    @Test
    public void dataTypesTimeConstructor() {
        Time t = new Time(23, 45, 32);
        Assert.assertEquals(23, (int)t.getHour());
        Assert.assertEquals(45, (int)t.getMinute());
        Assert.assertEquals(32, (int)t.getSecond());
    }

    @Test
    public void dataTypesTimeGetTime() {
        Time t = new Time(23, 45, 32);
        Assert.assertEquals(85532000, (int)t.getTime());
    }
}
