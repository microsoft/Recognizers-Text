// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.choice.extractors;

import java.util.regex.Pattern;

public interface IBooleanExtractorConfiguration extends IChoiceExtractorConfiguration {
    public Pattern getTrueRegex();

    public Pattern getFalseRegex();
}