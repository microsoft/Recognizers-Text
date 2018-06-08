package com.microsoft.recognizers.text.resources.writters;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.common.collect.ImmutableMap;

import java.util.Map;

public interface ICodeWriter {
    String write();

    default String sanitize(String input) {
        ObjectMapper mapper = new ObjectMapper();
        try {
            String stringified = mapper.writeValueAsString(input);
            return stringified.substring(1, stringified.length() - 1);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return "";
        }
    }

    Map<String, String> NumericTypes = ImmutableMap.of("Double", "D", "Long", "L");
    default String sanitize(String input, String valueType) {
        if(valueType.equals("Character")) return input;
        if(NumericTypes.containsKey(valueType)) {
            return input + (NumericTypes.get(valueType));
        }

        return sanitize(input);
    }
}
