package com.microsoft.recognizers.text.resources.writters;

import java.util.Arrays;

public class ListWriter implements ICodeWriter {

    private final String name;
    private final String type;
    private final String[] items;

    public ListWriter(String name, String type, String[] items) {
        this.name = name;
        this.type = type;
        this.items = items;
    }

    @Override
    public String write() {
        String typeName = this.type.equalsIgnoreCase("char") ? "Character" : "String";
        String quoteChar = this.type.equalsIgnoreCase("char") ? "'" : "\"";
        String[] stringItems = Arrays.stream(this.items)
                .map(s -> quoteChar + sanitize(s) + quoteChar)
                .toArray(size -> new String[size]);

        return String.format(
                "    public static final List<%s> %s = Arrays.asList(%s);",
                typeName,
                this.name,
                String.join(", ", stringItems));
    }
}
