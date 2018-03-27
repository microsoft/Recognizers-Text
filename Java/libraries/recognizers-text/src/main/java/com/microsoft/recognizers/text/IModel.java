package com.microsoft.recognizers.text;

import java.util.List;

public interface IModel {
    String getModelTypeName();
    List<ModelResult> parse(String query);
}
