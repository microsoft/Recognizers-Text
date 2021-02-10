// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.config.IpConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BaseIp;
import com.microsoft.recognizers.text.utilities.Match;
import com.microsoft.recognizers.text.utilities.RegExpUtility;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;
import java.util.regex.Pattern;
import java.util.stream.Stream;

import org.apache.commons.lang3.StringUtils;

public class BaseIpExtractor extends BaseSequenceExtractor {
    private IpConfiguration config;
    protected String extractType = Constants.SYS_IP;

    protected String getExtractType() {
        return this.extractType;
    }

    // The Ipv6 address regexes is written following the Recommendation:
    // https://tools.ietf.org/html/rfc5952
    public BaseIpExtractor(IpConfiguration config) {
        Map<Pattern, String> regexes = new HashMap<Pattern, String>() {
            {
                put(Pattern.compile(BaseIp.Ipv4Regex), Constants.IP_REGEX_IPV4);
                put(Pattern.compile(BaseIp.Ipv6Regex), Constants.IP_REGEX_IPV6);
            }
        };

        super.regexes = regexes;
    }

    @Override
    public List<ExtractResult> extract(String text) {
        List<ExtractResult> result = new ArrayList<ExtractResult>();

        if (StringUtils.isBlank(text)) {
            return result;
        }

        HashMap<Match, String> matchSource = new HashMap<>();
        boolean[] matched = new boolean[text.length()];

        // Traverse every match results to see each position in the text is matched or
        // not.
        HashMap<Match[], String> collections = new HashMap<>();
        regexes.forEach((key, value) -> {
            Match[] matches = RegExpUtility.getMatches(key, text);
            collections.put(matches, value);
        });

        collections.forEach((key, value) -> {
            for (Match match : key) {
                for (int j = 0; j < match.length; j++) {
                    matched[match.index + j] = true;
                }

                // Keep Source Data for extra information
                matchSource.put(match, value);
            }
        });

        int lastNotMatched = -1;
        for (int i = 0; i < text.length(); i++) {
            if (matched[i]) {
                if (i + 1 == text.length() || !matched[i + 1]) {
                    int start = lastNotMatched + 1;
                    int length = i - lastNotMatched;
                    String substr = text.substring(start, start + length);
                    if (substr.startsWith(Constants.IPV6_ELLIPSIS) && (start > 0 && Character.isLetterOrDigit(text.charAt(start - 1)))) {
                        break;
                    }

                    if (substr.endsWith(Constants.IPV6_ELLIPSIS) && (i + 1 < text.length() && Character.isLetterOrDigit(text.charAt(i + 1)))) {
                        break;
                    }

                    Function<Match, Boolean> matchFunc = match -> match.index == start && match.length == length;

                    if (matchSource.keySet().stream().anyMatch(o -> matchFunc.apply(o))) {
                        Match srcMatch = (Match)matchSource.keySet().toArray()[0];
                        ExtractResult extResult = new ExtractResult();

                        extResult.setStart(start);
                        extResult.setLength(length);
                        extResult.setText(substr);
                        extResult.setType(this.extractType);
                        extResult.setData(matchSource.getOrDefault(srcMatch, null));
                        result.add(extResult);
                    }
                }
            } else {
                lastNotMatched = i;
            }
        }

        return result;
    }
}
