package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.util.List;
import java.util.Map;

@JsonIgnoreProperties(ignoreUnknown = true)
public class TestCase {

    public String language;
    public String recognizerName;
    public String modelName;

    public String testType;
    public String input;
    public Map<String, Object> context;
    public Boolean debug = false;
    public String notSupported;
    public String notSupportedByDesign;
    public List<Object> results;

    public String toString() {
        return String.format("%sRecognizer - %s - %s - \"%s\"", this.recognizerName, this.language, this.modelName, this.input);
    }
}
