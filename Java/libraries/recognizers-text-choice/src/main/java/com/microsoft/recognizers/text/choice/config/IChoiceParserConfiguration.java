// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.choice.config;

import java.util.Map;

public interface IChoiceParserConfiguration<T> {
    public Map<String, Boolean> getResolutions();
}