// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.english.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.sequence.parsers.BaseSequenceParser;
import com.microsoft.recognizers.text.sequence.resources.BasePhoneNumbers;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.Arrays;
import java.util.regex.Pattern;

public class PhoneNumberParser extends BaseSequenceParser {
    private static Double SCORE_UPPER_LIMIT = 100d;
    private static Double SCORE_LOWER_LIMIT = 0d;
    private static Double BASE_SCORE = 30d;
    private static Double COUNTRY_CODE_AWARD = 40d;
    private static Double AREA_CODE_AWARD = 30d;
    private static Double FORMATTED_AWARD = 20d;
    private static Double LENGTH_AWARD = 10d;
    private static Double TYPICAL_FORMAT_DEDUCTION_SCORE = 40d;
    private static Double CONTINUE_DIGIT_DEDUCTION_SCORE = 10d;
    private static Double TAIL_SAME_DEDUCTION_SCORE = 10d;
    private static Double CONTINUE_FORMAT_INDICATOR_DEDUCTION_SCORE = 20d;
    private static Double WRONG_FORMAT_DEDUCTION_SCORE = 20d;
    private static Integer MAX_FORMAT_INDICATOR_NUM = 3;
    private static Integer MAX_LENGTH_AWARD_NUM = 3;
    private static Integer TAIL_SAME_LIMIT = 2;
    private static Integer PHONE_NUMBER_LENGTH_BASE = 8;
    private static Integer PURE_DIGIT_LENGTH_LIMIT = 11;

    // @TODO move regexes to base resource files
    private static String COMPLETE_BRACKET_REGEX = "\\(.*\\)";
    private static String SINGLE_BRACKER_REGEX = "\\(|\\)";
    private static String TAIL_SAME_DIGIT_REGEX = "([\\d])\\1{2,10}$";
    private static String PURE_DIGIT_REGEX = "^\\d*$";
    private static String CONTINUE_DIGIT_REGEX = "\\d{5}\\d*";
    private static String DIGIT_REGEX = "\\d";

    private static final Pattern COUNTRY_CODE_REGEX = Pattern.compile(BasePhoneNumbers.CountryCodeRegex);
    private static final Pattern AREA_CODE_REGEX = Pattern.compile(BasePhoneNumbers.AreaCodeIndicatorRegex);
    private static final Pattern FORMAT_INDICATOR_REGEX = Pattern.compile(BasePhoneNumbers.FormatIndicatorRegex);
    private static final Pattern NO_AREA_CODE_US_PHONE_NUMBER_REGEX = Pattern
            .compile(BasePhoneNumbers.NoAreaCodeUSPhoneNumberRegex);

    public static Double scorePhoneNumber(String phoneNumberText) {
        Double score = BASE_SCORE;

        // Country code score or area code score
        score += COUNTRY_CODE_REGEX.matcher(phoneNumberText).find() ? COUNTRY_CODE_AWARD
                : AREA_CODE_REGEX.matcher(phoneNumberText).find() ? AREA_CODE_AWARD : 0;

        // Formatted score
        Match[] formatMatches = RegExpUtility.getMatches(FORMAT_INDICATOR_REGEX, phoneNumberText);
        if (formatMatches.length > 0) {
            Integer formatIndicatorCount = formatMatches.length;
            score += Math.min(formatIndicatorCount, MAX_FORMAT_INDICATOR_NUM) * FORMATTED_AWARD;
            Boolean anyMatch = Arrays.stream(formatMatches).anyMatch(match -> match.value.length() > 1);
            score -= anyMatch ? CONTINUE_FORMAT_INDICATOR_DEDUCTION_SCORE : 0;
            if (Pattern.matches(SINGLE_BRACKER_REGEX, phoneNumberText) && !Pattern.matches(COMPLETE_BRACKET_REGEX, phoneNumberText)) {
                score -= WRONG_FORMAT_DEDUCTION_SCORE;
            }
        }

        // Length score
        score += Math.min(RegExpUtility.getMatches(Pattern.compile(DIGIT_REGEX), phoneNumberText).length
                - PHONE_NUMBER_LENGTH_BASE, MAX_LENGTH_AWARD_NUM) * LENGTH_AWARD;

        // Same tailing digit deduction
        Match[] tailSameDigitMatches = RegExpUtility.getMatches(Pattern.compile(TAIL_SAME_DIGIT_REGEX),
                phoneNumberText);
        if (tailSameDigitMatches.length > 0) {
            score -= (tailSameDigitMatches[0].value.length() - TAIL_SAME_LIMIT) * TAIL_SAME_DEDUCTION_SCORE;
        }

        // Pure digit deduction
        Match[] pureDigitMatches = RegExpUtility.getMatches(Pattern.compile(PURE_DIGIT_REGEX), phoneNumberText);
        if (pureDigitMatches.length > 0) {
            score -= phoneNumberText.length() > PURE_DIGIT_LENGTH_LIMIT ? (phoneNumberText.length() - PURE_DIGIT_LENGTH_LIMIT) * LENGTH_AWARD
                    : 0;
        }

        // Special format deduction
        score -= BasePhoneNumbers.TypicalDeductionRegexList.stream().anyMatch(o -> Pattern.compile(o).matcher(phoneNumberText).find()) ? TYPICAL_FORMAT_DEDUCTION_SCORE : 0;

        // Continue digit deduction
        Match[] continueDigitMatches = RegExpUtility.getMatches(Pattern.compile(CONTINUE_DIGIT_REGEX), phoneNumberText);
        score -= Math.max(continueDigitMatches.length - 1, 0) * CONTINUE_DIGIT_DEDUCTION_SCORE;

        // Special award for US phonenumber without area code, i.e. 223-4567 or 223 -
        // 4567
        if (NO_AREA_CODE_US_PHONE_NUMBER_REGEX.matcher(phoneNumberText).find()) {
            score += LENGTH_AWARD * 1.5;
        }

        return Math.max(Math.min(score, SCORE_UPPER_LIMIT), SCORE_LOWER_LIMIT)
                / (SCORE_UPPER_LIMIT - SCORE_LOWER_LIMIT);
    }

    @Override
    public ParseResult parse(ExtractResult extResult) {
        return new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(), extResult.getType(),
                null, PhoneNumberParser.scorePhoneNumber(extResult.getText()), extResult.getText());
    }
}
