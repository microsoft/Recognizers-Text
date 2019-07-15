package com.microsoft.recognizers.text.resources.writters;

public class BooleanWriter implements ICodeWriter {

    private final String name;
    private final boolean value;

    public BooleanWriter(String name, boolean value) {
        this.name = name;
        this.value = value;
    }

    @Override
    public String write() {
        return String.format(
                "    public static final Boolean %s = %s;",
                this.name,
                sanitize(String.valueOf(this.value)));
    }
}
