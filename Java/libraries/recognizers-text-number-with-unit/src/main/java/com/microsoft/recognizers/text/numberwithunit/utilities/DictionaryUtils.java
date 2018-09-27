package com.microsoft.recognizers.text.numberwithunit.utilities;

import com.microsoft.recognizers.text.utilities.FormatUtility;

import java.util.Map;
import java.util.regex.Pattern;

public abstract class DictionaryUtils {

    /**
     * Safely bind dictionary which contains several key-value pairs to the destination dictionary.
     * This function is used to bind all the prefix and suffix for units.
     */
    public static void bindDictionary(Map<String, String> dictionary,
                                      Map<String, String> sourceDictionary) {
        if (dictionary == null) return;

        for (Map.Entry<String, String> pair : dictionary.entrySet()) {
            if (pair.getKey() == null || pair.getKey().isEmpty()) {
                continue;
            }

            bindUnitsString(sourceDictionary, pair.getKey(), pair.getValue());
        }
    }

    /**
     * Bind keys in a string which contains words separated by '|'.
     */
    public static void bindUnitsString(Map<String, String> sourceDictionary, String key, String source) {
        String[] values = source.trim().split(Pattern.quote("|"));

        for (String token : values) {
            if (token.isEmpty() || sourceDictionary.containsKey(token)) {
                continue;
            }

            sourceDictionary.put(token, key);
        }
    }
}
