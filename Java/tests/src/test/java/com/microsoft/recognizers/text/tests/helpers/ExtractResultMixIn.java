// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.tests.helpers;

import com.fasterxml.jackson.annotation.JsonProperty;

public abstract class ExtractResultMixIn {
    ExtractResultMixIn(@JsonProperty("start") Integer start,
                       @JsonProperty("length") Integer length,
                       @JsonProperty("text") String text,
                       @JsonProperty("type") String type,
                       @JsonProperty("data") Object data) {
    }
}
