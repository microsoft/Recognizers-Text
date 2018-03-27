package com.microsoft.recognizers.text;

import java.util.Arrays;

public class Culture {
    public static final String English = "en-us";
    public static final String Chinese = "zh-cn";
    public static final String Spanish = "es-es";
    public static final String Portuguese = "pt-br";
    public static final String French = "fr-fr";
    public static final String Japanese = "ja-jp";

    private static final Culture[] SupportedCultures = new Culture[]{
            new Culture("English", Culture.English),
            new Culture("Chinese", Culture.Chinese),
            new Culture("Spanish", Culture.Spanish),
            new Culture("Portuguese", Culture.Portuguese),
            new Culture("French", Culture.French),
            new Culture("Japanese", Culture.Japanese)
    };


    public final String cultureName;
    public final String cultureCode;

    public Culture(String cultureName, String cultureCode) {
        this.cultureName = cultureName;
        this.cultureCode = cultureCode;
    }

    public static String[] getSupportedCultureCodes() {
        return Arrays.asList(SupportedCultures).stream()
                .map(c -> c.cultureCode)
                .toArray(size -> new String[size]);
    }
}
