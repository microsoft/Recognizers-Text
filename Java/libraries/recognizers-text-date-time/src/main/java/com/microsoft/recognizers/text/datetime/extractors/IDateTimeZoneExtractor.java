// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.ExtractResult;

import java.util.List;

public interface IDateTimeZoneExtractor extends IDateTimeExtractor {
    List<ExtractResult> removeAmbiguousTimezone(List<ExtractResult> extractResults);
}
