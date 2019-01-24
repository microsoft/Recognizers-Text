package com.microsoft.recognizers.text.datetime.utilities;

import com.google.common.collect.ImmutableMap;

public abstract class StringExtension {
    public static String normalize(String text, ImmutableMap<Character, Character> dic) {
        for (ImmutableMap.Entry<Character, Character> keyPair : dic.entrySet()) {
            text = text.replace(keyPair.getKey(), keyPair.getValue());
        }

        return text;
    }
}
