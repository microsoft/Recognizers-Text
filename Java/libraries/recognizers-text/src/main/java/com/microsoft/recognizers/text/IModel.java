// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

package com.microsoft.recognizers.text;

import java.util.List;

public interface IModel {
    String getModelTypeName();

    List<ModelResult> parse(String query);

}
