package com.microsoft.recognizers.text.numberwithunit.models;

public class UnitValue {

    public final String number;
    public final String unit;

    public UnitValue(String number, String unit) {
        this.unit = unit;
        this.number = number;
    }
}
