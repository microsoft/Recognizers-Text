// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.config.PhoneNumberConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BasePhoneNumbers;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;

public class BasePhoneNumberExtractor extends BaseSequenceExtractor {
    private static final Pattern INTERNATIONAL_DIALING_PREFIX_REGEX = Pattern
            .compile(BasePhoneNumbers.InternationDialingPrefixRegex);

    private static final Pattern PRE_CHECK_PHONE_NUMBER_REGEX = Pattern
            .compile(BasePhoneNumbers.PreCheckPhoneNumberRegex);

    private static final Pattern SSN_FILTER_REGEX = Pattern.compile(BasePhoneNumbers.SSNFilterRegex);

    private static List<Character> SPECIAL_BOUNDARY_MARKERS = BasePhoneNumbers.SpecialBoundaryMarkers;

    private PhoneNumberConfiguration config;

    protected String extractType = Constants.SYS_PHONE_NUMBER;

    protected String getExtractType() {
        return this.extractType;
    }

    public BasePhoneNumberExtractor(PhoneNumberConfiguration config) {
        this.config = config;

        String wordBoundariesRegex = config.getWordBoundariesRegex();
        String nonWordBoundariesRegex = config.getNonWordBoundariesRegex();
        String endWordBoundariesRegex = config.getEndWordBoundariesRegex();

        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(Pattern
                        .compile(BasePhoneNumbers.GeneralPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_GENERAL);
                put(Pattern.compile(BasePhoneNumbers.BRPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex,
                        endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_BR);
                put(Pattern.compile(BasePhoneNumbers.UKPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex,
                        endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_UK);
                put(Pattern.compile(BasePhoneNumbers.DEPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_DE);
                put(Pattern.compile(BasePhoneNumbers.USPhoneNumberRegex(wordBoundariesRegex, nonWordBoundariesRegex,
                        endWordBoundariesRegex)), Constants.PHONE_NUMBER_REGEX_US);
                put(Pattern.compile(BasePhoneNumbers.CNPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_CN);
                put(Pattern.compile(BasePhoneNumbers.DKPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_DK);
                put(Pattern.compile(BasePhoneNumbers.ITPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_IT);
                put(Pattern.compile(BasePhoneNumbers.NLPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_NL);
                put(Pattern
                        .compile(BasePhoneNumbers.SpecialPhoneNumberRegex(wordBoundariesRegex, endWordBoundariesRegex)),
                        Constants.PHONE_NUMBER_REGEX_SPECIAL);
            }
        };

        this.regexes = regexes;
    }

    @Override
    public List<ExtractResult> extract(String text) {
        if (!PRE_CHECK_PHONE_NUMBER_REGEX.matcher(text).find()) {
            return new ArrayList<ExtractResult>();
        }

        List<ExtractResult> ers = super.extract(text);

        for (int i = 0; i < ers.size(); i++) {
            ExtractResult er = ers.get(i);
            if ((BasePhoneNumberExtractor.countDigits(er.getText()) < 7 && er.getData().toString() != "ITPhoneNumber") ||
                Pattern.matches(SSN_FILTER_REGEX.toString(), er.getText())) {
                ers.remove(er);
                i--;
                continue;
            }

            if ((BasePhoneNumberExtractor.countDigits(er.getText()) == 16 && !er.getText().startsWith("+"))) {
                ers.remove(er);
                i--;
                continue;
            }

            if (BasePhoneNumberExtractor.countDigits(er.getText()) == 15) {
                Boolean flag = false;
                for (String numSpan : er.getText().split(" ")) {
                    if (BasePhoneNumberExtractor.countDigits(numSpan) == 4 || BasePhoneNumberExtractor.countDigits(numSpan) == 3) {
                        flag = false;
                    } else {
                        flag = true;
                        break;
                    }
                }

                if (flag == false) {
                    ers.remove(er);
                    i--;
                    continue;
                }
            }

            if (er.getStart() + er.getLength() < text.length()) {
                Character ch = text.charAt(er.getStart() + er.getLength());
                if (BasePhoneNumbers.ForbiddenSuffixMarkers.contains(ch)) {
                    ers.remove(er);
                    i--;
                    continue;
                }
            }

            if (er.getStart() != 0) {
                Character ch = text.charAt(er.getStart() - 1);
                String front = text.substring(0, er.getStart() - 1);

                if (this.config.getFalsePositivePrefixRegex() != null &&
                    this.config.getFalsePositivePrefixRegex().matcher(front).find()) {
                    ers.remove(er);
                    i--;
                    continue;
                }

                if (BasePhoneNumbers.BoundaryMarkers.contains(ch)) {
                    if (SPECIAL_BOUNDARY_MARKERS.contains(ch) &&
                        BasePhoneNumberExtractor.checkFormattedPhoneNumber(er.getText()) && er.getStart() >= 2) {
                        Character charGap = text.charAt(er.getStart() - 2);
                        if (!Character.isDigit(charGap) && !Character.isWhitespace(charGap)) {
                            // check if the extracted string has a non-digit string before "-".
                            Boolean flag = Pattern.matches("^[^0-9]+$", text.substring(0, er.getStart() - 2));

                            // Handle cases like "91a-677-0060".
                            if (Character.isLowerCase(charGap) && !flag) {
                                ers.remove(er);
                                i--;
                            }

                            continue;
                        }

                        // check the international dialing prefix
                        if (INTERNATIONAL_DIALING_PREFIX_REGEX.matcher(front).find()) {
                            Integer moveOffset = RegExpUtility.getMatches(INTERNATIONAL_DIALING_PREFIX_REGEX,
                                    front)[0].length + 1;
                            er.setStart(er.getStart() - moveOffset);
                            er.setLength(er.getLength() + moveOffset);
                            er.setText(text.substring(er.getStart(), er.getStart() + er.getLength()));
                            continue;
                        }
                    }

                    // Handle cases like "-1234567" and "-1234+5678"
                    ers.remove(er);
                    i--;
                }

                if (this.config.getForbiddenPrefixMarkers().contains(ch)) {
                    {
                        // Handle "tel:123456"
                        if (BasePhoneNumbers.ColonMarkers.contains(ch)) {
                            if (this.config.getColonPrefixCheckRegex().matcher(front).find()) {
                                continue;
                            }
                        }

                        ers.remove(er);
                        i--;
                    }
                }
            }
        }

        // filter hexadecimal address like 00 10 00 31 46 D9 E9 11
        Match[] maskMatchCollection = RegExpUtility.getMatches(Pattern.compile(BasePhoneNumbers.PhoneNumberMaskRegex),
                text);

        for (int index = ers.size() - 1; index >= 0; --index) {
            for (Match m : maskMatchCollection) {
                if (ers.get(index).getStart() >= m.index &&
                    ers.get(index).getStart() + ers.get(index).getLength() <= m.index + m.length) {
                    ers.remove(index);
                    break;
                }
            }
        }

        return ers;
    }

    private static Boolean checkFormattedPhoneNumber(String phoneNumberText) {
        return Pattern.compile(BasePhoneNumbers.FormatIndicatorRegex).matcher(phoneNumberText).find();
    }

    private static Integer countDigits(String candidateString) {
        Integer count = 0;
        for (Character t : candidateString.toCharArray()) {
            if (Character.isDigit(t)) {
                ++count;
            }
        }

        return count;
    }
}
