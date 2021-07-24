// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.extractors;

import com.microsoft.recognizers.text.utilities.Match;

public interface IDateExtractor extends IDateTimeExtractor {
    int getYearFromText(Match match);
}
