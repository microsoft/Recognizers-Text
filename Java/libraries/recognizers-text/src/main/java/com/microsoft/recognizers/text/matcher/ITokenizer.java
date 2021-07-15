// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.matcher;

import java.util.List;

public interface ITokenizer {
    List<Token> tokenize(String input);
}
