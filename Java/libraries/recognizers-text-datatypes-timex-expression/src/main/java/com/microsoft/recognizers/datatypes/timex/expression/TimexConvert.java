// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.datatypes.timex.expression;

import com.microsoft.recognizers.datatypes.timex.expression.english.TimexConvertEnglish;

public class TimexConvert {
    public static String convertTimexToString(TimexProperty timex) {
        return TimexConvertEnglish.convertTimexToString(timex);
    }

    public static String convertTimexSetToString(TimexSet timexSet) {
        return TimexConvertEnglish.convertTimexSetToString(timexSet);
    }
}
