// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.ParseResult;
import com.microsoft.recognizers.text.utilities.StringUtility;

import org.apache.commons.lang3.StringUtils;

public class BaseIpParser extends BaseSequenceParser {
    @Override
    public ParseResult parse(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(),
                extResult.getType(), extResult.getData(), null, BaseIpParser.dropLeadingZeros(extResult.getText()));

        return result;
    }

    private static String dropLeadingZeros(String text) {
        String result = new String();
        String number = new String();
        for (int i = 0; i < text.length(); i++) {
            Character c = text.charAt(i);
            if (c == '.' || c == ':') {
                if (!StringUtils.isBlank(number)) {
                    number = number == "0" ? number : StringUtility.trimStart(number, "^[0]+","");
                    number = StringUtils.isBlank(number) ? "0" : number;
                    result += number;
                }

                result += text.charAt(i);
                number = new String();
            } else {
                number += c.toString();
                if (i == text.length() - 1) {
                    number = number == "0" ? number : StringUtility.trimStart(number, "^[0]+","");
                    number = StringUtils.isBlank(number) ? "0" : number;
                    result += number;
                }
            }
        }

        return result;
    }
}
