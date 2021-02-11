// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.parsers;

import com.microsoft.recognizers.text.ExtractResult;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.ParseResult;

public class BaseSequenceParser implements IParser {
    @Override
    public ParseResult parse(ExtractResult extResult) {
        ParseResult result = new ParseResult(extResult.getStart(), extResult.getLength(), extResult.getText(),
                extResult.getType(), null, null, extResult.getText());
        return result;
    }
}
