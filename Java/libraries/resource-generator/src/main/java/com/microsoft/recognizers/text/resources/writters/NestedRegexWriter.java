package com.microsoft.recognizers.text.resources.writters;

import com.microsoft.recognizers.text.resources.datatypes.NestedRegex;

import java.util.Arrays;

public class NestedRegexWriter implements ICodeWriter {

    private final String name;
    private final NestedRegex def;

    public NestedRegexWriter(String name, NestedRegex def) {
        this.name = name;
        this.def = def;
    }

    @Override
    public String write() {
        String replace = String.join("", Arrays.stream(this.def.references).map(p -> "\n            .replace(\"{" + p + "}\", " + p + ")").toArray(size -> new String[size]));

        String template = "    public static final String %s = \"%s\"%s;";
        return String.format(template, this.name, sanitize(this.def.def), replace);
    }
}
