// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.numberwithunit.models;

public class CurrencyUnitValue {

    public final String number;
    public final String unit;
    public final String isoCurrency;

    public CurrencyUnitValue(String number, String unit, String isoCurrency) {
        this.number = number;
        this.unit = unit;
        this.isoCurrency = isoCurrency;
    }
}
