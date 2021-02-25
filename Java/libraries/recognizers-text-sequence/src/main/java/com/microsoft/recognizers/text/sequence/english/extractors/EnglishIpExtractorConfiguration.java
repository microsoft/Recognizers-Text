// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.english.extractors;

import com.microsoft.recognizers.text.sequence.SequenceOptions;
import com.microsoft.recognizers.text.sequence.config.IpConfiguration;
import com.microsoft.recognizers.text.sequence.resources.BaseIp;

import java.util.regex.Pattern;

public class EnglishIpExtractorConfiguration extends IpConfiguration {
    public EnglishIpExtractorConfiguration(SequenceOptions options) {
        super(options);

        this.setIpv4Regex(Pattern.compile(BaseIp.Ipv4Regex));
        this.setIpv6Regex(Pattern.compile(BaseIp.Ipv6Regex));
    }
}