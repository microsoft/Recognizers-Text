package com.microsoft.recognizers.text.utilities;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public abstract class DefinitionLoader {

    public static Map<Pattern, Pattern> loadAmbiguityFilters(Map<String, String> filters) {

        HashMap<Pattern, Pattern> ambiguityFiltersDict = new HashMap<>();

        for (Map.Entry<String, String> pair : filters.entrySet()) {

            if (!"null".equals(pair.getKey())) {
                Pattern key = RegExpUtility.getSafeRegExp(pair.getKey());
                Pattern val = RegExpUtility.getSafeRegExp(pair.getValue());
                ambiguityFiltersDict.put(key, val);
            }
        }

        return ambiguityFiltersDict;
    }

}
