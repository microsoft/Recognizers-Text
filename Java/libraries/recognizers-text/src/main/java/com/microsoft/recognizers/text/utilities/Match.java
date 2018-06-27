package com.microsoft.recognizers.text.utilities;

import java.util.Map;

public class Match {
    public final int index;
    public final int length;
    public final String value;
    public final Map<String, MatchGroup> innerGroups;

    public Match(int index, int length, String value, Map<String, MatchGroup> innerGroups) {
        this.index = index;
        this.length = length;
        this.value = value;
        this.innerGroups = innerGroups;
    }

    public MatchGroup getGroup(String key) {
        if (innerGroups.containsKey(key)) {
            return innerGroups.get(key);
        }

        return new MatchGroup("", 0, 0, new String[0]);
    }
}
