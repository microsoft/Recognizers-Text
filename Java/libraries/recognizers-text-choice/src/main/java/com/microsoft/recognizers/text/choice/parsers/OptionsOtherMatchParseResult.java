// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.choice.parsers;

public class OptionsOtherMatchParseResult {

    public final double score;
    public final String text;
    public final Object value;

    public OptionsOtherMatchParseResult(String parseResultText, Object parseResultValue, double parseResultScore) {
        score = parseResultScore;
        text = parseResultText;
        value = parseResultValue;
    }
}