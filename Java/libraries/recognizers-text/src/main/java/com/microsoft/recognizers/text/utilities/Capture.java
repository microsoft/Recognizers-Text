// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.utilities;

public class Capture {
    public final String value;
    public final int index;
    public final int length;

    public Capture(String value, int index, int length) {
        this.value = value;
        this.index = index;
        this.length = length;
    }
}
