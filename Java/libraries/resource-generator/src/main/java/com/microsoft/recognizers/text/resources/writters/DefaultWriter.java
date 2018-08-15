package com.microsoft.recognizers.text.resources.writters;

public class DefaultWriter implements ICodeWriter {

    private final String name;
    private final String value;

    public DefaultWriter(String name, String value) {

        this.name = name;
        this.value = value;
    }

    @Override
    public String write() {
        return String.format(
                "    public static final String %s = \"%s\";",
                this.name,
                sanitize(this.value));
    }
}
