// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text;

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
