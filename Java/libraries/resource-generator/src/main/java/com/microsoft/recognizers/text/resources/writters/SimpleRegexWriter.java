package com.microsoft.recognizers.text.resources.writters;

import com.microsoft.recognizers.text.resources.datatypes.SimpleRegex;

public class SimpleRegexWriter implements ICodeWriter {

    private String name;
    private SimpleRegex def;

    public SimpleRegexWriter(String name, SimpleRegex def) {
        this.name = name;
        this.def = def;
    }

    @Override
    public String write() {
        return String.format("    public static final String %s = \"%s\";", this.name, sanitize(this.def.def));
    }
}
