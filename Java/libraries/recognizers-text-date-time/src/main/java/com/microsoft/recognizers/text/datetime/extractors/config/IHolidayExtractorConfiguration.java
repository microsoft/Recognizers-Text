// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.extractors.config;

import java.util.regex.Pattern;

public interface IHolidayExtractorConfiguration {
    Iterable<Pattern> getHolidayRegexes();
}
