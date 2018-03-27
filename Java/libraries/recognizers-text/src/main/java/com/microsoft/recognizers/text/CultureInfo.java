package com.microsoft.recognizers.text;

public class CultureInfo {

    private final String cultureCode;

    public CultureInfo(String cultureCode) {
        this.cultureCode = cultureCode;
    }

    public static CultureInfo getCultureInfo(String cultureCode) {
        return new CultureInfo(cultureCode);
    }
}
