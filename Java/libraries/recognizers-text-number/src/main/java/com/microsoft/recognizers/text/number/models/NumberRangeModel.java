package com.microsoft.recognizers.text.number.models;

import com.microsoft.recognizers.text.IExtractor;
import com.microsoft.recognizers.text.IParser;
import com.microsoft.recognizers.text.number.Constants;

public class NumberRangeModel extends AbstractNumberModel {

    public NumberRangeModel(IParser parser, IExtractor extractor) {
        super(parser, extractor);
    }

    @Override
    public String getModelTypeName() {
        return Constants.MODEL_NUMBERRANGE;
    }
}
