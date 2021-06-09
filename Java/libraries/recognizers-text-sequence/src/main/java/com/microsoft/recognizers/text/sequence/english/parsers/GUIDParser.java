// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.english.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.sequence.parsers.BaseSequenceParser;
import com.microsoft.recognizers.text.sequence.resources.BaseGUID;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.regex.Pattern;

public class GUIDParser extends BaseSequenceParser {
    private static Double SCORE_UPPER_LIMIT = 100d;
    private static Double SCORE_LOWER_LIMIT = 0d;
    private static Double BASE_SCORE = 100d;
    private static Double NO_BOUNDARY_PENALTY = 10d;
    private static Double NO_FORMAT_PENALTY = 10d;
    private static Double PURE_DIGIT_PENALTY = 15d;
    private static String PURE_DIGIT_REGEX = "^\\d*$";
    private static String FORMAT_REGEX = "-";

    private static final Pattern GUID_ELEMENT_REGEX = Pattern.compile(BaseGUID.GUIDRegexElement);

    public static Double scoreGUID(String textGUID) {
        Double score = BASE_SCORE;

        Match[] elementMatch = RegExpUtility.getMatches(GUID_ELEMENT_REGEX, textGUID);
        if (elementMatch.length > 0) {
            Integer startIndex = elementMatch[0].index;
            String guidElement = elementMatch[0].value;
            score -= startIndex == 0 ? NO_BOUNDARY_PENALTY : 0;
            score -= Pattern.compile(FORMAT_REGEX).matcher(guidElement).find() ? 0 : NO_FORMAT_PENALTY;
            score -= Pattern.compile(PURE_DIGIT_REGEX).matcher(textGUID).find() ? PURE_DIGIT_PENALTY : 0;
        }

        return Math.max(Math.min(score, SCORE_UPPER_LIMIT), SCORE_LOWER_LIMIT)
                / (SCORE_UPPER_LIMIT - SCORE_LOWER_LIMIT);
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {
        return new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(),
                null, GUIDParser.scoreGUID(extResult.getText()), extResult.getText());
    }
}
