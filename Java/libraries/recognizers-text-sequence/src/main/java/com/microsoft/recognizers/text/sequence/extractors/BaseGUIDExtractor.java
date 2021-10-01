// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.resources.BaseGUID;

import java.util.HashMap;
import java.util.Map;
import java.util.regex.Pattern;

public class BaseGUIDExtractor extends BaseSequenceExtractor {
    protected final String extractType = Constants.SYS_GUID;

    protected String getExtractType() {
        return this.extractType;
    }
    
    public BaseGUIDExtractor() {
        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(Pattern.compile(BaseGUID.GUIDRegex), Constants.GUID_REGEX);
            }
        };

        super.regexes = regexes;
    }
}
