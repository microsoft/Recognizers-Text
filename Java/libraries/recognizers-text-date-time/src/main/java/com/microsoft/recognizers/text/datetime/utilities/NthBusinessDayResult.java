// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.datetime.utilities;

import java.time.LocalDateTime;
import java.util.List;

public class NthBusinessDayResult {
    public final LocalDateTime result;
    public final List<LocalDateTime> dateList;

    public NthBusinessDayResult(LocalDateTime result, List<LocalDateTime> dateList) {
        this.result = result;
        this.dateList = dateList;
    }
}
