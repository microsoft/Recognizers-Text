package com.microsoft.recognizers.text.tests;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
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

    public LocalDateTime getReferenceDateTime() {
        if (context != null && context.containsKey("ReferenceDateTime")) {
            Object objectDateTime = context.get("ReferenceDateTime");
            return LocalDateTime.parse(objectDateTime.toString(), DateTimeFormatter.ISO_LOCAL_DATE_TIME);
        }

        return LocalDateTime.now();
    }

    public String toString() {
        return String.format("%sRecognizer - %s - %s - \"%s\"", this.recognizerName, this.language, this.modelName, this.input);
    }
}
