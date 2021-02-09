// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.sequence.Constants;
import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.config.BaseSequenceConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BaseEmail;
import com.microsoft.recognizers.text.utilities.StringUtility;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class BaseEmailExtractor extends BaseSequenceExtractor {
    private static final Pattern RFC_5322_VALIDATION_REGEX = Pattern.compile(BaseEmail.RFC5322Regex);
    private final BaseSequenceConfiguration config;

    protected final String extractType = Constants.SYS_EMAIL;

    protected Map<Pattern, String> getRegexes() {
        return regexes;
    }

    protected String getExtractType() {
        return extractType;
    }

    @Override
    protected List<ExtractResult> postFilter(List<ExtractResult> results) {
        // If Relaxed is on, no extra validation is applied
        if (config.getOptions() != SequenceOptions.None) {
            return results;
        } else {
            // Not return malformed e-mail addresses and trim ending '.'
            results.forEach(result -> {
                if (result.getText().endsWith(".")) {
                    result.setText(StringUtility.trimEnd(result.getText()));
                    result.setLength(result.getLength() - 1);
                }
            });
        }

        return results.stream().filter((o -> RFC_5322_VALIDATION_REGEX.matcher((o).getText()).matches()))
                .collect(Collectors.toList());
    }

    public BaseEmailExtractor(BaseSequenceConfiguration config) {
        this.config = config;
        Map<Pattern, String> regexes = new HashMap<Pattern, String>();
        regexes.put(Pattern.compile(BaseEmail.EmailRegex), Constants.EMAIL_REGEX);
        // EmailRegex2 will break the code as it's not supported in Java, comment out for now
        // Error: java.util.regex.PatternSyntaxException: Unknown inline modifier near index 4
        // The same issue happens in JS and it was disabled in https://github.com/microsoft/Recognizers-Text/pull/905
        // @TODO: we need to search a way to handle inline modifier.
        // See https://stackoverflow.com/questions/48189725/java-regex-pattern-compilation-error
        // regexes.put(Pattern.compile(BaseEmail.EmailRegex2), Constants.EMAIL_REGEX);

        super.regexes = regexes;
    }
}
