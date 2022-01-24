// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.german.parsers;

import com.google.common.collect.ImmutableMap;
import com.microsoft.recognizers.text.datetime.german.extractors.GermanHolidayExtractorConfiguration;
import com.microsoft.recognizers.text.datetime.parsers.BaseHolidayParserConfiguration;
import com.microsoft.recognizers.text.datetime.resources.GermanDateTime;
import com.microsoft.recognizers.text.datetime.utilities.DateUtil;
import com.microsoft.recognizers.text.datetime.utilities.HolidayFunctions;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.time.DayOfWeek;
import java.time.LocalDateTime;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Locale;
import java.util.Map;
import java.util.Optional;
import java.util.regex.Pattern;

public class GermanHolidayParserConfiguration extends BaseHolidayParserConfiguration {

    private final Pattern thisPrefixRegex;
    private final Pattern nextPrefixRegex;
    private final Pattern previousPrefixRegex;

    public GermanHolidayParserConfiguration() {

        super();

        this.setHolidayRegexList(GermanHolidayExtractorConfiguration.HolidayRegexList);

        HashMap<String, Iterable<String>> newMap = new HashMap<>();
        for (Map.Entry<String, String[]> entry : GermanDateTime.HolidayNames.entrySet()) {
            if (entry.getValue() instanceof String[]) {
                newMap.put(entry.getKey(), Arrays.asList(entry.getValue()));
            }
        }
        this.setHolidayNames(ImmutableMap.copyOf(newMap));

        thisPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.ThisPrefixRegex);
        nextPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.NextPrefixRegex);
        previousPrefixRegex = RegExpUtility.getSafeRegExp(GermanDateTime.PreviousPrefixRegex);
    }



    @Override
    public int getSwiftYear(String text) {

        String trimmedText = StringUtility.trimStart(StringUtility.trimEnd(text)).toLowerCase(Locale.ROOT);
        int swift = -10;
        Optional<Match> matchNextPrefixRegex = Arrays.stream(RegExpUtility.getMatches(nextPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchPreviousPrefixRegex = Arrays.stream(RegExpUtility.getMatches(previousPrefixRegex, trimmedText)).findFirst();
        Optional<Match> matchThisPrefixRegex = Arrays.stream(RegExpUtility.getMatches(thisPrefixRegex, trimmedText)).findFirst();

        if (matchNextPrefixRegex.isPresent()) {
            swift = 1;
        } else if (matchPreviousPrefixRegex.isPresent()) {
            swift = -1;
        } else if (matchThisPrefixRegex.isPresent()) {
            swift = 0;
        }

        return swift;
    }

    public String sanitizeHolidayToken(String holiday) {
        return holiday.replace(" ", "")
                .replace("-", "")
                .replace("'", "");
    }
}
