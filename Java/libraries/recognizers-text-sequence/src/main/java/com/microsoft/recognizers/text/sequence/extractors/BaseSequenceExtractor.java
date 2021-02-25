// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;
import java.util.regex.Pattern;
import java.util.stream.Stream;

public abstract class BaseSequenceExtractor implements IExtractor {
    protected Map<Pattern, String> regexes;
    protected String extractType = "";

    protected List<ExtractResult> postFilter(List<ExtractResult> results) {
        return results;
    }

    protected Map<Pattern, String> getRegexes() {
        return regexes;
    }

    protected String getExtractType() {
        return extractType;
    }

    @Override
    public List<ExtractResult> extract(String text) {
        List<ExtractResult> result = new ArrayList<>();

        if (text.isEmpty()) {
            return result;
        }

        HashMap<Match, String> matchSource = new HashMap<>();
        boolean[] matched = new boolean[text.length()];

        // Traverse every match results to see each position in the text is matched or
        // not.
        HashMap<Match[], String> collections = new HashMap<>();
        regexes.forEach((key, value) -> {
            Match[] matches = RegExpUtility.getMatches(key, text);
            collections.put(matches, value);
        });

        collections.forEach((key, value) -> {
            for (Match match : key) {
                if (isValidMatch(match)) {
                    for (int j = 0; j < match.length; j++) {
                        matched[match.index + j] = true;
                    }

                    // Keep Source Data for extra information
                    matchSource.put(match, value);
                }
            }
        });

        // Form the extracted results mark all the matched intervals in the text.
        int lastNotMatched = -1;
        for (int i = 0; i < text.length(); i++) {
            if (matched[i]) {
                if (i + 1 == text.length() || !matched[i + 1]) {
                    int start = lastNotMatched + 1;
                    int length = i - lastNotMatched;
                    String substr = text.substring(start, start + length);
                    Function<Match, Boolean> matchFunc = match -> match.index == start && match.length == length;

                    if (matchSource.keySet().stream().anyMatch(o -> matchFunc.apply(o))) {
                        Match srcMatch = (Match)matchSource.keySet().toArray()[0];
                        ExtractResult extResult = new ExtractResult();

                        extResult.setStart(start);
                        extResult.setLength(length);
                        extResult.setText(substr);
                        extResult.setType(this.extractType);
                        extResult.setData(matchSource.getOrDefault(srcMatch, null));
                        result.add(extResult);
                    }
                }
            } else {
                lastNotMatched = i;
            }
        }

        return this.postFilter(result);
    }

    public Boolean isValidMatch(Match match) {
        return true;
    }
}