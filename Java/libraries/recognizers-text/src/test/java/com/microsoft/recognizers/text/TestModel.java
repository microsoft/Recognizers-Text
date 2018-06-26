package com.microsoft.recognizers.text;

import com.microsoft.recognizers.text.IModel;
import com.microsoft.recognizers.text.ModelResult;
import org.apache.commons.lang3.NotImplementedException;

import java.util.ArrayList;
import java.util.List;

public class TestModel implements IModel {
    @Override
    public String getModelTypeName() {
        return "com.microsoft.recognizers.text.TestModel";
    }

    @Override
    public List<ModelResult> parse(String query) {
        return new ArrayList<>();
    }
}
