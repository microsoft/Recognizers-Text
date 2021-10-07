// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.config;

import com.microsoft.recognizers.text.sequence.SequenceOptions;

import java.util.regex.Pattern;

public class IpConfiguration implements ISequenceConfiguration {
    private SequenceOptions options;
    private Pattern ipv4Regex;
    private Pattern ipv6Regex;

    public IpConfiguration(SequenceOptions options) {
        this.options = options != null ? options : SequenceOptions.None;
    }
    
    public SequenceOptions getOptions() {
        return options;
    }

    public Pattern getIpv4Regex() {
        return ipv4Regex;
    }

    public void setIpv4Regex(Pattern withIpv4Regex) {
        ipv4Regex = withIpv4Regex;
    }

    public Pattern getIpv6Regex() {
        return ipv6Regex;
    }

    public void setIpv6Regex(Pattern withIpv6Regex) {
        ipv6Regex = withIpv6Regex;
    }
}
