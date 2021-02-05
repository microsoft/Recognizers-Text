// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text.sequence.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.sequence.Constants;

public class EmailModel extends AbstractSequenceModel {
    public EmailModel(IParser parser, IExtractor extractor) {
        super(parser, extractor);
        this.modelTypeName = Constants.MODEL_EMAIL;
    }
}
