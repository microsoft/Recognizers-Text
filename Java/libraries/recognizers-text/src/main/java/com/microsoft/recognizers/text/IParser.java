// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text;

public interface IParser {
    ParseResult parse(ExtractResult extractResult);
}
