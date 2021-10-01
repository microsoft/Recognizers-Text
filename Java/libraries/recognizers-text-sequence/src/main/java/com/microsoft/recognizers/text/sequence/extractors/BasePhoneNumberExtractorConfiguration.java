// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.extractors;

import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.config.PhoneNumberConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BasePhoneNumbers;

import java.util.regex.Pattern;

public class BasePhoneNumberExtractorConfiguration extends PhoneNumberConfiguration {
    public BasePhoneNumberExtractorConfiguration(SequenceOptions options) {
        super(options);
        setWordBoundariesRegex(BasePhoneNumbers.WordBoundariesRegex);
        setNonWordBoundariesRegex(BasePhoneNumbers.NonWordBoundariesRegex);
        setEndWordBoundariesRegex(BasePhoneNumbers.EndWordBoundariesRegex);
        setColonPrefixCheckRegex(Pattern.compile(BasePhoneNumbers.ColonPrefixCheckRegex));
        setColonMarkers(BasePhoneNumbers.ColonMarkers);
        setForbiddenPrefixMarkers(BasePhoneNumbers.ForbiddenPrefixMarkers);
        setForbiddenSuffixMarkers(BasePhoneNumbers.ForbiddenSuffixMarkers);
    }
}
