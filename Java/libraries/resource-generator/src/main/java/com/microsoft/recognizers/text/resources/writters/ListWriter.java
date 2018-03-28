package com.microsoft.recognizers.text.resources.writters;

import java.util.Arrays;

public class ListWriter implements ICodeWriter {

    private final String name;
    private final String[] items;

    public ListWriter(String name, String[] items) {
        this.name = name;
        this.items = items;
    }

    @Override
    public String write() {
        String[] stringItems = Arrays.stream(this.items)
                .map(s -> "\"" + sanitize(s) + "\"")
                .toArray(size -> new String[size]);

        return String.format(
                "    public static final List<String> %s = Arrays.asList(%s);",
                this.name,
                String.join(", ", stringItems));
    }
}
