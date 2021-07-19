// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit;

public enum NumberWithUnitOptions {
    None(0);

    private final int value;

    NumberWithUnitOptions(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
