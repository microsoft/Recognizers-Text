package com.microsoft.recognizers.text;

import org.apache.commons.lang3.NotImplementedException;

import java.util.List;

public interface IModel {
    String getModelTypeName();
    List<ModelResult> parse(String query);
}
