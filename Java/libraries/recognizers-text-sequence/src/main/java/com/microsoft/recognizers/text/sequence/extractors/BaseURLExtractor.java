// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.matcher.MatchResult;
import com.microsoft.recognizers.text.matcher.StringMatcher;
import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.config.URLConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BaseURL;
import com.microsoft.recognizers.text.utilities.Match;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;

public class BaseURLExtractor extends BaseSequenceExtractor {
    private final URLConfiguration config;
    private StringMatcher tldMatcher;
    private Pattern ambiguousTimeTerm;

    protected final String extractType = Constants.SYS_URL;

    protected Map<Pattern, String> getRegexes() {
        return regexes;
    }

    protected String getExtractType() {
        return extractType;
    }

    public BaseURLExtractor(URLConfiguration config) {
        this.config = config;
        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(config.getUrlRegex(), Constants.URL_REGEX);
                put(config.getIpUrlRegex(), Constants.URL_REGEX);
                put(Pattern.compile(BaseURL.UrlRegex2), Constants.URL_REGEX);
            }
        };

        super.regexes = regexes;
        this.ambiguousTimeTerm = Pattern.compile(BaseURL.AmbiguousTimeTerm);

        this.tldMatcher = new StringMatcher();
        this.tldMatcher.init(BaseURL.TldList);
    }

    @Override
    public Boolean isValidMatch(Match match) {
        // For cases like "7.am" or "8.pm" which are more likely time terms.
        return !this.ambiguousTimeTerm.matcher(match.value).find();
    }
}
