// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.english.extractors;

import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.config.URLConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BaseURL;

import java.util.regex.Pattern;

public class EnglishURLExtractorConfiguration extends URLConfiguration {
    public EnglishURLExtractorConfiguration(SequenceOptions options) {
        super(options);

        this.setIpUrlRegex(Pattern.compile(BaseURL.IpUrlRegex));
        this.setUrlRegex(Pattern.compile(BaseURL.UrlRegex));
    }
}
