// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.resources.BaseMention;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class BaseMentionExtractor extends BaseSequenceExtractor {
    protected final String extractType = Constants.SYS_MENTION;

    protected String getExtractType() {
        return this.extractType;
    }

    public BaseMentionExtractor() {
        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(Pattern.compile(BaseMention.MentionRegex), Constants.MENTION_REGEX);
            }
        };

        super.regexes = regexes;
    }
}
