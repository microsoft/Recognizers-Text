package com.microsoft.recognizers.text;

import java.util.Arrays;

public class Culture {
    public static final String English = "en-us";
    public static final String Chinese = "zh-cn";
    public static final String Spanish = "es-es";
    public static final String Portuguese = "pt-br";
    public static final String French = "fr-fr";
    public static final String German = "de-de";
    public static final String Japanese = "ja-jp";
    public static final String Dutch = "nl-nl";

    public static final Culture[] SupportedCultures = new Culture[]{
            new Culture("English", English),
            new Culture("Chinese", Chinese),
            new Culture("Spanish", Spanish),
            new Culture("Portuguese", Portuguese),
            new Culture("French", French),
            new Culture("German", German),
            new Culture("Japanese", Japanese),
            new Culture("Dutch", Dutch)
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
