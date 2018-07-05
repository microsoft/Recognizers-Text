package com.microsoft.recognizers.text.numberwithunit.models;

public class PrefixUnitResult {
    public final int offset;
    public final String unitStr;

    public PrefixUnitResult(int offset, String unitStr) {
        this.offset = offset;
        this.unitStr = unitStr;
    }
}
