// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.utilities;

import java.util.regex.Matcher;

public interface StringReplacerCallback {
    public String replace(Matcher match);
}
