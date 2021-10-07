// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.resources.BaseHashtag;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class BaseHashTagExtractor extends BaseSequenceExtractor {
    protected final String extractType = Constants.SYS_HASHTAG;

    protected String getExtractType() {
        return this.extractType;
    }
    
    public BaseHashTagExtractor() {
        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(Pattern.compile(BaseHashtag.HashtagRegex), Constants.HASHTAG_REGEX);
            }
        };

        super.regexes = regexes;
    }
}
