package com.microsoft.recognizers.text.utilities;

import java.util.regex.Matcher;

public interface StringReplacerCallback {
    public String replace(Matcher match);
}
