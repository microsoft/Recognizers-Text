// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.english.extractors;

import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.extractors.BasePhoneNumberExtractorConfiguration;
import com.microsoft.recognizers.text.sequence.resources.EnglishPhoneNumbers;

import java.util.regex.Pattern;

public class EnglishPhoneNumberExtractorConfiguration extends BasePhoneNumberExtractorConfiguration {
    public EnglishPhoneNumberExtractorConfiguration(SequenceOptions options) {
        super(options);

        super.setFalsePositivePrefixRegex(Pattern.compile(EnglishPhoneNumbers.FalsePositivePrefixRegex));
    }
}
